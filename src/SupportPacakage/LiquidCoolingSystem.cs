// BSD-3 LICENSE
// Copyright 2019. SuperComic.
// All rights reserved.
// 
// Redistribution and use in source and binary forms, with or without modification, are permitted provided that the following conditions are met:
//
//  - Redistributions of source code must retain the above copyright notice, this list of conditions and the following disclaimer.
// 
//  - Redistributions in binary form must reproduce the above copyright notice, this list of conditions and the following disclaimer in the documentation and/or other materials provided with the distribution.
//
//  - Neither the name of the copyright holder nor the names of its contributors may be used to endorse or promote products derived from this software without specific prior written permission.
//
// THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND
// ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIEDi
// WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE
// DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT HOLDER OR CONTRIBUTORS BE LIABLE FOR
// ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES
// (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES;
// LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND
// ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT
// (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS
// SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.

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
