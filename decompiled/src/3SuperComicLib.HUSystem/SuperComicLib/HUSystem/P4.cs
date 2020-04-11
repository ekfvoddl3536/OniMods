using System;
using System.Reflection;
using Harmony;
using UnityEngine;

namespace SuperComicLib.HUSystem
{
	// Token: 0x0200001C RID: 28
	[HarmonyPatch(typeof(BuildingCellVisualizer), "DrawIcons")]
	public class P4
	{
		// Token: 0x0600007F RID: 127 RVA: 0x000049DC File Offset: 0x00002BDC
		public static bool Prepare()
		{
			Type typeFromHandle = typeof(BuildingCellVisualizer);
			BindingFlags f = BindingFlags.Instance | BindingFlags.NonPublic;
			P4.oi = typeFromHandle.GetField("outputVisualizer", f);
			P4.ii = typeFromHandle.GetField("inputVisualizer", f);
			P4.building = typeFromHandle.GetField("building", f);
			P4.draw = typeFromHandle.GetMethod("DrawUtilityIcon", f, null, new Type[]
			{
				typeof(int),
				typeof(Sprite),
				typeof(GameObject).MakeByRefType(),
				typeof(Color)
			}, null);
			return true;
		}

		// Token: 0x06000080 RID: 128 RVA: 0x00004A7C File Offset: 0x00002C7C
		public static bool Prefix(HashedString mode, BuildingCellVisualizer __instance)
		{
			if (mode == "HUPipe")
			{
				Building bd = (Building)P4.building.GetValue(__instance);
				HUBuildingDef def = bd.Def as HUBuildingDef;
				if (def != null)
				{
					P4.Handler func = (P4.Handler)Delegate.CreateDelegate(typeof(P4.Handler), __instance, P4.draw);
					BuildingCellVisualizerResources resource = BuildingCellVisualizerResources.Instance();
					if ((def.hutypes & HUPipeTypes.InputOnly) != HUPipeTypes.None)
					{
						GameObject op = null;
						func(bd.GetRotatedOffsetCell(def.huInputOffset), resource.liquidInputIcon, ref op, resource.liquidIOColours.input.connected);
						P4.ii.SetValue(__instance, op);
					}
					if ((def.hutypes & HUPipeTypes.OutputOnly) != HUPipeTypes.None)
					{
						GameObject op2 = null;
						func(bd.GetRotatedOffsetCell(def.huOutputOffset), resource.liquidOutputIcon, ref op2, resource.liquidIOColours.output.connected);
						P4.oi.SetValue(__instance, op2);
					}
				}
				return false;
			}
			return true;
		}

		// Token: 0x04000054 RID: 84
		private static FieldInfo oi;

		// Token: 0x04000055 RID: 85
		private static FieldInfo ii;

		// Token: 0x04000056 RID: 86
		private static FieldInfo building;

		// Token: 0x04000057 RID: 87
		private static MethodInfo draw;

		// Token: 0x0200002C RID: 44
		// (Invoke) Token: 0x060000A7 RID: 167
		private delegate void Handler(int cell, Sprite icon_img, ref GameObject visualizerObj, Color tint);
	}
}
