using System;
using UnityEngine;

namespace SuperComicLib.HUSystem
{
	// Token: 0x02000008 RID: 8
	public abstract class HUBuildingConfig : IBuildingConfig
	{
		// Token: 0x06000006 RID: 6 RVA: 0x00002174 File Offset: 0x00000374
		public override void DoPostConfigureComplete(GameObject go)
		{
			EntityTemplateExtensions.AddOrGet<BuildingCellVisualizer>(go);
		}

		// Token: 0x06000007 RID: 7 RVA: 0x0000217D File Offset: 0x0000037D
		public override void DoPostConfigureUnderConstruction(GameObject go)
		{
			EntityTemplateExtensions.AddOrGet<BuildingCellVisualizer>(go);
		}

		// Token: 0x06000008 RID: 8 RVA: 0x00002186 File Offset: 0x00000386
		public override void DoPostConfigurePreview(BuildingDef def, GameObject go)
		{
			EntityTemplateExtensions.AddOrGet<BuildingCellVisualizer>(go);
		}
	}
}
