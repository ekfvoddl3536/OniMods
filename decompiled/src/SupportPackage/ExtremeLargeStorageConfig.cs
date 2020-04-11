using System;

namespace SupportPackage
{
	// Token: 0x0200000B RID: 11
	public sealed class ExtremeLargeStorageConfig : LStorageBase
	{
		// Token: 0x06000028 RID: 40 RVA: 0x00003140 File Offset: 0x00001340
		public override BuildingDef CreateBuildingDef()
		{
			return base.Create("ExtremeLargeStorage", 5, 3, "gasstorage_kanim", 40f, IN_Constants.ExtremeLargeStorage.TMates, IN_Constants.ExtremeLargeStorage.TMass, 200000);
		}
	}
}
