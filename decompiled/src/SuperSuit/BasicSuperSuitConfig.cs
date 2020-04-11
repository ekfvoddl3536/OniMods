using System;
using System.Collections.Generic;
using Klei.AI;
using TUNING;
using UnityEngine;

namespace SuperVest
{
	// Token: 0x02000003 RID: 3
	public class BasicSuperSuitConfig : IEquipmentConfig
	{
		// Token: 0x06000005 RID: 5 RVA: 0x000021B0 File Offset: 0x000003B0
		public EquipmentDef CreateEquipmentDef()
		{
			EquipmentDef equipmentDef = EquipmentTemplates.CreateEquipmentDef("Basic_SuperVest", EQUIPMENT.CLOTHING.SLOT, 947100397, 8f, "shirt_decor01_kanim", "snapTo_body", "body_shirt_decor01_kanim", 4, new List<AttributeModifier>(), "snapTo_arm", true, 1, 0.75f, 0.4f, null, null);
			string i = SV_CONST.DESC_THERMAL(SV_CONST.Basic_SuperVest.SUPER_VEST_1.conductivityMod);
			equipmentDef.additionalDescriptors.Add(new Descriptor(i, i, 1, false));
			i = SV_CONST.DESC_DECOR(SV_CONST.Basic_SuperVest.SUPER_VEST_1.decorMod);
			equipmentDef.additionalDescriptors.Add(new Descriptor(i, i, 1, false));
			equipmentDef.OnEquipCallBack = new Action<Equippable>(BasicSuperSuitConfig.OnEquipVest);
			equipmentDef.OnUnequipCallBack = new Action<Equippable>(CoolVestConfig.OnUnequipVest);
			equipmentDef.RecipeDescription = SV_CONST.Basic_SuperVest.RECIPE;
			return equipmentDef;
		}

		// Token: 0x06000006 RID: 6 RVA: 0x0000227A File Offset: 0x0000047A
		public void DoPostConfigure(GameObject go)
		{
			KPrefabID component = go.GetComponent<KPrefabID>();
			component.AddTag(GameTags.Clothes, false);
			component.AddTag(GameTags.PedestalDisplayable, false);
			EntityTemplateExtensions.AddOrGet<Equippable>(go).SetQuality(1);
			go.GetComponent<KBatchedAnimController>().sceneLayer = 18;
		}

		// Token: 0x06000007 RID: 7 RVA: 0x000022B4 File Offset: 0x000004B4
		public static void OnEquipVest(Equippable eq)
		{
			if (eq == null || eq.assignee == null)
			{
				return;
			}
			Ownables solo = eq.assignee.GetSoleOwner();
			if (solo != null)
			{
				ClothingWearer com = (solo.GetComponent<MinionAssignablesProxy>().target as KMonoBehaviour).GetComponent<ClothingWearer>();
				if (com != null)
				{
					com.ChangeClothes(SV_CONST.Basic_SuperVest.SUPER_VEST_1);
				}
			}
		}
	}
}
