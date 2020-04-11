using System;
using UnityEngine;

namespace CarbonatedWater
{
	// Token: 0x02000007 RID: 7
	internal static class Extend
	{
		// Token: 0x06000014 RID: 20 RVA: 0x00002460 File Offset: 0x00000660
		public static GameObject ExtendEntityToSpiecalFood(GameObject template, FoodMagicInfo info)
		{
			EntityTemplateExtensions.AddOrGet<EntitySplitter>(template);
			if (info.FoodInfo.CanRot)
			{
				Rottable.Def def = EntityTemplateExtensions.AddOrGetDef<Rottable.Def>(template);
				def.rotTemperature = info.FoodInfo.RotTemperature;
				def.spoilTime = info.FoodInfo.SpoilTime;
				def.staleTime = info.FoodInfo.SpoilTime;
				EntityTemplates.CreateAndRegisterCompostableFromPrefab(template);
			}
			KPrefabID compo = template.GetComponent<KPrefabID>();
			compo.AddTag(GameTags.PedestalDisplayable, false);
			if (info.FoodInfo.CaloriesPerUnit > 0f)
			{
				EntityTemplateExtensions.AddOrGet<Edible>(template).FoodInfo = info.FoodInfo;
				compo.instantiateFn += delegate(GameObject go)
				{
					go.GetComponent<Edible>().FoodInfo = info.FoodInfo;
				};
				GameTags.DisplayAsCalories.Add(compo.PrefabTag);
			}
			else
			{
				compo.AddTag(GameTags.CookingIngredient, false);
				EntityTemplateExtensions.AddOrGet<HasSortOrder>(template);
			}
			EntityTemplateExtensions.AddOrGet<MedicinalPill>(template).info = info.MedicineInfo;
			return template;
		}
	}
}
