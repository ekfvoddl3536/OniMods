using Harmony;

namespace AsynchronousAutosave.OtherPatch
{
    [HarmonyPatch(typeof(FMODUnity.RuntimeManager), "OnApplicationPause")]
    public class PauseGamePatch
    {
        public static void Prefix(bool pauseStatus)
        {
            if (SAVEPATCH.autosaving && pauseStatus)
            {
                Game.Instance.StopCoroutine(SAVEPATCH.asyncMethod);
                Debug.Log(AsyncAutosaveCS.MODLOG_HEADER);
                Debug.Log("Waiting for autosave to complete...");
                while (SAVEPATCH.asyncMethod.MoveNext()) ;
            }
        }
    }

    [HarmonyPatch(typeof(Global), "OnApplicationFocus")]
    public class FocusGamePatch
    {
        public static void Prefix(bool focus)
        {
            if (SAVEPATCH.autosaving && !focus)
            {
                Game.Instance.StopCoroutine(SAVEPATCH.asyncMethod);
                Debug.Log(AsyncAutosaveCS.MODLOG_HEADER);
                Debug.Log("Waiting for autosave to complete...");
                while (SAVEPATCH.asyncMethod.MoveNext()) ;
            }
        }
    }
}
