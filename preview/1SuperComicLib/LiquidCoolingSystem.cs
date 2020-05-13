#pragma warning disable CA1034
using System.Collections.Generic;
using UnityEngine;

namespace SuperComicLib.Oni
{
    public class LiquidCoolingSystem : StateMachineComponent<LiquidCoolingSystem.StatesInstance>
    {
        public Storage inStorage;
        public Storage outStorage;
        public float minCoolantMass = 5f;
        public float thermalFudge = 0.8f;
        public float coolingPerSecond = 0.5f;
        public float lowTemperature = 274f;
        public Tag coolantTag;
        [MyCmpReq]
        protected Operational operational;
        [MyCmpReq]
        protected PrimaryElement primary;

        protected override void OnSpawn() => smi.StartSM();

        public bool HasEnoughCoolant => inStorage.GetAmountAvailable(coolantTag) >= minCoolantMass;

        protected virtual void Cooling()
        {
            if (HasEnoughCoolant && primary.Temperature > lowTemperature)
            {
                float forchange = TemperatureUtil.CalculateLostEnergyChange(primary, coolingPerSecond);
                List<GameObject> list = new List<GameObject>();
                inStorage.Find(coolantTag, list);

                PrimaryElement compo = list[0].GetComponent<PrimaryElement>();
                float fmass = compo.Mass * compo.Element.specificHeatCapacity;
                compo.Temperature += GameUtil.CalculateTemperatureChange(compo.Element.specificHeatCapacity, compo.Mass, -forchange * fmass * thermalFudge);
                primary.Temperature -= coolingPerSecond;
                inStorage.Transfer(list[0], outStorage, false, true);
            }
        }

        public sealed class StatesInstance : GameStateMachine<States, StatesInstance, LiquidCoolingSystem>.GameInstance
        {
            public StatesInstance(LiquidCoolingSystem master) : base(master)
            {
            }
        }

        public sealed class States : GameStateMachine<States, StatesInstance, LiquidCoolingSystem>
        {
            private readonly State disabled;
            private readonly State cooling;

            public override void InitializeStates(out BaseState default_state)
            {
                default_state = disabled;
                disabled.EventTransition(GameHashes.ActiveChanged, cooling, smi => smi.master.operational.IsActive);
                cooling
                    .EventTransition(GameHashes.ActiveChanged, disabled, smi => !smi.master.operational.IsActive)
                    .Update(CB, UpdateRate.SIM_1000ms, true);
            }

            private void CB(StatesInstance smi, float dt) => smi.master.Cooling();
        }
    }
}
