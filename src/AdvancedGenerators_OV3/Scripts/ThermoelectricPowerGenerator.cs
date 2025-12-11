// The MIT License (MIT)
// Copyright (c) 2019-2025. Super Comic (ekfvoddl3535@naver.com)

namespace AdvancedGenerators;

public sealed class ThermoelectricPowerGenerator : AdvancedGeneratorBase
{
    public override void EnergySim200ms(float dt)
    {
        base.EnergySim200ms(dt);

        var op = operational;
        op.SetFlag(wireConnectedFlag, CircuitID != ushort.MaxValue);
        op.SetActive(op.IsOperational);

        if (HasMeter)
            meter.SetPositionPercent(op.IsActive ? 1 : 0);

        if (op.IsOperational)
        {
            GenerateJoules(WattageRating * dt, true);

            var db = Db.Get();
            selectable.SetStatusItem(db.StatusItemCategories.Power, db.BuildingStatusItems.Wattage, this);
        }
    }
}
