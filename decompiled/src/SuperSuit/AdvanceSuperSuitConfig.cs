using System;
using System.Collections.Generic;
using Klei.AI;
using TUNING;
using UnityEngine;

namespace SuperVest
{
	// Token: 0x02000002 RID: 2
	public class AdvanceSuperSuitConfig : IEquipmentConfig
	{
		// Token: 0x06000001 RID: 1 RVA: 0x00002050 File Offset: 0x00000250
		public EquipmentDef CreateEquipmentDef()
		{
			EquipmentDef equipmentDef = EquipmentTemplates.CreateEquipmentDef("Advance_SuperSuit", EQUIPMENT.CLOTHING.SLOT, 947100397, 20f, "shirt_cold01_kanim", "snapTo_body", "body_shirt_cold01_kanim", 4, new List<AttributeModifier>(), "snapTo_arm", true, 1, 0.75f, 0.4f, null, null);
			string i = SV_CONST.DESC_THERMAL(SV_CONST.Advance_SuperSuit.SUPER_VEST_1.conductivityMod);
			equipmentDef.additionalDescriptors.Add(new Descriptor(i, i, 1, false));
			i = SV_CONST.DESC_DECOR(SV_CONST.Advance_SuperSuit.SUPER_VEST_1.decorMod);
			equipmentDef.additionalDescriptors.Add(new Descriptor(i, i, 1, false));
			equipmentDef.OnEquipCallBack = new Action<Equippable>(AdvanceSuperSuitConfig.OnEquipVest);
			equipmentDef.OnUnequipCallBack = new Action<Equippable>(CoolVestConfig.OnUnequipVest);
			equipmentDef.RecipeDescription = SV_CONST.Advance_SuperSuit.RECIPE;
			return equipmentDef;
		}

		// Token: 0x06000002 RID: 2 RVA: 0x0000211A File Offset: 0x0000031A
		public void DoPostConfigure(GameObject go)
		{
			KPrefabID component = go.GetComponent<KPrefabID>();
			component.AddTag(GameTags.Clothes, false);
			component.AddTag(GameTags.PedestalDisplayable, false);
			EntityTemplateExtensions.AddOrGet<Equippable>(go).SetQuality(1);
			go.GetComponent<KBatchedAnimController>().sceneLayer = 18;
		}

		// Token: 0x06000003 RID: 3 RVA: 0x00002154 File Offset: 0x00000354
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
					com.ChangeClothes(SV_CONST.Advance_SuperSuit.SUPER_VEST_1);
				}
			}
		}
	}
}
