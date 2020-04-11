using System;
using System.Collections.Generic;
using System.Reflection;
using Harmony;

namespace SuperComicLib.HUSystem
{
	// Token: 0x0200000B RID: 11
	[HarmonyPatch(typeof(BuildMenu), "OnRecipeElementsFullySelected")]
	public class PATCH_BM_0
	{
		// Token: 0x06000010 RID: 16 RVA: 0x000023FC File Offset: 0x000005FC
		public static bool Prepare()
		{
			Type typeFromHandle = typeof(BuildMenu);
			BindingFlags f = BindingFlags.Instance | BindingFlags.NonPublic;
			PATCH_BM_0.sb = typeFromHandle.GetField("selectedBuilding", f);
			PATCH_BM_0.pic = typeFromHandle.GetField("productInfoScreen", f);
			return true;
		}

		// Token: 0x06000011 RID: 17 RVA: 0x00002438 File Offset: 0x00000638
		public static bool Prefix(BuildMenu __instance)
		{
			BuildingDef selDef = PATCH_BM_0.sb.GetValue(__instance) as BuildingDef;
			if (selDef != null)
			{
				IList<Tag> eles = ((ProductInfoScreen)PATCH_BM_0.pic.GetValue(__instance)).materialSelectionPanel.GetSelectedElementAsList;
				if (selDef.isKAnimTile && selDef.isUtility)
				{
					if (selDef.BuildingComplete.GetComponent<Wire>() != null || selDef.BuildingComplete.GetComponent<HUPipe>() != null)
					{
						WireBuildTool.Instance.Activate(selDef, eles);
					}
					else
					{
						UtilityBuildTool.Instance.Activate(selDef, eles);
					}
				}
				else
				{
					BuildTool.Instance.Activate(selDef, eles, null);
				}
			}
			else
			{
				Debug.Log("No def!");
			}
			return false;
		}

		// Token: 0x04000012 RID: 18
		private static FieldInfo sb;

		// Token: 0x04000013 RID: 19
		private static FieldInfo pic;
	}
}
