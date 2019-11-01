using System.Collections.Generic;

namespace SuperComicLib.HUSystem
{
    public static class HUSimUpdater
    {
        // 이벤트 안쓰겠음, 이벤트는 내가 알기로 ThreadSafe 하지 않음
        private static readonly HashSet<IHUSim200ms> Items = new HashSet<IHUSim200ms>();

        public static void Clear() => Items.Clear();

        public static void AddListener(IHUSim200ms listener) => Items.Add(listener);

        public static void RemoveListener(IHUSim200ms listener) => Items.Remove(listener);

        public static void Update(float dt)
        {
            foreach (IHUSim200ms x in Items)
                x.HUSim200ms(dt);
        }
    }
}
