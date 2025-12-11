// The MIT License (MIT)
// Copyright (c) 2019-2025. Super Comic (ekfvoddl3535@naver.com)

using HarmonyLib;
using SuperComicLib.ModONI;

namespace EcoFriendlyToilet;

using static GlobalConsts;
public sealed class ModLoad : V1XXModImpl
{
    public override void OnLoad(Harmony harmony)
    {
        GlobalVars.FerzToilet_ =
            new StatusItem(
                EcoFriendlyToilet.ID,
                "BUILDING",
                string.Empty,
                StatusItem.IconType.Info,
                NotificationType.Neutral,
                false,
                OverlayModes.None.ID)
            {
                resolveStringCallback = STATIC_CALLBACK
            };
            
        base.OnLoad(harmony);
    }

    protected override void OnPreResourceLoad(ref StringResourceLoadContext context)
    {
        context.MappingStringBuilder
        .AddId(
            EcoFriendlyToilet.ID,
            AutoPurifyWashsink.ID,
            EcoFriendlyWildFarmTile.ID,
            EcoFriendlyWildFlowerPot.ID,
            EcoFriendlyWildHangingPot.ID);
    }

    protected override void OnReady(ref InitializeContext context)
    {
        context.Utils
            .Plumbing(EcoFriendlyToilet.ID)
            .Medical(AutoPurifyWashsink.ID)
            .Food(EcoFriendlyWildFarmTile.ID)
            .Furniture(EcoFriendlyWildFlowerPot.ID, EcoFriendlyWildHangingPot.ID);

        context.Techs
        .AddTo(WellKnownTechId.Agriculture, EcoFriendlyWildFarmTile.ID)
        .AddTo(WellKnownTechId.InteriorDecor,
            EcoFriendlyWildFlowerPot.ID,
            EcoFriendlyWildHangingPot.ID);
    }

    protected override void OnPatch(Harmony harmony) { } // no has patchable classes
    
    private static string STATIC_CALLBACK(string data, object arg) =>
        arg is FerzToilet.StatesInstance smi
        ? data.Replace("{FlushesRemaining}", (EcoFriendlyToilet.MAX_FLUSHES - smi.master._flushesUsed).ToString())
        : data;
}