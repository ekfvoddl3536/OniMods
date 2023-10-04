// MIT License
//
// Copyright (c) 2022-2023. Super Comic (ekfvoddl3535@naver.com)
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in all
// copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
// SOFTWARE.

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
