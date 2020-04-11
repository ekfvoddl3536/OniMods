using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using Harmony;
using STRINGS;
using UnityEngine;

namespace SuperComicLib.HUSystem
{
	// Token: 0x0200000C RID: 12
	[HarmonyPatch(typeof(BaseUtilityBuildTool), "BuildPath")]
	public class BUBT_HARD_PATCH
	{
		// Token: 0x06000013 RID: 19 RVA: 0x000024E8 File Offset: 0x000006E8
		public static bool Prepare()
		{
			Type typeFromHandle = typeof(BaseUtilityBuildTool);
			BindingFlags f = BindingFlags.Instance | BindingFlags.NonPublic;
			BUBT_HARD_PATCH.apply = typeFromHandle.GetMethod("ApplyPathToConduitSystem", f);
			BUBT_HARD_PATCH.path = typeFromHandle.GetField("path", f);
			BUBT_HARD_PATCH.mgr = typeFromHandle.GetField("conduitMgr", f);
			BUBT_HARD_PATCH.def = typeFromHandle.GetField("def", f);
			BUBT_HARD_PATCH.selElem = typeFromHandle.GetField("selectedElements", f);
			return true;
		}

		// Token: 0x06000014 RID: 20 RVA: 0x00002558 File Offset: 0x00000758
		public static bool Prefix(BaseUtilityBuildTool __instance)
		{
			BUBT_HARD_PATCH.apply.Invoke(__instance, null);
			int cc = 0;
			IEnumerable enumerable = (IList)BUBT_HARD_PATCH.path.GetValue(__instance);
			IUtilityNetworkMgr imgr = (IUtilityNetworkMgr)BUBT_HARD_PATCH.mgr.GetValue(__instance);
			BuildingDef bdef = (BuildingDef)BUBT_HARD_PATCH.def.GetValue(__instance);
			IList<Tag> tags = (IList<Tag>)BUBT_HARD_PATCH.selElem.GetValue(__instance);
			bool sandboxInstant = Game.Instance.SandboxModeActive && SandboxToolParameterMenu.instance.settings.InstantBuild;
			bool debugInstant = DebugHandler.InstantBuildMode || sandboxInstant;
			foreach (object o in enumerable)
			{
				int pnode = BUBT_HARD_PATCH.GetCell(o);
				Vector3 poscbc = Grid.CellToPosCBC(pnode, 19);
				GameObject go = Grid.Objects[pnode, bdef.TileLayer];
				UtilityConnections uc2;
				if (go == null)
				{
					uc2 = imgr.GetConnections(pnode, false);
					string text;
					if (debugInstant && bdef.IsValidBuildLocation(__instance.visualizer, poscbc, 0) && bdef.IsValidPlaceLocation(__instance.visualizer, poscbc, 0, ref text))
					{
						go = bdef.Build(pnode, 0, null, tags, 293.15f, true, GameClock.Instance.GetTime());
					}
					else
					{
						go = bdef.TryPlace(null, poscbc, 0, tags, 0);
						if (go != null)
						{
							if (!bdef.MaterialsAvailable(tags) && !DebugHandler.InstantBuildMode)
							{
								PopFXManager.Instance.SpawnFX(PopFXManager.Instance.sprite_Resource, UI.TOOLTIPS.NOMATERIAL, null, poscbc, 1.5f, false, false);
							}
							float delay = 0.1f * (float)cc;
							Constructable com = go.GetComponent<Constructable>();
							if (com.IconConnectionAnimation(delay, cc, "Wire", "OutletConnected_release") || com.IconConnectionAnimation(delay, cc, "Pipe", "OutletConnected_release"))
							{
								cc++;
							}
							Prioritizable com2 = go.GetComponent<Prioritizable>();
							if (com2 != null)
							{
								if (BuildMenu.Instance != null)
								{
									com2.SetMasterPriority(BuildMenu.Instance.GetBuildingPriority());
								}
								if (PlanScreen.Instance != null)
								{
									com2.SetMasterPriority(PlanScreen.Instance.GetBuildingPriority());
								}
							}
						}
					}
				}
				else
				{
					uc2 = imgr.GetConnections(pnode, false);
					KAnimGraphTileVisualizer com3 = go.GetComponent<KAnimGraphTileVisualizer>();
					if (com3 != null)
					{
						uc2 |= com3.Connections;
						if (go.GetComponent<BuildingComplete>() != null)
						{
							com3.UpdateConnections(uc2);
						}
					}
				}
				if (bdef.ReplacementLayer != 40 && !debugInstant && bdef.IsValidBuildLocation(null, poscbc, 0) && Grid.Objects[pnode, bdef.ReplacementLayer] == null)
				{
					GameObject gameObject = Grid.Objects[pnode, bdef.TileLayer];
					BuildingComplete complete = (gameObject != null) ? gameObject.GetComponent<BuildingComplete>() : null;
					if (complete != null && complete.Def != bdef)
					{
						Constructable component = bdef.BuildingUnderConstruction.GetComponent<Constructable>();
						component.IsReplacementTile = true;
						go = bdef.Instantiate(poscbc, 0, tags, 0);
						component.IsReplacementTile = false;
						if (!bdef.MaterialsAvailable(tags) && !DebugHandler.InstantBuildMode)
						{
							PopFXManager.Instance.SpawnFX(PopFXManager.Instance.sprite_Resource, UI.TOOLTIPS.NOMATERIAL, null, poscbc, 1.5f, false, false);
						}
						Grid.Objects[pnode, bdef.ReplacementLayer] = go;
						KAnimGraphTileVisualizer com4 = go.GetComponent<KAnimGraphTileVisualizer>();
						if (com4 != null)
						{
							uc2 = com4.Connections;
							if (go.GetComponent<BuildingComplete>() != null)
							{
								com4.UpdateConnections(uc2);
							}
						}
						uc2 |= imgr.GetConnections(pnode, false);
						string vs = imgr.GetVisualizerString(uc2);
						string anim = vs + "_place";
						KBatchedAnimController component2 = go.GetComponent<KBatchedAnimController>();
						component2.Play(component2.HasAnimation(anim) ? anim : vs, 1, 1f, 0f);
					}
				}
				if (go != null)
				{
					KAnimGraphTileVisualizer last = go.GetComponent<KAnimGraphTileVisualizer>();
					if (last != null)
					{
						last.Connections = uc2;
					}
				}
				TileVisualizer.RefreshCell(pnode, bdef.TileLayer, bdef.ReplacementLayer);
			}
			ResourceRemainingDisplayScreen.instance.SetNumberOfPendingConstructions(0);
			return false;
		}

		// Token: 0x06000015 RID: 21 RVA: 0x00002988 File Offset: 0x00000B88
		public static int GetCell(object o)
		{
			return (int)o.GetType().GetField("cell").GetValue(o);
		}

		// Token: 0x04000014 RID: 20
		private static MethodInfo apply;

		// Token: 0x04000015 RID: 21
		private static FieldInfo path;

		// Token: 0x04000016 RID: 22
		private static FieldInfo mgr;

		// Token: 0x04000017 RID: 23
		private static FieldInfo def;

		// Token: 0x04000018 RID: 24
		private static FieldInfo selElem;
	}
}
