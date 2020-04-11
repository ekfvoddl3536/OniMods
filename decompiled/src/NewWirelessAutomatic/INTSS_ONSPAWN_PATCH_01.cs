using System;
using Harmony;

namespace NewWirelessAutomatic
{
	// Token: 0x02000008 RID: 8
	[HarmonyPatch(typeof(IntSliderSideScreen), "OnSpawn")]
	public class INTSS_ONSPAWN_PATCH_01
	{
		// Token: 0x06000011 RID: 17 RVA: 0x00002378 File Offset: 0x00000578
		public static void Postfix(IntSliderSideScreen __instance)
		{
			foreach (SliderSet sliderSet in __instance.sliderSets)
			{
				sliderSet.numberInput.field.characterLimit = 7;
			}
		}
	}
}
