using KSerialization;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace AsynchronousAutosave
{
    partial class SAVEPATCH
    {
        public static void OriginalCode(SaveLoader __instance, string filename, bool updateSavePointer)
        {
            Manager.Clear();
            SaveManager manager = __instance.saveManager;
            SaveGame sg = SaveGame.Instance;
            int cycle = GameClock.Instance.GetCycle();
            if (ThreadedHttps<KleiMetrics>.Instance != null && ThreadedHttps<KleiMetrics>.Instance.enabled)
            {
                Dictionary<string, object> ed = new Dictionary<string, object>();
                ed[GameClock.NewCycleKey] = cycle + 1;
                ed["WasDebugEverUsed"] = Game.Instance.debugWasUsed;
                ed["IsAutoSave"] = false;
                ed["SavedPrefabs"] = GetPrefabMetricsDatas(manager.GetLists());
                ed["ResourcesAccessible"] = GetWorldInventory();
                ed["MinionMetrics"] = GetMinionMetricsDatas();

                ed["DailyReport"] = GetDailyReportMetrics(cycle);
                ed["PerformanceMeasurements"] = new List<CommonMetricsData>() { new CommonMetricsData("FramesAbove30"), new CommonMetricsData("FramesBelow30") };
                ed["AverageFrameTime"] = 30f;

                ed["CustomGameSettings"] = CustomGameSettings.Instance.GetSettingsForMetrics();
                ThreadedHttps<KleiMetrics>.Instance.SendEvent(ed);
                ed.Clear();
            }
            RetireColonyUtility.SaveColonySummaryData();
            #region 주석처리된 구간
            /* 
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
            */
            #endregion
            byte[] buffer = null;
            using (MemoryStream ms = new MemoryStream())
            {
                ms.WriteByte(AsyncAutosaveCS.WORLDHEADER);

                //
                // serialize SAVEFILEROOT
                //
                SaveFileRoot sfr = PrepSaveFile();

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
                // end SAVEFILEROOT
                // 


                using (BinaryWriter wr = new BinaryWriter(ms))
                {
                    Game.SaveSettings(wr);

                    #region manager.Save(BinaryWriter)
                    Dictionary<Tag, List<SaveLoadRoot>> objs = manager.GetLists();
                    wr.Write(AsyncAutosaveCS.SAVE_HEADER);
                    wr.Write(SaveManager.SAVE_MAJOR_VERSION);
                    wr.Write(SaveManager.SAVE_MINOR_VERSION);
                    int numx = 0;
                    foreach (KeyValuePair<Tag, List<SaveLoadRoot>> kv in objs)
                        if (kv.Value.Count > 0)
                            numx++;
                    wr.Write(numx);
                    HashSet<Tag> tempkeys = new HashSet<Tag>(objs.Keys);
                    Tag tag = SaveGame.Instance.PrefabID();
                    tempkeys.Remove(tag);

                    // IOrderedEnumerable<Tag> tree = from a in tempkeys orderby a.Name.Contains(AsyncAutosaveCS.NameContains) select a;

                    #region firstWrite
                    wr.WriteKleiString(tag.Name);
                    wr.Write(1); // list count
                    wr.WriteLengthWithAction(PrepTWrite(sg.GetComponent<SaveLoadRoot>()).SafeTWrite);
                    #endregion

                    foreach (Tag ok in tempkeys)
                    {
                        List<SaveLoadRoot> so = objs[ok];
                        foreach (SaveLoadRoot root in so)
                            if (root != null && root.GetComponent<SimCellOccupier>() != null)
                            {
                                wr.WriteKleiString(ok.Name);
                                wr.Write(so.Count);
                                wr.WriteLengthWithAction(tmp =>
                                {
                                    foreach (SaveLoadRoot rx in so)
                                        PrepTWrite(rx).SafeTWrite(tmp);
                                });
                                break;
                            }
                    }

                    foreach (Tag ok in tempkeys)
                    {
                        List<SaveLoadRoot> so = objs[ok];
                        foreach (SaveLoadRoot root in so)
                            if (root != null && root.GetComponent<SimCellOccupier>() == null)
                            {
                                wr.WriteKleiString(ok.Name);
                                wr.Write(so.Count);
                                wr.WriteLengthWithAction(tmp =>
                                {
                                    foreach (SaveLoadRoot rx in so)
                                        PrepTWrite(rx).SafeTWrite(tmp);
                                });
                                break;
                            }
                    }
                    #endregion

                    Game.Instance.Save(wr);
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
                        SaveGame.GameInfo info = __instance.GameInfo;
                        ms.WriteInt32(cycle);
                        ms.WriteInt32(Components.LiveMinionIdentities.Count);
                        ms.WriteString(sg.BaseName);
                        ms.WriteBool(false);
                        ms.WriteString(SaveLoader.GetActiveSaveFilePath());
                        ms.WriteInt32(SaveManager.SAVE_MAJOR_VERSION);
                        ms.WriteInt32(SaveManager.SAVE_MINOR_VERSION);
                        ms.WriteString(info.worldID);
                        ms.WriteStringArray(info.worldTraits);
                        ms.WriteBool(sg.sandboxEnabled);

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
            if (updateSavePointer)
                SaveLoader.SetActiveSaveFilePath(filename);
            Game.Instance.timelapser.SaveColonyPreview(filename);
            DebugUtil.LogArgs("Saved to", "[" + filename + "]");
        }

        internal static SaveFileRoot PrepSaveFile()
        {
            SaveFileRoot sfr = new SaveFileRoot();
            sfr.WidthInCells = Grid.WidthInCells;
            sfr.HeightInCells = Grid.HeightInCells;
            using (MemoryStream ms = new MemoryStream())
            using (BinaryWriter br = new BinaryWriter(ms))
            {
                Sim.Save(br);
                sfr.Sim = ms.ToArray();
            }
            sfr.GridVisible = Grid.Visible;
            sfr.GridSpawnable = Grid.Spawnable;
            sfr.GridDamage = ToBytes(Grid.Damage);
            Global.Instance.modManager.SendMetricsEvent();
            sfr.active_mods = new List<KMod.Label>();
            foreach (KMod.Mod mod in Global.Instance.modManager.mods)
                if (mod.enabled)
                    sfr.active_mods.Add(mod.label);
            using (MemoryStream ms2 = new MemoryStream())
            using (BinaryWriter br2 = new BinaryWriter(ms2))
            {

                Camera.main.transform.parent.GetComponent<CameraController>().Save(br2);
                sfr.Camera = ms2.ToArray();
            }
            return sfr;
        }
    }
}
