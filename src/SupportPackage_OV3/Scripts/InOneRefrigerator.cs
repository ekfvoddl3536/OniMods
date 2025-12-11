// The MIT License (MIT)
// Copyright (c) 2019-2025. Super Comic (ekfvoddl3535@naver.com)

using KSerialization;
using UnityEngine;

namespace SupportPackage;

public class InOneRefrigerator : KMonoBehaviour, IUserControlledCapacity
{
    // private const float SIM_TEMP = 253.15f;
    // private const float HEAT_CAPACITY = 800f;
    // private const float CONDUCITIY = 2000f;

    public Storage storage;
    [Serialize]
    private float _userMaxCapacity = GlobalConsts.TwoInOneGrill.STORED_MAX;

    private FilteredStorage filteredStorage;

    public float UserMaxCapacity
    {
        get => _userMaxCapacity;
        set
        {
            _userMaxCapacity = value;
            filteredStorage.FilterChanged();
        }
    }
    public float AmountStored => storage.MassStored();
    public float MinCapacity => 0f;
    public float MaxCapacity => storage.capacityKg;
    public bool WholeValues => false;
    public LocString CapacityUnits => GameUtil.GetCurrentMassUnit();

    protected override void OnPrefabInit() =>
        filteredStorage = new FilteredStorage(this,
        [
            GameTags.Compostable
        ], this, false, Db.Get().ChoreTypes.FoodFetch);

    protected override void OnSpawn()
    {
        GetComponent<FoodStorage>().FilteredStorage = filteredStorage;
        filteredStorage.FilterChanged();

        // Subscribe((int)GameHashes.OnStorageChange, OnStorageChanged_CB);
        Subscribe((int)GameHashes.CopySettings, OnCopySettings_CB);
    }

    private void OnCopySettings_CB(object data)
    {
        if (data == null)
            return;

        var comp = ((GameObject)data).GetComponent<InOneRefrigerator>();
        if (!comp)
            return;

        _userMaxCapacity = comp._userMaxCapacity;
    }

    protected override void OnCleanUp() => filteredStorage.CleanUp();
}
