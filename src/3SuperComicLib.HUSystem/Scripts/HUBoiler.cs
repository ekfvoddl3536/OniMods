using KSerialization;
using System.Collections.Generic;

namespace SuperComicLib.HUSystem
{
    using static EnergyGenerator;
    using static STRINGS.UI.BUILDINGEFFECTS;
    [SerializationConfig(MemberSerialization.OptIn)]
    public class HUBoiler : HUConsumer
    {
        [MyCmpGet]
        public Storage storage;
        public Formula descript_items = new Formula();
        [MyCmpGet]
        protected KSelectable selectable;
        protected int consumedHU;
        // [SerializeField]
        [Serialize]
        public float lowTemperature = 413.15f;
        public CellOffset emitOffset;
        protected float savedTemp;
        public int convertingMass;

        #region 설명
        public override List<Descriptor> GetDescriptors(BuildingDef def)
        {
            List<Descriptor> dl = new List<Descriptor>();
            RequirementsDescriptors(def, dl);
            EffectDescriptors(def, dl);
            return dl;
        }

        protected virtual void EffectDescriptors(BuildingDef def, List<Descriptor> list)
        {
            if (descript_items.outputs.Length > 0)
                foreach (OutputItem o in descript_items.outputs)
                {
                    string str = ElementLoader.FindElementByHash(o.element).tag.ProperName(),
                        creation = GameUtil.GetFormattedMass(o.creationRate, GameUtil.TimeSlice.PerSecond);
                    Descriptor desc = new Descriptor();
                    if (o.minTemperature > 0.0f)
                    {
                        string temp = GameUtil.GetFormattedTemperature(o.minTemperature);
                        desc.SetupDescriptor(
                            string.Format(ELEMENTEMITTED_MINORENTITYTEMP, str, creation, temp),
                            string.Format(TOOLTIPS.ELEMENTEMITTED_MINORENTITYTEMP, str, creation, temp),
                            Descriptor.DescriptorType.Effect);
                    }
                    else
                        desc.SetupDescriptor(
                            string.Format(ELEMENTEMITTED_ENTITYTEMP, str, creation),
                            string.Format(TOOLTIPS.ELEMENTEMITTED_ENTITYTEMP, str, creation),
                            Descriptor.DescriptorType.Effect);
                    list.Add(desc);
                }
        }

        protected virtual void RequirementsDescriptors(BuildingDef def, List<Descriptor> list)
        {
            if (HUMin > 0)
            {
                list.Add(
                    new Descriptor(
                    $"최소 HU 소비: -{HUMin}/s",
                    string.Format(UIEX.HUPERSEC_CONSUMED, "최소", HUMin),
                    Descriptor.DescriptorType.Effect));
                if (HUMax >= HUMin)
                    list.Add(
                        new Descriptor(
                            $"최대 HU 소비: -{HUMax}/s",
                            string.Format(UIEX.HUPERSEC_CONSUMED, "최대", HUMax),
                            Descriptor.DescriptorType.Effect));
            }
            if (descript_items.inputs.Length > 0)
                foreach (InputItem i in descript_items.inputs)
                {
                    string str = i.tag.ProperName();
                    string persec = GameUtil.GetFormattedMass(i.consumptionRate, GameUtil.TimeSlice.PerSecond, GameUtil.MetricMassFormat.UseThreshold, transform, "{0:0.##}");
                    list.Add(
                        new Descriptor(
                        string.Format(ELEMENTCONSUMED, str, persec),
                        string.Format(TOOLTIPS.ELEMENTCONSUMED, str, persec),
                        Descriptor.DescriptorType.Requirement));
                }
        }
        #endregion

        protected override void OnSpawn()
        {
            base.OnSpawn();
            savedTemp = -1;
        }

        public override void HUSim200ms(float dt)
        {
            if (operational.IsOperational)
            {
                bool convertible = IsConvertible(dt, out PrimaryElement res);
                selectable.ToggleStatusItem(Db.Get().BuildingStatusItems.NeedResourceMass, !convertible, descript_items);
                bool canstarting = convertible & consumedHU >= HUMin;
                operational.SetActive(canstarting);

                if (canstarting)
                    BoilerWork(dt, res);
            }
        }

        protected virtual void BoilerWork(float dt, PrimaryElement element)
        {
            if (savedTemp < 0)
            {
                float temp = element.Temperature + consumedHU / element.Mass * dt;
                if (temp < lowTemperature)
                    savedTemp = temp;
                else
                    OnBoilerWork(element.Element.idx, convertingMass * dt, temp, element.DiseaseIdx, element.DiseaseCount);
            }
            else
            {
                savedTemp += consumedHU / element.Mass * dt;
                if (savedTemp >= lowTemperature)
                {
                    OnBoilerWork(element.Element.idx, convertingMass * dt, element.Temperature, element.DiseaseIdx, element.DiseaseCount);
                    savedTemp = -1;
                }
                else
                    element.Temperature = savedTemp;
            }
        }

        protected virtual void OnBoilerWork(byte idx, float mass, float temperature, byte diseaseIdx, int diseaseCount)
        {
            storage.ConsumeIgnoringDisease(GameTags.Water, mass);
            FallingWater.instance.AddParticle(building.GetRotatedOffsetCell(emitOffset), idx, mass, temperature, diseaseIdx, diseaseCount, true);
        }

        protected virtual bool IsConvertible(float dt, out PrimaryElement result)
        {
            result = storage.FindFirstWithMass(GameTags.Water);
            return result != null && result.Mass >= convertingMass * dt;
        }

        protected override int OnConsumedHU(int hu, float dt)
        {
            consumedHU = hu;
            return hu;
        }
    }
}
