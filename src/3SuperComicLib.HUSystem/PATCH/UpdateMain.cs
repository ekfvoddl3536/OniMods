using Harmony;

namespace SuperComicLib.HUSystem
{
    using static Constants;
    [HarmonyPatch(typeof(CircuitManager), nameof(CircuitManager.Sim200msFirst))]
    public class UPDATEMAIN_00
    {
        public static void Postfix(float dt) => manager.Refresh(dt);
    }

    [HarmonyPatch(typeof(CircuitManager), nameof(CircuitManager.Sim200msLast))]
    public class UPDATEMAIN_LAST
    {
        public static void Postfix(float dt) => manager.Sim200msLast(dt);
    }

    [HarmonyPatch(typeof(EnergySim), nameof(EnergySim.EnergySim200ms))]
    public class UPDATE_SIM_200ms
    {
        public static void Postfix(float dt) => HUSimUpdater.Update(dt);
    }
}
