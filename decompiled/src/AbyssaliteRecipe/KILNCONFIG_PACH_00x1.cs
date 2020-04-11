using System;
using System.Collections.Generic;
using Harmony;
using STRINGS;
using UnityEngine;

namespace AbyssaliteRecipe
{
	// Token: 0x02000003 RID: 3
	[HarmonyPatch(typeof(KilnConfig), "ConfigureBuildingTemplate")]
	public static class KILNCONFIG_PACH_00x1
	{
		// Token: 0x06000003 RID: 3 RVA: 0x000022B4 File Offset: 0x000004B4
		public static void Postfix(GameObject go, Tag prefab_tag)
		{
			KILNCONFIG_PACH_00x1.Add(TagManager.Create("Kiln"), ELEMENTS.CARBON.DESC, 35f, false, new ComplexRecipe.RecipeElement(GameTagExtensions.CreateTag(947100397), 100f), new ComplexRecipe.RecipeElement[]
			{
				new ComplexRecipe.RecipeElement(WoodLogConfig.TAG, 100f)
			});
		}

		// Token: 0x06000004 RID: 4 RVA: 0x0000230C File Offset: 0x0000050C
		private static void Add(Tag _tag, string desc, float _time, bool inoutReverse, ComplexRecipe.RecipeElement result, params ComplexRecipe.RecipeElement[] inputs)
		{
			ComplexRecipe.RecipeElement[] array;
			if (!inoutReverse)
			{
				array = inputs;
			}
			else
			{
				(array = new ComplexRecipe.RecipeElement[1])[0] = result;
			}
			ComplexRecipe.RecipeElement[] __input = array;
			ComplexRecipe.RecipeElement[] array2;
			if (!inoutReverse)
			{
				(array2 = new ComplexRecipe.RecipeElement[1])[0] = result;
			}
			else
			{
				array2 = inputs;
			}
			ComplexRecipe.RecipeElement[] __output = array2;
			ComplexRecipe complexRecipe = new ComplexRecipe(ComplexRecipeManager.MakeRecipeID("Kiln", __input, __output), __input, __output);
			complexRecipe.time = _time;
			complexRecipe.description = desc;
			complexRecipe.nameDisplay = 1;
			complexRecipe.fabricators = new List<Tag>
			{
				_tag
			};
		}

		// Token: 0x04000002 RID: 2
		public const string ID = "Kiln";
	}
}
