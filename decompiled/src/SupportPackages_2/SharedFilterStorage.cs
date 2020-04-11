using System;
using UnityEngine;

namespace SupportPackages
{
	// Token: 0x02000009 RID: 9
	public class SharedFilterStorage
	{
		// Token: 0x06000021 RID: 33 RVA: 0x00002578 File Offset: 0x00000778
		public SharedFilterStorage(KMonoBehaviour _root, Tag[] reqs, Tag[] forbiddens, IUserControlledCapacity _control, ChoreType fetch_chore)
		{
			this.root = _root;
			this.requiredTags = reqs;
			this.forbiddenTags = forbiddens;
			this.sizeCtrl = _control;
			this.choret = fetch_chore;
			this.root.Subscribe(-1697596308, new Action<object>(this.StorageChanged));
			this.root.Subscribe(-543130682, new Action<object>(this.UserSettingsChanged));
			this.filterable = this.root.FindOrAdd<TreeFilterable>();
			TreeFilterable treeFilterable = this.filterable;
			treeFilterable.OnFilterChanged = (Action<Tag[]>)Delegate.Combine(treeFilterable.OnFilterChanged, new Action<Tag[]>(this.OnFilterChanged));
			this.storage = this.root.GetComponent<ShareStorage>();
			this.storage.Subscribe(644822890, delegate(object a)
			{
				this.OnFilterChanged(this.filterable.GetTags());
			});
			if (SharedFilterStorage.capStatus == null)
			{
				SharedFilterStorage.capStatus = new StatusItem("StorageLocker", "BUILDING", string.Empty, 0, 4, false, OverlayModes.None.ID, true, 63486);
				SharedFilterStorage.capStatus.resolveStringCallback = new Func<string, object, string>(this.CapacityStatusResolved);
				SharedFilterStorage.noFilterStatus = new StatusItem("NoStorageFilterSet", "BUILDING", "status_item_no_filter_set", 2, 3, false, OverlayModes.None.ID, true, 63486);
			}
			this.root.GetComponent<KSelectable>().SetStatusItem(Db.Get().StatusItemCategories.Main, SharedFilterStorage.capStatus, this);
		}

		// Token: 0x06000022 RID: 34 RVA: 0x00002704 File Offset: 0x00000904
		protected virtual string CapacityStatusResolved(string str, object dt)
		{
			SharedFilterStorage sfs = (SharedFilterStorage)dt;
			float amst = sfs.GetAmountStored;
			float b = sfs.storage.capacityKg;
			IUserControlledCapacity com = sfs.root.GetComponent<IUserControlledCapacity>();
			if (com != null)
			{
				b = Mathf.Min(com.UserMaxCapacity, b);
				str = str.Replace("{Units}", com.CapacityUnits);
			}
			else
			{
				str = str.Replace("{Units}", GameUtil.GetCurrentMassUnit(false));
			}
			str = str.Replace("{Stored}", Util.FormatWholeNumber((amst <= b - sfs.storage.storageFullMargin || amst >= b) ? Mathf.Floor(amst) : b));
			str = str.Replace("{Capacity}", Util.FormatWholeNumber(b));
			return str;
		}

		// Token: 0x06000023 RID: 35 RVA: 0x000027BC File Offset: 0x000009BC
		public virtual void SetHasMeter(bool _has)
		{
			this.hasMeter = _has;
		}

		// Token: 0x06000024 RID: 36 RVA: 0x000027C8 File Offset: 0x000009C8
		protected virtual void OnFilterChanged(Tag[] tags)
		{
			bool flag = tags != null && tags.Length != 0;
			this.root.GetComponent<KBatchedAnimController>().TintColour = (flag ? this.filterTint : this.noFilterTint);
			if (this.fetchList != null)
			{
				this.fetchList.Cancel(string.Empty);
				this.fetchList = null;
			}
			float ams = this.GetAmountStored;
			if (Mathf.Max(0f, this.GetMaxStorageMargin - ams) > 0f && flag)
			{
				this.fetchList = new FetchList2(this.storage, this.choret, null);
				this.fetchList.ShowStatusItem = false;
				this.fetchList.Add(tags, this.requiredTags, this.forbiddenTags, Mathf.Max(0f, this.GetMaxCapacity - ams), 1);
				this.fetchList.Submit(new Action(this.OnFetchComplete), false);
			}
			this.root.GetComponent<KSelectable>().ToggleStatusItem(SharedFilterStorage.noFilterStatus, !flag, this);
		}

		// Token: 0x06000025 RID: 37 RVA: 0x000028C8 File Offset: 0x00000AC8
		protected virtual void UserSettingsChanged(object data)
		{
			this.OnFilterChanged(this.filterable.GetTags());
			this.UpdateMeter();
		}

		// Token: 0x06000026 RID: 38 RVA: 0x000028E1 File Offset: 0x00000AE1
		protected virtual void StorageChanged(object data)
		{
			if (this.fetchList == null)
			{
				this.OnFilterChanged(this.filterable.GetTags());
			}
			SS_CONST.AnotherSpaceStorage.STATIC_ITEMS.Add(data as GameObject);
			this.UpdateMeter();
		}

		// Token: 0x06000027 RID: 39 RVA: 0x00002914 File Offset: 0x00000B14
		protected virtual void CreateMeter()
		{
			SharedFilterStorage.meter = new MeterController(this.root.GetComponent<KBatchedAnimController>(), "meter_target", "meter", 0, -2, new string[]
			{
				"meter_frame",
				"meter_level"
			});
		}

		// Token: 0x06000028 RID: 40 RVA: 0x0000295C File Offset: 0x00000B5C
		public virtual void CleanUp()
		{
			if (this.filterable != null)
			{
				TreeFilterable treeFilterable = this.filterable;
				treeFilterable.OnFilterChanged = (Action<Tag[]>)Delegate.Remove(treeFilterable.OnFilterChanged, new Action<Tag[]>(this.OnFilterChanged));
			}
			if (this.fetchList != null)
			{
				this.fetchList.Cancel("Parent destroyed");
			}
		}

		// Token: 0x06000029 RID: 41 RVA: 0x000029B7 File Offset: 0x00000BB7
		public virtual void NewFilter()
		{
			if (this.hasMeter && SharedFilterStorage.meter == null)
			{
				this.CreateMeter();
			}
			this.OnFilterChanged(this.filterable.GetTags());
			this.UpdateMeter();
		}

		// Token: 0x0600002A RID: 42 RVA: 0x000029E8 File Offset: 0x00000BE8
		public virtual bool IsFull()
		{
			float pf = Mathf.Clamp01(this.GetAmountStored / this.GetMaxStorageMargin);
			MeterController meterController = SharedFilterStorage.meter;
			if (meterController != null)
			{
				meterController.SetPositionPercent(pf);
			}
			return pf >= 1f;
		}

		// Token: 0x0600002B RID: 43 RVA: 0x00002A26 File Offset: 0x00000C26
		protected virtual void OnFetchComplete()
		{
			this.OnFilterChanged(this.filterable.GetTags());
		}

		// Token: 0x0600002C RID: 44 RVA: 0x00002A39 File Offset: 0x00000C39
		public virtual void UpdateMeter()
		{
			MeterController meterController = SharedFilterStorage.meter;
			if (meterController == null)
			{
				return;
			}
			meterController.SetPositionPercent(Mathf.Clamp01(this.GetAmountStored / this.GetMaxStorageMargin));
		}

		// Token: 0x17000007 RID: 7
		// (get) Token: 0x0600002D RID: 45 RVA: 0x00002A5E File Offset: 0x00000C5E
		protected float GetAmountStored
		{
			get
			{
				if (this.sizeCtrl == null)
				{
					return this.storage.MassStored();
				}
				return this.sizeCtrl.AmountStored;
			}
		}

		// Token: 0x17000008 RID: 8
		// (get) Token: 0x0600002E RID: 46 RVA: 0x00002A7F File Offset: 0x00000C7F
		protected float GetMaxCapacity
		{
			get
			{
				if (this.sizeCtrl == null)
				{
					return this.storage.capacityKg;
				}
				return Mathf.Min(this.storage.capacityKg, this.sizeCtrl.UserMaxCapacity);
			}
		}

		// Token: 0x17000009 RID: 9
		// (get) Token: 0x0600002F RID: 47 RVA: 0x00002AB0 File Offset: 0x00000CB0
		protected float GetMaxStorageMargin
		{
			get
			{
				return this.GetMaxCapacity - this.storage.storageFullMargin;
			}
		}

		// Token: 0x04000006 RID: 6
		public const int OnStorageChanged = -1697596308;

		// Token: 0x04000007 RID: 7
		public const int OnUserSettingsChanged = -543130682;

		// Token: 0x04000008 RID: 8
		public const int OnOnlyFetchItemsChanged = 644822890;

		// Token: 0x04000009 RID: 9
		public Color32 filterTint = FilteredStorage.FILTER_TINT;

		// Token: 0x0400000A RID: 10
		public Color32 noFilterTint = FilteredStorage.NO_FILTER_TINT;

		// Token: 0x0400000B RID: 11
		public static volatile MeterController meter;

		// Token: 0x0400000C RID: 12
		protected bool hasMeter = true;

		// Token: 0x0400000D RID: 13
		protected KMonoBehaviour root;

		// Token: 0x0400000E RID: 14
		protected FetchList2 fetchList;

		// Token: 0x0400000F RID: 15
		protected IUserControlledCapacity sizeCtrl;

		// Token: 0x04000010 RID: 16
		protected TreeFilterable filterable;

		// Token: 0x04000011 RID: 17
		protected ShareStorage storage;

		// Token: 0x04000012 RID: 18
		protected Tag[] requiredTags;

		// Token: 0x04000013 RID: 19
		protected Tag[] forbiddenTags;

		// Token: 0x04000014 RID: 20
		protected static StatusItem capStatus;

		// Token: 0x04000015 RID: 21
		protected static StatusItem noFilterStatus;

		// Token: 0x04000016 RID: 22
		protected ChoreType choret;
	}
}
