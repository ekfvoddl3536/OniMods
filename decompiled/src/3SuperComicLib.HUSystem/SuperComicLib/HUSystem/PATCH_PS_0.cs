using System;
using System.Collections.Generic;
using System.Reflection;
using Harmony;

namespace SuperComicLib.HUSystem
{
	// Token: 0x0200000A RID: 10
	[HarmonyPatch(typeof(PlanScreen), "OnRecipeElementsFullySelected")]
	public class PATCH_PS_0
	{
		// Token: 0x0600000D RID: 13 RVA: 0x000022B8 File Offset: 0x000004B8
		public static bool Prepare()
		{
			Type typeFromHandle = typeof(PlanScreen);
			BindingFlags f = BindingFlags.Instance | BindingFlags.NonPublic;
			PATCH_PS_0.pic = typeFromHandle.GetField("productInfoScreen", f);
			PATCH_PS_0.current = typeFromHandle.GetField("currentlySelectedToggle", f);
			return true;
		}

		// Token: 0x0600000E RID: 14 RVA: 0x000022F4 File Offset: 0x000004F4
		public static bool Prefix(PlanScreen __instance)
		{
			BuildingDef def = null;
			KToggle toggle = (KToggle)PATCH_PS_0.current.GetValue(__instance);
			foreach (KeyValuePair<BuildingDef, KToggle> at in __instance.ActiveToggles)
			{
				if (at.Value == toggle)
				{
					def = at.Key;
					break;
				}
			}
			if (def != null)
			{
				IList<Tag> eles = ((ProductInfoScreen)PATCH_PS_0.pic.GetValue(__instance)).materialSelectionPanel.GetSelectedElementAsList;
				if (def.isKAnimTile && def.isUtility)
				{
					if (def.BuildingComplete.GetComponent<Wire>() != null || def.BuildingComplete.GetComponent<HUPipe>() != null)
					{
						WireBuildTool.Instance.Activate(def, eles);
					}
					else
					{
						UtilityBuildTool.Instance.Activate(def, eles);
					}
				}
				else
				{
					BuildTool.Instance.Activate(def, eles, null);
				}
			}
			return false;
		}

		// Token: 0x04000010 RID: 16
		private static FieldInfo pic;

		// Token: 0x04000011 RID: 17
		private static FieldInfo current;
	}
}
