// MIT License
//
// Copyright (c) 2022-2023. SuperComic (ekfvoddl3535@naver.com)
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

using KSerialization;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace AdvancedGenerators
{
    using static GlobalConsts.ThermoconvertGenerator;
    [SerializationConfig(MemberSerialization.OptIn)]
    public sealed class ThermoconvertPowerGenerator : AdvancedGeneratorBase, IIntSliderControl, ISaveLoadable
    {
        [SerializeField]
        public Storage inStorage;
        [SerializeField]
        public Storage outStorage;
        [Serialize]
        public float target_temp;
        [Serialize]
        public float convert_kg;
        private int retryCnt;

        #region interface impl
        public string SliderTitleKey => SLIDER_KEY;
        public string SliderUnits => GameUtil.GetTemperatureUnitSuffix();
        public int SliderDecimalPlaces(int _) => 0;
        public float GetSliderMin(int _) => GetFromKelvin(MIN_TARGET_TEMP);
        public float GetSliderMax(int _) => GetFromKelvin(MAX_TARGET_TEMP);
        public float GetSliderValue(int _) => GetFromKelvin(target_temp);
        public void SetSliderValue(float value, int _) => target_temp = (float)GetToKelvin(value);
        public string GetSliderTooltipKey(int _) => SLIDER_TOOLTIP_KEY;
        public string GetSliderTooltip(int _) => Strings.Get(SLIDER_TOOLTIP_KEY).String;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static float GetFromKelvin(float v) => Mathf.Round(GameUtil.GetTemperatureConvertedFromKelvin(v, GameUtil.temperatureUnit));
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static double GetToKelvin(double v)
        {
            switch (GameUtil.temperatureUnit)
            {
                case GameUtil.TemperatureUnit.Celsius:
                    return v + 273.15;
                case GameUtil.TemperatureUnit.Fahrenheit:
                    return (v + 459.67) * 0.5555555555555555555555555555d;
                default:
                    return v + 0.15;
            }
        }
        #endregion

        public override void EnergySim200ms(float dt)
        {
            base.EnergySim200ms(dt);

            var id = CircuitID;
            operational.SetFlag(wireConnectedFlag, id != ushort.MaxValue);
            if (operational.IsOperational == false)
                return;

            var list = inStorage.items;
            for (var idx = list.Count; --idx >= 0;)
            {
                var pe = list[idx].GetComponent<PrimaryElement>();

                list.RemoveAt(idx);

                if (pe.Mass == 0)
                    continue;

                operational.SetActive(true);
                retryCnt = 0;

                var bt = pe.Temperature;
                pe.Temperature = Mathf.Min(bt, target_temp);
                // pe.Mass *= 0.96875f; // NOTE: O.P. PATCH!

                var storage = outStorage;
                if (storage.items.Count != 0)
                    storage.DropAll(true, true, new Vector3(1f, 0.5f, 0f));

                storage.Store(pe.gameObject, true, false, false);

                var tt = Mathf.Clamp(bt - target_temp, 0f, WATT);
                building.Def.GeneratorWattageRating = tt;
                GenerateJoules(tt * dt, true);

                return;
            }

            if (++retryCnt >= 10)
            {
                operational.SetActive(false);
                retryCnt = 0;
            }
        }

        protected override void OnSpawnPost() { }
    }
}
