using Database;
using Harmony;
using System.Collections.Generic;

namespace AdvancedGeneratos
{
    using static Constans;
    [HarmonyPatch(typeof(GeneratedBuildings))]
    [HarmonyPatch(nameof(GeneratedBuildings.LoadGeneratedBuildings))]
    public sealed class GeneratedBuildings_LoadGeneratedBuildings_Patch
    {
        public static void Prefix()
        {
            SetString(RefinedCarbonGenerator.ID_UPPER, RefinedCarbonGenerator.NAME, RefinedCarbonGenerator.DESC, RefinedCarbonGenerator.EFFC);
            SetString(ThermoelectricGenerator.ID_UPPER, ThermoelectricGenerator.NAME, ThermoelectricGenerator.DESC, ThermoelectricGenerator.EFFC);
            SetString(NaphthaGenerator.ID_UPPER, NaphthaGenerator.NAME, NaphthaGenerator.DESC, NaphthaGenerator.EFFC);
            SetString(EcoFriendlyMethaneGenerator.ID_UPPER, EcoFriendlyMethaneGenerator.NAME, EcoFriendlyMethaneGenerator.DESC, EcoFriendlyMethaneGenerator.EFFC);

            ModUtil.AddBuildingToPlanScreen(TabCategory, RefinedCarbonGenerator.ID);
            ModUtil.AddBuildingToPlanScreen(TabCategory, ThermoelectricGenerator.ID);
            ModUtil.AddBuildingToPlanScreen(TabCategory, NaphthaGenerator.ID);
            ModUtil.AddBuildingToPlanScreen(TabCategory, EcoFriendlyMethaneGenerator.ID);
        }

        private static void SetString(string path, string name, string desc, string eff)
        {
            Strings.Add($"{Kpath}{path}.NAME", name);
            Strings.Add($"{Kpath}{path}.DESC", desc);
            Strings.Add($"{Kpath}{path}.EFFECT", eff);
        }
    }

    [HarmonyPatch(typeof(Db))]
    [HarmonyPatch(nameof(Db.Initialize))]
    public static class Db_Initialize_Patch
    {
        public static void Prefix()
        {
            Add("AdvancedPowerRegulation", RefinedCarbonGenerator.ID);
            Add("Plastics", NaphthaGenerator.ID);
            AddRange("RenewableEnergy", ThermoelectricGenerator.ID, EcoFriendlyMethaneGenerator.ID);
        }

        private static void Add(string group, string id)
        {
            List<string> tech = new List<string>(Techs.TECH_GROUPING[group]) { id };
            Techs.TECH_GROUPING[group] = tech.ToArray();
        }

        private static void AddRange(string g, params string[] ids)
        {
            List<string> tech = new List<string>(Techs.TECH_GROUPING[g]);
            tech.AddRange(ids);
            Techs.TECH_GROUPING[g] = tech.ToArray();
        }
    }
}
