using System;
using UnityEngine;

namespace SuperComicLib
{
	// Token: 0x02000002 RID: 2
	public class AdvancedEnergyGenerator : Generator
	{
		// Token: 0x06000001 RID: 1 RVA: 0x00002050 File Offset: 0x00000250
		private static void OnActivateChangedStatic(AdvancedEnergyGenerator gen, object data)
		{
			gen.OnActivateChanged(data as Operational);
		}

		// Token: 0x06000002 RID: 2 RVA: 0x00002060 File Offset: 0x00000260
		protected virtual void OnActivateChanged(Operational data)
		{
			this.selectable.SetStatusItem(Db.Get().StatusItemCategories.Power, data.IsActive ? Db.Get().BuildingStatusItems.Wattage : Db.Get().BuildingStatusItems.GeneratorOffline, this);
		}

		// Token: 0x06000003 RID: 3 RVA: 0x000020B1 File Offset: 0x000002B1
		protected override void OnPrefabInit()
		{
			base.OnPrefabInit();
			base.Subscribe<AdvancedEnergyGenerator>(824508782, AdvancedEnergyGenerator.OnActivateChangeDelegate);
		}

		// Token: 0x06000004 RID: 4 RVA: 0x000020CA File Offset: 0x000002CA
		protected override void OnSpawn()
		{
			base.OnSpawn();
			this.OnSpawnPost();
		}

		// Token: 0x06000005 RID: 5 RVA: 0x000020D8 File Offset: 0x000002D8
		protected virtual void OnSpawnPost()
		{
			if (this.InStorage == null || this.OutStorage == null || this.InOutItems.inputs == null || this.InOutItems.outputs == null)
			{
				throw new Exception("저장소 또는 input, output 이 지정되지 않았습니다.");
			}
			if (this.HasMeter)
			{
				this.meter = new MeterController(base.GetComponent<KBatchedAnimController>(), "meter_target", "meter", this.MeterOffset, -2, new string[]
				{
					"meter_target",
					"meter_fill",
					"meter_frame",
					"meter_OL"
				});
			}
		}

		// Token: 0x06000006 RID: 6 RVA: 0x00002178 File Offset: 0x00000378
		protected virtual bool IsConvertible(float dt)
		{
			int x = 0;
			int max = this.InOutItems.inputs.Length;
			while (x < max)
			{
				PrimaryElement pe = this.InStorage.FindFirstWithMass(this.InOutItems.inputs[x].tag, 0f);
				if (pe == null || pe.Mass < this.InOutItems.inputs[x].consumptionRate * dt)
				{
					return false;
				}
				x++;
			}
			return true;
		}

		// Token: 0x06000007 RID: 7 RVA: 0x000021EC File Offset: 0x000003EC
		public override void EnergySim200ms(float dt)
		{
			base.EnergySim200ms(dt);
			this.MeterUpdate(dt);
			this.GeneratePowerPre(dt, this.operational.IsOperational);
		}

		// Token: 0x06000008 RID: 8 RVA: 0x00002210 File Offset: 0x00000410
		protected virtual void MeterUpdate(float dt)
		{
			if (this.HasMeter)
			{
				EnergyGenerator.InputItem ind = this.InOutItems.inputs[0];
				float percent = 0f;
				PrimaryElement first = this.InStorage.FindFirstWithMass(ind.tag, 0f);
				if (first != null)
				{
					percent = first.Mass / ind.maxStoredMass;
				}
				this.meter.SetPositionPercent(percent);
			}
		}

		// Token: 0x06000009 RID: 9 RVA: 0x00002274 File Offset: 0x00000474
		protected virtual void GeneratePowerPre(float dt, bool logicOn)
		{
			this.operational.SetFlag(Generator.wireConnectedFlag, base.CircuitID != ushort.MaxValue);
			if (!this.LogicOnCheckPre(logicOn))
			{
				return;
			}
			bool flag = this.IsConvertible(dt);
			this.selectable.ToggleStatusItem(Db.Get().BuildingStatusItems.NeedResourceMass, !flag, this.InOutItems);
			this.operational.SetActive(flag, false);
			if (this.SetStatusPre(flag))
			{
				this.GenerateWork(dt);
			}
		}

		// Token: 0x0600000A RID: 10 RVA: 0x000022FA File Offset: 0x000004FA
		protected virtual bool LogicOnCheckPre(bool isOn)
		{
			return isOn;
		}

		// Token: 0x0600000B RID: 11 RVA: 0x000022FD File Offset: 0x000004FD
		protected virtual bool SetStatusPre(bool canConvert)
		{
			return canConvert;
		}

		// Token: 0x0600000C RID: 12 RVA: 0x00002300 File Offset: 0x00000500
		protected virtual void GenerateWork(float dt)
		{
			PrimaryElement com = base.GetComponent<PrimaryElement>();
			Vector3 now = TransformExtensions.GetPosition(base.transform);
			int x = 0;
			int max = this.InOutItems.inputs.Length;
			while (x < max)
			{
				this.ConsumedInput(this.InOutItems.inputs[x], dt);
				x++;
			}
			x = 0;
			max = this.InOutItems.outputs.Length;
			while (x < max)
			{
				this.EmitElements(com, now, this.InOutItems.outputs[x], dt);
				x++;
			}
			base.GenerateJoules(base.WattageRating * dt, false);
			this.selectable.SetStatusItem(Db.Get().StatusItemCategories.Power, Db.Get().BuildingStatusItems.Wattage, this);
		}

		// Token: 0x0600000D RID: 13 RVA: 0x000023C0 File Offset: 0x000005C0
		protected virtual void ConsumedInput(EnergyGenerator.InputItem inp, float dt)
		{
			this.InStorage.ConsumeIgnoringDisease(inp.tag, inp.consumptionRate * dt);
		}

		// Token: 0x0600000E RID: 14 RVA: 0x000023DC File Offset: 0x000005DC
		protected virtual void EmitElements(PrimaryElement root_pe, Vector3 now, EnergyGenerator.OutputItem oti, float dt)
		{
			Element elbh = ElementLoader.FindElementByHash(oti.element);
			float temp = Mathf.Max(root_pe.Temperature, oti.minTemperature);
			float rate = oti.creationRate * dt;
			if (oti.store)
			{
				if (elbh.IsGas)
				{
					this.OutStorage.AddGasChunk(oti.element, rate, temp, byte.MaxValue, 0, true, true);
					return;
				}
				if (elbh.IsLiquid)
				{
					this.OutStorage.AddLiquid(oti.element, rate, temp, byte.MaxValue, 0, true, true);
					return;
				}
				this.OutStorage.Store(elbh.substance.SpawnResource(now, rate, temp, byte.MaxValue, 0, false, false, false), true, false, true, false);
				return;
			}
			else
			{
				int pos = Grid.OffsetCell(Grid.PosToCell(now), oti.emitOffset);
				if (elbh.IsGas)
				{
					SimMessages.ModifyMass(pos, rate, byte.MaxValue, 0, CellEventLogger.Instance.EnergyGeneratorModifyMass, temp, oti.element);
					return;
				}
				if (elbh.IsLiquid)
				{
					FallingWater.instance.AddParticle(pos, (byte)ElementLoader.GetElementIndex(oti.element), rate, temp, byte.MaxValue, 0, true, false, false, false);
					return;
				}
				elbh.substance.SpawnResource(Grid.CellToPosCCC(pos, 26), rate, temp, byte.MaxValue, 0, true, false, false);
				return;
			}
		}

		// Token: 0x04000001 RID: 1
		protected const int OnActivateChangeFlag = 824508782;

		// Token: 0x04000002 RID: 2
		protected static readonly EventSystem.IntraObjectHandler<AdvancedEnergyGenerator> OnActivateChangeDelegate = new EventSystem.IntraObjectHandler<AdvancedEnergyGenerator>(new Action<AdvancedEnergyGenerator, object>(AdvancedEnergyGenerator.OnActivateChangedStatic));

		// Token: 0x04000003 RID: 3
		public bool HasMeter = true;

		// Token: 0x04000004 RID: 4
		public Storage InStorage;

		// Token: 0x04000005 RID: 5
		public Storage OutStorage;

		// Token: 0x04000006 RID: 6
		public Meter.Offset MeterOffset;

		// Token: 0x04000007 RID: 7
		public EnergyGenerator.Formula InOutItems;

		// Token: 0x04000008 RID: 8
		protected MeterController meter;
	}
}
