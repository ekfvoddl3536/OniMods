namespace SuperComicLib.Oni
{
    public static class TemperatureUtil
	{
		public static float CalculateGetEnergyChange(this PrimaryElement primary, float add_temperature) =>
			CalculateLostEnergyChange(primary, -add_temperature);

		public static float CalculateLostEnergyChange(this PrimaryElement primary, float remove_temperature) => 
			GameUtil.CalculateEnergyDeltaForElementChange(primary.Mass, primary.Element.specificHeatCapacity, primary.Temperature, primary.Temperature - remove_temperature);
	}
}
