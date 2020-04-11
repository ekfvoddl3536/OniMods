using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace EasyFarming
{
	// Token: 0x0200000F RID: 15
	public static class CON
	{
		// Token: 0x06000011 RID: 17 RVA: 0x00002F34 File Offset: 0x00001134
		public static void FUNC(GameObject go)
		{
			DiseaseDropper.Def def = StateMachineControllerExtensions.GetDef<DiseaseDropper.Def>(go);
			def.diseaseIdx = byte.MaxValue;
			def.emitFrequency = (float)(def.singleEmitQuantity = (def.averageEmitPerSecond = 0));
		}

		// Token: 0x06000012 RID: 18 RVA: 0x00002F6B File Offset: 0x0000116B
		// Note: this type is marked as 'beforefieldinit'.
		static CON()
		{
			SimHashes[] array = new SimHashes[7];
			RuntimeHelpers.InitializeArray(array, fieldof(<PrivateImplementationDetails>.093CBDDB3EDBE196B42519BEE66D992DE68D2F4F843C4C2F17B31DB4C3D67FEA).FieldHandle);
			CON.air = array;
			CON.CP = new List<Tag>
			{
				GameTags.CropSeed
			};
		}

		// Token: 0x0400000F RID: 15
		public const string CR = "CreatePrefab";

		// Token: 0x04000010 RID: 16
		public static readonly SimHashes[] air;

		// Token: 0x04000011 RID: 17
		public static readonly List<Tag> CP;
	}
}
