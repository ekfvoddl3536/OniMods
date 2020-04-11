using System;
using Harmony;
using UnityEngine;

namespace EasyFarming
{
	// Token: 0x02000003 RID: 3
	[HarmonyPatch(typeof(FlowerVaseConfig), "ConfigureBuildingTemplate")]
	public static class FVC_P_SB
	{
		// Token: 0x06000002 RID: 2 RVA: 0x00002058 File Offset: 0x00000258
		public static bool Prefix(GameObject go)
		{
			EntityTemplateExtensions.AddOrGet<Storage>(go);
			Prioritizable.AddRef(go);
			PlantablePlot plantablePlot = EntityTemplateExtensions.AddOrGet<PlantablePlot>(go);
			plantablePlot.AddDepositTag(GameTags.DecorSeed);
			plantablePlot.AddDepositTag("ColdBreatherSeed");
			EntityTemplateExtensions.AddOrGet<FlowerVase>(go);
			go.GetComponent<KPrefabID>().AddTag(GameTags.Decoration, false);
			return false;
		}
	}
}
