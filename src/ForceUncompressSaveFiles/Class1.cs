using Harmony;
using System.Collections.Generic;
using Klei.AI;
using System.Linq;
using System.IO;
using System;
using UnityEngine;
using System.Runtime.InteropServices;
using SuperComicLib.Threading;

namespace AsynchronousAutosave
{
    [HarmonyPatch(typeof(SaveLoader), nameof(SaveLoader.Save), typeof(string), typeof(bool), typeof(bool))]
    public partial class SAVEPATCH
    {
        public static List<CommonMetricsData> performance;
        public static List<MinionMetricsData> minionDatas;
        public static SaveFileRoot saveFile;

        public static bool Prefix(ref string __result, SaveLoader __instance, string filename, bool isAutoSave, bool updateSavePointer)
        {
            __result = filename;
            if (isAutoSave)
                __instance.StartCoroutine(AwaitibleThreads.Create(() => TestMethod(GameClock.Instance.GetCycle(), Components.LiveMinionIdentities.Count, SaveGame.Instance.BaseName, SaveGame.Instance.sandboxEnabled, SaveLoader.GetActiveSaveFilePath(), __instance.saveManager, __instance.GameInfo, filename, true, updateSavePointer)));
            else
            {
                IEnumerator<System.Action> enumerator = TestMethod(GameClock.Instance.GetCycle(), Components.LiveMinionIdentities.Count, SaveGame.Instance.BaseName, SaveGame.Instance.sandboxEnabled, SaveLoader.GetActiveSaveFilePath(), __instance.saveManager, __instance.GameInfo, filename, false, updateSavePointer);
                while (enumerator.MoveNext()) enumerator.Current();
                // collect
                GC.Collect();
            }
            return false;
        }

        internal static SaveFileRoot PrepSaveFile()
        {
            SaveFileRoot sfr = new SaveFileRoot();
            sfr.WidthInCells = Grid.WidthInCells;
            sfr.HeightInCells = Grid.HeightInCells;
            using (MemoryStream ms = new MemoryStream())
            {
                using (BinaryWriter br = new BinaryWriter(ms))
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
            {
                using (BinaryWriter br2 = new BinaryWriter(ms2))
                    Camera.main.transform.parent.GetComponent<CameraController>().Save(br2);
                sfr.Camera = ms2.ToArray();
            }
            return sfr;
        }

        internal static byte[] ToBytes(float[] floats)
        {
            byte[] num = new byte[floats.Length * 4];
            Buffer.BlockCopy(floats, 0, num, 0, num.Length);
            return num;
        }

        internal static List<CommonMetricsData> GetWorldInventory()
        {
            Dictionary<Tag, float> accessibleAmounts = WorldInventory.Instance.GetAccessibleAmounts();
            List<CommonMetricsData> inventoryMetricsDatas = new List<CommonMetricsData>(accessibleAmounts.Count);
            foreach (KeyValuePair<Tag, float> kv in accessibleAmounts)
            {
                float f = kv.Value;
                if (!float.IsInfinity(f) && !float.IsNaN(f))
                    inventoryMetricsDatas.Add(new CommonMetricsData
                    {
                        Name = kv.Key.ToString(),
                        Value = f
                    });
            }
            return inventoryMetricsDatas;
        }

        internal static List<PrefabMetricsData> GetPrefabMetricsDatas(Dictionary<Tag, List<SaveLoadRoot>> lists)
        {
            List<PrefabMetricsData> pmd = new List<PrefabMetricsData>(lists.Count);
            foreach (KeyValuePair<Tag, List<SaveLoadRoot>> kv in lists)
            { 
                List<SaveLoadRoot> slrl = kv.Value;
                if (slrl.Count > 0)
                    pmd.Add(new PrefabMetricsData
                    {
                        Name = kv.Key.ToString(),
                        Count = slrl.Count
                    });
            }
            return pmd;
        }

        internal static List<MinionMetricsData> GetMinionMetricsDatas()
        {
            List<MinionMetricsData> mmdl = new List<MinionMetricsData>();
            foreach (MinionIdentity mi in Components.LiveMinionIdentities.Items)
                if (mi != null)
                {
                    Amounts amounts = mi.gameObject.GetComponent<Modifiers>().amounts;
                    List<CommonMetricsData> minionAttrFloatDataList = new List<CommonMetricsData>(amounts.Count);
                    foreach (AmountInstance amountInstance in amounts)
                    {
                        float f = amountInstance.value;
                        if (!float.IsNaN(f) && !float.IsInfinity(f))
                            minionAttrFloatDataList.Add(new CommonMetricsData
                            {
                                Name = amountInstance.modifier.Id,
                                Value = amountInstance.value
                            });
                    }
                    MinionResume component = mi.gameObject.GetComponent<MinionResume>();
                    float experienceGained = component.TotalExperienceGained;
                    List<string> stringList = new List<string>();
                    foreach (KeyValuePair<string, bool> keyValuePair in component.MasteryBySkillID)
                    {
                        if (keyValuePair.Value)
                            stringList.Add(keyValuePair.Key);
                    }
                    mmdl.Add(new MinionMetricsData
                    {
                        Name = mi.name,
                        Modifiers = minionAttrFloatDataList,
                        TotalExperienceGained = experienceGained,
                        Skills = stringList
                    });
                }
            return mmdl;
        }

        internal static List<CommonMetricsData> GetPerformanceMeasurements()
        {
            List<CommonMetricsData> pml = new List<CommonMetricsData>();
            if (Global.Instance != null)
            {
                PerformanceMonitor component = Global.Instance.GetComponent<PerformanceMonitor>();
                pml.Add(new CommonMetricsData()
                {
                    Name = "FramesAbove30",
                    Value = component.NumFramesAbove30
                });
                pml.Add(new CommonMetricsData()
                {
                    Name = "FramesBelow30",
                    Value = component.NumFramesBelow30
                });
                component.Reset();
            }
            return pml;
        }

        internal static List<DailyReportMetricsDataSAFE> GetDailyReportMetrics(int cycle)
        {
            List<DailyReportMetricsDataSAFE> dl = new List<DailyReportMetricsDataSAFE>();
            ReportManager.DailyReport report = ReportManager.Instance.FindReport(cycle);
            if (report != null)
            {
                ReportManager.ReportEntry entry = report.reportEntries.LastOrDefault();
                if (entry != null)
                {
                    DailyReportMetricsDataSAFE safe = new DailyReportMetricsDataSAFE();
                    safe.Name = entry.reportType.ToString();
                    if (!float.IsInfinity(entry.Net) && !float.IsNaN(entry.Net))
                        safe.Net = entry.Net;
                    if (!float.IsInfinity(entry.Positive) && !float.IsNaN(entry.Positive))
                        safe.Positive = entry.Positive;
                    if (!float.IsInfinity(entry.Negative) && !float.IsNaN(entry.Negative))
                        safe.Negative = entry.Negative;
                    dl.Add(safe);
                }
                dl.Add(new DailyReportMetricsDataSAFE
                {
                    Name = "MinionCount",
                    Net = Components.LiveMinionIdentities.Count,
                    Positive = 0.0f,
                    Negative = 0.0f
                });
            }
            return dl;
        }
    }

    public struct CommonMetricsData
    {
        public string Name;
        public float Value;
    }

    public struct PrefabMetricsData
    {
        public string Name;
        public int Count;
    }

    public struct MinionMetricsData
    {
        public string Name;
        public List<CommonMetricsData> Modifiers;
        public float TotalExperienceGained;
        public List<string> Skills;
    }

    public struct DailyReportMetricsDataSAFE
    {
        public string Name;
        public float Net;
        public float Positive;
        public float Negative;
    }

    [Serializable]
    [StructLayout(LayoutKind.Sequential)]
    public sealed class SaveFileRoot
    {
        public int WidthInCells;
        public int HeightInCells;
        public byte[] Sim, GridVisible, GridSpawnable, GridDamage, Camera;
        public string worldID;
        public List<KMod.Label> active_mods;
    }
}
