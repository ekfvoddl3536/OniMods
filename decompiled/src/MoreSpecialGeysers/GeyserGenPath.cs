using System;
using System.Collections.Generic;
using System.Reflection;
using Harmony;
using TUNING;
using UnityEngine;

namespace MoreSpecialGeysers
{
	// Token: 0x02000007 RID: 7
	[HarmonyPatch(typeof(GeyserGenericConfig))]
	[HarmonyPatch("CreatePrefabs")]
	public class GeyserGenPath
	{
		// Token: 0x06000017 RID: 23 RVA: 0x00002790 File Offset: 0x00000990
		public static bool Prefix(GeyserGenericConfig __instance, ref List<GameObject> __result)
		{
			__result = new List<GameObject>();
			List<GeyserGenericConfig.GeyserPrefabParams> confgs = new List<GeyserGenericConfig.GeyserPrefabParams>();
			List<GeyserGenericConfig.GeyserPrefabParams> temp = typeof(GeyserGenericConfig).GetMethod("GenerateConfigs", BindingFlags.Instance | BindingFlags.NonPublic).Invoke(__instance, new object[0]) as List<GeyserGenericConfig.GeyserPrefabParams>;
			if (temp != null && temp.Count > 0)
			{
				confgs.AddRange(temp);
			}
			confgs.Add(GeyserGenPath.MakeGeyserPrefab("geyser_molten_iron_kanim", 3, 3, GeyserGenPath.MakeGeyserCnf("mt_tungsten", -509585641, 5750f, 200f, 1500f, 800f, 480f, 1080f, 0.025f, 0.3f, 15000f, 135000f, 0.4f, 0.8f)));
			confgs.Add(GeyserGenPath.MakeGeyserPrefab("geyser_gas_steam_kanim", 2, 4, GeyserGenPath.MakeGeyserCnf("cool_water", 873952427, 249.15f, 120f, 800f, 100f, 60f, 1140f, 0.1f, 0.2f, 15000f, 135000f, 0.4f, 0.8f)));
			confgs.Add(GeyserGenPath.MakeGeyserPrefab("geyser_liquid_water_slush_kanim", 4, 2, GeyserGenPath.MakeGeyserCnf("liq_oxygen", -1908044868, 79f, 1f, 5f, 5f, 60f, 2000f, 0.05f, 0.2f, 15000f, 135000f, 0.4f, 0.8f)));
			confgs.Add(GeyserGenPath.MakeGeyserPrefab("geyser_liquid_oil_kanim", 4, 2, GeyserGenPath.MakeGeyserCnf("liq_helium", -1934139602, 4f, 10f, 200f, 100f, 60f, 1140f, 0.1f, 0.9f, 15000f, 135000f, 0.4f, 0.8f)));
			CustomGeysers.GetList(ref confgs, true);
			foreach (GeyserGenericConfig.GeyserPrefabParams g in confgs)
			{
				__result.Add(GeyserGenPath.CreateGeyser(g.id, g.anim, g.width, g.height, Strings.Get(g.nameStringKey), Strings.Get(g.descStringKey), g.geyserType.idHash, 0, 1));
			}
			GameObject go = EntityTemplates.CreateEntity("GeyserGeneric", "Random Geyser Spawner", true);
			EntityTemplateExtensions.AddOrGet<SaveLoadRoot>(go);
			go.GetComponent<KPrefabID>().prefabInitFn += delegate(GameObject inst)
			{
				int num = 0;
				if (SaveLoader.Instance.worldDetailSave != null)
				{
					num = SaveLoader.Instance.worldDetailSave.globalWorldSeed;
				}
				Vector3 a = TransformExtensions.GetPosition(inst.transform);
				int r = GeyserGenPath.Rnd(num, (int)a.x, (int)a.y, confgs.Count);
				GameUtil.KInstantiate(Assets.GetPrefab(confgs[r].id), a, 18, null, 0).SetActive(true);
				TracesExtesions.DeleteObject(inst);
			};
			__result.Add(go);
			return false;
		}

		// Token: 0x06000018 RID: 24 RVA: 0x00002A5C File Offset: 0x00000C5C
		private static int Rnd(int num, int k, int b, int max)
		{
			return new Random(num + b + k).Next(0, max);
		}

		// Token: 0x06000019 RID: 25 RVA: 0x00002A70 File Offset: 0x00000C70
		public static GameObject CreateGeyser(string id, string anim, int width, int height, string name, string desc, HashedString presetType, int offsetX = 0, int offsetY = 1)
		{
			GameObject pe = EntityTemplates.CreatePlacedEntity(id, name, desc, 2000f, Assets.GetAnim(anim), "inactive", 18, width, height, BUILDINGS.DECOR.BONUS.TIER1, NOISE_POLLUTION.NOISY.TIER6, 976099455, null, 293f);
			EntityTemplateExtensions.AddOrGet<OccupyArea>(pe).objectLayers = new ObjectLayer[]
			{
				1
			};
			PrimaryElement component = pe.GetComponent<PrimaryElement>();
			component.SetElement(1071649902);
			component.Temperature = 372.15f;
			EntityTemplateExtensions.AddOrGet<Prioritizable>(pe);
			EntityTemplateExtensions.AddOrGet<Uncoverable>(pe);
			EntityTemplateExtensions.AddOrGet<Geyser>(pe).outputOffset = new Vector2I(offsetX, offsetY);
			EntityTemplateExtensions.AddOrGet<GeyserConfigurator>(pe).presetType = presetType;
			Studyable studyable = EntityTemplateExtensions.AddOrGet<Studyable>(pe);
			studyable.meterTrackerSymbol = "geotracker_target";
			studyable.meterAnim = "tracker";
			EntityTemplateExtensions.AddOrGet<LoopingSounds>(pe);
			SoundEventVolumeCache.instance.AddVolume("geyser_side_steam_kanim", "Geyser_shake_LP", NOISE_POLLUTION.NOISY.TIER5);
			SoundEventVolumeCache.instance.AddVolume("geyser_side_steam_kanim", "Geyser_erupt_LP", NOISE_POLLUTION.NOISY.TIER6);
			return pe;
		}

		// Token: 0x0600001A RID: 26 RVA: 0x00002B6A File Offset: 0x00000D6A
		protected internal static GeyserGenericConfig.GeyserPrefabParams MakeGeyserPrefab(string anim, int width, int height, GeyserConfigurator.GeyserType gtype)
		{
			return new GeyserGenericConfig.GeyserPrefabParams(anim, width, height, gtype);
		}

		// Token: 0x0600001B RID: 27 RVA: 0x00002B78 File Offset: 0x00000D78
		protected internal static GeyserConfigurator.GeyserType MakeGeyserCnf(string id, SimHashes element, float temperature, float minRatePerCycle, float maxRatePerCycle, float maxPressure, float minIterationLength = 60f, float maxIterationLength = 1140f, float minIterationPercent = 0.1f, float maxIterationPercent = 0.9f, float minYearLength = 15000f, float maxYearLength = 135000f, float minYearPercent = 0.4f, float maxYearPercent = 0.8f)
		{
			return new GeyserConfigurator.GeyserType(id, element, temperature, minRatePerCycle, maxRatePerCycle, maxPressure, minIterationLength, maxIterationLength, minIterationPercent, maxIterationPercent, minYearLength, maxYearLength, minYearPercent, maxYearPercent);
		}

		// Token: 0x0600001C RID: 28 RVA: 0x00002BA4 File Offset: 0x00000DA4
		protected internal static GeyserConfigurator.GeyserType MakeMoltenRefinedMetalType(string id, SimHashes element, float temperature)
		{
			return GeyserGenPath.MakeGeyserCnf(id, element, temperature, 50f, 500f, 150f, 480f, 1080f, 0.0166667f, 0.1f, 15000f, 135000f, 0.4f, 0.8f);
		}
	}
}
