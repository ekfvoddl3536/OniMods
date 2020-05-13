using Harmony;
using System.Collections.Generic;
using System.Reflection;
using TUNING;
using UnityEngine;

namespace MoreSpecialGeysers.patch
{
    using static MSG_CONST;

    [HarmonyPatch(typeof(GeyserGenericConfig), nameof(GeyserGenericConfig.CreatePrefabs))]
    public static class GeyserGenPatch
    {
        private static List<GeyserGenericConfig.GeyserPrefabParams> geysers;

        public static bool Prefix(GeyserGenericConfig __instance, ref List<GameObject> __result)
        {
            __result = new List<GameObject>();

            List<GeyserGenericConfig.GeyserPrefabParams> confgs = new List<GeyserGenericConfig.GeyserPrefabParams>();
            List<GeyserGenericConfig.GeyserPrefabParams> temp =
                typeof(GeyserGenericConfig)
                    .GetMethod("GenerateConfigs", BindingFlags.Instance | BindingFlags.NonPublic)
                    .Invoke(__instance, null) 
                as List<GeyserGenericConfig.GeyserPrefabParams>;

            if (temp != null && temp.Count > 0)
                confgs.AddRange(temp);

            confgs.Add(Make(TUNGSTEN.ID, TUNGSTEN.ANISTR, 3, 3, SimHashes.Tungsten, 5750, 800, 1600, 800, 480, 1080, 0.025f, 0.3f));
            confgs.Add(Make(COOLWATER.ID, COOLWATER.ANISTR, 2, 4, SimHashes.Ice, 249.15f, 120, 1000, 10, 60, 1140, 0.1f, 0.2f));
            confgs.Add(Make(LIQHYDROGEN.ID, LIQHYDROGEN.ANISTR, 4, 2, SimHashes.LiquidHydrogen, 19f, 1, 30, 200, 60, 600, 0.4f, 0.8f));
            confgs.Add(Make(COOLGASOXYGEN.ID, COOLGASOXYGEN.ANISTR, 4, 2, SimHashes.Oxygen, 273.15f, 24, 60, 60, 120, 1080, 0.25f, 0.8f));

            CustomGeysers.GetList(confgs);
            foreach (var g in confgs)
                __result.Add(
                    CreateGeyser(
                        g.id, 
                        g.anim, 
                        g.width, 
                        g.height,
                        Strings.Get(g.nameStringKey), 
                        Strings.Get(g.descStringKey), 
                        g.geyserType.idHash));

            geysers = confgs;

            GameObject go = EntityTemplates.CreateEntity("GeyserGeneric", "Random Geyser Spawner", true);
            go.GetComponent<KPrefabID>().prefabInitFn += InitFn;

            __result.Add(go);
            return false;
        }

        private static void InitFn(GameObject inst)
        {
            int num = 0;
            if (SaveLoader.Instance.worldDetailSave != null)
                num = SaveLoader.Instance.worldDetailSave.globalWorldSeed;

            Vector3 pos = inst.transform.GetPosition();
            int r = new System.Random(num + (int)pos.x + (int)pos.y).Next(0, geysers.Count);
            GameUtil.KInstantiate(Assets.GetPrefab(geysers[r].id), pos, Grid.SceneLayer.BuildingBack);
            inst.DeleteObject();
        }

        public static GameObject CreateGeyser(string id, string anim, int width, int height, string name, string desc, HashedString presetType, int offsetX = 0, int offsetY = 1)
        {
            GameObject go =
                EntityTemplates.CreatePlacedEntity(
                    id,
                    name,
                    desc,
                    2000f,
                    Assets.GetAnim(anim),
                    "inactive",
                    Grid.SceneLayer.BuildingBack,
                    width,
                    height,
                    BUILDINGS.DECOR.BONUS.TIER1,
                    NOISE_POLLUTION.NOISY.TIER6);

            go.AddOrGet<OccupyArea>().objectLayers = new ObjectLayer[] { ObjectLayer.Building };

            PrimaryElement pe = go.GetComponent<PrimaryElement>();
            pe.SetElement(SimHashes.Katairite);
            pe.Temperature = 372.15f;

            go.AddOrGet<Prioritizable>();
            go.AddOrGet<Uncoverable>();
            go.AddOrGet<Geyser>().outputOffset = new Vector2I(offsetX, offsetY);
            go.AddOrGet<GeyserConfigurator>().presetType = presetType;

            Studyable st = go.AddOrGet<Studyable>();
            st.meterTrackerSymbol = "geotracker_target";
            st.meterAnim = "tracker";

            go.AddOrGet<LoopingSounds>();

            SoundEventVolumeCache.instance.AddVolume("geyser_side_steam_kanim", "Geyser_shake_LP", NOISE_POLLUTION.NOISY.TIER5);
            SoundEventVolumeCache.instance.AddVolume("geyser_side_steam_kanim", "Geyser_erupt_LP", NOISE_POLLUTION.NOISY.TIER6);
            return go;
        }

        private static GeyserGenericConfig.GeyserPrefabParams Make(
            string id,
            string anim,
            int w,
            int h,
            SimHashes element,
            float temp,
            float minrate,
            float maxrate,
            float maxprsu,
            float minlen,
            float maxlen,
            float minlenprnt,
            float maxlenprnt)
        =>
            new GeyserGenericConfig.GeyserPrefabParams(
                anim,
                w,
                h,
                new GeyserConfigurator.GeyserType(
                    id,
                    element,
                    temp,
                    minrate,
                    maxrate,
                    maxprsu,
                    minlen,
                    maxlen,
                    minlenprnt,
                    maxlenprnt
                    ));
    }
}
