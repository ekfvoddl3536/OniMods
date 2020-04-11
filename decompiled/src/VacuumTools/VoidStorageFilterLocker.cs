using System;

namespace VacuumTools
{
	// Token: 0x02000006 RID: 6
	public class VoidStorageFilterLocker : StorageLocker
	{
		// Token: 0x06000010 RID: 16 RVA: 0x00002281 File Offset: 0x00000481
		protected static void OnStorageChangedStatic(VoidStorageFilterLocker locker, object data)
		{
			locker.OnStorageChanged();
		}

		// Token: 0x06000011 RID: 17 RVA: 0x00002289 File Offset: 0x00000489
		protected override void OnSpawn()
		{
			base.OnSpawn();
			base.Subscribe<VoidStorageFilterLocker>(-1697596308, VoidStorageFilterLocker.OnStorageChangedDelegate);
			this.OnStorageChanged();
		}

		// Token: 0x06000012 RID: 18 RVA: 0x000022A8 File Offset: 0x000004A8
		protected virtual void OnStorageChanged()
		{
			this.storage.items.Clear();
		}

		// Token: 0x17000001 RID: 1
		// (get) Token: 0x06000013 RID: 19 RVA: 0x000022BA File Offset: 0x000004BA
		// (set) Token: 0x06000014 RID: 20 RVA: 0x000022C2 File Offset: 0x000004C2
		public override float UserMaxCapacity
		{
			get
			{
				return base.UserMaxCapacity;
			}
			set
			{
			}
		}

		// Token: 0x04000004 RID: 4
		protected const int StorageChangedFlag = -1697596308;

		// Token: 0x04000005 RID: 5
		protected static readonly EventSystem.IntraObjectHandler<VoidStorageFilterLocker> OnStorageChangedDelegate = new EventSystem.IntraObjectHandler<VoidStorageFilterLocker>(new Action<VoidStorageFilterLocker, object>(VoidStorageFilterLocker.OnStorageChangedStatic));

		// Token: 0x04000006 RID: 6
		[MyCmpReq]
		protected Storage storage;
	}
}
