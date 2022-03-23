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
using KSerialization;
using UnityEngine;

namespace EcoFriendlyToliet
{
    using static GlobalConsts.AutoPurifyWashsink;
    [SerializationConfig(MemberSerialization.OptIn)]
    public class AutoPurify : EnergyConsumer, IEnergyConsumer, ISim1000ms, ICircuitConnected
    {
        protected ElementConverter converter_;
        protected ushort circuitID_;
        protected bool power_;
        protected int powerCell_;

        float IEnergyConsumer.WattsNeededWhenActive => USE_POWER;

        bool IEnergyConsumer.IsConnected => circuitID_ != ushort.MaxValue;

        bool IEnergyConsumer.IsPowered => power_;

        int ICircuitConnected.PowerCell => powerCell_;

        protected override void OnPrefabInit()
        {
            converter_ = GetComponent<ElementConverter>();
            BaseWattageRating = USE_POWER;
        }

        protected override void OnSpawn()
        {
            Components.EnergyConsumers.Add(this);

            powerCell_ = GetComponent<Building>().GetPowerInputCell();

            Game.Instance.circuitManager.Connect(this);
            Game.Instance.energySim.AddEnergyConsumer(this);

            operational.SetActive(true);
        }

        public override void SetConnectionStatus(CircuitManager.ConnectionStatus connection_status)
        {
            bool res = connection_status == CircuitManager.ConnectionStatus.Powered;
            if (res)
                PlayCircuitSound("powered");
            else if (connection_status == CircuitManager.ConnectionStatus.Unpowered)
            {
                circuitOverloadTime = 6f;
                PlayCircuitSound("overdraw");
            }

            SetPowerState(res);
        }

        protected virtual void SetPowerState(bool flag)
        {
            var conv = converter_;
            if (circuitID_ == ushort.MaxValue || flag == false)
            {
                conv.consumedElements[0].massConsumptionRate =
                    conv.outputElements[0].massGenerationRate = UNPOWERED_CONV;

                conv.consumedElements[1].massConsumptionRate =
                    conv.outputElements[1].massGenerationRate = UNPOWERED_FCON;
            }
            else
            {
                conv.consumedElements[0].massConsumptionRate =
                    conv.outputElements[0].massGenerationRate = POWERED_CONV;

                conv.consumedElements[1].massConsumptionRate =
                    conv.outputElements[1].massGenerationRate = POWERED_FCON;
            }

            power_ = flag;
        }

        public override void EnergySim200ms(float dt)
        {
            ushort id = Game.Instance.circuitManager.GetCircuitID(this);

            if (id == ushort.MaxValue)
                SetPowerState(false);

            circuitID_ = id;

            circuitOverloadTime = Mathf.Max(0f, circuitOverloadTime - dt);
        }

        public void Sim1000ms(float dt)
        {
            if (operational.IsActive ^ converter_.HasEnoughMassToStartConverting())
                operational.SetActive(!operational.IsActive, false);
        }
    }
}
