using System;
using KSerialization;
using UnityEngine;

namespace EasyFarming
{
	// Token: 0x02000014 RID: 20
	[SerializationConfig(1)]
	public class WildPlantablePlot : PlantablePlot, ISaveLoadable, IEffectDescriptor, IGameObjectEffectDescriptor
	{
		// Token: 0x0600001B RID: 27 RVA: 0x000031CC File Offset: 0x000013CC
		protected virtual void SyncPriority(PrioritySetting setting)
		{
			Prioritizable com = base.GetComponent<Prioritizable>();
			if (com.GetMasterPriority() != setting)
			{
				com.SetMasterPriority(setting);
			}
			if (base.occupyingObject != null)
			{
				Prioritizable com2 = base.occupyingObject.GetComponent<Prioritizable>();
				if (com2 != null && com2.GetMasterPriority() != setting)
				{
					com2.SetMasterPriority(com.GetMasterPriority());
				}
			}
		}

		// Token: 0x0600001C RID: 28 RVA: 0x0000322C File Offset: 0x0000142C
		protected override void OnSpawn()
		{
			KPrefabID temp = base.plant;
			base.plant = null;
			base.OnSpawn();
			if (temp != null)
			{
				base.plant = temp;
				this.SetOccupyInternal(base.plant.gameObject);
			}
			this.autoReplaceEntity = false;
			Components.PlantablePlots.Add(this);
			Prioritizable component = base.GetComponent<Prioritizable>();
			component.onPriorityChanged = (Action<PrioritySetting>)Delegate.Combine(component.onPriorityChanged, new Action<PrioritySetting>(this.SyncPriority));
		}

		// Token: 0x0600001D RID: 29 RVA: 0x000032A8 File Offset: 0x000014A8
		public override void OrderRemoveOccupant()
		{
			if (base.Occupant != null)
			{
				Uprootable com = base.Occupant.GetComponent<Uprootable>();
				if (com != null)
				{
					com.MarkForUproot(true);
				}
			}
		}

		// Token: 0x0600001E RID: 30 RVA: 0x000032DC File Offset: 0x000014DC
		public override GameObject SpawnOccupyingObject(GameObject go)
		{
			PlantableSeed ps = go.GetComponent<PlantableSeed>();
			if (ps == null)
			{
				return null;
			}
			Vector3 poscbc = Grid.CellToPosCBC(Grid.PosToCell(this), this.plantLayer);
			GameObject pt = GameUtil.KInstantiate(Assets.GetPrefab(ps.PlantID), poscbc, this.plantLayer, null, 0);
			pt.SetActive(true);
			base.plant = pt.GetComponent<KPrefabID>();
			this.SetOccupyInternal(pt);
			UprootedMonitor up = pt.GetComponent<UprootedMonitor>();
			if (up)
			{
				up.canBeUprooted = false;
			}
			this.autoReplaceEntity = false;
			Prioritizable com = base.GetComponent<Prioritizable>();
			if (com != null)
			{
				Prioritizable com2 = pt.GetComponent<Prioritizable>();
				if (com2 != null)
				{
					com2.SetMasterPriority(com.GetMasterPriority());
					Prioritizable prioritizable = com2;
					prioritizable.onPriorityChanged = (Action<PrioritySetting>)Delegate.Combine(prioritizable.onPriorityChanged, new Action<PrioritySetting>(this.SyncPriority));
				}
			}
			return pt;
		}

		// Token: 0x0600001F RID: 31 RVA: 0x000033AC File Offset: 0x000015AC
		protected virtual void SetOccupyInternal(GameObject go)
		{
			base.occupyingObject = go;
			HarvestDesignatable po = go.GetComponent<HarvestDesignatable>();
			if (po != null)
			{
				po.SetHarvestWhenReady(true);
			}
		}

		// Token: 0x04000012 RID: 18
		protected const int OccupyObjHash = -216549700;
	}
}
