using System;
using Harmony;

namespace SupportPackages
{
	// Token: 0x02000005 RID: 5
	[HarmonyPatch(typeof(GeneratedBuildings), "LoadGeneratedBuildings")]
	public class Patch
	{
		// Token: 0x06000008 RID: 8 RVA: 0x0000217B File Offset: 0x0000037B
		public static void Prefix()
		{
			Patch.SetString("STRINGS.BUILDINGS.PREFABS." + SS_CONST.AnotherSpaceStorage.ID_UPPER, SS_CONST.AnotherSpaceStorage.NAME, "이 저장소에 저장되는 물질은 어떤 이공간으로 복사되며, 언제든지 다시 가져올 수 있습니다!", "자원 공유가 가능한 저장소!");
			ModUtil.AddBuildingToPlanScreen("Base", "AnotherSpaceStorage");
		}

		// Token: 0x06000009 RID: 9 RVA: 0x000021BC File Offset: 0x000003BC
		private static void SetString(string fullid, string name, string desc, string effect)
		{
			Strings.Add(new string[]
			{
				fullid + ".NAME",
				name
			});
			Strings.Add(new string[]
			{
				fullid + ".DESC",
				desc
			});
			Strings.Add(new string[]
			{
				fullid + ".EFFECT",
				effect
			});
		}
	}
}
