using Harmony;
using System.Collections.Generic;
using System.Reflection;

namespace AsynchronousAutosave.OtherPatch
{
    [HarmonyPatch(typeof(SaveLoadRoot), "OnPrefabInit")]
    public static class SaveLoadRootONPREFABINIT
    {
        public static void Postfix()
        {
            if (AsyncAutosaveCS.serializableComs == null)
            {
                Debug.Log(AsyncAutosaveCS.MODLOG_HEADER);
                AsyncAutosaveCS.serializableComs = (Dictionary<string, ISerializableComponentManager>)typeof(SaveLoadRoot).
                        GetField("serializableComponentManagers", BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Static).
                        GetValue(null);
            }
        }
    }
}
