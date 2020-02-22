using Klei;
using KSerialization;
using SuperComicLib.Threading;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace AsynchronousAutosave
{
    partial class SAVEPATCH
    {
        public static volatile bool autosaving;

        private static void TestMethod(AwaitibleAsyncTask.Instance tempinst, int cycle, int lives, string bn, bool sandbox, string actpath, SaveManager manager, SaveGame.GameInfo info, string filename, bool updatepointer)
        {
            autosaving = true;
            Manager.Clear();
            SaveFileRoot sfr = new SaveFileRoot();

            SaveLoadRootDATAList slrdlist = new SaveLoadRootDATAList();

            using (MemoryStream gmms = new MemoryStream())
            using (BinaryWriter gmwr = new BinaryWriter(gmms))
            {
                if (ThreadedHttps<KleiMetrics>.Instance != null && ThreadedHttps<KleiMetrics>.Instance.enabled)
                {
                    Dictionary<string, object> ed = new Dictionary<string, object>();
                    ed[GameClock.NewCycleKey] = cycle + 1;
                    ed["WasDebugEverUsed"] = Game.Instance.debugWasUsed;
                    ed["IsAutoSave"] = true;
                    tempinst.Invoke(() =>
                    {
                        Dictionary<Tag, List<SaveLoadRoot>> objs = manager.GetLists();
                        HashSet<Tag> tempkeys = new HashSet<Tag>(objs.Keys);

                        ed["SavedPrefabs"] = GetPrefabMetricsDatas(objs);
                        ed["ResourcesAccessible"] = GetWorldInventory();
                        ed["MinionMetrics"] = GetMinionMetricsDatas();
                        sfr.GridVisible = Grid.Visible;
                        sfr.GridSpawnable = Grid.Spawnable;
                        sfr.GridDamage = ToBytes(Grid.Damage);

                        Tag savgameTag = SaveGame.Instance.PrefabID();
                        tempkeys.Remove(savgameTag);
                        slrdlist.Add(false, savgameTag.Name, PrepTWrite(SaveGame.Instance.GetComponent<SaveLoadRoot>()));

                        foreach (Tag ok in tempkeys)
                        {
                            foreach (SaveLoadRoot root in objs[ok])
                                if (root != null)
                                    slrdlist.Add(root.GetComponent<SimCellOccupier>() == null, ok.Name, PrepTWrite(root));
                        }
                        tempkeys.Clear();

                        using (MemoryStream ms2 = new MemoryStream())
                        using (BinaryWriter br2 = new BinaryWriter(ms2))
                        {
                            Camera.main.transform.parent.GetComponent<CameraController>().Save(br2);
                            sfr.Camera = ms2.ToArray();
                        }

                        Game.Instance.Save(gmwr);
                        RetireColonyUtility.SaveColonySummaryData();

                        objs = null;
                        tempkeys = null;
                        savgameTag = null;
                    }).Wait();

                    ed["DailyReport"] = GetDailyReportMetrics(cycle);
                    ed["PerformanceMeasurements"] = new List<CommonMetricsData>() { new CommonMetricsData("FramesAbove30"), new CommonMetricsData("FramesBelow30") };
                    ed["AverageFrameTime"] = 30f; // why write this.
                    ed["CustomGameSettings"] = CustomGameSettings.Instance.GetSettingsForMetrics();
                    ThreadedHttps<KleiMetrics>.Instance.SendEvent(ed);
                    ed.Clear();
                }

                if (!GenericGameSettings.instance.keepAllAutosaves)
                {
                    List<string> sfs = SaveLoader.GetSaveFiles(Path.GetDirectoryName(filename));
                    for (int x = sfs.Count - 1; x >= 9; x--)
                        try
                        {
                            string p1 = sfs[x];
                            File.Delete(p1);
                            string p2 = Path.ChangeExtension(p1, ".png");
                            if (File.Exists(p2))
                                File.Delete(p2);
                        }
                        catch (Exception ex)
                        {
                            Debug.LogWarning("Something is wrong: " + ex.ToString());
                            throw ex;
                        }

                    sfs.Clear();
                }

                // 직렬화 시작
                // 파트 1
                byte[] buffer = null;
                using (MemoryStream ms = new MemoryStream())
                {
                    // 메인 헤더 "world" 대신에
                    ms.WriteByte(AsyncAutosaveCS.WORLDHEADER);
                    //
                    // serialize
                    //
                    sfr.WidthInCells = Grid.WidthInCells;
                    sfr.HeightInCells = Grid.HeightInCells;
                    using (MemoryStream opm = new MemoryStream())
                    using (BinaryWriter br = new BinaryWriter(opm))
                    {
                        Sim.Save(br);
                        sfr.Sim = opm.ToArray();
                    }
                    Global.Instance.modManager.SendMetricsEvent();
                    sfr.active_mods = new List<KMod.Label>();
                    foreach (KMod.Mod mod in Global.Instance.modManager.mods)
                        if (mod.enabled)
                            sfr.active_mods.Add(mod.label);

                    ms.WriteInt32(sfr.WidthInCells);
                    ms.WriteInt32(sfr.HeightInCells);
                    ms.WriteBytes(sfr.Sim);
                    ms.WriteBytes(sfr.GridVisible);
                    ms.WriteBytes(sfr.GridSpawnable);
                    ms.WriteBytes(sfr.GridDamage);
                    ms.WriteBytes(sfr.Camera);

                    // active_mods
                    ms.WriteInt32(sfr.active_mods.Count);
                    foreach (KMod.Label label in sfr.active_mods)
                    {
                        ms.WriteByte((byte)label.distribution_platform);
                        ms.WriteString(label.id);
                        ms.WriteInt64(label.version);
                        ms.WriteString(label.title);
                    }
                    //
                    // end
                    //
                    using (BinaryWriter wr = new BinaryWriter(ms))
                    {
                        Game.SaveSettings(wr);
                        //
                        // FUNC
                        //
                        wr.Write(AsyncAutosaveCS.SAVE_HEADER);
                        wr.Write(SaveManager.SAVE_MAJOR_VERSION);
                        wr.Write(SaveManager.SAVE_MINOR_VERSION);

                        long pos1 = wr.BaseStream.Position;
                        wr.Write(uint.MinValue);

                        // wr.WriteKleiString(savgameTag.Name);
                        // wr.Write(1);
                        // wr.WriteLengthWithAction(tmp => savgameData.TWrite(tempinst, tmp, ref count));

                        uint count = 0;
                        slrdlist.WriteAuto(tempinst, wr, ref count);

                        long posEnd = wr.BaseStream.Position;
                        wr.BaseStream.Position = pos1;
                        wr.Write(count);
                        wr.BaseStream.Position = posEnd;

                        slrdlist = null;
                        //
                        // END
                        //
                        wr.Write(gmms.ToArray());
                        buffer = ms.ToArray();
                    }
                }
                try
                {
                    using (BinaryWriter wr2 = new BinaryWriter(File.Open(filename, FileMode.Create)))
                    {
                        using (MemoryStream ms = new MemoryStream())
                        {
                            // GameInfo 
                            ms.WriteInt32(cycle);
                            ms.WriteInt32(lives);
                            ms.WriteString(bn);
                            ms.WriteBool(true);
                            ms.WriteString(actpath);
                            ms.WriteInt32(SaveManager.SAVE_MAJOR_VERSION);
                            ms.WriteInt32(SaveManager.SAVE_MINOR_VERSION);
                            ms.WriteString(info.worldID);
                            ms.WriteStringArray(info.worldTraits);
                            ms.WriteBool(sandbox);

                            byte[] temp = ms.ToArray();

                            // 헤더 
                            wr2.Write(KleiVersion.ChangeList); // buildversion
                            wr2.Write(temp.Length); // headersize
                            wr2.Write(1U); // headerversion
                            wr2.Write(0); // compression
                            wr2.Write(temp);
                        }

                        Manager.SerializeDirectory(wr2);
                        wr2.Write(buffer);
                    }
                }
                catch (Exception ex2)
                {
                    if (ex2 is UnauthorizedAccessException)
                    {
                        DebugUtil.LogArgs((object)("UnauthorizedAccessException for " + filename));
                        ((ConfirmDialogScreen)GameScreenManager.Instance.StartScreen(ScreenPrefabs.Instance.ConfirmDialogScreen.gameObject, GameScreenManager.Instance.ssOverlayCanvas.gameObject, GameScreenManager.UIRenderTarget.ScreenSpaceOverlay)).PopupConfirmDialog(string.Format(STRINGS.UI.CRASHSCREEN.SAVEFAILED, "Unauthorized Access Exception"), null, null);
                    }
                    if (!(ex2 is IOException))
                        throw ex2;
                    DebugUtil.LogArgs((object)("IOException (probably out of disk space) for " + filename));
                    ((ConfirmDialogScreen)GameScreenManager.Instance.StartScreen(ScreenPrefabs.Instance.ConfirmDialogScreen.gameObject, GameScreenManager.Instance.ssOverlayCanvas.gameObject, GameScreenManager.UIRenderTarget.ScreenSpaceOverlay)).PopupConfirmDialog(string.Format(STRINGS.UI.CRASHSCREEN.SAVEFAILED, "IOException. You may not have enough free space!"), null, null);
                    return;
                }
                if (updatepointer)
                    SaveLoader.SetActiveSaveFilePath(filename);
                DebugUtil.LogArgs("Saved to", "[" + filename + "]");

                tempinst.Invoke(() =>
                {
                    Messenger.Instance.QueueMessage(Translation.StringPatch.Message);
                    autosaving = false;
                }).Wait();
            }
        }

        public static SaveLoadRootDATA PrepTWrite(SaveLoadRoot root)
        {
            Transform restr = root.transform;
            return new SaveLoadRootDATA(
                restr.GetPosition(),
                restr.rotation,
                restr.localScale,
                root.GetComponents<KMonoBehaviour>(),
                root.gameObject);
        }
    }
}
