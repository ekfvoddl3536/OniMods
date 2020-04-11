using System;

namespace SupportPackage
{
	// Token: 0x0200000D RID: 13
	public sealed class LargeStorageConfig : LStorageBase
	{
		// Token: 0x0600002B RID: 43 RVA: 0x00003184 File Offset: 0x00001384
		public override BuildingDef CreateBuildingDef()
		{
			return base.Create("LargeStorage", 2, 3, "liquidreservoir_kanim", 30f, IN_Constants.LargeStorage.TMates, IN_Constants.LargeStorage.TMass, 80000);
		}
	}
}
