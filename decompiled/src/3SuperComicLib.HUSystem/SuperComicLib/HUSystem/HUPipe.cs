using System;
using System.Collections;
using UnityEngine;

namespace SuperComicLib.HUSystem
{
	// Token: 0x02000017 RID: 23
	[SkipSaveFileSerialization]
	public class HUPipe : KMonoBehaviour, IDisconnectable, IFirstFrameCallback, IHaveUtilityNetworkMgr, IUtilityNetworkItem, IHaveHUCell
	{
		// Token: 0x1700000B RID: 11
		// (get) Token: 0x06000063 RID: 99 RVA: 0x000044CC File Offset: 0x000026CC
		// (set) Token: 0x06000064 RID: 100 RVA: 0x000044D4 File Offset: 0x000026D4
		public int HUCell { get; protected set; }

		// Token: 0x1700000C RID: 12
		// (get) Token: 0x06000065 RID: 101 RVA: 0x000044DD File Offset: 0x000026DD
		public ushort NetworkID
		{
			get
			{
				return Constants.manager.GetID(this.HUCell);
			}
		}

		// Token: 0x06000066 RID: 102 RVA: 0x000044EF File Offset: 0x000026EF
		protected override void OnSpawn()
		{
			this.HUCell = Grid.PosToCell(TransformExtensions.GetPosition(base.transform));
			Constants.hupipeSystem.AddToNetworks(this.HUCell, this, false);
			this.Connect();
		}

		// Token: 0x06000067 RID: 103 RVA: 0x00004520 File Offset: 0x00002720
		protected override void OnCleanUp()
		{
			Constants.hupipeSystem.RemoveFromNetworks(this.HUCell, this, false);
			this.Disconnect();
		}

		// Token: 0x06000068 RID: 104 RVA: 0x0000453C File Offset: 0x0000273C
		public bool Connect()
		{
			BuildingHP com = base.GetComponent<BuildingHP>();
			if (com == null || com.HitPoints > 0)
			{
				this.disconnected = false;
				Constants.hupipeSystem.ForceRebuildNetworks();
			}
			return !this.disconnected;
		}

		// Token: 0x06000069 RID: 105 RVA: 0x0000457C File Offset: 0x0000277C
		public void Disconnect()
		{
			this.disconnected = true;
			Constants.hupipeSystem.ForceRebuildNetworks();
		}

		// Token: 0x0600006A RID: 106 RVA: 0x0000458F File Offset: 0x0000278F
		public IUtilityNetworkMgr GetNetworkManager()
		{
			return Constants.hupipeSystem;
		}

		// Token: 0x0600006B RID: 107 RVA: 0x00004596 File Offset: 0x00002796
		public bool IsDisconnected()
		{
			return this.disconnected;
		}

		// Token: 0x0600006C RID: 108 RVA: 0x0000459E File Offset: 0x0000279E
		public void SetFirstFrameCallback(Action ffCb)
		{
			this.callback = ffCb;
			base.StartCoroutine(this.RunCallback());
		}

		// Token: 0x0600006D RID: 109 RVA: 0x000045B4 File Offset: 0x000027B4
		private IEnumerator RunCallback()
		{
			yield return null;
			Action action = this.callback;
			if (action != null)
			{
				action();
			}
			yield return null;
			yield break;
		}

		// Token: 0x0400004C RID: 76
		[SerializeField]
		protected bool disconnected = true;

		// Token: 0x0400004D RID: 77
		private Action callback;
	}
}
