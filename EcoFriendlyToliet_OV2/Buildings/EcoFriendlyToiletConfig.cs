#region LICENSE
/*
MIT License

Copyright (c) 2022. Super Comic (ekfvoddl3535@naver.com)

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.
*/
#endregion
using UnityEngine;
using TUNING;

namespace EcoFriendlyToliet
{
    using static GlobalConsts;
    using static GlobalConsts.EcoFriendlyToilet;
    public class EcoFriendlyToiletConfig : IBuildingConfig
    {
        public override BuildingDef CreateBuildingDef()
        {
            var res = BuildingTemplates.CreateBuildingDef(ID, 2, 3, ANISTR,
                HITPT, CONTIME,
                MassKG, Materials,
                MELPT, BuildLocationRule.OnFloor,
                DECOR.BONUS.TIER0, NOISE_POLLUTION.NONE);

            res.Overheatable = false;
            res.ExhaustKilowattsWhenActive = HEAT_EXHAUST;

            res.DiseaseCellVisName = DISEASE_ID;

            res.AudioCategory = "Metal";

            var inst = SoundEventVolumeCache.instance;
            inst.AddVolume(ANISTR, "Latrine_door_open", NOISE_POLLUTION.NOISY.TIER1);
            inst.AddVolume(ANISTR, "Latrine_door_close", NOISE_POLLUTION.NOISY.TIER1);

            return res;
        }

        public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
        {
            go.AddOrGet<LoopingSounds>();

            go.GetComponent<KPrefabID>().AddTag(RoomConstraints.ConstraintTags.Toilet);

            var st = go.AddOrGet<Storage>();
            st.capacityKg = MAX_CAPA;
            st.showInUI = true;

            var fz = go.AddComponent<FerzToilet>();
            fz.solidWastePerUse = new Toilet.SpawnInfo(SimHashes.Fertilizer, USE_PER_EMIT_KG, 0f);
            fz.needSrcElems = new[]
            {
                new Tag(nameof(SimHashes.Water)),
                new Tag(nameof(SimHashes.Dirt)),
            };

            var kf = new KAnimFile[] { Assets.GetAnim("anim_interacts_outhouse_kanim") };

            var tw = go.AddOrGet<ToiletWorkableUse>();
            tw.overrideAnims = kf;
            tw.workLayer = Grid.SceneLayer.BuildingFront;

            var cl = go.AddOrGet<ToiletWorkableClean>();
            cl.workTime = 50f;
            cl.overrideAnims = kf;
            cl.workLayer = Grid.SceneLayer.BuildingFront;

            Prioritizable.AddRef(go);

            var md1 = go.AddOrGet<ManualDeliveryKG>();
            md1.SetStorage(st);
            md1.capacity = DIRT_CAPA;
            md1.requestedItemTag = new Tag(nameof(SimHashes.Dirt));

            var md2 = go.AddComponent<ManualDeliveryKG>();
            md2.SetStorage(st);
            md2.capacity = WATER_CAPA;
            md2.requestedItemTag = new Tag(nameof(SimHashes.Water));

            md1.allowPause = md2.allowPause = false;
            md1.refillMass = md2.refillMass = 0.01f;
            md1.choreTypeIDHash = md2.choreTypeIDHash = Db.Get().ChoreTypes.FetchCritical.IdHash;

            var ow = go.AddOrGet<Ownable>();
            ow.slotID = Db.Get().AssignableSlots.Toilet.Id;
            ow.canBePublic = true;

            go.AddOrGetDef<RocketUsageRestriction.Def>();
        }

        public override void DoPostConfigureComplete(GameObject go)
        {
        }
    }
}
