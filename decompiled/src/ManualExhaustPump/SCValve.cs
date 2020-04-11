using System;
using KSerialization;

namespace ManualExhaustPump
{
	// Token: 0x0200000A RID: 10
	[SerializationConfig(1)]
	public class SCValve : KMonoBehaviour
	{
		// Token: 0x06000021 RID: 33 RVA: 0x000027B1 File Offset: 0x000009B1
		protected override void OnSpawn()
		{
			this.cell = base.GetComponent<Building>().GetUtilityInputCell();
			this.flowMgr = Conduit.GetFlowManager(2);
			Conduit.GetFlowManager(2).AddConduitUpdater(new Action<float>(this.ConduitUpdate), 0);
			this.CancelAnim();
		}

		// Token: 0x06000022 RID: 34 RVA: 0x000027EF File Offset: 0x000009EF
		protected override void OnCleanUp()
		{
			this.flowMgr = null;
			Conduit.GetFlowManager(2).RemoveConduitUpdater(new Action<float>(this.ConduitUpdate));
		}

		// Token: 0x06000023 RID: 35 RVA: 0x00002810 File Offset: 0x00000A10
		protected virtual void ConduitUpdate(float dt)
		{
			if (this.flowMgr.HasConduit(this.cell) && !this.InStorage.IsFull())
			{
				this.WorkAnim();
				return;
			}
			this.CancelAnim();
		}

		// Token: 0x06000024 RID: 36 RVA: 0x0000283F File Offset: 0x00000A3F
		public virtual void CancelAnim()
		{
			this.controller.Play("off", 1, 1f, 0f);
		}

		// Token: 0x06000025 RID: 37 RVA: 0x00002861 File Offset: 0x00000A61
		public virtual void WorkAnim()
		{
			this.controller.Play("work_pst", 0, 1f, 0f);
		}

		// Token: 0x04000013 RID: 19
		protected const float MaxFlow = 10f;

		// Token: 0x04000014 RID: 20
		protected const ConduitType ctype = 2;

		// Token: 0x04000015 RID: 21
		[MyCmpReq]
		public Storage InStorage;

		// Token: 0x04000016 RID: 22
		[MyCmpGet]
		protected KBatchedAnimController controller;

		// Token: 0x04000017 RID: 23
		protected ConduitFlow flowMgr;

		// Token: 0x04000018 RID: 24
		protected int cell;
	}
}
