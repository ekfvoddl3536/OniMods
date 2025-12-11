// The MIT License (MIT)
// Copyright (c) 2019-2025. Super Comic (ekfvoddl3535@naver.com)

using SuperComicLib.ModONI;

namespace SupportPackage;

using static GlobalConsts;
using static GlobalConsts.SUPER_SUIT;
public sealed class ModLoad : V1XXModImpl
{
    protected override void OnPreResourceLoad(ref StringResourceLoadContext context)
    {
        context.MappingStringBuilder
#if PRIVATE_CORE
        .AddId(
            EasyGlassForge.ID,
            SolarLiquidHeater.ID,
            GermSlaughterExecutor.ID,
            EasyOilWellCap.ID,
            LavaMurder.ID,
            LiquidCopier.ID,
            GasCopier.ID)
#endif
        .AddId(
            EasyElectrolyzer.ID,
            TwoInOneGrill.ID,
            WastewaterSterilizer.ID,
            MagicTile.ID,
            OrganicDeoxidizer.ID,
            H2OSynthesizer.ID,
            CO2DecomposerEZ.ID)
        // merge super_suit
        .AddId(
            Basic_SuperSuit.ID,
            Advance_SuperSuit.ID,
            SmallBackpack.ID,
            MediumBackpack.ID,
            LargeBackpack.ID
        );
    }

    protected override void OnReady(ref InitializeContext context)
    {
        context.Utils
#if PRIVATE_CORE
        .Medical(GermSlaughterExecutor.ID)
        .Refining(EasyGlassForge.ID)
        .Utilities(
            SolarLiquidHeater.ID,
            EasyOilWellCap.ID,
            LavaMurder.ID,
            LiquidCopier.ID,
            GasCopier.ID)
#endif
        .Base(MagicTile.ID)
        .Oxygen(
            EasyElectrolyzer.ID,
            OrganicDeoxidizer.ID,
            CO2DecomposerEZ.ID)
        .Food(TwoInOneGrill.ID)
        .Refining(WastewaterSterilizer.ID)
        .Utilities(H2OSynthesizer.ID);

        SuperSuitOrigin.CONTINUE_PATCH.OnLoad();

        InOneRefrigeratorController.StatusItemInit();
    }
}
