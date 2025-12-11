// The MIT License (MIT)
// Copyright (c) 2019-2025. Super Comic (ekfvoddl3535@naver.com)

using SuperComicLib.ModONI;

namespace AdvancedGenerators;

using static GlobalConsts;
public sealed class ModLoad : V1XXModImpl
{
    protected override void OnPreResourceLoad(ref StringResourceLoadContext context)
    {
        context.MappingStringBuilder
        .AddId(
            RefinedCarbonGenerator.ID,
            ThermoelectricGenerator.ID,
            NaphthaGenerator.ID,
            EcoFriendlyMethaneGenerator.ID,
            FusionPowerPlant.ID,
            ThermoconvertGenerator.ID)
        .AddVar("ThermoelectricGeneratorHeat", -(ThermoelectricGenerator.HEAT_SELF + ThermoelectricGenerator.HEAT_EXHAUST));
    }
    
    protected override void OnReady(ref InitializeContext context)
    {
        context.Utils.Power(
            RefinedCarbonGenerator.ID,
            ThermoelectricGenerator.ID,
            NaphthaGenerator.ID,
            EcoFriendlyMethaneGenerator.ID,
            FusionPowerPlant.ID,
            ThermoconvertGenerator.ID);

        context.Techs
        .AddTo(WellKnownTechId.AdvancedPowerRegulation,
            RefinedCarbonGenerator.ID,
            ThermoconvertGenerator.ID)
        .AddTo(WellKnownTechId.RenewableEnergy,
            ThermoelectricGenerator.ID,
            FusionPowerPlant.ID)
        .AddTo(WellKnownTechId.Plastics, NaphthaGenerator.ID)
        .AddTo(WellKnownTechId.ImprovedCombustion, EcoFriendlyMethaneGenerator.ID);
    }
}