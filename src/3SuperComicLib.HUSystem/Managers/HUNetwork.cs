using System.Collections.Generic;

namespace SuperComicLib.HUSystem
{
    public class HUNetwork : UtilityNetwork
    {
        public List<IHaveHUCell> pipes = new List<IHaveHUCell>();

        public override void AddItem(int cell, object item) => pipes.Add(item as IHaveHUCell);

        public override void Reset(UtilityNetworkGridNode[] grid)
        {
            for (int x = 0; x < pipes.Count; x++)
                grid[pipes[x].HUCell].networkIdx = -1;
            pipes.Clear();
        }
    }
}
