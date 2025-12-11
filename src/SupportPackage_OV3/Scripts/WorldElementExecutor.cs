// The MIT License (MIT)
// Copyright (c) 2019-2025. Super Comic (ekfvoddl3535@naver.com)

using KSerialization;

namespace SupportPackage;

[SerializationConfig(MemberSerialization.OptIn)]
public sealed class WorldElementExecutor : WorldCellAccess, ISim4000ms
{
    public SimHashes targetElement;

    public unsafe void Sim4000ms(float dt)
    {
        // const ushort VACCUM = 0;
        if (!_operational.IsOperational) return;

        var cells = _cells;
        for (int i = 0; i < cells.Length; ++i)
        {
            var cell = cells[i];

            var element = Grid.Element[cell];
            if (element.id == targetElement)
            {
                SimMessages.ConsumeMass(cell, targetElement, Grid.mass[cell], 1);
                // SimMessages.ModifyCell(cell, VACCUM, 0f, 0f, byte.MaxValue, 0);
            }
        }
    }
}
