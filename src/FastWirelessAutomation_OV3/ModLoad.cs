// The MIT License (MIT)
// Copyright (c) 2019-2025. Super Comic (ekfvoddl3535@naver.com)

using SuperComicLib.ModONI;

namespace FastWirelessAutomation;

using static GlobalConsts;
public sealed class ModLoad : V1XXModImpl
{
    protected override void OnPreResourceLoad(ref StringResourceLoadContext context)
    {
        context.MappingStringBuilder
        .AddId(
            FastWirelessEmitter.ID,
            FastWirelessReceiver.ID);
    }

    protected override void OnReady(ref InitializeContext context)
    {
        context.Utils.Automation(
            FastWirelessEmitter.ID,
            FastWirelessReceiver.ID);

        context.Techs
        .AddTo(WellKnownTechId.DupeTrafficControl,
            FastWirelessEmitter.ID,
            FastWirelessReceiver.ID);
    }
}
