using System;
using System.Collections.Generic;
using UnityEngine;

namespace SuperComicLib
{
	// Token: 0x02000005 RID: 5
	public class CoolingTower : SimComponent, ISim1000ms
	{
		// Token: 0x0600001B RID: 27 RVA: 0x0000273C File Offset: 0x0000093C
		protected override void OnSpawn()
		{
			base.OnSpawn();
			if (this.cells == null)
			{
				this.cells = new Vector3[]
				{
					new Vector2(-1f, 0f),
					Vector2.zero,
					new Vector2(1f, 0f)
				};
			}
			if (this.inputStorage.GetMassAvailable(GameTags.Gas) > 0f)
			{
				this.DropAllGasElements();
			}
			if (this.outputStorage == null)
			{
				this.outputStorage = base.GetComponent<Storage>();
			}
		}

		// Token: 0x0600001C RID: 28 RVA: 0x000027E4 File Offset: 0x000009E4
		public virtual void Cooling()
		{
			PrimaryElement water = this.inputStorage.FindFirstWithMass(GameTags.Water, 0f);
			if (water != null && water.Temperature > this.minTemperature)
			{
				if (water.Temperature > this.maxTemperature)
				{
					Element steam = water.Element.highTempTransition;
					GasSourceManager.Instance.CreateChunk(steam.id, water.Mass * (1f - this.overTemperatureLoss), steam.lowTemp - GameUtil.CalculateEnergyDeltaForElement(water, water.Temperature, this.maxTemperature), water.DiseaseIdx, water.DiseaseCount, TransformExtensions.GetPosition(base.transform) + this.outputCenterCell);
					water.Mass *= this.overTemperatureLoss;
					return;
				}
				Vector3 pos = TransformExtensions.GetPosition(base.transform);
				int x = 0;
				int max = this.cells.Length;
				while (x < max)
				{
					SimMessages.SetElementConsumerData(this.simHandle, this.ToFinalCell(pos, this.cells[x]), this.airConsumptionRatePerCell);
					x++;
				}
				List<GameObject> gos = new List<GameObject>();
				this.inputStorage.Find(GameTags.Gas, gos);
				if (gos.Count > 0)
				{
					float cooling = Mathf.Max(water.Temperature - this.removeTemperature, this.minTemperature);
					float heat_balance = -GameUtil.CalculateEnergyDeltaForElementChange(water.Mass, water.Element.specificHeatCapacity, water.Temperature, cooling) / (float)gos.Count;
					for (int x2 = 0; x2 < gos.Count; x2++)
					{
						PrimaryElement pe = gos[x2].GetComponent<PrimaryElement>();
						pe.Temperature += GameUtil.CalculateTemperatureChange(pe.Element.specificHeatCapacity, pe.Mass, heat_balance * pe.Element.specificHeatCapacity * pe.Mass);
					}
					water.Temperature -= cooling;
					this.DropAllGasElements();
				}
				this.inputStorage.Transfer(water.gameObject, this.outputStorage, false, true);
			}
		}

		// Token: 0x0600001D RID: 29 RVA: 0x000029F0 File Offset: 0x00000BF0
		public void DropAllGasElements()
		{
			List<GameObject> gos = this.inputStorage.Drop(GameTags.Gas);
			int len = this.cells.Length;
			if (len > 0)
			{
				for (int x = 0; x < gos.Count; x++)
				{
					TransformExtensions.SetPosition(gos[x].transform, TransformExtensions.GetPosition(gos[x].transform) + this.outputCenterCell + this.cells[x % len]);
				}
			}
		}

		// Token: 0x0600001E RID: 30 RVA: 0x00002A6D File Offset: 0x00000C6D
		protected override void OnSimRegister(HandleVector<Game.ComplexCallbackInfo<int>>.Handle cbHnd)
		{
			SimMessages.AddElementConsumer(Grid.PosToCell(TransformExtensions.GetPosition(base.transform)), 2, 758759285, 1, cbHnd.index);
		}

		// Token: 0x0600001F RID: 31 RVA: 0x00002A92 File Offset: 0x00000C92
		protected int ToFinalCell(Vector3 centercell, Vector3 cell)
		{
			return Grid.PosToCell(cell + centercell);
		}

		// Token: 0x06000020 RID: 32 RVA: 0x00002AA0 File Offset: 0x00000CA0
		protected override Action<int> GetStaticUnregister()
		{
			return new Action<int>(this.SimUnregister);
		}

		// Token: 0x06000021 RID: 33 RVA: 0x00002AAF File Offset: 0x00000CAF
		protected virtual void SimUnregister(int sim_hnd)
		{
			SimMessages.RemoveElementConsumer(-1, sim_hnd);
		}

		// Token: 0x06000022 RID: 34 RVA: 0x00002AB8 File Offset: 0x00000CB8
		public void Sim1000ms(float dt)
		{
			this.Cooling();
		}

		// Token: 0x04000014 RID: 20
		public Vector3 outputCenterCell;

		// Token: 0x04000015 RID: 21
		public Vector3[] cells;

		// Token: 0x04000016 RID: 22
		public float airConsumptionRatePerCell = 2f;

		// Token: 0x04000017 RID: 23
		public float minTemperature = 278.15f;

		// Token: 0x04000018 RID: 24
		public float maxTemperature = 368.15f;

		// Token: 0x04000019 RID: 25
		public float removeTemperature = 65.15f;

		// Token: 0x0400001A RID: 26
		public float overTemperatureLoss = 0.5f;

		// Token: 0x0400001B RID: 27
		[MyCmpGet]
		public Storage inputStorage;

		// Token: 0x0400001C RID: 28
		public Storage outputStorage;
	}
}
