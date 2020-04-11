using System;
using Harmony;

namespace SuperVest
{
	// Token: 0x02000006 RID: 6
	[HarmonyPatch(typeof(GeneratedEquipment), "LoadGeneratedEquipment")]
	public sealed class VEST_CONFIG_LOADER_PATCH
	{
		// Token: 0x06000013 RID: 19 RVA: 0x000026C8 File Offset: 0x000008C8
		public static void Postfix()
		{
			VEST_CONFIG_LOADER_PATCH.SetString(SV_CONST.Basic_SuperVest.ID_UPPER, SV_CONST.Basic_SuperVest.NAME, SV_CONST.Basic_SuperVest.DESC, SV_CONST.Basic_SuperVest.EFFC, SV_CONST.Basic_SuperVest.RECIPE);
			VEST_CONFIG_LOADER_PATCH.SetString(SV_CONST.Advance_SuperSuit.ID_UPPER, SV_CONST.Advance_SuperSuit.NAME, SV_CONST.Advance_SuperSuit.DESC, SV_CONST.Advance_SuperSuit.EFFC, SV_CONST.Advance_SuperSuit.RECIPE);
			VEST_CONFIG_LOADER_PATCH.SetString(SV_CONST.SmallBackpack.ID_UPPER, SV_CONST.SmallBackpack.NAME, SV_CONST.SmallBackpack.DESC, SV_CONST.SmallBackpack.EFFC, SV_CONST.SmallBackpack.RECIPE);
			VEST_CONFIG_LOADER_PATCH.SetString(SV_CONST.MediumBackpack.ID_UPPER, SV_CONST.MediumBackpack.NAME, SV_CONST.MediumBackpack.DESC, SV_CONST.MediumBackpack.EFFC, SV_CONST.MediumBackpack.RECIPE);
			VEST_CONFIG_LOADER_PATCH.SetString(SV_CONST.LargeBackpack.ID_UPPER, SV_CONST.LargeBackpack.NAME, SV_CONST.LargeBackpack.DESC, SV_CONST.LargeBackpack.EFFC, SV_CONST.LargeBackpack.RECIPE);
			EquipmentConfigManager instance = EquipmentConfigManager.Instance;
			instance.RegisterEquipment(new BasicSuperSuitConfig());
			instance.RegisterEquipment(new AdvanceSuperSuitConfig());
			instance.RegisterEquipment(new SmallBackpackConfig());
			instance.RegisterEquipment(new MediumBackpackConfig());
			instance.RegisterEquipment(new LargeBackpackConfig());
		}

		// Token: 0x06000014 RID: 20 RVA: 0x0000280C File Offset: 0x00000A0C
		public static void SetString(string idup, string name, string desc, string effec, string recipe)
		{
			string temp = "STRINGS.EQUIPMENT.PREFABS." + idup + ".";
			Strings.Add(new string[]
			{
				temp + "NAME",
				name
			});
			Strings.Add(new string[]
			{
				temp + "GENERICNAME",
				"의류"
			});
			Strings.Add(new string[]
			{
				temp + "DESC",
				desc
			});
			Strings.Add(new string[]
			{
				temp + "EFFECT",
				effec
			});
			Strings.Add(new string[]
			{
				temp + "RECIPE_DESC",
				recipe
			});
		}
	}
}
