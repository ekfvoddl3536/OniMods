using System;
using System.Collections.Generic;
using Database;
using KSerialization;
using Harmony;

namespace NewWirelessAutomatic
{
    using static NWA_CONST;
    using static WirelessMgr;
    [HarmonyPatch(typeof(Game), "OnPrefabInit")]
    public class GameOnPrefab_Patch_01
    {
        public static void Prefix()
        {
            if (NetSystem == null)
                NetSystem = new NetworkManager.ChannelNetworkMgrBase();
        }

        public static void Postfix() => NetSystem.Reset();
    }

    [HarmonyPatch(typeof(GeneratedBuildings), nameof(GeneratedBuildings.LoadGeneratedBuildings))]
    public class GeneratedBuildings_LOAD_PATCH
    {
        public static void Prefix()
        {
            Stpre(Emitter.ID_UPPER, Emitter.NAME, Emitter.DESC, Emitter.EFFC);
            Stpre(Receiver.ID_UPPER, Receiver.NAME, Receiver.DESC, Receiver.EFFC);

            Strings.Add(ToolTipKey, ToolTip);
            Strings.Add(TitleKey, Title);

            ModUtil.AddBuildingToPlanScreen("Automation", Emitter.ID);
            ModUtil.AddBuildingToPlanScreen("Automation", Receiver.ID);

            if (NetSystem == null)
                NetSystem = new NetworkManager.ChannelNetworkMgrBase();
        }

        private static void Stpre(string idupper, string name, string desc, string effc)
        {
            Strings.Add($"{PREFAB}{idupper}.NAME", name);
            Strings.Add($"{PREFAB}{idupper}.DESC", desc);
            Strings.Add($"{PREFAB}{idupper}.EFFECT", effc);
        }
    }

    [HarmonyPatch(typeof(Db), "Initialize")]
    public class DB_PATCH_01
    {
        public static void Prefix()
        {
            List<string> tech = new List<string>(Techs.TECH_GROUPING["DupeTrafficControl"]) { Emitter.ID, Receiver.ID };
            Techs.TECH_GROUPING["DupeTrafficControl"] = tech.ToArray();
        }
    }

    [HarmonyPatch(typeof(Manager), "GetType", new[] { typeof(string) })]
    public class MGR_GETTYPE_PATCH_01
    {
        public static void Postfix(string type_name, ref Type __result)
        {
            if (type_name == typeof(SignalEmitter).AssemblyQualifiedName)
                __result = typeof(SignalEmitter);
            else if (type_name == typeof(SignalReceiver).AssemblyQualifiedName)
                __result = typeof(SignalReceiver);
        }
    }
    
    [HarmonyPatch(typeof(IntSliderSideScreen), "OnSpawn")]
    public class INTSS_ONSPAWN_PATCH_01
    {
        public static void Postfix(IntSliderSideScreen __instance)
        {
            foreach (SliderSet x in __instance.sliderSets)
                x.numberInput.field.characterLimit = 7;
        }
    }
}
