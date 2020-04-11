using System;
using System.Collections.Generic;
using Harmony;
using TUNING;

namespace EasyFarming
{
	// Token: 0x02000004 RID: 4
	[HarmonyPatch(typeof(GeneratedBuildings), "LoadGeneratedBuildings")]
	public static class MainPatch
	{
		// Token: 0x06000003 RID: 3 RVA: 0x000020AC File Offset: 0x000002AC
		public static bool Prepare()
		{
			List<Crop.CropVal> list = CROPS.CROP_TYPES;
			Crop.CropVal[] cps = list.ToArray();
			int x = 0;
			int max = list.Count;
			while (x < max)
			{
				ref Crop.CropVal i = ref cps[x];
				if (i.cropId == "ColdWheatSeed")
				{
					i.cropDuration = 4500f;
				}
				else if (i.cropId == "Lettuce")
				{
					i.cropDuration = 4500f;
				}
				else if (i.cropId == "BeanPlantSeed")
				{
					i.cropDuration = 4800f;
				}
				x++;
			}
			list.Clear();
			list.AddRange(cps);
			return true;
		}

		// Token: 0x06000004 RID: 4 RVA: 0x00002151 File Offset: 0x00000351
		public static void Prefix()
		{
			MainPatch.SetString(ConstsEF.WildFarmTile.ID_UPPER, ConstsEF.WildFarmTile.NAME, ConstsEF.WildFarmTile.DESC, ConstsEF.WildFarmTile.EFFC);
			ModUtil.AddBuildingToPlanScreen("Food", "WildFarmTile");
		}

		// Token: 0x06000005 RID: 5 RVA: 0x00002190 File Offset: 0x00000390
		private static void SetString(string idup, string name, string desc, string effect)
		{
			string temp = "STRINGS.BUILDINGS.PREFABS." + idup;
			Strings.Add(new string[]
			{
				temp + ".NAME",
				name
			});
			Strings.Add(new string[]
			{
				temp + ".DESC",
				desc
			});
			Strings.Add(new string[]
			{
				temp + ".EFFECT",
				effect
			});
		}
	}
}
