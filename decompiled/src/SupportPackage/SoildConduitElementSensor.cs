using System;
using System.Reflection;
using KSerialization;

namespace SupportPackage
{
	// Token: 0x0200000A RID: 10
	[SerializationConfig(1)]
	public sealed class SoildConduitElementSensor : ConduitElementSensor
	{
		// Token: 0x06000025 RID: 37 RVA: 0x0000309E File Offset: 0x0000129E
		protected override void OnPrefabInit()
		{
			this.desrelem = typeof(ConduitElementSensor).GetField("desiredElement", BindingFlags.Instance | BindingFlags.NonPublic);
			base.OnPrefabInit();
		}

		// Token: 0x06000026 RID: 38 RVA: 0x000030C4 File Offset: 0x000012C4
		protected override void ConduitUpdate(float dt)
		{
			SolidConduitFlow mgr = SolidConduit.GetFlowManager();
			SolidConduitFlow.ConduitContents cc = mgr.GetContents(Grid.PosToCell(TransformExtensions.GetPosition(base.transform)));
			SimHashes data = (SimHashes)this.desrelem.GetValue(this);
			if (cc.pickupableHandle.IsValid() && (base.IsSwitchedOn ^ mgr.GetPickupable(cc.pickupableHandle).PrimaryElement.ElementID == data))
			{
				this.Toggle();
			}
		}

		// Token: 0x04000005 RID: 5
		private FieldInfo desrelem;
	}
}
