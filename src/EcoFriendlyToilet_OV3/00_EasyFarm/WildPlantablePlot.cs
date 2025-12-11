// The MIT License (MIT)
// Copyright (c) 2019-2025. Super Comic (ekfvoddl3535@naver.com)

using KSerialization;
using UnityEngine;

namespace EcoFriendlyToilet;

[SerializationConfig(MemberSerialization.OptIn)]
public sealed unsafe class WildPlantablePlot : PlantablePlot, ISaveLoadable
{
    private void SyncPriority(PrioritySetting setting)
    {
        var pcomp = GetComponent<Prioritizable>();
        SetPrioritySingle(pcomp, setting);

        if (occupyingObject == null)
            return;

        pcomp = occupyingObject.GetComponent<Prioritizable>();
        if (pcomp == null)
            return;

        SetPrioritySingle(pcomp, setting);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static void SetPrioritySingle(Prioritizable target, PrioritySetting value)
    {
        var left = target.GetMasterPriority();
        if (*(long*)&left != *(long*)&value)
            target.SetMasterPriority(value);
    }

    protected override void OnSpawn()
    {
        var plant_ = plant;
        plant = null;

        base.OnSpawn();

        if (plant_)
            ForceDeposit(plant_.gameObject);
    }

    private void PlantSettingInternal(GameObject g1)
    {
        var um = g1.GetComponent<UprootedMonitor>();
        if (um != null)
            um.canBeUprooted = false;

        autoReplaceEntity = false;
        var pz = GetComponent<Prioritizable>();
        var pz2 = g1.GetComponent<Prioritizable>();

        if (pz != null && pz2 != null)
        {
            pz2.SetMasterPriority(pz.GetMasterPriority());
            pz2.onPriorityChanged += SyncPriority;
        }
    }

    protected override void ConfigureOccupyingObject(GameObject newPlant)
    {
        plant = newPlant.GetComponent<KPrefabID>();

        PlantSettingInternal(newPlant);
    }
}
