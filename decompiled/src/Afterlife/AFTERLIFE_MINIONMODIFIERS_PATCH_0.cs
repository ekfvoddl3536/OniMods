using System;
using Harmony;
using Klei.AI;

namespace Afterlife
{
	// Token: 0x02000004 RID: 4
	[HarmonyPatch(typeof(MinionModifiers), "OnSpawn")]
	public static class AFTERLIFE_MINIONMODIFIERS_PATCH_0
	{
		// Token: 0x06000003 RID: 3 RVA: 0x00002140 File Offset: 0x00000340
		public static void Postfix(MinionModifiers __instance)
		{
			Amounts amounts = __instance.amounts;
			AmountInstance ai = (amounts != null) ? amounts.Get("Calories") : null;
			if (ai == null)
			{
				Debug.LogWarning("AmountInstance is null: " + KSelectableExtensions.GetProperName(__instance));
				return;
			}
			AFTERLIFE_MINIONMODIFIERS_PATCH_0.SafeAdd(ref Constants.full, ref ai.maxAttribute.Modifiers);
			AFTERLIFE_MINIONMODIFIERS_PATCH_0.SafeAdd(ref Constants.delta, ref ai.deltaAttribute.Modifiers);
		}

		// Token: 0x06000004 RID: 4 RVA: 0x000021A8 File Offset: 0x000003A8
		private static void SafeAdd(ref AttributeModifier mod, ref ArrayRef<AttributeModifier> array)
		{
			string temp = mod.Description;
			if (array.FindIndex((AttributeModifier m) => m.Description == temp) == -1)
			{
				array.Add(mod);
			}
		}
	}
}
