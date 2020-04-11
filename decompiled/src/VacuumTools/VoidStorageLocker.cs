using System;

namespace VacuumTools
{
	// Token: 0x02000005 RID: 5
	public class VoidStorageLocker : KMonoBehaviour
	{
		// Token: 0x0600000B RID: 11 RVA: 0x0000222E File Offset: 0x0000042E
		protected static void OnStorageChangedStatic(VoidStorageLocker locker, object data)
		{
			locker.OnStorageChanged();
		}

		// Token: 0x0600000C RID: 12 RVA: 0x00002236 File Offset: 0x00000436
		protected override void OnSpawn()
		{
			base.Subscribe<VoidStorageLocker>(-1697596308, VoidStorageLocker.OnStorageChangedDelegate);
			this.OnStorageChanged();
		}

		// Token: 0x0600000D RID: 13 RVA: 0x0000224F File Offset: 0x0000044F
		protected virtual void OnStorageChanged()
		{
			this.storage.items.Clear();
		}

		// Token: 0x04000001 RID: 1
		protected const int StorageChangedFlag = -1697596308;

		// Token: 0x04000002 RID: 2
		protected static readonly EventSystem.IntraObjectHandler<VoidStorageLocker> OnStorageChangedDelegate = new EventSystem.IntraObjectHandler<VoidStorageLocker>(new Action<VoidStorageLocker, object>(VoidStorageLocker.OnStorageChangedStatic));

		// Token: 0x04000003 RID: 3
		[MyCmpReq]
		protected Storage storage;
	}
}
