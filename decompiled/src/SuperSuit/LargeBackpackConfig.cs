using System;
using System.Collections.Generic;
using Klei.AI;
using TUNING;
using UnityEngine;

namespace SuperVest
{
	// Token: 0x02000005 RID: 5
	public class LargeBackpackConfig : IEquipmentConfig
	{
		// Token: 0x0600000E RID: 14 RVA: 0x000024EC File Offset: 0x000006EC
		public EquipmentDef CreateEquipmentDef()
		{
			List<AttributeModifier> lis = new List<AttributeModifier>
			{
				new AttributeModifier("Athletics", -4f, SV_CONST.LargeBackpack.NAME, false, false, true),
				new AttributeModifier("CarryAmount", 3200f, SV_CONST.LargeBackpack.NAME, false, false, true)
			};
			EquipmentDef equipmentDef = EquipmentTemplates.CreateEquipmentDef("LargeBackpack", EQUIPMENT.CLOTHING.SLOT, 947100397, 20f, "shirt_cold01_kanim", "snapTo_body", "body_shirt_cold01_kanim", 4, lis, "snapTo_arm", true, 1, 0.75f, 0.4f, null, null);
			string i = SV_CONST.DESC_THERMAL(SV_CONST.LargeBackpack.SUPER_VEST_1.conductivityMod);
			equipmentDef.additionalDescriptors.Add(new Descriptor(i, i, 1, false));
			i = SV_CONST.DESC_DECOR(SV_CONST.LargeBackpack.SUPER_VEST_1.decorMod);
			equipmentDef.additionalDescriptors.Add(new Descriptor(i, i, 1, false));
			equipmentDef.OnEquipCallBack = new Action<Equippable>(this.OnEquipVest);
			equipmentDef.OnUnequipCallBack = new Action<Equippable>(this.OnUnequipVest);
			equipmentDef.RecipeDescription = SV_CONST.LargeBackpack.RECIPE;
			return equipmentDef;
		}

		// Token: 0x0600000F RID: 15 RVA: 0x000025FC File Offset: 0x000007FC
		public void DoPostConfigure(GameObject go)
		{
			KPrefabID component = go.GetComponent<KPrefabID>();
			component.AddTag(GameTags.Clothes, false);
			component.AddTag(GameTags.PedestalDisplayable, false);
			EntityTemplateExtensions.AddOrGet<Equippable>(go).SetQuality(1);
			go.GetComponent<KBatchedAnimController>().sceneLayer = 18;
		}

		// Token: 0x06000010 RID: 16 RVA: 0x00002634 File Offset: 0x00000834
		private void OnEquipVest(Equippable eq)
		{
			if (eq == null || eq.assignee == null)
			{
				return;
			}
			ClothingWearer com = eq.assignee.GetSoleOwner().GetComponent<MinionAssignablesProxy>().GetTargetGameObject().GetComponent<ClothingWearer>();
			if (com != null)
			{
				com.ChangeClothes(SV_CONST.LargeBackpack.SUPER_VEST_1);
			}
		}

		// Token: 0x06000011 RID: 17 RVA: 0x0000267C File Offset: 0x0000087C
		private void OnUnequipVest(Equippable eq)
		{
			if (eq == null || eq.assignee == null)
			{
				return;
			}
			ClothingWearer com = eq.assignee.GetSoleOwner().GetComponent<MinionAssignablesProxy>().GetTargetGameObject().GetComponent<ClothingWearer>();
			if (com != null)
			{
				com.ChangeToDefaultClothes();
			}
		}
	}
}
