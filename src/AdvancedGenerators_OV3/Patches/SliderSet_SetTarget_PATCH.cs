// The MIT License (MIT)
// Copyright (c) 2019-2025. Super Comic (ekfvoddl3535@naver.com)

using HarmonyLib;
using UnityEngine;

namespace AdvancedGenerators;

[HarmonyPatch(typeof(SliderSet), nameof(SliderSet.SetTarget))]
public static class SliderSet_SetTarget_PATCH
{
    public static void Postfix(SliderSet __instance, ISliderControl target)
    {
        if (target is ThermoconvertPowerGenerator)
        {
            const int LIMIT = GlobalConsts.ThermoconvertGenerator.InputFieldCharacterLimit;

            var input = __instance.numberInput;
            input.field.characterLimit = LIMIT;

            var rect = input.GetComponent<RectTransform>();
            var size = rect.sizeDelta;
            size.x = (LIMIT + 1) * 10;
            rect.sizeDelta = size;
        }
    }
}