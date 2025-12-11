// The MIT License (MIT)
// Copyright (c) 2019-2025. Super Comic (ekfvoddl3535@naver.com)

#if PRIVATE_CORE
using KSerialization;

namespace SupportPackage;

using static GlobalConsts.SolarLiquidHeater;
using SELF_HANDLER = EventSystem.IntraObjectHandler<SolarLiquidBoiler>;
[SerializationConfig(MemberSerialization.OptIn)]
public sealed unsafe class SolarLiquidBoiler : KMonoBehaviour, IIntSliderControl, ISim200ms, ISecondaryOutput
{
    private static readonly SELF_HANDLER OnCopySettingsDelegate = new SELF_HANDLER(OnCopySettingsStatic);

    private static void OnCopySettingsStatic(SolarLiquidBoiler self, object data) => self.OnCopySettings((SolarLiquidBoiler)data);

    public Storage inStorage;
    public Storage outStorage;

    [Serialize]
    public float target_temp;

    private Operational operational;
    private HandleVector<int>.Handle accumulator = HandleVector<int>.InvalidHandle;

    private int[] lightGridCells;
    private WorldContainer world;

    private MeterController meter;

    private int retryCnt;

    // private int printOnce;

    #region slider interface
    public string SliderTitleKey => SLIDER_TITLE;
    public string SliderUnits => GameUtil.GetTemperatureUnitSuffix();

    public int SliderDecimalPlaces(int index) => 0;
    public float GetSliderMin(int index) => GameUtil.GetConvertedTemperature(MIN_TEMP);
    public float GetSliderMax(int index) => GameUtil.GetConvertedTemperature(MAX_TEMP);
    public float GetSliderValue(int index) => GameUtil.GetConvertedTemperature(target_temp);
    public void SetSliderValue(float value, int index) => target_temp = GameUtil.GetTemperatureConvertedToKelvin(value);
    public string GetSliderTooltipKey(int index) => SLIDER_TOOLTIP;
    public string GetSliderTooltip(int index) => Strings.Get(SLIDER_TOOLTIP).String;
    #endregion

    private void OnCopySettings(SolarLiquidBoiler source) => target_temp = source.target_temp;

    protected override void OnSpawn()
    {
        accumulator = Game.Instance.accumulators.Add(null!, this);

        var offsets = SolarCells;

        lightGridCells = new int[offsets.Length];
        var dst = lightGridCells;

        var bcell = Grid.PosToCell(this);
        world = ClusterManager.Instance.GetWorld(Grid.WorldIdx[bcell]);

        for (int i = 0; i < offsets.Length; ++i)
            dst[i] = Grid.OffsetCell(bcell, offsets[i]);

        operational = GetComponent<Operational>();

        meter = new MeterController(GetComponent<KBatchedAnimController>(), "meter_target", "meter", Meter.Offset.Infront, Grid.SceneLayer.NoLayer,
        [
            "meter_target",
            "meter_fill",
            "meter_frame",
            "meter_OL"
        ]);
    }

    protected override void OnCleanUp()
    {
        // UnsafeUtility.Free(lightGridCells, Unity.Collections.Allocator.Persistent);
        Game.Instance.accumulators.Remove(accumulator);
    }

    public void Sim200ms(float dt)
    {
        if (!operational.IsOperational)
            return;

        float sunlightIntensity =
            world != null
            ? world.currentSunlightIntensity
            : Game.Instance.currentFallbackSunlightIntensity;

        float sumX = 0.0f;
        int sumY = 0;

        var lights = lightGridCells;
        for (int i = 0; i < lights.Length; ++i)
        {
            var m = lights[i];

            sumX += UnityEngine.Mathf.Floor(Grid.exposedToSunlight[m] / (float)byte.MaxValue * sunlightIntensity);
            sumY += Grid.LightCount[m];
        }

        sumX = (sumX + sumY) * dt * HEAT_PER_LUX;
        if (sumX > 0.0f)
        {
            retryCnt = 0;
            operational.SetActive(true);

            //sumX *= dt;
            var H = sumX;

            //  need out storage empty
            if (outStorage.ExactMassStored() > ConduitFlow.MAX_LIQUID_MASS * 2)
                goto _SKIP;

            var list = inStorage.items;
            for (var idx = list.Count; --idx >= 0;)
            {
                var pe = list[idx].GetComponent<PrimaryElement>();

                list.RemoveAt(idx);

                var mass = pe.Mass;
                if (mass == 0) continue;

                var temp = pe.Temperature;
                if (temp >= target_temp)
                {
                    outStorage.Store(pe.gameObject, true, false, false);
                    continue;
                }

                var delta = target_temp - temp;
                var Mshc = pe.Element.specificHeatCapacity * mass;

                var Q = Mshc * delta * dt;
                if (Q > H)
                {
                    temp += H / Mshc;
                    H = 0;
                }
                else
                {
                    temp = target_temp;
                    H -= Q;
                }

                if (temp >= pe.Element.highTemp)
                    outStorage.AddGasChunk(pe.Element.highTempTransitionTarget, mass, temp, pe.DiseaseIdx, pe.DiseaseCount, false, false);
                else
                {
                    pe.Temperature = temp;
                    outStorage.Store(pe.gameObject, true, false, false);
                }

                if (H <= 0.0f)
                    break;
            }

        _SKIP:
            var H_used = 1.0f - H / sumX;
            var accs = Game.Instance.accumulators;

            accs.Accumulate(accumulator, H_used);
            meter.SetPositionPercent(accs.GetAverageRate(accumulator));
        }
        else if (++retryCnt > 10)
        {
            operational.SetActive(false);
        }
    }

    public bool HasSecondaryConduitType(ConduitType type) => type == ConduitType.Gas;
    public CellOffset GetSecondaryConduitOffset(ConduitType type) => GasOutputOffset;
}
#endif