// MIT License
//
// Copyright (c) 2022-2023. Super Comic (ekfvoddl3535@naver.com)
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in all
// copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
// SOFTWARE.

using STRINGS;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace SupportPackage
{
    public sealed class InOneRefrigeratorController : GameStateMachine<InOneRefrigeratorController, InOneRefrigeratorController.StatesInstance, IStateMachineTarget, InOneRefrigeratorController.Def>
    {
        #region nested classes
        public sealed class Def : BaseDef, IGameObjectEffectDescriptor
        {
            public float activeCoolingStartBuffer = 2f;

            public float activeCoolingStopBuffer = 0.1f;

            public float simulatedInternalTemperature = 274.15f;

            public float simulatedInternalHeatCapacity = 400f;

            public float simulatedThermalConductivity = 1000f;

            public float powerSaverEnergyUsage;

            public float coolingHeatKW;

            public float steadyHeatKW;

            public List<Descriptor> GetDescriptors(GameObject _)
            {
                List<Descriptor> list = new List<Descriptor>();
                list.AddRange(SimulatedTemperatureAdjuster.GetDescriptors(simulatedInternalTemperature));
                
                string formattedHeatEnergy = GameUtil.GetFormattedHeatEnergy(coolingHeatKW * 1000f);

                Descriptor item = 
                    new Descriptor(
                        string.Format(UI.BUILDINGEFFECTS.HEATGENERATED, formattedHeatEnergy),
                        string.Format(UI.BUILDINGEFFECTS.TOOLTIPS.HEATGENERATED, formattedHeatEnergy));
                list.Add(item);

                return list;
            }
        }

        public sealed class StatesInstance : GameInstance
        {
            [MyCmpReq]
            public Operational operational;
            [MyCmpReq]
            public Storage storage;

            public readonly HandleVector<int>.Handle structureTemperature;
            public readonly SimTT temperatureAdjuster;

            public StatesInstance(IStateMachineTarget master, Def def) : base(master, def)
            {
                temperatureAdjuster = new SimTT(def.simulatedInternalTemperature, def.simulatedInternalHeatCapacity, def.simulatedThermalConductivity, storage);
                structureTemperature = GameComps.StructureTemperatures.GetHandle(gameObject);
            }

            protected override void OnCleanUp()
            {
                temperatureAdjuster.CleanUp();
                base.OnCleanUp();
            }

            public float SaverPower => def.powerSaverEnergyUsage;

            public float NormalPower => GetComponent<EnergyConsumer>().WattsNeededWhenActive;

            public void SetEnergySaver(bool energySaving)
            {
                var ec = GetComponent<EnergyConsumer>();
                ec.BaseWattageRating = energySaving ? def.powerSaverEnergyUsage : ec.WattsNeededWhenActive;
            }

            public void ApplyCoolingExhaust(float dt) =>
                GameComps.StructureTemperatures.ProduceEnergy(structureTemperature, def.coolingHeatKW * dt, BUILDING.STATUSITEMS.OPERATINGENERGY.FOOD_TRANSFER, dt);

            public void ApplySteadyExhaust(float dt) =>
                GameComps.StructureTemperatures.ProduceEnergy(structureTemperature, def.steadyHeatKW * dt, BUILDING.STATUSITEMS.OPERATINGENERGY.FOOD_TRANSFER, dt);
        }

        public sealed class OperationalStates : State
        {
            public State cooling, steady;
        }
        #endregion

        public static StatusItem FridgeCooling1;
        public static StatusItem FridgeSteady1;

        public State inoperational;
        public OperationalStates operational;

        public override void InitializeStates(out BaseState default_state)
        {
            const string NAME = "Cooling exhaust";

            var operational = this.operational;

            default_state = inoperational;
            inoperational.EventTransition(GameHashes.OperationalFlagChanged, operational, IsOperational);

            operational
                .DefaultState(operational.steady)
                .EventTransition(GameHashes.OperationalFlagChanged, inoperational, IsNotOperational)
                .Enter(CB_SMI_ENABLE)
                .Exit(CB_SMI_DISABLE);

            var status = Db.Get().StatusItemCategories.Main;
            operational.cooling
                .Update(NAME, CB_APPLYCOOLINGEXHAUST, UpdateRate.SIM_200ms, true)
                .UpdateTransition(operational.steady, CB_ALLFOODCOOL, UpdateRate.SIM_4000ms, true)
                .ToggleStatusItem(FridgeCooling1, CB_RCX2RAX, status);

            operational.steady
                .Update(NAME, CB_APPLYSTEADYEXHAUST, UpdateRate.SIM_200ms, true)
                .UpdateTransition(operational.cooling, CB_ANYWARMFOOD, UpdateRate.SIM_4000ms, true)
                .ToggleStatusItem(FridgeSteady1, CB_RCX2RAX, status)
                .Enter(CB_ON_ENERGYSAVER)
                .Exit(CB_OFF_ENERGYSAVER);
        }

        private static void CB_OFF_ENERGYSAVER(StatesInstance smi) => smi.SetEnergySaver(false);
        private static void CB_ON_ENERGYSAVER(StatesInstance smi) => smi.SetEnergySaver(true);

        private static object CB_RCX2RAX(StatesInstance smi) => smi;

        private static bool CB_ALLFOODCOOL(StatesInstance smi, float _)
        {
            var list = smi.storage.items;
            var simIntTemp = smi.def.simulatedInternalTemperature + smi.def.activeCoolingStopBuffer;

            return ForEachFoodTempGE(list, simIntTemp);
        }
        private static bool CB_ANYWARMFOOD(StatesInstance smi, float _)
        {
            var list = smi.storage.items;
            var simInteTemp = smi.def.simulatedInternalTemperature + smi.def.activeCoolingStartBuffer;

            return !ForEachFoodTempGE(list, simInteTemp);
        }
        private static bool ForEachFoodTempGE(List<GameObject> list, float temperature)
        {
            var cnt = list.Count;
            for (int i = 0; i < cnt; i++)
            {
                var go = list[i];
                if (go != null)
                {
                    var pe = go.GetComponent<PrimaryElement>();
                    if (pe != null && pe.Mass >= 0.01f && pe.Temperature >= temperature)
                        return false;
                }
            }

            return true;
        }

        private static void ApplyExhaust(HandleVector<int>.Handle handle, float heatKw, float dt) =>
            GameComps.StructureTemperatures.ProduceEnergy(handle, heatKw * dt, BUILDING.STATUSITEMS.OPERATINGENERGY.FOOD_TRANSFER, dt);
        private static void CB_APPLYSTEADYEXHAUST(StatesInstance smi, float dt) => ApplyExhaust(smi.structureTemperature, smi.def.steadyHeatKW * dt, dt);
        private static void CB_APPLYCOOLINGEXHAUST(StatesInstance smi, float dt) => ApplyExhaust(smi.structureTemperature, smi.def.coolingHeatKW * dt, dt);

        private static void CB_SMI_DISABLE(StatesInstance smi)
        {
            smi.operational.SetActive(false);
            smi.temperatureAdjuster.SetActive(false);
        }
        private static void CB_SMI_ENABLE(StatesInstance smi)
        {
            smi.operational.SetActive(true);
            smi.temperatureAdjuster.SetActive(true);
        }

        private static bool IsOperational(StatesInstance smi) => smi.operational.GetFlag(EnergyConsumer.PoweredFlag);
        private static bool IsNotOperational(StatesInstance smi) => !IsOperational(smi);

        internal static void StatusItemInit()
        {
            const string COMMON = "STRINGS.BUILDING.STATUSITEMS.";

            var s1 = COMMON + nameof(FridgeCooling1).ToUpper();
            Strings.Add($"{s1}.NAME", BUILDING.STATUSITEMS.FRIDGECOOLING.NAME);
            Strings.Add($"{s1}.TOOLTIP", BUILDING.STATUSITEMS.FRIDGECOOLING.TOOLTIP);

            s1 = COMMON + nameof(FridgeSteady1).ToUpper();
            Strings.Add($"{s1}.NAME", BUILDING.STATUSITEMS.FRIDGESTEADY.NAME);
            Strings.Add($"{s1}.TOOLTIP", BUILDING.STATUSITEMS.FRIDGESTEADY.TOOLTIP);

            FridgeCooling1 = Create(nameof(FridgeCooling1), R_CB_NORMALPOWER_USED);
            FridgeSteady1 = Create(nameof(FridgeSteady1), R_CB_SAVERPOWER_USED);
        }

        private static StatusItem Create(string name, Func<string, object, string> callback) =>
            new StatusItem(name, nameof(BUILDING), string.Empty, 0, NotificationType.Neutral, false, OverlayModes.None.ID)
            {
                resolveStringCallback = callback
            };

        private static string R_CB_NORMALPOWER_USED(string str, object data)
        {
            var smi = (StatesInstance)data;
            var watt = GameUtil.GetFormattedWattage(smi.NormalPower);
            return str.Replace("{UsedPower}", watt).Replace("{MaxPower}", watt);
        }
        private static string R_CB_SAVERPOWER_USED(string str, object data)
        {
            var smi = (StatesInstance)data;
            var watt = GameUtil.GetFormattedWattage(smi.SaverPower);
            return str.Replace("{UsedPower}", watt).Replace("{MaxPower}", watt);
        }
    }
}
