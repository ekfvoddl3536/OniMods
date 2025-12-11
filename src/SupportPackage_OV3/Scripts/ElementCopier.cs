// The MIT License (MIT)
// Copyright (c) 2019-2025. Super Comic (ekfvoddl3535@naver.com)

#nullable enable
using KSerialization;
using UnityEngine;

namespace SupportPackage;

[SerializationConfig(MemberSerialization.OptIn)]
public class ElementCopier : KMonoBehaviour, ISaveLoadable
{
    [SerializeField]
    public ConduitType portType;
    [SerializeField]
    public CellOffset portOffset;

    private Operational? _operational;

    private int inputCell = -1;
    private int outputCell = -1;

    private ConduitFlow? _fm1;

    protected override void OnPrefabInit()
    {
        _operational = GetComponent<Operational>();
    }

    protected override void OnSpawn()
    {
        Debug.Assert(
            portType == ConduitType.Gas ||
            portType == ConduitType.Liquid);

        var building = GetComponent<Building>();

        inputCell = building.GetUtilityInputCell();
        outputCell = building.GetUtilityOutputCell();

        var type = portType;

        GetComponent<ConduitConsumer>().isConsuming = false;

        _fm1 = Conduit.GetFlowManager(type);
        _fm1.AddConduitUpdater(OnConduitTick);
    }

    protected override void OnCleanUp()
    {
        _fm1!.RemoveConduitUpdater(OnConduitTick);
    }

    public void OnConduitTick(float dt)
    {
        bool __active = false;
        if (_operational!.IsOperational)
            __active = ConduitTick();

        _operational!.SetActive(__active);
    }

    private bool ConduitTick()
    {
        var m = _fm1!;
        
        var delta = 0f;

        var c1 = m.GetContents(inputCell);
        if (c1.mass > 0f)
            delta = m.AddElement(outputCell, c1.element, c1.mass, c1.temperature, c1.diseaseIdx, c1.diseaseCount);

        return delta > 0f;
    }
}
