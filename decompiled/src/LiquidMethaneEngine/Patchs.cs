using System;
using Harmony;

namespace LiquidMethaneEngine
{
	// Token: 0x02000004 RID: 4
	[HarmonyPatch(typeof(GeneratedBuildings))]
	[HarmonyPatch("LoadGeneratedBuildings")]
	internal class Patchs
	{
		// Token: 0x06000007 RID: 7 RVA: 0x000022A0 File Offset: 0x000004A0
		public static void Prefix()
		{
			string temp = "STRINGS.BUILDINGS.PREFABS." + Consts.ID_UPPER;
			Strings.Add(new string[]
			{
				temp + ".NAME",
				Consts.Displayname
			});
			Strings.Add(new string[]
			{
				temp + ".DESC",
				Consts.Descript
			});
			Strings.Add(new string[]
			{
				temp + ".EFFECT",
				Consts.Effect
			});
			ModUtil.AddBuildingToPlanScreen("Rocketry", "LiquidMethaneEngine");
		}
	}
}
