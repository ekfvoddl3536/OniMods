using System;
using System.Collections.Generic;
using STRINGS;
using UnityEngine;

namespace SuperComicLib.HUSystem
{
	// Token: 0x02000012 RID: 18
	public class HUEnergyGenerator : HUGenerator, IEffectDescriptor
	{
		// Token: 0x06000029 RID: 41 RVA: 0x000032F4 File Offset: 0x000014F4
		public List<Descriptor> GetDescriptors(BuildingDef def)
		{
			List<Descriptor> dl = new List<Descriptor>();
			this.RequirementDescriptors(def, dl);
			this.EffectDescriptors(def, dl);
			return dl;
		}

		// Token: 0x0600002A RID: 42 RVA: 0x00003318 File Offset: 0x00001518
		protected virtual void RequirementDescriptors(BuildingDef def, List<Descriptor> list)
		{
			if (this.items.inputs.Length != 0)
			{
				foreach (EnergyGenerator.InputItem inputItem in this.items.inputs)
				{
					string str = GameTagExtensions.ProperName(inputItem.tag);
					string persec = GameUtil.GetFormattedMass(inputItem.consumptionRate, 2, 0, base.transform, "{0:0.##}");
					list.Add(new Descriptor(string.Format(UI.BUILDINGEFFECTS.ELEMENTCONSUMED, str, persec), string.Format(UI.BUILDINGEFFECTS.TOOLTIPS.ELEMENTCONSUMED, str, persec), 0, false));
				}
			}
		}

		// Token: 0x0600002B RID: 43 RVA: 0x000033AC File Offset: 0x000015AC
		protected virtual void EffectDescriptors(BuildingDef def, List<Descriptor> list)
		{
			if (this.generateHU > 0)
			{
				list.Add(new Descriptor(string.Format("HU 생산: +{0}/s", this.generateHU), string.Format(UIEX.HUPERSEC_PRODUCE, this.generateHU), 1, false));
			}
			if (this.items.outputs.Length != 0)
			{
				foreach (EnergyGenerator.OutputItem o in this.items.outputs)
				{
					string str = GameTagExtensions.ProperName(ElementLoader.FindElementByHash(o.element).tag);
					string creation = GameUtil.GetFormattedMass(o.creationRate, 2, 0, true, "{0:0.#}");
					Descriptor desc = default(Descriptor);
					if (o.minTemperature > 0f)
					{
						string temp = GameUtil.GetFormattedTemperature(o.minTemperature, 0, 0, true, false);
						desc.SetupDescriptor(string.Format(UI.BUILDINGEFFECTS.ELEMENTEMITTED_MINORENTITYTEMP, str, creation, temp), string.Format(UI.BUILDINGEFFECTS.TOOLTIPS.ELEMENTEMITTED_MINORENTITYTEMP, str, creation, temp), 1);
					}
					else
					{
						desc.SetupDescriptor(string.Format(UI.BUILDINGEFFECTS.ELEMENTEMITTED_ENTITYTEMP, str, creation), string.Format(UI.BUILDINGEFFECTS.TOOLTIPS.ELEMENTEMITTED_ENTITYTEMP, str, creation), 1);
					}
					list.Add(desc);
				}
			}
		}

		// Token: 0x0600002C RID: 44 RVA: 0x000034F0 File Offset: 0x000016F0
		protected override void OnSpawn()
		{
			base.OnSpawn();
			if (this.hasMeter)
			{
				this.meter = new MeterController(base.GetComponent<KBatchedAnimController>(), "meter_target", "meter", this.meterOffset, -2, new string[]
				{
					"meter_target",
					"meter_fill",
					"meter_frame",
					"meter_OL"
				});
			}
		}

		// Token: 0x0600002D RID: 45 RVA: 0x00003554 File Offset: 0x00001754
		public override void HUSim200ms(float dt)
		{
			base.HUSim200ms(dt);
			if (this.hasMeter)
			{
				EnergyGenerator.InputItem item = this.items.inputs[0];
				GameObject go = this.instorage.FindFirst(item.tag);
				if (go != null)
				{
					this.meter.SetPositionPercent(go.GetComponent<PrimaryElement>().Mass / item.maxStoredMass);
				}
				else
				{
					this.meter.SetPositionPercent(0f);
				}
			}
			if (this.operational.IsOperational)
			{
				this.convertible = this.IsConvertible(dt);
				this.selectable.ToggleStatusItem(Db.Get().BuildingStatusItems.NeedResourceMass, !this.convertible, this.items);
				this.operational.SetActive(this.convertible, false);
				if (this.convertible)
				{
					this.GeneratorWork(dt);
				}
			}
		}

		// Token: 0x0600002E RID: 46 RVA: 0x00003630 File Offset: 0x00001830
		protected virtual void GeneratorWork(float dt)
		{
			foreach (EnergyGenerator.InputItem i in this.items.inputs)
			{
				this.Consumed(i, dt);
			}
			foreach (EnergyGenerator.OutputItem o in this.items.outputs)
			{
				this.Emit(o, dt, this.primary);
			}
		}

		// Token: 0x0600002F RID: 47 RVA: 0x0000369C File Offset: 0x0000189C
		private void Emit(in EnergyGenerator.OutputItem output, float dt, PrimaryElement root_pe)
		{
			Element eh = ElementLoader.FindElementByHash(output.element);
			if (output.store)
			{
				if (eh.IsGas)
				{
					this.outstorage.AddGasChunk(output.element, output.creationRate * dt, Mathf.Max(root_pe.Temperature, output.minTemperature), byte.MaxValue, 0, true, true);
					return;
				}
				if (eh.IsLiquid)
				{
					this.outstorage.AddLiquid(output.element, output.creationRate * dt, Mathf.Max(root_pe.Temperature, output.minTemperature), byte.MaxValue, 0, true, true);
					return;
				}
				this.outstorage.Store(eh.substance.SpawnResource(TransformExtensions.GetPosition(base.transform), output.creationRate * dt, Mathf.Max(root_pe.Temperature, output.minTemperature), byte.MaxValue, 0, false, false, false), true, false, true, false);
				return;
			}
			else
			{
				if (eh.IsGas)
				{
					SimMessages.ModifyMass(Grid.OffsetCell(Grid.PosToCell(TransformExtensions.GetPosition(base.transform)), output.emitOffset), output.creationRate * dt, byte.MaxValue, 0, null, Mathf.Max(root_pe.Temperature, output.minTemperature), output.element);
					return;
				}
				if (eh.IsLiquid)
				{
					FallingWater.instance.AddParticle(Grid.OffsetCell(Grid.PosToCell(TransformExtensions.GetPosition(base.transform)), output.emitOffset), (byte)ElementLoader.GetElementIndex(output.element), output.creationRate * dt, Mathf.Max(root_pe.Temperature, output.minTemperature), byte.MaxValue, 0, true, false, false, false);
					return;
				}
				eh.substance.SpawnResource(Grid.CellToPosCCC(Grid.OffsetCell(Grid.PosToCell(TransformExtensions.GetPosition(base.transform)), output.emitOffset), 26), output.creationRate * dt, Mathf.Max(root_pe.Temperature, output.minTemperature), byte.MaxValue, 0, true, false, false);
				return;
			}
		}

		// Token: 0x06000030 RID: 48 RVA: 0x0000387D File Offset: 0x00001A7D
		private void Consumed(in EnergyGenerator.InputItem i, float dt)
		{
			this.instorage.ConsumeIgnoringDisease(i.tag, i.consumptionRate * dt);
		}

		// Token: 0x06000031 RID: 49 RVA: 0x00003898 File Offset: 0x00001A98
		protected virtual bool IsConvertible(float dt)
		{
			foreach (EnergyGenerator.InputItem i in this.items.inputs)
			{
				PrimaryElement pe = this.instorage.FindFirstWithMass(i.tag, 0f);
				if (pe == null || pe.Mass < i.consumptionRate * dt)
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x06000032 RID: 50 RVA: 0x000038F4 File Offset: 0x00001AF4
		public override int GenerateHeat(float dt)
		{
			if (!this.operational.IsOperational || !this.convertible)
			{
				return 0;
			}
			return this.generateHU;
		}

		// Token: 0x04000026 RID: 38
		[MyCmpGet]
		public Storage instorage;

		// Token: 0x04000027 RID: 39
		public Storage outstorage;

		// Token: 0x04000028 RID: 40
		public EnergyGenerator.Formula items;

		// Token: 0x04000029 RID: 41
		[MyCmpGet]
		protected PrimaryElement primary;

		// Token: 0x0400002A RID: 42
		[MyCmpGet]
		protected KSelectable selectable;

		// Token: 0x0400002B RID: 43
		protected bool convertible;

		// Token: 0x0400002C RID: 44
		public bool hasMeter;

		// Token: 0x0400002D RID: 45
		public Meter.Offset meterOffset;

		// Token: 0x0400002E RID: 46
		protected MeterController meter;
	}
}
