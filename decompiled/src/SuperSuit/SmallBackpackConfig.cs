using System;
using System.Collections.Generic;
using Klei.AI;
using TUNING;
using UnityEngine;

namespace SuperVest
{
	// Token: 0x02000008 RID: 8
	public class SmallBackpackConfig : IEquipmentConfig
	{
		// Token: 0x06000019 RID: 25 RVA: 0x000029EC File Offset: 0x00000BEC
		public EquipmentDef CreateEquipmentDef()
		{
			List<AttributeModifier> lis = new List<AttributeModifier>
			{
				new AttributeModifier("Athletics", -2f, SV_CONST.SmallBackpack.NAME, false, false, true),
				new AttributeModifier("CarryAmount", 160f, SV_CONST.SmallBackpack.NAME, false, false, true)
			};
			EquipmentDef equipmentDef = EquipmentTemplates.CreateEquipmentDef("SmallBackpack", EQUIPMENT.CLOTHING.SLOT, 947100397, 4f, "shirt_hot01_kanim", "snapTo_body", "body_shirt_hot01_kanim", 4, lis, "snapTo_arm", true, 1, 0.75f, 0.4f, null, null);
			string i = SV_CONST.DESC_THERMAL(SV_CONST.SmallBackpack.SUPER_VEST_1.conductivityMod);
			equipmentDef.additionalDescriptors.Add(new Descriptor(i, i, 1, false));
			i = SV_CONST.DESC_DECOR(SV_CONST.SmallBackpack.SUPER_VEST_1.decorMod);
			equipmentDef.additionalDescriptors.Add(new Descriptor(i, i, 1, false));
			equipmentDef.OnEquipCallBack = new Action<Equippable>(this.OnEquipVest);
			equipmentDef.OnUnequipCallBack = new Action<Equippable>(this.OnUnequipVest);
			equipmentDef.RecipeDescription = SV_CONST.SmallBackpack.RECIPE;
			return equipmentDef;
		}

		// Token: 0x0600001A RID: 26 RVA: 0x00002AFC File Offset: 0x00000CFC
		public void DoPostConfigure(GameObject go)
		{
			KPrefabID component = go.GetComponent<KPrefabID>();
			component.AddTag(GameTags.Clothes, false);
			component.AddTag(GameTags.PedestalDisplayable, false);
			EntityTemplateExtensions.AddOrGet<Equippable>(go).SetQuality(1);
			go.GetComponent<KBatchedAnimController>().sceneLayer = 18;
		}

		// Token: 0x0600001B RID: 27 RVA: 0x00002B34 File Offset: 0x00000D34
		private void OnEquipVest(Equippable eq)
		{
			if (eq == null || eq.assignee == null)
			{
				return;
			}
			ClothingWearer com = eq.assignee.GetSoleOwner().GetComponent<MinionAssignablesProxy>().GetTargetGameObject().GetComponent<ClothingWearer>();
			if (com != null)
			{
				com.ChangeClothes(SV_CONST.SmallBackpack.SUPER_VEST_1);
			}
		}

		// Token: 0x0600001C RID: 28 RVA: 0x00002B7C File Offset: 0x00000D7C
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
