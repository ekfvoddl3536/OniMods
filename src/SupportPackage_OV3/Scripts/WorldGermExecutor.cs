// The MIT License (MIT)
// Copyright (c) 2019-2025. Super Comic (ekfvoddl3535@naver.com)

using KSerialization;

namespace SupportPackage;

[SerializationConfig(MemberSerialization.OptIn)]
public sealed unsafe class WorldGermExecutor : WorldCellAccess, ISim4000ms
{
    public Storage storage;
    public float filterUseRate;

    public unsafe void Sim4000ms(float dt)
    {
        var rate = filterUseRate * dt;

        var list = storage.items;
        for (int i = 0; i < list.Count; ++i)
        {
            var x = list[i];
            if (!x) continue;

            var pe = x.GetComponent<PrimaryElement>();
            if (!pe) continue;

            var mass = pe.Mass;
            if (mass < rate) continue;

            pe.Mass = mass - rate;
            goto L_WORK;
        }

        _operational.SetActive(false);
        return;

    L_WORK:
        _operational.SetActive(true);

        var cells = _cells;
        for (int i = 0; i < cells.Length; ++i)
        {
            var cell = cells[i];

            var dcnt = Grid.diseaseCount[cell];
            var didx = Grid.diseaseIdx[cell];

            SimMessages.ModifyDiseaseOnCell(cell, didx, -dcnt);

            var plant = Grid.Objects[cell, (int)ObjectLayer.Building];
            if (plant && plant.GetComponent<EvilFlower>())
            {
                plant.GetComponent<Uprootable>().Uproot();
            }
        }
    }
}
