using System;
using KSerialization;
using UnityEngine;

namespace SupportPackages
{
	// Token: 0x02000008 RID: 8
	public class SharedStorageLocker : KMonoBehaviour, IUserControlledCapacity
	{
		// Token: 0x06000011 RID: 17 RVA: 0x000023D8 File Offset: 0x000005D8
		protected static void OnCopySetting_Static(SharedStorageLocker st, object data)
		{
			st.OnCopySetting(data);
		}

		// Token: 0x06000012 RID: 18 RVA: 0x000023E4 File Offset: 0x000005E4
		protected virtual void OnCopySetting(object data)
		{
			GameObject go = data as GameObject;
			if (go != null)
			{
				SharedStorageLocker com = go.GetComponent<SharedStorageLocker>();
				if (com != null)
				{
					this.UserMaxCapacity = com.UserMaxCapacity;
				}
			}
		}

		// Token: 0x06000013 RID: 19 RVA: 0x00002411 File Offset: 0x00000611
		protected override void OnPrefabInit()
		{
			this.filterStorage = new SharedFilterStorage(this, null, null, this, Db.Get().ChoreTypes.StorageFetch);
			base.Subscribe<SharedStorageLocker>(-905833192, SharedStorageLocker.OnCopySettingDelegate);
		}

		// Token: 0x06000014 RID: 20 RVA: 0x00002443 File Offset: 0x00000643
		protected override void OnSpawn()
		{
			this.filterStorage.NewFilter();
			if (Util.IsNullOrWhiteSpace(this.lockerName))
			{
				return;
			}
			this.SetNameWhitout(this.lockerName);
		}

		// Token: 0x06000015 RID: 21 RVA: 0x0000246C File Offset: 0x0000066C
		protected override void OnCleanUp()
		{
			this.filterStorage.CleanUp();
		}

		// Token: 0x06000016 RID: 22 RVA: 0x0000247C File Offset: 0x0000067C
		public virtual void SetName(string _name)
		{
			base.name = _name;
			this.lockerName = _name;
			KSelectable com = base.GetComponent<KSelectable>();
			if (com != null)
			{
				com.SetName(_name);
			}
			base.gameObject.name = _name;
			NameDisplayScreen.Instance.UpdateName(base.gameObject);
		}

		// Token: 0x06000017 RID: 23 RVA: 0x000024C4 File Offset: 0x000006C4
		protected void SetNameWhitout(string _name)
		{
			base.name = _name;
			KSelectable com = base.GetComponent<KSelectable>();
			if (com != null)
			{
				com.SetName(_name);
			}
			base.gameObject.name = _name;
			NameDisplayScreen.Instance.UpdateName(base.gameObject);
		}

		// Token: 0x17000001 RID: 1
		// (get) Token: 0x06000018 RID: 24 RVA: 0x00002505 File Offset: 0x00000705
		// (set) Token: 0x06000019 RID: 25 RVA: 0x00002512 File Offset: 0x00000712
		public virtual float UserMaxCapacity
		{
			get
			{
				return base.GetComponent<ShareStorage>().capacityKg;
			}
			set
			{
				this.filterStorage.NewFilter();
			}
		}

		// Token: 0x17000002 RID: 2
		// (get) Token: 0x0600001A RID: 26 RVA: 0x00002521 File Offset: 0x00000721
		public float AmountStored
		{
			get
			{
				return base.GetComponent<ShareStorage>().MassStored();
			}
		}

		// Token: 0x17000003 RID: 3
		// (get) Token: 0x0600001B RID: 27 RVA: 0x0000252E File Offset: 0x0000072E
		public float MinCapacity
		{
			get
			{
				return 0f;
			}
		}

		// Token: 0x17000004 RID: 4
		// (get) Token: 0x0600001C RID: 28 RVA: 0x00002535 File Offset: 0x00000735
		public float MaxCapacity
		{
			get
			{
				return base.GetComponent<ShareStorage>().capacityKg;
			}
		}

		// Token: 0x17000005 RID: 5
		// (get) Token: 0x0600001D RID: 29 RVA: 0x00002542 File Offset: 0x00000742
		public bool WholeValues
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000006 RID: 6
		// (get) Token: 0x0600001E RID: 30 RVA: 0x00002545 File Offset: 0x00000745
		public LocString CapacityUnits
		{
			get
			{
				return GameUtil.GetCurrentMassUnit(false);
			}
		}

		// Token: 0x04000002 RID: 2
		protected const int OnCopySettings = -905833192;

		// Token: 0x04000003 RID: 3
		protected static readonly EventSystem.IntraObjectHandler<SharedStorageLocker> OnCopySettingDelegate = new EventSystem.IntraObjectHandler<SharedStorageLocker>(new Action<SharedStorageLocker, object>(SharedStorageLocker.OnCopySetting_Static));

		// Token: 0x04000004 RID: 4
		[Serialize]
		public string lockerName = string.Empty;

		// Token: 0x04000005 RID: 5
		public volatile SharedFilterStorage filterStorage;
	}
}
