using System;
using System.Reflection;
using Harmony;
using UnityEngine;

namespace SuperComicLib.HUSystem
{
	// Token: 0x0200000F RID: 15
	[HarmonyPatch(typeof(OverlayScreen), "OnSpawn")]
	public class OVERLAYSCREEN_PATCH_HSG
	{
		// Token: 0x0600001A RID: 26 RVA: 0x00002D78 File Offset: 0x00000F78
		public static bool Prepare()
		{
			Type t = typeof(OverlayScreen);
			BindingFlags x = BindingFlags.Instance | BindingFlags.NonPublic;
			OVERLAYSCREEN_PATCH_HSG.func = t.GetMethod("RegisterMode", x, null, new Type[]
			{
				typeof(OverlayModes.Mode)
			}, null);
			OVERLAYSCREEN_PATCH_HSG.parent = t.GetField("powerLabelParent", x);
			OVERLAYSCREEN_PATCH_HSG.prefab = t.GetField("powerLabelPrefab", x);
			OVERLAYSCREEN_PATCH_HSG.conColour = t.GetField("consumerColour", x);
			OVERLAYSCREEN_PATCH_HSG.genColour = t.GetField("generatorColour", x);
			return true;
		}

		// Token: 0x0600001B RID: 27 RVA: 0x00002E00 File Offset: 0x00001000
		public static void Postfix(OverlayScreen __instance)
		{
			OVERLAYSCREEN_PATCH_HSG.func.Invoke(__instance, new object[]
			{
				new HUPipeOverlay((Canvas)OVERLAYSCREEN_PATCH_HSG.parent.GetValue(__instance), (LocText)OVERLAYSCREEN_PATCH_HSG.prefab.GetValue(__instance), (Color)OVERLAYSCREEN_PATCH_HSG.conColour.GetValue(__instance), (Color)OVERLAYSCREEN_PATCH_HSG.genColour.GetValue(__instance))
			});
		}

		// Token: 0x04000019 RID: 25
		private static MethodInfo func;

		// Token: 0x0400001A RID: 26
		private static FieldInfo parent;

		// Token: 0x0400001B RID: 27
		private static FieldInfo prefab;

		// Token: 0x0400001C RID: 28
		private static FieldInfo conColour;

		// Token: 0x0400001D RID: 29
		private static FieldInfo genColour;
	}
}
