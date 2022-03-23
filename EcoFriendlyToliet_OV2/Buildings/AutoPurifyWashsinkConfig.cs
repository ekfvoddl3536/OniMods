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
    using static GlobalConsts.AutoPurifyWashsink;
    public class AutoPurifyWashsinkConfig : IBuildingConfig
    {
        public override BuildingDef CreateBuildingDef()
        {
            var res = BuildingTemplates.CreateBuildingDef(ID, 2, 3, ANISTR,
                HITPT, CONTIME,
                MassKG, Materials,
                MELPT, BuildLocationRule.OnFloor,
                DECOR.BONUS.TIER0, NOISE_POLLUTION.NONE);

            res.RequiresPowerInput = true;
            res.PowerInputOffset = new CellOffset(1, 1);

            res.AddLogicPowerPort = false;

            res.EnergyConsumptionWhenActive = USE_POWER;

            res.ViewMode = OverlayModes.Power.ID;

            res.AudioCategory = "Metal";

            return res;
        }

        public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
        {
            go.GetComponent<KPrefabID>().AddTag(RoomConstraints.ConstraintTags.WashStation);

            var st = go.AddOrGet<Storage>();
            st.SetDefaultStoredItemModifiers(Storage.StandardInsulatedStorage);
            st.showInUI = true;
            st.doDiseaseTransfer = false;
            st.capacityKg = 20000f;
            st.fetchCategory = 0;

            var hs = go.AddOrGet<HandSanitizer>();
            hs.massConsumedPerUse = WATER_USE;
            hs.consumedElement = SimHashes.Water;
            hs.outputElement = SimHashes.DirtyWater;
            hs.diseaseRemovalCount = DISEASE_REMOVAL;
            hs.maxUses = USED_MAX;
            hs.dirtyMeterOffset = Meter.Offset.Behind;
            hs.dumpWhenFull = true;

            var w = go.AddOrGet<HandSanitizer.Work>();
            w.overrideAnims = new[] { Assets.GetAnim("anim_interacts_washbasin_kanim") };
            w.workTime = WORKTIME;
            w.trackUses = true;

            var md1 = go.AddOrGet<ManualDeliveryKG>();
            md1.SetStorage(st);
            md1.requestedItemTag = new Tag(nameof(SimHashes.Water));
            md1.minimumMass = WATER_USE;
            md1.capacity = MAX_STORED;
            md1.refillMass = WATER_USE / 10f;

            var md2 = go.AddComponent<ManualDeliveryKG>();
            md2.SetStorage(st);
            md2.requestedItemTag = GameTags.Filter;
            md2.minimumMass = MIN_FILTER_DELIVERY_KG;
            md2.capacity = MAX_STORED;
            md2.refillMass = MIN_FILTER_DELIVERY_KG / 10f;

            md1.choreTypeIDHash = md2.choreTypeIDHash = Db.Get().ChoreTypes.FetchCritical.IdHash;

            var ec = go.AddOrGet<ElementConverter>();
            ec.SetStorage(st);
            ec.consumedElements = new ElementConverter.ConsumedElement[]
            {
                new ElementConverter.ConsumedElement(new Tag(nameof(SimHashes.DirtyWater)), UNPOWERED_CONV),
                new ElementConverter.ConsumedElement(GameTags.Filter, UNPOWERED_FCON),
            };
            ec.outputElements = new ElementConverter.OutputElement[]
            {
                new ElementConverter.OutputElement(UNPOWERED_CONV, SimHashes.Water, 0f, true, true, 0f, 0.5f, 0f),
                new ElementConverter.OutputElement(UNPOWERED_FCON, SimHashes.Fertilizer, 333.15f, false, true, 0f, 0.5f, 1f, Db.Get().Diseases.GetIndex(DISEASE_ID), 120)
            };

            var drop = go.AddOrGet<ElementDropper>();
            drop.emitMass = 1000f;
            drop.emitOffset = new Vector2(0, 1f);
            drop.emitTag = new Tag(nameof(SimHashes.Fertilizer));

            go.AddOrGet<LoopingSounds>();
            go.AddOrGet<DirectionControl>();
            go.AddOrGetDef<RocketUsageRestriction.Def>();
        }

        public override void DoPostConfigureComplete(GameObject go)
        {
            Object.DestroyImmediate(go.GetComponent<EnergyConsumer>());

            go.AddOrGet<AutoPurify>();
            go.GetComponent<RequireInputs>().SetRequirements(false, false);
        }
    }
}
