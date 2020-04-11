using System;
using System.Reflection;
using KSerialization;

namespace SuperComicLib
{
	// Token: 0x02000004 RID: 4
	[SerializationConfig(1)]
	public class NotRequirePoweredEnergyConsumer : EnergyConsumer
	{
		// Token: 0x17000001 RID: 1
		// (get) Token: 0x06000015 RID: 21 RVA: 0x00002699 File Offset: 0x00000899
		public override bool IsPowered
		{
			get
			{
				return this.m_power;
			}
		}

		// Token: 0x06000016 RID: 22 RVA: 0x000026A1 File Offset: 0x000008A1
		protected override void OnPrefabInit()
		{
			this.circuitID = typeof(EnergyConsumer).GetField("<CircuitID>k__BackingField", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
		}

		// Token: 0x06000017 RID: 23 RVA: 0x000026BF File Offset: 0x000008BF
		protected override void OnSpawn()
		{
			base.OnSpawn();
			this.operational.SetActive(true, false);
		}

		// Token: 0x06000018 RID: 24 RVA: 0x000026D4 File Offset: 0x000008D4
		public override void SetConnectionStatus(CircuitManager.ConnectionStatus connection_status)
		{
			if (connection_status == 1)
			{
				this.m_power = false;
				this.Overpwered(false);
				this.circuitOverloadTime = 6f;
				base.PlayCircuitSound("overdraw");
				return;
			}
			if (connection_status == 2)
			{
				this.m_power = true;
				this.Overpwered(true);
				base.PlayCircuitSound("powered");
				return;
			}
			this.m_power = false;
		}

		// Token: 0x06000019 RID: 25 RVA: 0x0000272F File Offset: 0x0000092F
		protected virtual void Overpwered(bool ison)
		{
		}

		// Token: 0x04000012 RID: 18
		protected bool m_power;

		// Token: 0x04000013 RID: 19
		protected FieldInfo circuitID;
	}
}
