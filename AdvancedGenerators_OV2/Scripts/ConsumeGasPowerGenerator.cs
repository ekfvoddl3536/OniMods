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

namespace AdvancedGenerators
{
    using static EnergyGenerator;
    public class ConsumGasPowerGenerator : AdvancedEnergyGenerator
    {
        public Storage InStorage;
        public Storage OutStorage;
        public Formula InOutItems = new Formula();

        [MyCmpGet]
        public ElementConsumer Consumer;

        #region prefabinit, spawn
        protected override void OnPrefabInit()
        {
            base.OnPrefabInit();
            Consumer.EnableConsumption(false);
        }

        protected override void OnSpawn()
        {
            base.OnSpawn();
            Consumer.EnableConsumption(true);
        }

        protected override void OnSpawnPost()
        {
            if (InStorage == null || OutStorage == null || InOutItems.inputs == null || InOutItems.outputs == null)
                throw new System.Exception("저장소 또는 input, output 이 지정되지 않았습니다.");

            base.OnSpawnPost();
        }
        #endregion

        #region generator work
        public override void EnergySim200ms(float dt)
        {
            base.EnergySim200ms(dt);

            if (HasMeter)
            {
                ref InputItem ind = ref InOutItems.inputs[0];
                float percent = 0;

                if (InStorage.FindFirstWithMass(ind.tag) is PrimaryElement first)
                    percent = first.Mass / ind.maxStoredMass;

                meter.SetPositionPercent(percent);
            }

            var op = operational;
            op.SetFlag(wireConnectedFlag, CircuitID != ushort.MaxValue);

            Consumer.EnableConsumption(op.IsOperational);
            if (op.IsOperational == false)
                return;

            bool flag = IsConvertible(dt);
            selectable.ToggleStatusItem(Db.Get().BuildingStatusItems.NeedResourceMass, !flag, InOutItems);
            
            op.SetActive(flag);

            if (flag)
                GenerateWork(dt);
        }

        protected virtual bool IsConvertible(float dt)
        {
            ref var inputs = ref InOutItems.inputs;

            var storage = InStorage;

            for (int x = inputs.Length; --x >= 0;)
                if (!(storage.FindFirstWithMass(inputs[x].tag) is PrimaryElement pe) ||
                    pe.Mass < inputs[x].consumptionRate * dt)
                    return false;

            return true;
        }

        protected virtual void GenerateWork(float dt)
        {
            PrimaryElement com = GetComponent<PrimaryElement>();
            Vector3 now = transform.GetPosition();

            ref var ioitem = ref InOutItems;

            int x;
            for (x = ioitem.inputs.Length; --x >= 0;)
            {
                ref var inp = ref ioitem.inputs[x];
                
            }

            for (x = ioitem.outputs.Length; --x >= 0;)
                EmitElements(com, now, ref ioitem.outputs[x], dt);

            GenerateJoules(WattageRating * dt);

            var db = Db.Get();
            selectable.SetStatusItem(db.StatusItemCategories.Power, db.BuildingStatusItems.Wattage, this);
        }

        protected virtual void ConsumedInput(InputItem inp, float dt) =>
            InStorage.ConsumeIgnoringDisease(inp.tag, inp.consumptionRate * dt);

        protected virtual void EmitElements(PrimaryElement root_pe, Vector3 now, ref OutputItem oti, float dt)
        {
            Element elbh = ElementLoader.FindElementByHash(oti.element);
            float temp = Mathf.Max(root_pe.Temperature, oti.minTemperature), rate = oti.creationRate * dt;

            if (oti.store)
            {
                var storage = OutStorage;
                if (elbh.IsGas)
                    storage.AddGasChunk(oti.element, rate, temp, 255, 0, true);
                else if (elbh.IsLiquid)
                    storage.AddLiquid(oti.element, rate, temp, 255, 0, true);
                else
                    storage.Store(elbh.substance.SpawnResource(now, rate, temp, 255, 0), true);
            }
            else
            {
                int pos = Grid.OffsetCell(Grid.PosToCell(now), oti.emitOffset);
                if (elbh.IsGas)
                    SimMessages.ModifyMass(pos, rate, 255, 0, CellEventLogger.Instance.EnergyGeneratorModifyMass, temp, oti.element);
                else if (elbh.IsLiquid)
                    FallingWater.instance.AddParticle(pos, (byte)ElementLoader.GetElementIndex(oti.element), rate, temp, 255, 0, true);
                else
                    elbh.substance.SpawnResource(Grid.CellToPosCCC(pos, Grid.SceneLayer.Front), rate, temp, 255, 0, true);
            }
        }
        #endregion
    }
}
