using System;
using SuperComicLib;

namespace AdvancedGeneratos
{
	// Token: 0x02000005 RID: 5
	public sealed class ConsumGasPowerGenerator : AdvancedEnergyGenerator
	{
		// Token: 0x0600000C RID: 12 RVA: 0x000025E5 File Offset: 0x000007E5
		protected override void OnPrefabInit()
		{
			base.OnPrefabInit();
			this.Consumer.EnableConsumption(false);
		}

		// Token: 0x0600000D RID: 13 RVA: 0x000025F9 File Offset: 0x000007F9
		protected override void OnSpawn()
		{
			base.OnSpawn();
			this.Consumer.EnableConsumption(true);
		}

		// Token: 0x0600000E RID: 14 RVA: 0x0000260D File Offset: 0x0000080D
		protected override bool LogicOnCheckPre(bool isOn)
		{
			this.Consumer.EnableConsumption(isOn);
			return isOn;
		}

		// Token: 0x04000008 RID: 8
		[MyCmpGet]
		public ElementConsumer Consumer;
	}
}
