using System.Collections.Generic;

namespace SuperComicLib.HUSystem
{
    public class HUPipeManager
    {
        public HashSet<IHUGenerator> generators = new HashSet<IHUGenerator>();
        public HashSet<IHUConsumer> consumers = new HashSet<IHUConsumer>();
        public List<Info> infos = new List<Info>();
        private bool dirty;

        public virtual void Refresh(float dt)
        {
            UtilityNetworkManager<HUNetwork, HUPipe> mgr = Constants.hupipeSystem;
            // 다시 업데이트할 필요가 없으므로, 리턴
            if (!mgr.IsDirty && !dirty)
                return;
            // mgr 업데이트
            mgr.Update();
            IList<UtilityNetwork> networks = mgr.GetNetworks();
            while (infos.Count < networks.Count)
                infos.Add(new Info
                {
                    generators = new List<IHUGenerator>(),
                    consumers = new List<IHUConsumer>()
                });
            Rebuild();
        }

        protected virtual void Rebuild()
        {
            for (int x = 0; x < infos.Count; x++)
            {
                Info bb = infos[x];
                bb.generators.Clear();
                bb.consumers.Clear();
            }
            foreach (IHUConsumer c in consumers)
            {
                ushort id = GetID(c.HUCell);
                if (id != ushort.MaxValue)
                    infos[id].consumers.Add(c);
            }
            foreach (IHUGenerator g in generators)
            {
                ushort id = GetID(g.HUCell);
                if (id != ushort.MaxValue)
                    infos[id].generators.Add(g);
            }
            dirty = false;
        }

        public virtual void Sim200msLast(float dt)
        {
            for (int x = 0; x < infos.Count; x++)
            {
                Info i = infos[x];
                i.totalConsumedHU = 0;
                int availableHUs = 0;

                for (int idx = 0; idx < infos[x].generators.Count; idx++)
                    availableHUs += infos[x].generators[idx].GenerateHeat(dt);

                if (availableHUs > 0)
                    for (int a = 0; a < infos[x].consumers.Count; a++)
                    {
                        infos[x].consumers[a].SetConnectionState(true);
                        if (availableHUs >= infos[x].consumers[a].HUMin)
                        {
                            int temp = infos[x].consumers[a].ConsumedHU(availableHUs, dt);
                            infos[x].totalConsumedHU += temp;
                            availableHUs -= temp;
                        }
                    }
                else
                    for (int a = 0; a < infos[x].consumers.Count; a++)
                        infos[x].consumers[a].SetConnectionState(false);
            }
        }

        public virtual void Connect(IHUGenerator gen)
        {
            if (Game.IsQuitting()) return;
            HUSimUpdater.AddListener(gen);
            generators.Add(gen);
            dirty = true;
        }

        public virtual void Connect(IHUConsumer con)
        {
            if (Game.IsQuitting()) return;
            HUSimUpdater.AddListener(con);
            consumers.Add(con);
            dirty = true;
        }

        public virtual void Disconnect(IHUGenerator gen)
        {
            if (Game.IsQuitting()) return;
            HUSimUpdater.RemoveListener(gen);
            generators.Remove(gen);
            dirty = true;
        }

        public virtual void Disconnect(IHUConsumer con)
        {
            if (Game.IsQuitting()) return;
            HUSimUpdater.RemoveListener(con);
            consumers.Remove(con);
            dirty = true;
        }

        public ushort GetID(int cell) =>
            Constants.hupipeSystem.GetNetworkForCell(cell) is UtilityNetwork uw ? (ushort)uw.id : ushort.MaxValue;

        public sealed class Info
        {
            public List<IHUGenerator> generators;
            public List<IHUConsumer> consumers;
            public int totalConsumedHU;
        }
    }
}
