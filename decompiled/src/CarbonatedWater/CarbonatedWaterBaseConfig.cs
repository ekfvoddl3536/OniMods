using System;
using UnityEngine;

namespace CarbonatedWater
{
	// Token: 0x0200000A RID: 10
	public abstract class CarbonatedWaterBaseConfig : IEntityConfig
	{
		// Token: 0x06000017 RID: 23
		public abstract GameObject CreatePrefab();

		// Token: 0x06000018 RID: 24 RVA: 0x000027E2 File Offset: 0x000009E2
		public virtual void OnPrefabInit(GameObject inst)
		{
		}

		// Token: 0x06000019 RID: 25 RVA: 0x000027E4 File Offset: 0x000009E4
		public virtual void OnSpawn(GameObject inst)
		{
		}
	}
}
