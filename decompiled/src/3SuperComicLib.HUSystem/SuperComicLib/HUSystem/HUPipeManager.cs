using System;
using System.Collections.Generic;

namespace SuperComicLib.HUSystem
{
	// Token: 0x02000018 RID: 24
	public class HUPipeManager
	{
		// Token: 0x0600006F RID: 111 RVA: 0x000045D4 File Offset: 0x000027D4
		public virtual void Refresh(float dt)
		{
			UtilityNetworkManager<HUNetwork, HUPipe> mgr = Constants.hupipeSystem;
			if (!mgr.IsDirty && !this.dirty)
			{
				return;
			}
			mgr.Update();
			IList<UtilityNetwork> networks = mgr.GetNetworks();
			while (this.infos.Count < networks.Count)
			{
				this.infos.Add(new HUPipeManager.Info
				{
					generators = new List<IHUGenerator>(),
					consumers = new List<IHUConsumer>()
				});
			}
			this.Rebuild();
		}

		// Token: 0x06000070 RID: 112 RVA: 0x00004648 File Offset: 0x00002848
		protected virtual void Rebuild()
		{
			for (int x = 0; x < this.infos.Count; x++)
			{
				HUPipeManager.Info info = this.infos[x];
				info.generators.Clear();
				info.consumers.Clear();
			}
			foreach (IHUConsumer c in this.consumers)
			{
				ushort id = this.GetID(c.HUCell);
				if (id != 65535)
				{
					this.infos[(int)id].consumers.Add(c);
				}
			}
			foreach (IHUGenerator g in this.generators)
			{
				ushort id2 = this.GetID(g.HUCell);
				if (id2 != 65535)
				{
					this.infos[(int)id2].generators.Add(g);
				}
			}
			this.dirty = false;
		}

		// Token: 0x06000071 RID: 113 RVA: 0x0000476C File Offset: 0x0000296C
		public virtual void Sim200msLast(float dt)
		{
			for (int x = 0; x < this.infos.Count; x++)
			{
				HUPipeManager.Info i = this.infos[x];
				i.totalConsumedHU = 0;
				int availableHUs = 0;
				for (int idx = 0; idx < i.generators.Count; idx++)
				{
					availableHUs += i.generators[idx].GenerateHeat(dt);
				}
				for (int a = 0; a < i.consumers.Count; a++)
				{
					if (availableHUs >= i.consumers[a].HUMin)
					{
						int temp = i.consumers[a].ConsumedHU(availableHUs, dt);
						i.totalConsumedHU += temp;
						availableHUs -= temp;
					}
					else
					{
						i.consumers[a].ConsumedHU(0, 0f);
					}
				}
			}
		}

		// Token: 0x06000072 RID: 114 RVA: 0x0000484A File Offset: 0x00002A4A
		public virtual void Connect(IHUGenerator gen)
		{
			if (Game.IsQuitting())
			{
				return;
			}
			HUSimUpdater.AddListener(gen);
			this.generators.Add(gen);
			this.dirty = true;
		}

		// Token: 0x06000073 RID: 115 RVA: 0x0000486E File Offset: 0x00002A6E
		public virtual void Connect(IHUConsumer con)
		{
			if (Game.IsQuitting())
			{
				return;
			}
			HUSimUpdater.AddListener(con);
			this.consumers.Add(con);
			this.dirty = true;
		}

		// Token: 0x06000074 RID: 116 RVA: 0x00004892 File Offset: 0x00002A92
		public virtual void Disconnect(IHUGenerator gen)
		{
			if (Game.IsQuitting())
			{
				return;
			}
			HUSimUpdater.RemoveListener(gen);
			this.generators.Remove(gen);
			this.dirty = true;
		}

		// Token: 0x06000075 RID: 117 RVA: 0x000048B6 File Offset: 0x00002AB6
		public virtual void Disconnect(IHUConsumer con)
		{
			if (Game.IsQuitting())
			{
				return;
			}
			HUSimUpdater.RemoveListener(con);
			this.consumers.Remove(con);
			this.dirty = true;
		}

		// Token: 0x06000076 RID: 118 RVA: 0x000048DC File Offset: 0x00002ADC
		public ushort GetID(int cell)
		{
			UtilityNetwork uw = Constants.hupipeSystem.GetNetworkForCell(cell);
			if (uw == null)
			{
				return ushort.MaxValue;
			}
			return (ushort)uw.id;
		}

		// Token: 0x0400004F RID: 79
		public HashSet<IHUGenerator> generators = new HashSet<IHUGenerator>();

		// Token: 0x04000050 RID: 80
		public HashSet<IHUConsumer> consumers = new HashSet<IHUConsumer>();

		// Token: 0x04000051 RID: 81
		public List<HUPipeManager.Info> infos = new List<HUPipeManager.Info>();

		// Token: 0x04000052 RID: 82
		private bool dirty;

		// Token: 0x0200002B RID: 43
		public sealed class Info
		{
			// Token: 0x04000064 RID: 100
			public List<IHUGenerator> generators;

			// Token: 0x04000065 RID: 101
			public List<IHUConsumer> consumers;

			// Token: 0x04000066 RID: 102
			public int totalConsumedHU;
		}
	}
}
