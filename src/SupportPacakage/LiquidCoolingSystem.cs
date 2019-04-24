using System.Collections.Generic;
using UnityEngine;

namespace SupportPackage
{
    public class LiquidCoolingSystem : StateMachineComponent<LiquidCoolingSystem.StatesInstance>
    {
        public float MinCoolantMass = 5f;
        public float ThermalFudge = 0.8f;
        public float CoolingPerSecond = 0.5f;
        public float LowTemperature = 274f;
        public Tag CoolantTag;
        [MyCmpReq]
        protected Operational operational;
        [MyCmpReq]
        protected PrimaryElement primary; 
        [SerializeField]
        public Storage storage;
        [SerializeField]
        public Storage OutStorage;

        protected override void OnSpawn() => smi.StartSM();

        public bool HasEnoughCoolant() => storage.GetAmountAvailable(CoolantTag) >= MinCoolantMass;

        protected virtual void Cooling()
        {
            if (!HasEnoughCoolant() || primary.Temperature <= LowTemperature) return;

            PrimaryElement compo;
            float forchange = GameUtil.CalculateEnergyDeltaForElementChange(primary.Element.specificHeatCapacity, primary.Mass, primary.Temperature, primary.Temperature - CoolingPerSecond);

            List<GameObject> plist = new List<GameObject>();
            storage.Find(CoolantTag, plist);

            float fmass, total = 0;
            for (int a = 0, max = plist.Count; a < max; a++)
            {
                compo = plist[a].GetComponent<PrimaryElement>();
                total += compo.Mass * compo.Element.specificHeatCapacity;
            }

            compo = plist[0].GetComponent<PrimaryElement>();
            fmass = compo.Mass * compo.Element.specificHeatCapacity / total;

            compo.Temperature += GameUtil.CalculateTemperatureChange(compo.Element.specificHeatCapacity, compo.Mass, -forchange * fmass * ThermalFudge); ;
            primary.Temperature -= CoolingPerSecond;

            storage.Transfer(plist[0], OutStorage, false, true);
        }

        public class StatesInstance : GameStateMachine<States, StatesInstance, LiquidCoolingSystem>.GameInstance
        {
            public StatesInstance(LiquidCoolingSystem master) : base(master) { }
        }

        public class States : GameStateMachine<States, StatesInstance, LiquidCoolingSystem>
        {
            public State disabled;
            public State cooling;

            public override void InitializeStates(out BaseState default_state)
            {
                default_state = disabled;
                disabled.EventTransition(GameHashes.ActiveChanged, cooling, smi => smi.master.operational != null ? smi.master.operational.IsActive : true);
                cooling.EventTransition(GameHashes.ActiveChanged, disabled, smi => smi.master.operational != null ? !smi.master.operational.IsActive : false).
                    Update("Cooling", (smi, dt) => smi.master.Cooling(), UpdateRate.SIM_1000ms, true);
            }
        }
    }
}
