using System;
using Harmony;

namespace SINT_SINT_Is_Not_Toilet
{
	// Token: 0x02000004 RID: 4
	[HarmonyPatch(typeof(GeneratedBuildings))]
	[HarmonyPatch("LoadGeneratedBuildings")]
	public static class GeneratedBuildings_LoadGeneratedBuildings_Patch
	{
		// Token: 0x06000009 RID: 9 RVA: 0x000023DC File Offset: 0x000005DC
		public static void Prefix()
		{
			Strings.Add(new string[]
			{
				"STRINGS.BUILDING.STATUSITEMS.FERZTOILET.NAME",
				"이용 \"{FlushesRemaining}\"회 남음"
			});
			Strings.Add(new string[]
			{
				"STRINGS.BUILDING.STATUSITEMS.FERZTOILET.TOOLTIP",
				"이 시설은 보수가 필요하기 전까지 \"{FlushesRemaining}\"명이 이용할 수 있습니다."
			});
			StatusItem statusItem = new StatusItem("FerzToilet", "BUILDING", string.Empty, 0, 4, false, OverlayModes.None.ID, true, 129022);
			statusItem.resolveStringCallback = delegate(string data, object ob)
			{
				FerzToilet.StatesInstance stn = (FerzToilet.StatesInstance)ob;
				if (stn != null)
				{
					data = data.Replace("{FlushesRemaining}", stn.GetFlushesRemaining().ToString());
				}
				return data;
			};
			Consts.FerzToilet = statusItem;
			GeneratedBuildings_LoadGeneratedBuildings_Patch.SetString(Consts.Toilet.ID_UPPER, Consts.Toilet.NAME, "복체제는 배출할 곳이 필요합니다.\n\n배관을 요구하지 않으며, 비료를 만듭니다.\n주기적으로 만들어진 비료를 비워야합니다.\n\n", "듀플에게 배출할 공간을 마련해줍니다.");
			GeneratedBuildings_LoadGeneratedBuildings_Patch.SetString(Consts.AutoPurifyWashsink.ID_UPPER, Consts.AutoPurifyWashsink.NAME, "세균에 덮인 복제체는 선택된 방향으로 지나갈 때 세면대를 사용합니다.\n모래가 있으면, 오염된 물을 깨끗한 물로 천천히 정화합니다. 정화된 물은 재사용됩니다.\n전력을 공급해주면, 더 빠른속도로 오염을 정화합니다.", "복제체로부터 세균을 제거하고, 건조물 스스로 오염된 물을 정화합니다.");
			ModUtil.AddBuildingToPlanScreen("Plumbing", "SintIsNotToilet");
			ModUtil.AddBuildingToPlanScreen("Medical", "AutoPurifyWashsink");
		}

		// Token: 0x0600000A RID: 10 RVA: 0x000024D0 File Offset: 0x000006D0
		private static void SetString(string idup, string name, string desc, string effc)
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
				effc
			});
		}
	}
}
