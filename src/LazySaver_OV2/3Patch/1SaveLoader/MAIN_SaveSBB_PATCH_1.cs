// MIT License
//
// Copyright (c) 2023. Super Comic (ekfvoddl3535@naver.com)
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in all
// copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
// SOFTWARE.

using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using HarmonyLib;
using Ionic.Zlib;
using Klei;
using KSerialization;
using SuperComicLib.MonoRuntime;

namespace LazySaver
{
    using static GlobalHeader;
    [HarmonyPatch(typeof(SaveLoader), nameof(SaveLoader.Save), typeof(string), typeof(bool), typeof(bool))]
    public static unsafe class SaveSSB_PATCH_1
    {
        // methods (delegate)
        private static MethodReportSaveMetrics m_rsm;
        private static MethodSave m_save;

        private static System.Action m_taskTargetMethod;

        public static void Prepare()
        {
            const BindingFlags INST = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance;

            var t0 = typeof(SaveLoader);

            var m1 = t0.GetMethod("ReportSaveMetrics", INST);
            m_rsm = (MethodReportSaveMetrics)Delegate.CreateDelegate(typeof(MethodReportSaveMetrics), m1);

            m1 = t0.GetMethod(nameof(SaveLoader.Save), INST, null, new[] { typeof(BinaryWriter) }, null);
            m_save = (MethodSave)Delegate.CreateDelegate(typeof(MethodSave), m1);

            m_taskTargetMethod = WriteToDiskAsync;
        }

        public static bool Prefix(ref string __result, string filename, bool isAutoSave, bool updateSavePointer)
        {
            Manager.Clear();

            if (m_previousTask != null && !m_previousTask.IsCompleted)
            {
                Debug.LogWarning("Previous save task did not completed. [ Waiting ]");
                m_previousTask.Wait();
            }

            string dirName = Path.GetDirectoryName(filename);

            try
            {
                if (!Directory.Exists(dirName))
                    Directory.CreateDirectory(dirName);
            }
            catch (Exception ex)
            {
                Debug.LogWarning("Problem creating save folder for " + filename + "!\n" + ex.ToString());
            }

            arg_isAutoSave = isAutoSave;
            arg_filename = filename;

            res_errMsg = null;
            res_exceptionUnhandled = false;

            // NOTE:
            //  원본. 510 ~ 544줄은 나중에 (파일 저장시)

            // NOTE:
            //  원본: 551 ~ 588줄은 나중에 (파일 저장시)

            if (isAutoSave)
            {
                // ready to async save operation
                SpeedControlScreen.Instance.Pause(false);

                m_previousTask = Task.Factory.StartNew(m_taskTargetMethod);

                Game.Instance.StartCoroutine(SLSavePostProcessor.Default);
            }
            else
            {
                WriteToDiskAsync();
                SavePostProcess.Complete();
            }

            if (updateSavePointer)
                SaveLoader.SetActiveSaveFilePath(filename);

            __result = filename;

            return false;
        }

        private static void SaveImmediateTo(BinaryWriter fw)
        {
            var saveHeader = SaveGame.Instance.GetSaveHeader(arg_isAutoSave, true, out var header);

            fw.Write(header.buildVersion);
            fw.Write(header.headerSize);
            fw.Write(header.headerVersion);
            fw.Write(header.compression);
            fw.Write(saveHeader);

            Manager.SerializeDirectory(fw);

            RetireColonyUtility.SaveColonySummaryData();
        }

        private static void WriteToDiskAsync()
        {
            try
            {
                var instance = SaveLoader.Instance;
                m_rsm.Invoke(instance, arg_isAutoSave);

                var os = (NativeStream)m_writer.BaseStream;
                os.FastClear();
                
                m_save.Invoke(instance, m_writer);

                // 원본 510 ~ 544줄
                if (arg_isAutoSave && !GenericGameSettings.instance.keepAllAutosaves)
                    KeepOrDeleteAutoSaveFiles();

                // 원본 551 ~ 588줄
                var fs = File.Open(arg_filename, FileMode.Create);

                var len = os.Position;
                SaveImmediateTo(m_writer);

                os.Position = len;
                os.UnsafeCopyTo(os.Length, fs, m_tempBuffer);

                // 원본 SaveLoader.cs 124번줄 CompressContents 메소드 인라인
                var zs = new ZlibStream(fs, CompressionMode.Compress, CompressionLevel.BestSpeed);
                os.Position = 0;
                os.UnsafeCopyTo(len, zs, m_tempBuffer);
                zs.Close();

                Game.Instance.timelapser.SaveColonyPreview(arg_filename);

                KCrashReporter.MOST_RECENT_SAVEFILE = arg_filename;
                Stats.Print();
            }
            catch (Exception ex)
            {
                UnityEngine.Debug.LogError(ex.ToString());

                string msg;
                switch (ex)
                {
                    case UnauthorizedAccessException _:
                        DebugUtil.LogArgs("UnauthorizedAccessException for " + arg_filename);
                        msg = string.Format((string)STRINGS.UI.CRASHSCREEN.SAVEFAILED, "Unauthorized Access Exception");
                        break;

                    case IOException _:
                        DebugUtil.LogArgs("IOException (probably out of disk space) for " + arg_filename);
                        msg = string.Format((string)STRINGS.UI.CRASHSCREEN.SAVEFAILED, "IOException. You may not have enough free space!");
                        break;

                    default:
                        msg = ex.ToString();
                        res_exceptionUnhandled = true;
                        break;
                }
            }
        }

        private static void KeepOrDeleteAutoSaveFiles()
        {
            var saveFiles = SaveLoader.GetSaveFiles(SaveLoader.GetActiveAutoSavePath(), true);
            var strList = new List<string>();

            var colonyGuid = SaveLoader.Instance.GameInfo.colonyGuid.ToString();
            for (int i = 0, cnt = saveFiles.Count; i < cnt; i++)
            {
                var entry = saveFiles[i];

                var _fi = SaveGame.GetFileInfo(entry.path);
                if (_fi != null && SaveGame.GetSaveUniqueID(_fi.second) == colonyGuid)
                    strList.Add(entry.path);
            }

            for (int i = strList.Count; --i >= 9;)
            {
                var p1 = strList[i];
                try
                {
                    Debug.Log("Deleting old autosave: " + p1);
                    File.Delete(p1);
                }
                catch (Exception ex)
                {
                    Debug.LogWarning("Problem deleting autosave: " + p1 + "\n" + ex.ToString());
                }

                p1 = Path.ChangeExtension(p1, ".png");
                try
                {
                    if (File.Exists(p1))
                        File.Delete(p1);
                }
                catch (Exception ex)
                {
                    Debug.LogWarning("Problem deleting autosave screenshot: " + p1 + "\n" + ex.ToString());
                }
            }
        }
    }
}