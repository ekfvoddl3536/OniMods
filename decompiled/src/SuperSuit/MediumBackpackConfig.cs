using System;
using System.Collections.Generic;
using Klei.AI;
using TUNING;
using UnityEngine;

namespace SuperVest
{
	// Token: 0x02000004 RID: 4
	public class MediumBackpackConfig : IEquipmentConfig
	{
		// Token: 0x06000009 RID: 9 RVA: 0x00002310 File Offset: 0x00000510
		public EquipmentDef CreateEquipmentDef()
		{
			List<AttributeModifier> lis = new List<AttributeModifier>
			{
				new AttributeModifier("Athletics", -2f, SV_CONST.MediumBackpack.NAME, false, false, true),
				new AttributeModifier("CarryAmount", 400f, SV_CONST.MediumBackpack.NAME, false, false, true)
			};
			EquipmentDef equipmentDef = EquipmentTemplates.CreateEquipmentDef("MediumBackpack", EQUIPMENT.CLOTHING.SLOT, 947100397, 8f, "shirt_hot01_kanim", "snapTo_body", "body_shirt_hot01_kanim", 4, lis, "snapTo_arm", true, 1, 0.75f, 0.4f, null, null);
			string i = SV_CONST.DESC_THERMAL(SV_CONST.MediumBackpack.SUPER_VEST_1.conductivityMod);
			equipmentDef.additionalDescriptors.Add(new Descriptor(i, i, 1, false));
			i = SV_CONST.DESC_DECOR(SV_CONST.MediumBackpack.SUPER_VEST_1.decorMod);
			equipmentDef.additionalDescriptors.Add(new Descriptor(i, i, 1, false));
			equipmentDef.OnEquipCallBack = new Action<Equippable>(this.OnEquipVest);
			equipmentDef.OnUnequipCallBack = new Action<Equippable>(this.OnUnequipVest);
			equipmentDef.RecipeDescription = SV_CONST.MediumBackpack.RECIPE;
			return equipmentDef;
		}

		// Token: 0x0600000A RID: 10 RVA: 0x00002420 File Offset: 0x00000620
		public void DoPostConfigure(GameObject go)
		{
			KPrefabID component = go.GetComponent<KPrefabID>();
			component.AddTag(GameTags.Clothes, false);
			component.AddTag(GameTags.PedestalDisplayable, false);
			EntityTemplateExtensions.AddOrGet<Equippable>(go).SetQuality(1);
			go.GetComponent<KBatchedAnimController>().sceneLayer = 18;
		}

		// Token: 0x0600000B RID: 11 RVA: 0x00002458 File Offset: 0x00000658
		private void OnEquipVest(Equippable eq)
		{
			if (eq == null || eq.assignee == null)
			{
				return;
			}
			ClothingWearer com = eq.assignee.GetSoleOwner().GetComponent<MinionAssignablesProxy>().GetTargetGameObject().GetComponent<ClothingWearer>();
			if (com != null)
			{
				com.ChangeClothes(SV_CONST.MediumBackpack.SUPER_VEST_1);
			}
		}

		// Token: 0x0600000C RID: 12 RVA: 0x000024A0 File Offset: 0x000006A0
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
