using Harmony;

namespace AsynchronousAutosave.OtherPatch
{
    [HarmonyPatch(typeof(LoadScreen), nameof(LoadScreen.ForceStopGame))]
    public static class StopGamePatch
    {
        public static void Prefix()
        {
            if (SAVEPATCH.autosaving)
            {
                Debug.Log(AsyncAutosaveCS.MODLOG_HEADER);
                Debug.Log("Waiting for autosave to complete...");
                while (SAVEPATCH.asyncMethod.MoveNext()) ;
            }
        }
    }

    [HarmonyPatch(typeof(Game), "OnApplicationQuit")]
    public static class OnApplicationQuitGAME_PATCH
    {
        public static void Prefix()
        {
            if (SAVEPATCH.autosaving)
            {
                Debug.Log(AsyncAutosaveCS.MODLOG_HEADER);
                Debug.Log("Waiting for autosave to complete...");
                while (SAVEPATCH.asyncMethod.MoveNext()) ;
            }
        }
    }
}
