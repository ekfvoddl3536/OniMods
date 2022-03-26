using ProcGen;

namespace SupportPackage
{
    public class PublicZoneTile : KMonoBehaviour
    {
		protected static readonly EventSystem.IntraObjectHandler<PublicZoneTile> OnObjectReplacedDelegate = new EventSystem.IntraObjectHandler<PublicZoneTile>(TempDelegate);

		[MyCmpReq]
		public Building building;
		protected bool wasReplaced;

        protected static void TempDelegate(PublicZoneTile com, object data) => com.OnObjectReplaced(data);

        protected override void OnSpawn()
		{
			int max = building.PlacementCells.Length;
			for (int x = 0; x < max; x++)
				SimMessages.ModifyCellWorldZone(building.PlacementCells[x], 0);

			Subscribe((int)GameHashes.ObjectReplaced, OnObjectReplacedDelegate);
		}

		protected override void OnCleanUp()
		{
			if (!wasReplaced)
				ClearZone();
		}

		protected virtual void OnObjectReplaced(object data)
		{
			ClearZone();
			wasReplaced = true;
		}

		protected virtual unsafe void ClearZone()
		{
			var list = building.PlacementCells;
			for (int i = 0, max = list.Length; i < max; i++)
			{
				var pc = list[i];

				int sb = (int)World.Instance.zoneRenderData.GetSubWorldZoneType(pc);
				
				bool temp = sb != (int)SubWorld.ZoneType.Space;

				SimMessages.ModifyCellWorldZone(pc, (byte)(sb * *(byte*)&temp));
			}
		}
	}
}
