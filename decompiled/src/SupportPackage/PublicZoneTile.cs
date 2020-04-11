using System;
using ProcGen;

namespace SupportPackage
{
	// Token: 0x02000012 RID: 18
	public class PublicZoneTile : KMonoBehaviour
	{
		// Token: 0x06000041 RID: 65 RVA: 0x00003745 File Offset: 0x00001945
		protected override void OnPrefabInit()
		{
			if (PublicZoneTile.OnObjectReplacedDelegate == null)
			{
				PublicZoneTile.OnObjectReplacedDelegate = new EventSystem.IntraObjectHandler<PublicZoneTile>(new Action<PublicZoneTile, object>(PublicZoneTile.TempDelegate));
			}
		}

		// Token: 0x06000042 RID: 66 RVA: 0x00003764 File Offset: 0x00001964
		protected static void TempDelegate(PublicZoneTile com, object data)
		{
			com.OnObjectReplaced(data);
		}

		// Token: 0x06000043 RID: 67 RVA: 0x00003770 File Offset: 0x00001970
		protected override void OnSpawn()
		{
			int x = 0;
			int max = this.building.PlacementCells.Length;
			while (x < max)
			{
				SimMessages.ModifyCellWorldZone(this.building.PlacementCells[x], 0);
				x++;
			}
			base.Subscribe<PublicZoneTile>(1606648047, PublicZoneTile.OnObjectReplacedDelegate);
		}

		// Token: 0x06000044 RID: 68 RVA: 0x000037BB File Offset: 0x000019BB
		protected override void OnCleanUp()
		{
			if (this.wasReplaced)
			{
				return;
			}
			this.ClearZone();
		}

		// Token: 0x06000045 RID: 69 RVA: 0x000037CC File Offset: 0x000019CC
		protected virtual void OnObjectReplaced(object data)
		{
			this.ClearZone();
			this.wasReplaced = true;
		}

		// Token: 0x06000046 RID: 70 RVA: 0x000037DC File Offset: 0x000019DC
		protected virtual void ClearZone()
		{
			foreach (int pc in this.building.PlacementCells)
			{
				SubWorld.ZoneType sb = World.Instance.zoneRenderData.GetSubWorldZoneType(pc);
				SimMessages.ModifyCellWorldZone(pc, (sb != 7) ? sb : byte.MaxValue);
			}
		}

		// Token: 0x04000014 RID: 20
		protected static EventSystem.IntraObjectHandler<PublicZoneTile> OnObjectReplacedDelegate;

		// Token: 0x04000015 RID: 21
		[MyCmpReq]
		public Building building;

		// Token: 0x04000016 RID: 22
		protected bool wasReplaced;
	}
}
