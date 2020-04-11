using System;
using System.Collections.Generic;
using Harmony;

namespace SuperVest
{
	// Token: 0x02000007 RID: 7
	[HarmonyPatch(typeof(ClothingFabricatorConfig), "ConfigureRecipes")]
	public sealed class FAB_PATCH_SV_01
	{
		// Token: 0x06000016 RID: 22 RVA: 0x000028C8 File Offset: 0x00000AC8
		public static void Postfix()
		{
			Tag me = TagExtensions.ToTag("ClothingFabricator");
			FAB_PATCH_SV_01.Add(8, "Basic_SuperVest", 220, SV_CONST.Basic_SuperVest.RECIPE, me);
			FAB_PATCH_SV_01.Add(20, "Advance_SuperSuit", 1000, SV_CONST.Advance_SuperSuit.RECIPE, me);
			FAB_PATCH_SV_01.Add(4, "SmallBackpack", 30, SV_CONST.SmallBackpack.RECIPE, me);
			FAB_PATCH_SV_01.Add(8, "MediumBackpack", 60, SV_CONST.MediumBackpack.RECIPE, me);
			FAB_PATCH_SV_01.Add(20, "LargeBackpack", 700, SV_CONST.LargeBackpack.RECIPE, me);
		}

		// Token: 0x06000017 RID: 23 RVA: 0x00002964 File Offset: 0x00000B64
		private static void Add(int mass, string id, int fabTime, string recipe, Tag me)
		{
			ComplexRecipe.RecipeElement[] inp = new ComplexRecipe.RecipeElement[]
			{
				new ComplexRecipe.RecipeElement(TagExtensions.ToTag("BasicFabric"), (float)mass)
			};
			ComplexRecipe.RecipeElement[] outp = new ComplexRecipe.RecipeElement[]
			{
				new ComplexRecipe.RecipeElement(id, 1f)
			};
			ComplexRecipe complexRecipe = new ComplexRecipe(ComplexRecipeManager.MakeRecipeID("ClothingFabricator", inp, outp), inp, outp);
			complexRecipe.time = (float)fabTime;
			complexRecipe.description = recipe;
			complexRecipe.nameDisplay = 1;
			complexRecipe.fabricators = new List<Tag>
			{
				me
			};
		}
	}
}
