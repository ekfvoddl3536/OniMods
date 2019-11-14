using System.Collections.Generic;
using Klei;
using System.IO;
using System;
using KSerialization;
using UnityEngine;
using System.Text;

namespace AsynchronousAutosave
{
    partial class SAVEPATCH
    {
        private static IEnumerator<System.Action> TestMethod(int cycle, int lives, string bn, bool sandbox, string actpath, SaveManager manager, SaveGame.GameInfo info, string filename, bool autosave, bool updatepointer)
        {
            // Thread.Sleep(5000);
            Manager.Clear();
            if (ThreadedHttps<KleiMetrics>.Instance != null && ThreadedHttps<KleiMetrics>.Instance.enabled)
            {
                Dictionary<string, object> ed = new Dictionary<string, object>();
                ed[GameClock.NewCycleKey] = cycle + 1;
                ed["WasDebugEverUsed"] = Game.Instance.debugWasUsed;
                ed["IsAutoSave"] = autosave;
                ed["SavedPrefabs"] = GetPrefabMetricsDatas(manager.GetLists());
                ed["ResourcesAccessible"] = GetWorldInventory();
                yield return () => ed["MinionMetrics"] = GetMinionMetricsDatas();
                if (autosave)
                {
                    ed["DailyReport"] = GetDailyReportMetrics(cycle);
                    yield return () => ed["PerformanceMeasurements"] = GetPerformanceMeasurements();
                    ed["AverageFrameTime"] = 30f; // why write this.
                }
                ed["CustomGameSettings"] = CustomGameSettings.Instance.GetSettingsForMetrics();
                ThreadedHttps<KleiMetrics>.Instance.SendEvent(ed);
                ed.Clear();
            }
            yield return () => RetireColonyUtility.SaveColonySummaryData();
            if (autosave && !GenericGameSettings.instance.keepAllAutosaves)
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
            byte[] buffer = null;
            using (MemoryStream ms = new MemoryStream())
            {
                ms.WriteString("world", Encoding.UTF8);
                //
                // serialize
                //
                SaveFileRoot sfr = new SaveFileRoot();
                sfr.WidthInCells = Grid.WidthInCells;
                sfr.HeightInCells = Grid.HeightInCells;
                using (MemoryStream opm = new MemoryStream())
                {
                    using (BinaryWriter br = new BinaryWriter(opm))
                        Sim.Save(br);
                    sfr.Sim = opm.ToArray();
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
                {
                    using (BinaryWriter br2 = new BinaryWriter(ms2))
                        yield return () => Camera.main.transform.parent.GetComponent<CameraController>().Save(br2);
                    sfr.Camera = ms2.ToArray();
                }
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
                    yield return () =>
                    {
                        manager.Save(wr);
                        Game.Instance.Save(wr);
                    };
                    buffer = ms.ToArray();
                }
            }
            try
            {
                using (BinaryWriter wr2 = new BinaryWriter(File.Open(filename, FileMode.Create)))
                {
                    using (MemoryStream mx = new MemoryStream())
                    {
                        mx.WriteInt32(cycle);
                        mx.WriteInt32(lives);
                        mx.WriteString(bn);
                        mx.WriteBoolean(autosave);
                        mx.WriteString(actpath);
                        mx.WriteInt32(SaveManager.SAVE_MAJOR_VERSION);
                        mx.WriteInt32(SaveManager.SAVE_MINOR_VERSION);
                        mx.WriteString(info.worldID);
                        mx.WriteStringArray(info.worldTraits);
                        mx.WriteBoolean(sandbox);

#if boost
                        byte[] temp = mx.ToArray();
                        wr2.Write(KleiVersion.ChangeList); // buildversion
                        wr2.Write(temp.Length); // headersize
                        wr2.Write(1U); // headerversion
                        wr2.Write(0); // compression
                        wr2.Write(temp);
#else
                        wr2.Write(KleiVersion.ChangeList);
                        wr2.Write(mx.ToArray());
#endif
                    }
                    Manager.SerializeDirectory(wr2);
                    wr2.Write(buffer);
                    // Stats.Print();
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
                yield break;
            }
            if (updatepointer)
                SaveLoader.SetActiveSaveFilePath(filename);
            // Game.Instance.timelapser.SaveColonyPreview(filename);
            DebugUtil.LogArgs("Saved to", "[" + filename + "]");
            // GC.Collect(0, GCCollectionMode.Optimized);
            yield break;
        }
    }
}
