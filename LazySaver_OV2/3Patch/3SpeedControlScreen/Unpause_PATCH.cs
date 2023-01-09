using HarmonyLib;

namespace LazySaver
{
    using static SharedData;
    [HarmonyPatch(typeof(SpeedControlScreen), nameof(SpeedControlScreen.Unpause))]
    public static class Unpause_PATCH
    {
        public static bool Prefix() => m_previousTask == null || m_previousTask.IsCompleted;
    }
}
