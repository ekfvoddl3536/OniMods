// The MIT License (MIT)
// Copyright (c) 2019-2025. Super Comic (ekfvoddl3535@naver.com)

using HarmonyLib;

namespace FastWirelessAutomation;

[HarmonyPatch(typeof(Game), "OnSpawn")]
public static class Game_OnSpawn_Patch_OV2
{
    public static void Postfix() => Networks.ChannelManager.Reset();
}
