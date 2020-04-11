using System;
using System.Collections.Generic;
using Klei;
using STRINGS;
using UnityEngine;

namespace SupportPackage
{
	// Token: 0x0200000F RID: 15
	public class CookingStationEx : CookingStation
	{
		// Token: 0x06000031 RID: 49 RVA: 0x000032C8 File Offset: 0x000014C8
		protected override List<GameObject> SpawnOrderProduct(ComplexRecipe completed_order)
		{
			List<GameObject> gol = this.SpawnOrderProductBase(completed_order);
			for (int x = 0; x < gol.Count; x++)
			{
				this.OnSpawnItem(gol[x]);
			}
			base.GetComponent<Operational>().SetActive(false, false);
			return gol;
		}

		// Token: 0x06000032 RID: 50 RVA: 0x0000330C File Offset: 0x0000150C
		protected virtual List<GameObject> SpawnOrderProductBase(ComplexRecipe completed_order)
		{
			List<GameObject> gol = new List<GameObject>();
			SimUtil.DiseaseInfo disease = default(SimUtil.DiseaseInfo);
			float num = 0f;
			float tmax = completed_order.TotalResultUnits();
			int max;
			float num2;
			this.GetAmounts(completed_order.ingredients, out max, out num2);
			this.GetDiseaseInfo(completed_order.ingredients, max, num2, ref disease, ref num);
			int a = 0;
			max = completed_order.results.Length;
			while (a < max)
			{
				float amount = completed_order.results[a].amount;
				GameObject gameObject = this.buildStorage.FindFirst(completed_order.results[a].material);
				Edible compo = (gameObject != null) ? gameObject.GetComponent<Edible>() : null;
				if (compo != null && compo)
				{
					ReportManager.Instance.ReportValue(1, -compo.Calories, StringFormatter.Replace(UI.ENDOFDAYREPORT.NOTES.CRAFTED_USED, "{0}", KSelectableExtensions.GetProperName(compo)), UI.ENDOFDAYREPORT.NOTES.CRAFTED_CONTEXT);
				}
				ComplexFabricator.ResultState resultState = this.resultState;
				if (resultState > 1)
				{
					if (resultState == 2)
					{
						this.MeltOutStorageStore(completed_order.results[a]);
					}
				}
				else
				{
					gol.Add(this.ResultStateIsHot(completed_order.results[a], tmax, amount, num, disease));
					this.OutStorageStore(gol[gol.Count - 1]);
				}
				if (gol.Count > 0)
				{
					this.SymbolProcessing(gol[0]);
				}
				a++;
			}
			return gol;
		}

		// Token: 0x06000033 RID: 51 RVA: 0x00003465 File Offset: 0x00001665
		protected virtual void OnSpawnItem(GameObject go)
		{
			PrimaryElement component = go.GetComponent<PrimaryElement>();
			component.ModifyDiseaseCount(-component.DiseaseCount, "CookingStation.CompleteOrder");
			component.Temperature = 368.15f;
		}

		// Token: 0x06000034 RID: 52 RVA: 0x0000348C File Offset: 0x0000168C
		protected virtual void GetAmounts(ComplexRecipe.RecipeElement[] comod, out int maximum, out float count)
		{
			count = 0f;
			maximum = comod.Length;
			for (int x = 0; x < maximum; x++)
			{
				count += comod[x].amount;
			}
		}

		// Token: 0x06000035 RID: 53 RVA: 0x000034C0 File Offset: 0x000016C0
		protected virtual void GetDiseaseInfo(ComplexRecipe.RecipeElement[] comod, int maximum, float num2, ref SimUtil.DiseaseInfo diseaseInfo, ref float count)
		{
			for (int x = 0; x < maximum; x++)
			{
				SimUtil.DiseaseInfo temp;
				float tx;
				this.buildStorage.ConsumeAndGetDisease(comod[x].material, comod[x].amount, ref temp, ref tx);
				if (temp.count > diseaseInfo.count)
				{
					diseaseInfo = temp;
				}
				count += tx * (comod[x].amount / num2);
			}
		}

		// Token: 0x06000036 RID: 54 RVA: 0x00003524 File Offset: 0x00001724
		protected virtual GameObject ResultStateIsHot(ComplexRecipe.RecipeElement now, float max, float amount, float temp, SimUtil.DiseaseInfo disease)
		{
			GameObject go = GameUtil.KInstantiate(Assets.GetPrefab(now.material), 23, null, 0);
			int pos = Grid.PosToCell(this);
			TransformExtensions.SetPosition(go.transform, Grid.CellToPosCCC(pos, 23) + this.outputOffset);
			PrimaryElement component = go.GetComponent<PrimaryElement>();
			component.Units = amount;
			component.Temperature = temp;
			go.SetActive(true);
			component.AddDisease(disease.idx, Mathf.RoundToInt((float)disease.count * (amount / max)), "ComplexFabricator.CompleteOrder");
			go.GetComponent<KMonoBehaviour>().Trigger(748399584, null);
			return go;
		}

		// Token: 0x06000037 RID: 55 RVA: 0x000035BC File Offset: 0x000017BC
		protected virtual void OutStorageStore(GameObject go)
		{
			if (this.storeProduced && !this.outStorage.IsFull())
			{
				this.outStorage.Store(go, false, false, true, false);
			}
		}

		// Token: 0x06000038 RID: 56 RVA: 0x000035E4 File Offset: 0x000017E4
		protected virtual void MeltOutStorageStore(ComplexRecipe.RecipeElement now)
		{
			if (this.storeProduced && !this.outStorage.IsFull())
			{
				this.outStorage.AddLiquid(ElementLoader.GetElementID(now.material), now.amount, ElementLoader.GetElement(now.material).lowTemp + (ElementLoader.GetElement(now.material).highTemp - ElementLoader.GetElement(now.material).lowTemp) / 2f, 0, 0, false, true);
			}
		}

		// Token: 0x06000039 RID: 57 RVA: 0x00003660 File Offset: 0x00001860
		protected virtual void SymbolProcessing(GameObject first)
		{
			SymbolOverrideController com2 = base.GetComponent<SymbolOverrideController>();
			if (com2 != null)
			{
				KAnim.Build build = first.GetComponent<KBatchedAnimController>().AnimFiles[0].GetData().build;
				KAnim.Build.Symbol sb = build.GetSymbol(build.name);
				if (sb != null)
				{
					SymbolOverrideControllerUtil.TryRemoveSymbolOverride(com2, "output_tracker", 0);
					com2.AddSymbolOverride("output_tracker", sb, 0);
					return;
				}
				Debug.LogWarning("NULL SYMBOL !!");
			}
		}
	}
}
