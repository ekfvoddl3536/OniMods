using System.Collections.Generic;

namespace AsynchronousAutosave
{
    public struct CommonMetricsData
    {
        public string Name;
        public float Value;

        public CommonMetricsData(string v1)
        {
            Name = v1;
            Value = 1.0f;
        }
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

    public sealed class SaveFileRoot
    {
        public int WidthInCells;
        public int HeightInCells;
        public byte[] Sim, GridVisible, GridSpawnable, GridDamage, Camera;
        public string worldID;
        // public List<ModInfo> requiredMods;
        public List<KMod.Label> active_mods;
    }
}
