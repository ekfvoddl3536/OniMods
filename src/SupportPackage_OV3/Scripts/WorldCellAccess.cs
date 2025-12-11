// The MIT License (MIT)
// Copyright (c) 2019-2025. Super Comic (ekfvoddl3535@naver.com)

using KSerialization;
using Unity.Collections;

namespace SupportPackage;

[SerializationConfig(MemberSerialization.OptIn)]
public abstract class WorldCellAccess : KMonoBehaviour
{
    public int radius;

    [NonSerialized]
    protected NativeArray<int> _cells;

    [NonSerialized]
    protected Operational _operational;

    protected override void OnCleanUp()
    {
        _cells.Dispose();
    }

    protected override void OnSpawn()
    {
        _operational = GetComponent<Operational>();

        var bcell = Grid.PosToCell(this);
        var r = radius;

        var d = (r << 1) + 1;
        var cells = new NativeArray<int>(d * d, Allocator.Persistent, NativeArrayOptions.UninitializedMemory);
        _cells = cells;

        int i = 0;
        for (int dx = -r; dx <= r; ++dx)
            for (int dy = -r; dy <= r; ++dy)
            {
                int c = Grid.OffsetCell(bcell, dx, dy);
                if (!Grid.IsWorldValidCell(c))
                    c = bcell;

                cells[i++] = c;
            }
    }
}
