using System.Collections;
using Harmony;

namespace SafeSpace
{
    [HarmonyPatch(typeof(SeasonManager))]
    [HarmonyPatch("OnSpawn")]
    public class CRT
    {
        public const string SAFE = "Default";
        public const string DANGER = "MeteorShower";

        public static void Postfix(SeasonManager __instance)
        {
            Traverse t = Traverse.Create(__instance);

            string[] vs = t.Field<string[]>("SeasonLoop").Value;
            IDictionary vs2 = t.Field<IDictionary>("seasons").Value;
            for (int x = 0, max = vs.Length; x < max; x++)
                vs[x] = SAFE;

            vs2.Remove(DANGER);
        }
    }

    [HarmonyPatch(typeof(SeasonManager))]
    [HarmonyPatch("OnNewDay")]
    [HarmonyPatch(new[] { typeof(object) })]
    public class CRT2
    {
        public static bool Prefix(object data) => false;
    }

    [HarmonyPatch(typeof(SeasonManager))]
    [HarmonyPatch("UpdateState")]
    public class CRT3
    {
        public static bool Prefix() => false;
    }

    [HarmonyPatch(typeof(SeasonManager))]
    [HarmonyPatch("Sim200ms")]
    [HarmonyPatch(new[] { typeof(float) })]
    public class CRT6
    {
        public static bool Prefix() => false;
    }

    [HarmonyPatch(typeof(SeasonManager), "DoBombardment")]
    public class DoBombardment_PATCH_01
    {
        public static bool Prefix() => false;
    }
}
