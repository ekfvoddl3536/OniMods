// The MIT License (MIT)
// Copyright (c) 2019-2025. Super Comic (ekfvoddl3535@naver.com)

using KSerialization;
using UnityEngine;

namespace AdvancedGenerators;

using static GlobalConsts.ThermoconvertGenerator;
[SerializationConfig(MemberSerialization.OptIn)]
public sealed class ThermoconvertPowerGenerator : AdvancedGeneratorBase, IIntSliderControl, ISaveLoadable
{
    // [SerializeField]
    public Storage inStorage;
    // [SerializeField]
    public Storage outStorage;
    [Serialize]
    public float target_temp;

    private HandleVector<int>.Handle accumulator = HandleVector<int>.InvalidHandle;
    private Guid statusHandle;

    private int retryCnt;

    #region interface impl
    public string SliderTitleKey => SLIDER_KEY;
    public string SliderUnits => GameUtil.GetTemperatureUnitSuffix();

    public int SliderDecimalPlaces(int _) => 0;
    public float GetSliderMin(int _) => GameUtil.GetConvertedTemperature(MIN_TARGET_TEMP);
    public float GetSliderMax(int _) => GameUtil.GetConvertedTemperature(MAX_TARGET_TEMP);
    public float GetSliderValue(int _) => GameUtil.GetConvertedTemperature(target_temp);
    public void SetSliderValue(float value, int _) => target_temp = GameUtil.GetTemperatureConvertedToKelvin(value);
    public string GetSliderTooltipKey(int _) => SLIDER_TOOLTIP_KEY;
    public string GetSliderTooltip(int _) => Strings.Get(SLIDER_TOOLTIP_KEY).String;
    #endregion

    public float CurrentWattage => Game.Instance.accumulators.GetAverageRate(accumulator);

    protected override void OnCopySettings(GameObject from)
    {
        var source = from.GetComponent<ThermoconvertPowerGenerator>();

        target_temp = source.target_temp;
    }

    protected override void OnSpawn()
    {
        base.OnSpawn();

        accumulator = Game.Instance.accumulators.Add(null!, this);
    }

    protected override void OnCleanUp()
    {
        Game.Instance.accumulators.Remove(accumulator);

        base.OnCleanUp();
    }

    private void UpdateStatusItem()
    {
        var selectable = this.selectable;
        var statusHandle = this.statusHandle;

        selectable.RemoveStatusItem(Db.Get().BuildingStatusItems.Wattage);

        if (statusHandle == Guid.Empty)
            this.statusHandle = selectable.AddStatusItem(StatusWattage, this);
        else
            selectable.ReplaceStatusItem(statusHandle, StatusWattage, this);
    }

    public override void EnergySim200ms(float dt)
    {
        base.EnergySim200ms(dt);

        operational.SetFlag(wireConnectedFlag, CircuitID != ushort.MaxValue);
        if (operational.IsOperational == false) goto _RETURN;

        var list = inStorage.items;
        for (var idx = list.Count; --idx >= 0;)
        {
            var pe = list[idx].GetComponent<PrimaryElement>();

            list.RemoveAt(idx);

            if (pe.Mass == 0)
                continue;

            operational.SetActive(true);
            retryCnt = 0;

            var st = pe.Temperature;
            var et = target_temp;

            pe.Temperature = et;

            var mass = pe.Mass;
            var shc = pe.Element.specificHeatCapacity;

            var energy = (st - et) * shc * mass * SCALE_FACTOR;
            energy = Mathf.Clamp(energy * dt, 0f, WATT);

            // var storage = outStorage;
            // if (storage.items.Count != 0)
            //     storage.DropAll(true, true, new Vector3(1f, 0.5f, 0f));

            outStorage.Store(pe.gameObject, true, false, false);

            // var joules = energy * dt;

            Game.Instance.accumulators.Accumulate(accumulator, energy);
            if (energy > 0f)
                GenerateJoules(energy, true);

            UpdateStatusItem();
            goto _RETURN;
        }

        if (++retryCnt >= 10)
        {
            operational.SetActive(false);
            retryCnt = 0;
        }

    //  Giving hints to the stupid Mono JIT
    _RETURN:;
    }

    protected override void OnSpawnPost() { }
}
