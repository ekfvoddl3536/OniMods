using System;
using Harmony;
using TemplateClasses;
using UnityEngine;

namespace SuperComicLib.HUSystem
{
	// Token: 0x0200000E RID: 14
	[HarmonyPatch(typeof(TemplateLoader), "PlaceUtilityConnection")]
	public class PATCH4
	{
		// Token: 0x06000018 RID: 24 RVA: 0x00002ADC File Offset: 0x00000CDC
		public static bool Prefix(GameObject spawned, Prefab bc, int root_cell)
		{
			string id = bc.id;
			if (id == null)
			{
				return false;
			}
			int cell = Grid.OffsetCell(root_cell, bc.location_x, bc.location_y);
			UtilityConnections cons = bc.connections;
			if (id != null)
			{
				uint num = <PrivateImplementationDetails>.ComputeStringHash(id);
				if (num <= 1827504487u)
				{
					if (num <= 609727380u)
					{
						if (num != 379600269u)
						{
							if (num != 609727380u)
							{
								goto IL_267;
							}
							if (!(id == "GasConduit"))
							{
								goto IL_267;
							}
						}
						else
						{
							if (!(id == "LiquidConduit"))
							{
								goto IL_267;
							}
							goto IL_1EF;
						}
					}
					else if (num != 848332507u)
					{
						if (num != 1213766155u)
						{
							if (num != 1827504487u)
							{
								goto IL_267;
							}
							if (!(id == "InsulatedWire"))
							{
								goto IL_267;
							}
							goto IL_1AD;
						}
						else
						{
							if (!(id == "TravelTube"))
							{
								goto IL_267;
							}
							spawned.GetComponent<TravelTube>().SetFirstFrameCallback(delegate()
							{
								Game.Instance.travelTubeSystem.SetConnections(cons, cell, true);
								KAnimGraphTileVisualizer compo = spawned.GetComponent<KAnimGraphTileVisualizer>();
								if (compo != null)
								{
									compo.Refresh();
								}
							});
							return false;
						}
					}
					else if (!(id == "InsulatedGasConduit"))
					{
						goto IL_267;
					}
					spawned.GetComponent<Conduit>().SetFirstFrameCallback(delegate()
					{
						Game.Instance.gasConduitSystem.SetConnections(cons, cell, true);
						KAnimGraphTileVisualizer compo = spawned.GetComponent<KAnimGraphTileVisualizer>();
						if (compo != null)
						{
							compo.Refresh();
						}
					});
					return false;
				}
				if (num <= 3228988836u)
				{
					if (num != 1938276536u)
					{
						if (num != 3228988836u)
						{
							goto IL_267;
						}
						if (!(id == "LogicWire"))
						{
							goto IL_267;
						}
						spawned.GetComponent<LogicWire>().SetFirstFrameCallback(delegate()
						{
							Game.Instance.logicCircuitSystem.SetConnections(cons, cell, true);
							KAnimGraphTileVisualizer compo = spawned.GetComponent<KAnimGraphTileVisualizer>();
							if (compo != null)
							{
								compo.Refresh();
							}
						});
						return false;
					}
					else if (!(id == "Wire"))
					{
						goto IL_267;
					}
				}
				else if (num != 3711470516u)
				{
					if (num != 3716494409u)
					{
						if (num != 4113070310u)
						{
							goto IL_267;
						}
						if (!(id == "SolidConduit"))
						{
							goto IL_267;
						}
						spawned.GetComponent<SolidConduit>().SetFirstFrameCallback(delegate()
						{
							Game.Instance.solidConduitSystem.SetConnections(cons, cell, true);
							KAnimGraphTileVisualizer compo = spawned.GetComponent<KAnimGraphTileVisualizer>();
							if (compo != null)
							{
								compo.Refresh();
							}
						});
						return false;
					}
					else if (!(id == "HighWattageWire"))
					{
						goto IL_267;
					}
				}
				else
				{
					if (!(id == "InsulatedLiquidConduit"))
					{
						goto IL_267;
					}
					goto IL_1EF;
				}
				IL_1AD:
				spawned.GetComponent<Wire>().SetFirstFrameCallback(delegate()
				{
					Game.Instance.electricalConduitSystem.SetConnections(cons, cell, true);
					KAnimGraphTileVisualizer compo = spawned.GetComponent<KAnimGraphTileVisualizer>();
					if (compo != null)
					{
						compo.Refresh();
					}
				});
				return false;
				IL_1EF:
				spawned.GetComponent<Conduit>().SetFirstFrameCallback(delegate()
				{
					Game.Instance.liquidConduitSystem.SetConnections(cons, cell, true);
					KAnimGraphTileVisualizer compo = spawned.GetComponent<KAnimGraphTileVisualizer>();
					if (compo != null)
					{
						compo.Refresh();
					}
				});
				return false;
			}
			IL_267:
			spawned.GetComponent<HUPipe>().SetFirstFrameCallback(delegate
			{
				Constants.hupipeSystem.SetConnections(cons, cell, true);
				KAnimGraphTileVisualizer compo = spawned.GetComponent<KAnimGraphTileVisualizer>();
				if (compo != null)
				{
					compo.Refresh();
				}
			});
			return false;
		}
	}
}
