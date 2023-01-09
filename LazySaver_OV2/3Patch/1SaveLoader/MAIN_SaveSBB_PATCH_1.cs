using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using HarmonyLib;
using Ionic.Zlib;
using Klei;
using LazySaver.MemoryIO;
using LazySaver.Presets;

namespace LazySaver
{
    using static SharedData;
    [HarmonyPatch(typeof(SaveLoader), nameof(SaveLoader.Save), typeof(string), typeof(bool), typeof(bool))]
    public static class SaveSSB_PATCH_1
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

            var preset = PresetUtil.GetIOProfile(ProfilePreset.Medium);
            m_writer = UnmanagedWriter.Create(preset.InitialSize, preset.IncreaseSize, Encoding.UTF8);

            GC.AddMemoryPressure(preset.InitialSize);

            m_taskTargetMethod = WriteToDiskAsync;
        }

        public static bool Prefix(ref string __result, SaveLoader __instance, string filename, bool isAutoSave, bool updateSavePointer)
        {
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

            // NOTE:
            //  원본. 510 ~ 544줄은 나중에 (파일 저장시)

            var bw = m_writer;
            bw.m_stream.FastClear();

            // NOTE:
            //  원본: 551 ~ 588줄은 나중에 (파일 저장시)

            if (updateSavePointer)
                SaveLoader.SetActiveSaveFilePath(filename);

            // ready to async save operation
            arg_isAutoSave = isAutoSave;
            arg_filename = filename;
            
            res_errMsg = null;
            res_exceptionUnhandled = false;

            SpeedControlScreen.Instance.Pause();
            m_previousTask = Task.Factory.StartNew(m_taskTargetMethod);

            __instance.StartCoroutine(SLSavePostProcessor.Default);

            __result = filename;

            return false;
        }

        private static void WriteToDiskAsync()
        {
            try
            {
                KSerialization.Manager.Clear();

                var instance = SaveLoader.Instance;
                m_rsm.Invoke(instance, arg_isAutoSave);

                m_save.Invoke(instance, m_writer);
                Game.Instance.timelapser.SaveColonyPreview(arg_filename);

                // 원본 510 ~ 544줄
                RetireColonyUtility.SaveColonySummaryData();
            }
            catch (Exception ex)
            {
                UnityEngine.Debug.Log(ex.ToString());
                UnityEngine.Debug.LogError(ex.ToString());
            }

            if (arg_isAutoSave && !GenericGameSettings.instance.keepAllAutosaves)
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

            // 원본 551 ~ 588줄
            try
            {
                var fw = new BinaryWriter(File.Open(arg_filename, FileMode.Create));

                var saveHeader = SaveGame.Instance.GetSaveHeader(arg_isAutoSave, true, out var header);
                fw.Write(header.buildVersion);
                fw.Write(header.headerSize);
                fw.Write(header.headerVersion);
                fw.Write(header.compression);
                fw.Write(saveHeader);
                KSerialization.Manager.SerializeDirectory(fw);

                // 원본 SaveLoader.cs 124번줄 CompressContents 메소드 인라인
                var zs = new ZlibStream(fw.BaseStream, CompressionMode.Compress, CompressionLevel.BestSpeed);

                var os = m_writer.m_stream;
                os.Position = 0;
                os.CopyTo(zs, 0x100_0000); // 16.0MiB

                zs.Flush();
                zs.Close();

                fw.Close();

                KCrashReporter.MOST_RECENT_SAVEFILE = arg_filename;
            }
            catch (Exception ex)
            {
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
    }
}