using System.Reflection;

namespace SuperComicLib.Oni
{
    public static class ResearchKeys
	{
		public static readonly string
			FarmingTech,
			FineDining,
			FoodRepurposing,
			FinerDining,
			Agriculture,
			Ranching,
			AnimalControl,
			ImprovedOxygen,
			GasPiping,
			ImprovedGasPiping,
			PressureManagement,
			DirectedAirStreams,
			LiquidFiltering,
			MedicineI,
			MedicineII,
			MedicineIII,
			MedicineIV,
			LiquidPiping,
			ImprovedLiquidPiping,
			PrecisionPlumbing,
			SanitationSciences,
			FlowRedirection,
			AdvancedFiltration,
			Distillation,
			Catalytics,
			PowerRegulation,
			AdvancedPowerRegulation,
			PrettyGoodConductors,
			RenewableEnergy,
			Combustion,
			ImprovedCombustion,
			InteriorDecor,
			Artistry,
			Clothing,
			Acoustics,
			FineArt,
			EnvironmentalAppreciation,
			Luxury,
			RefractiveDecor,
			GlassFurnishings,
			Screens,
			RenaissanceArt,
			Plastics,
			ValveMiniaturization,
			Suits,
			Jobs,
			AdvancedResearch,
			NotificationSystems,
			ArtificialFriends,
			BasicRefinement,
			RefinedObjects,
			Smelting,
			HighTempForging,
			TemperatureModulation,
			HVAC,
			LiquidTemperature,
			LogicControl,
			GenericSensors,
			LogicCircuits,
			ParallelAutomation,
			DupeTrafficControl,
			Multiplexing,
			SkyDetectors,
			TravelTubes,
			SmartStorage,
			SolidTransport,
			SolidManagement,
			BasicRocketry,
			CargoI,
			CargoII,
			CargoIII,
			EnginesI,
			EnginesII,
			EnginesIII,
			Jetpacks;


		static ResearchKeys()
		{
			FieldInfo[] fds = typeof(ResearchKeys).GetFields(BindingFlags.Public | BindingFlags.Static);
			for (int x = fds.Length - 1; x >= 0; x--)
			{
				FieldInfo f = fds[x];
				f.SetValue(null, f.Name); // nameof()
			}
		}
	}
}
