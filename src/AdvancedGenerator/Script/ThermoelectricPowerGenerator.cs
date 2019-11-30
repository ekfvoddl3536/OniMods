namespace AdvancedGeneratos
{
    using static EventSystem;
    public class ThermoelectricPowerGenerator : Generator
    {
        protected const int OnActivateChangeFlag = 824508782;
        protected static readonly IntraObjectHandler<ThermoelectricPowerGenerator> OnActivateChangeDelegate = new IntraObjectHandler<ThermoelectricPowerGenerator>(OnActivateChangedStatic);

        public bool HasMeter = true;
        public Meter.Offset MeterOffset = Meter.Offset.Infront;
        protected MeterController meter;

        protected static void OnActivateChangedStatic(ThermoelectricPowerGenerator gen, object data) =>
            gen.OnActivateChanged(data as Operational);

        protected override void OnPrefabInit()
        {
            base.OnPrefabInit();
            Subscribe(OnActivateChangeFlag, OnActivateChangeDelegate);
        }

        protected override void OnSpawn()
        {
            base.OnSpawn();

            if (HasMeter)
                meter = new MeterController(GetComponent<KBatchedAnimController>(), "meter_target", "meter", MeterOffset, Grid.SceneLayer.NoLayer, new[]
                {
                    "meter_target",
                    "meter_fill",
                    "meter_frame",
                    "meter_OL"
                });
        }

        protected virtual void OnActivateChanged(Operational op) =>
            selectable.SetStatusItem(Db.Get().StatusItemCategories.Power, op.IsActive ?
                Db.Get().BuildingStatusItems.Wattage :
                Db.Get().BuildingStatusItems.GeneratorOffline, this);

        public override void EnergySim200ms(float dt)
        {
            base.EnergySim200ms(dt);

            operational.SetFlag(wireConnectedFlag, CircuitID != ushort.MaxValue);
            operational.SetActive(operational.IsOperational);

            if (HasMeter)
                MeterSet();

            if (!operational.IsOperational)
                return;

            GenerateJoules(WattageRating * dt);
            selectable.SetStatusItem(Db.Get().StatusItemCategories.Power, Db.Get().BuildingStatusItems.Wattage, this);
        }

        protected virtual void MeterSet() => meter.SetPositionPercent(operational.IsActive ? 1 : 0);
    }
}
