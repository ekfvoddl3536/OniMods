using Harmony;
using Klei.AI;
using SuperComicLib.Threading;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace AsynchronousAutosave
{
    [HarmonyPatch(typeof(SaveLoader), nameof(SaveLoader.Save), typeof(string), typeof(bool), typeof(bool))]
    public partial class SAVEPATCH
    {
        public static IEnumerator asyncMethod;

        public static bool Prefix(ref string __result, SaveLoader __instance, string filename, bool isAutoSave, bool updateSavePointer)
        {
            SaveGame sg = SaveGame.Instance;
            if (sg.TimelapseResolution.x > 0)
                sg.TimelapseResolution = new Vector2I();
            __result = filename;
            if (autosaving)
                Debug.LogWarning("Autosave is already in progress.");
            else if (isAutoSave)
            {
                Debug.Log(AsyncAutosaveCS.MODLOG_HEADER);
                asyncMethod = AwaitibleThreads.TaskRun(tempinst => TestMethod(tempinst, GameClock.Instance.GetCycle(), Components.LiveMinionIdentities.Count, sg.BaseName, sg.sandboxEnabled, SaveLoader.GetActiveSaveFilePath(), __instance.saveManager, __instance.GameInfo, filename, updateSavePointer));
                Game.Instance.StartCoroutine(asyncMethod);
            }
            else
            {
                Debug.Log(AsyncAutosaveCS.MODTHD_HEADER);
                OriginalCode(__instance, filename, updateSavePointer);
                GC.Collect();
            }
            return false;
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
            };
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
}
