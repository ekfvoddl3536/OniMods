using System.Collections.Generic;
using UnityEngine;

namespace SupportPackage
{
    public class LiquidCoolingSystem : StateMachineComponent<LiquidCoolingSystem.StatesInstance>
    {
        public static readonly Operational.Flag CoolantOutputPipeEmpty = new Operational.Flag(nameof(CoolantOutputPipeEmpty), Operational.Flag.Type.Requirement);
        
        public float MinCoolantMass = 5f;
        public float ThermalFudge = 0.8f;
        // 경고: 이 값이 너무 크면 너무 빨리 냉각되어, 게임이 튕길 수도 있습니다.
        public float CoolingPerSecond = 0.5f;
        public float LowTemperature = 274f;
        // 냉매 태그 입니다. 보통 GameTags.Liquid로 합니다.
        public Tag CoolantTag;
        [MyCmpReq]
        protected Operational operational;
        [MyCmpReq]
        protected PrimaryElement primary; 
        // 중요: input storage 이며, 냉매가 들어있는 Storage 여야하며, 냉매 이외에 다른게 있으면 버그가 납니다.
        [SerializeField]
        public Storage storage;
        // 중요: output storage 이며, 반드시 out 전용이 되어야 합니다.
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

            Debug.Log($"RTMP: {primary.Temperature} TOTLA: {total} FORCH: {forchange}");
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
