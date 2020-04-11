using System;
using System.Collections.Generic;
using Harmony;
using STRINGS;
using UnityEngine;

namespace AbyssaliteRecipe
{
	// Token: 0x02000002 RID: 2
	[HarmonyPatch(typeof(SupermaterialRefineryConfig), "ConfigureBuildingTemplate")]
	public static class SUPERREF_PATCH_001
	{
		// Token: 0x06000001 RID: 1 RVA: 0x00002050 File Offset: 0x00000250
		public static void Postfix(GameObject go, Tag prefab_tag)
		{
			Tag ot = TagManager.Create("SupermaterialRefinery");
			Tag snd = GameTagExtensions.CreateTag(381796644);
			Tag ss = GameTagExtensions.CreateTag(493438017);
			SUPERREF_PATCH_001.Add(ot, ELEMENTS.ISORESIN.DESC, 80f, true, new ComplexRecipe.RecipeElement(GameTagExtensions.CreateTag(1071649902), 100f), new ComplexRecipe.RecipeElement[]
			{
				new ComplexRecipe.RecipeElement(GameTagExtensions.CreateTag(-2008682336), 50f),
				new ComplexRecipe.RecipeElement(snd, 50f)
			});
			SUPERREF_PATCH_001.Add(ot, ELEMENTS.FULLERENE.DESC, 60f, false, new ComplexRecipe.RecipeElement(GameTagExtensions.CreateTag(245514112), 100f), new ComplexRecipe.RecipeElement[]
			{
				new ComplexRecipe.RecipeElement(GameTagExtensions.CreateTag(947100397), 100f),
				new ComplexRecipe.RecipeElement(GameTagExtensions.CreateTag(-2079931820), 50f),
				new ComplexRecipe.RecipeElement(GameTagExtensions.CreateTag(-279785280), 50f)
			});
			SUPERREF_PATCH_001.Add(ot, ELEMENTS.SANDSTONE.DESC, 20f, false, new ComplexRecipe.RecipeElement(ss, 100f), new ComplexRecipe.RecipeElement[]
			{
				new ComplexRecipe.RecipeElement(snd, 100f)
			});
			SUPERREF_PATCH_001.Add(ot, ELEMENTS.BLEACHSTONE.DESC, 5f, false, new ComplexRecipe.RecipeElement(GameTagExtensions.CreateTag(-839728230), 10f), new ComplexRecipe.RecipeElement[]
			{
				new ComplexRecipe.RecipeElement(GameTagExtensions.CreateTag(-1324664829), 5f),
				new ComplexRecipe.RecipeElement(GameTagExtensions.CreateTag(1624244999), 5f),
				new ComplexRecipe.RecipeElement(GameTagExtensions.CreateTag(1306370440), 0.015f)
			});
			SUPERREF_PATCH_001.Add(ot, ELEMENTS.PHOSPHORITE.DESC, 30f, false, new ComplexRecipe.RecipeElement(GameTagExtensions.CreateTag(-877427037), 100f), new ComplexRecipe.RecipeElement[]
			{
				new ComplexRecipe.RecipeElement(ss, 100f),
				new ComplexRecipe.RecipeElement(GameTagExtensions.CreateTag(-1396791454), 50f)
			});
		}

		// Token: 0x06000002 RID: 2 RVA: 0x00002248 File Offset: 0x00000448
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
			ComplexRecipe complexRecipe = new ComplexRecipe(ComplexRecipeManager.MakeRecipeID("SupermaterialRefinery", __input, __output), __input, __output);
			complexRecipe.time = _time;
			complexRecipe.description = desc;
			complexRecipe.nameDisplay = 1;
			complexRecipe.fabricators = new List<Tag>
			{
				_tag
			};
		}

		// Token: 0x04000001 RID: 1
		public const string KID = "SupermaterialRefinery";
	}
}
