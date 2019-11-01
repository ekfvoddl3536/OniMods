using System.Collections.Generic;
using UnityEngine;

namespace SuperComicLib.HUSystem
{
    using static EnergyGenerator;
    using static STRINGS.UI.BUILDINGEFFECTS;
    public class HUEnergyGenerator : HUGenerator, IEffectDescriptor
    {
        [MyCmpGet]
        public Storage instorage;
        public Storage outstorage;
        public Formula items = new Formula();
        [MyCmpGet]
        protected PrimaryElement primary;
        [MyCmpGet]
        protected KSelectable selectable;
        protected bool convertible;
        public bool hasMeter;
        public Meter.Offset meterOffset;
        protected MeterController meter;

        #region 설명
        public List<Descriptor> GetDescriptors(BuildingDef def)
        {
            List<Descriptor> dl = new List<Descriptor>();
            RequirementDescriptors(def, dl);
            EffectDescriptors(def, dl);
            return dl;
        }

        protected virtual void RequirementDescriptors(BuildingDef def, List<Descriptor> list)
        {
            if (items.inputs.Length > 0)
                foreach (InputItem i in items.inputs)
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

        protected virtual void EffectDescriptors(BuildingDef def, List<Descriptor> list)
        {
            if (generateHU > 0)
                list.Add(
                    new Descriptor(
                        $"HU 생산: +{generateHU}/s",
                        string.Format(UIEX.HUPERSEC_PRODUCE, generateHU),
                        Descriptor.DescriptorType.Effect));
            if (items.outputs.Length > 0)
                foreach (OutputItem o in items.outputs)
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
        #endregion

        protected override void OnSpawn()
        {
            base.OnSpawn();
            if (hasMeter)
                meter = new MeterController(GetComponent<KBatchedAnimController>(), "meter_target", "meter", meterOffset, Grid.SceneLayer.NoLayer, new string[4]
                {
                    "meter_target",
                    "meter_fill",
                    "meter_frame",
                    "meter_OL"
                });
        }

        public override void HUSim200ms(float dt)
        {
            base.HUSim200ms(dt);
            if (hasMeter)
            {
                InputItem item = items.inputs[0];
                if (instorage.FindFirst(item.tag) is GameObject go)
                    meter.SetPositionPercent(go.GetComponent<PrimaryElement>().Mass / item.maxStoredMass);
                else
                    meter.SetPositionPercent(0);
            }
            if (operational.IsOperational)
            {
                convertible = IsConvertible(dt);
                selectable.ToggleStatusItem(Db.Get().BuildingStatusItems.NeedResourceMass, !convertible, items);
                operational.SetActive(convertible);

                if (convertible)
                    GeneratorWork(dt);
            }
        }

        protected virtual void GeneratorWork(float dt)
        {
            foreach (InputItem i in items.inputs)
                Consumed(in i, dt);
            foreach (OutputItem o in items.outputs)
                Emit(in o, dt, primary);
        }

        private void Emit(in OutputItem output, float dt, PrimaryElement root_pe)
        {
            Element eh = ElementLoader.FindElementByHash(output.element);
            if (output.store)
                if (eh.IsGas)
                    outstorage.AddGasChunk(output.element, output.creationRate * dt, Mathf.Max(root_pe.Temperature, output.minTemperature), 255, 0, true);
                else if (eh.IsLiquid)
                    outstorage.AddLiquid(output.element, output.creationRate * dt, Mathf.Max(root_pe.Temperature, output.minTemperature), 255, 0, true);
                else
                    outstorage.Store(eh.substance.SpawnResource(transform.GetPosition(), output.creationRate * dt, Mathf.Max(root_pe.Temperature, output.minTemperature), 255, 0), true);
            else if (eh.IsGas)
                SimMessages.ModifyMass(Grid.OffsetCell(Grid.PosToCell(transform.GetPosition()), output.emitOffset), output.creationRate * dt, 255, 0, null, Mathf.Max(root_pe.Temperature, output.minTemperature), output.element);
            else if (eh.IsLiquid)
                FallingWater.instance.AddParticle(Grid.OffsetCell(Grid.PosToCell(transform.GetPosition()), output.emitOffset), (byte)ElementLoader.GetElementIndex(output.element), output.creationRate * dt, Mathf.Max(root_pe.Temperature, output.minTemperature), 255, 0, true);
            else
                eh.substance.SpawnResource(Grid.CellToPosCCC(Grid.OffsetCell(Grid.PosToCell(transform.GetPosition()), output.emitOffset), Grid.SceneLayer.Front), output.creationRate * dt, Mathf.Max(root_pe.Temperature, output.minTemperature), 255, 0, true);
        }

        private void Consumed(in InputItem i, float dt) =>
            instorage.ConsumeIgnoringDisease(i.tag, i.consumptionRate * dt);

        protected virtual bool IsConvertible(float dt)
        {
            foreach (InputItem i in items.inputs)
                if (!(instorage.FindFirstWithMass(i.tag) is PrimaryElement pe) || pe.Mass < i.consumptionRate * dt)
                    return false;
            return true;
        }

        public override int GenerateHeat(float dt) => operational.IsOperational && convertible ? generateHU : 0;
    }
}
