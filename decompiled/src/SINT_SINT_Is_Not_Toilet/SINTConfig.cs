using System;
using TUNING;
using UnityEngine;

namespace SINT_SINT_Is_Not_Toilet
{
	// Token: 0x02000006 RID: 6
	public sealed class SINTConfig : IBuildingConfig
	{
		// Token: 0x0600001E RID: 30 RVA: 0x00002AFC File Offset: 0x00000CFC
		public override BuildingDef CreateBuildingDef()
		{
			BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef("SintIsNotToilet", 2, 3, "outhouse_kanim", 30, 30f, Consts.Toilet.MASSKG, Consts.Toilet.Materials, 800f, 1, DECOR.PENALTY.TIER5, NOISE_POLLUTION.NONE, 0.2f);
			buildingDef.Overheatable = false;
			buildingDef.ExhaustKilowattsWhenActive = 0.6f;
			buildingDef.DiseaseCellVisName = "FoodPoisoning";
			buildingDef.AudioCategory = "Metal";
			SoundEventVolumeCache.instance.AddVolume("outhouse_kanim", "Latrine_door_open", NOISE_POLLUTION.NOISY.TIER1);
			SoundEventVolumeCache.instance.AddVolume("outhouse_kanim", "Latrine_door_close", NOISE_POLLUTION.NOISY.TIER1);
			return buildingDef;
		}

		// Token: 0x0600001F RID: 31 RVA: 0x00002B9C File Offset: 0x00000D9C
		public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
		{
			EntityTemplateExtensions.AddOrGet<LoopingSounds>(go);
			go.GetComponent<KPrefabID>().AddTag(RoomConstraints.ConstraintTags.Toilet, false);
			Storage st = EntityTemplateExtensions.AddOrGet<Storage>(go);
			st.capacityKg = 500f;
			st.showInUI = true;
			FerzToilet ferzToilet = go.AddComponent<FerzToilet>();
			ferzToilet.solidWastePerUse = new Toilet.SpawnInfo(-1396791454, 9f, 0f);
			ferzToilet.gasWasteWhenFull = new Toilet.SpawnInfo(721531317, 0.2f, 50f);
			ferzToilet.diseasePerFlush = 100000;
			ferzToilet.diseaseOnDupePerFlush = 100000;
			ferzToilet.UseDisease = true;
			ferzToilet.diseaseId = "FoodPoisoning";
			ferzToilet.needSrcElems = new Tag[]
			{
				GameTagExtensions.CreateTag(1836671383),
				GameTagExtensions.CreateTag(1624244999)
			};
			KAnimFile[] kf = new KAnimFile[]
			{
				Assets.GetAnim("anim_interacts_outhouse_kanim")
			};
			ToiletWorkableUse tw = EntityTemplateExtensions.AddOrGet<ToiletWorkableUse>(go);
			tw.overrideAnims = kf;
			tw.workLayer = 21;
			ToiletWorkableClean toiletWorkableClean = EntityTemplateExtensions.AddOrGet<ToiletWorkableClean>(go);
			toiletWorkableClean.workTime = 50f;
			toiletWorkableClean.overrideAnims = kf;
			toiletWorkableClean.workLayer = tw.workLayer;
			Prioritizable.AddRef(go);
			ManualDeliveryKG manualDeliveryKG = EntityTemplateExtensions.AddOrGet<ManualDeliveryKG>(go);
			manualDeliveryKG.SetStorage(st);
			manualDeliveryKG.capacity = 200f;
			manualDeliveryKG.requestedItemTag = GameTagExtensions.CreateTag(1624244999);
			ManualDeliveryKG md2 = go.AddComponent<ManualDeliveryKG>();
			md2.SetStorage(st);
			md2.capacity = 70f;
			md2.requestedItemTag = GameTagExtensions.CreateTag(1836671383);
			manualDeliveryKG.allowPause = (md2.allowPause = false);
			manualDeliveryKG.refillMass = (md2.refillMass = 0.01f);
			manualDeliveryKG.choreTypeIDHash = (md2.choreTypeIDHash = Db.Get().ChoreTypes.FetchCritical.IdHash);
			Ownable ownable = EntityTemplateExtensions.AddOrGet<Ownable>(go);
			ownable.slotID = Db.Get().AssignableSlots.Toilet.Id;
			ownable.canBePublic = true;
		}

		// Token: 0x06000020 RID: 32 RVA: 0x00002D84 File Offset: 0x00000F84
		public override void DoPostConfigureComplete(GameObject go)
		{
		}
	}
}
