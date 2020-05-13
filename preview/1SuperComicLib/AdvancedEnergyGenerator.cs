namespace SuperComicLib.Oni
{
    public class AdvancedEnergyGenerator : Generator
    {
        public const int OnActivateChangeFlag = 824508782;

		public bool HasMeter = true;
		public Storage InStorage;
		public Storage OutStorage;
		public Meter.Offset MeterOffset;
		public EnergyGenerator.Formula InOutItems;
		protected MeterController meter;

		protected static readonly EventSystem.IntraObjectHandler<AdvancedEnergyGenerator> OnActivateChangeDelegate = new EventSystem.IntraObjectHandler<AdvancedEnergyGenerator>(OnActivateChStatic);

        private static void OnActivateChStatic(AdvancedEnergyGenerator gen, object data) => gen.OnActivateChanged((Operational)data);

		protected virtual void OnActivateChanged(Operational data) { }
    }
}
