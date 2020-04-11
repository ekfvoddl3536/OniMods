using System;
using System.Collections.Generic;
using KSerialization;
using STRINGS;

namespace SuperComicLib.HUSystem
{
	// Token: 0x02000011 RID: 17
	[SerializationConfig(1)]
	public class HUBoiler : HUConsumer
	{
		// Token: 0x0600001F RID: 31 RVA: 0x00002E84 File Offset: 0x00001084
		public override List<Descriptor> GetDescriptors(BuildingDef def)
		{
			List<Descriptor> dl = new List<Descriptor>();
			this.RequirementsDescriptors(def, dl);
			this.EffectDescriptors(def, dl);
			return dl;
		}

		// Token: 0x06000020 RID: 32 RVA: 0x00002EA8 File Offset: 0x000010A8
		protected virtual void EffectDescriptors(BuildingDef def, List<Descriptor> list)
		{
			if (this.descript_items.outputs.Length != 0)
			{
				foreach (EnergyGenerator.OutputItem o in this.descript_items.outputs)
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

		// Token: 0x06000021 RID: 33 RVA: 0x00002FA8 File Offset: 0x000011A8
		protected virtual void RequirementsDescriptors(BuildingDef def, List<Descriptor> list)
		{
			if (this.HUMin > 0)
			{
				list.Add(new Descriptor(string.Format("최소 HU 소비: -{0}/s", this.HUMin), string.Format(UIEX.HUPERSEC_CONSUMED, "최소", this.HUMin), 1, false));
				if (this.HUMax >= this.HUMin)
				{
					list.Add(new Descriptor(string.Format("최대 HU 소비: -{0}/s", this.HUMax), string.Format(UIEX.HUPERSEC_CONSUMED, "최대", this.HUMax), 1, false));
				}
			}
			if (this.descript_items.inputs.Length != 0)
			{
				foreach (EnergyGenerator.InputItem inputItem in this.descript_items.inputs)
				{
					string str = GameTagExtensions.ProperName(inputItem.tag);
					string persec = GameUtil.GetFormattedMass(inputItem.consumptionRate, 2, 0, base.transform, "{0:0.##}");
					list.Add(new Descriptor(string.Format(UI.BUILDINGEFFECTS.ELEMENTCONSUMED, str, persec), string.Format(UI.BUILDINGEFFECTS.TOOLTIPS.ELEMENTCONSUMED, str, persec), 0, false));
				}
			}
		}

		// Token: 0x06000022 RID: 34 RVA: 0x000030D8 File Offset: 0x000012D8
		protected override void OnSpawn()
		{
			base.OnSpawn();
			this.savedTemp = -1f;
		}

		// Token: 0x06000023 RID: 35 RVA: 0x000030EC File Offset: 0x000012EC
		public override void HUSim200ms(float dt)
		{
			base.HUSim200ms(dt);
			if (this.operational.IsOperational)
			{
				PrimaryElement res;
				bool convertible = this.IsConvertible(dt, out res);
				this.selectable.ToggleStatusItem(Db.Get().BuildingStatusItems.NeedResourceMass, !convertible, this.descript_items);
				bool canstarting = convertible & this.consumedHU >= this.HUMin;
				this.operational.SetActive(canstarting, false);
				if (canstarting)
				{
					this.BoilerWork(dt, res);
				}
			}
		}

		// Token: 0x06000024 RID: 36 RVA: 0x00003170 File Offset: 0x00001370
		protected virtual void BoilerWork(float dt, PrimaryElement element)
		{
			if (this.savedTemp < 0f)
			{
				float temp = element.Temperature + (float)this.consumedHU / element.Mass * dt;
				if (temp < this.lowTemperature)
				{
					this.savedTemp = temp;
					return;
				}
				this.OnBoilerWork(element.Element.idx, (float)this.convertingMass * dt, temp, element.DiseaseIdx, element.DiseaseCount);
				return;
			}
			else
			{
				this.savedTemp += (float)this.consumedHU / element.Mass * dt;
				if (this.savedTemp >= this.lowTemperature)
				{
					this.OnBoilerWork(element.Element.idx, (float)this.convertingMass * dt, element.Temperature, element.DiseaseIdx, element.DiseaseCount);
					this.savedTemp = -1f;
					return;
				}
				element.Temperature = this.savedTemp;
				return;
			}
		}

		// Token: 0x06000025 RID: 37 RVA: 0x0000324C File Offset: 0x0000144C
		protected virtual void OnBoilerWork(byte idx, float mass, float temperature, byte diseaseIdx, int diseaseCount)
		{
			this.storage.ConsumeIgnoringDisease(GameTags.Water, mass);
			FallingWater.instance.AddParticle(this.building.GetRotatedOffsetCell(this.emitOffset), idx, mass, temperature, diseaseIdx, diseaseCount, true, false, false, false);
		}

		// Token: 0x06000026 RID: 38 RVA: 0x00003290 File Offset: 0x00001490
		protected virtual bool IsConvertible(float dt, out PrimaryElement result)
		{
			result = this.storage.FindFirstWithMass(GameTags.Water, 0f);
			return result != null && result.Mass >= (float)this.convertingMass * dt;
		}

		// Token: 0x06000027 RID: 39 RVA: 0x000032CA File Offset: 0x000014CA
		public override int ConsumedHU(int HUavailable, float dt)
		{
			this.consumedHU = base.ConsumedHU(HUavailable, dt);
			return this.consumedHU;
		}

		// Token: 0x0400001E RID: 30
		[MyCmpGet]
		public Storage storage;

		// Token: 0x0400001F RID: 31
		public EnergyGenerator.Formula descript_items;

		// Token: 0x04000020 RID: 32
		[MyCmpGet]
		protected KSelectable selectable;

		// Token: 0x04000021 RID: 33
		protected int consumedHU;

		// Token: 0x04000022 RID: 34
		[Serialize]
		public float lowTemperature = 413.15f;

		// Token: 0x04000023 RID: 35
		public CellOffset emitOffset;

		// Token: 0x04000024 RID: 36
		protected float savedTemp;

		// Token: 0x04000025 RID: 37
		public int convertingMass;
	}
}
