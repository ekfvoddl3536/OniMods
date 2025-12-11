// The MIT License (MIT)
// Copyright (c) 2019-2025. Super Comic (ekfvoddl3535@naver.com)

//using System;
//using System.Runtime.CompilerServices;
//using KSerialization;
//using UnityEngine;
//using NetItem = FlowUtilityNetwork.NetworkItem;

//namespace AdvancedGenerators;

//using SELF_DELEGATE = EventSystem.IntraObjectHandler<LogicConduitModeController>;

//[SerializationConfig(MemberSerialization.OptIn)]
//public sealed class LogicConduitModeController : KMonoBehaviour, ISecondaryInput, ISecondaryOutput, ISim200ms
//{
//    private readonly struct ConduitPort
//    {
//        public readonly IUtilityNetworkMgr networkMgr;
//        public readonly NetItem networkItem;
//        public readonly object obj;
//        public readonly int cell;

//        [MethodImpl(MethodImplOptions.AggressiveInlining)]
//        public ConduitPort(IUtilityNetworkMgr networkMgr, NetItem networkItem, object obj, int cell)
//        {
//            this.networkMgr = networkMgr;
//            this.networkItem = networkItem;
//            this.obj = obj;
//            this.cell = cell;
//        }

//        [MethodImpl(MethodImplOptions.AggressiveInlining)]
//        public readonly T As<T>() where T : KMonoBehaviour => Unsafe.As<T>(obj);

//        public readonly bool IsOn
//        {
//            [MethodImpl(MethodImplOptions.AggressiveInlining)]
//            get => networkMgr.GetNetworkForCell(cell) != null;
//        }

//        [MethodImpl(MethodImplOptions.AggressiveInlining)]
//        public readonly void On()
//        {
//            networkMgr.AddToNetworks(cell, networkItem, true);
//        }

//        [MethodImpl(MethodImplOptions.AggressiveInlining)]
//        public readonly void Off()
//        {
//            networkMgr.RemoveFromNetworks(cell, networkItem, true);
//        }
//    }

//    public const float GasFlow = ConduitFlow.MAX_GAS_MASS;
//    public const float LiquidFlow = ConduitFlow.MAX_LIQUID_MASS;
//    public const float SolidFlow = SolidConduitFlow.MAX_SOLID_MASS;

//    // public static CellOffset NullOffset => new CellOffset(int.MinValue, int.MinValue);
//    public static readonly HashedString PORT_ID = new HashedString(nameof(LogicConduitModeController) + "Input");

//    private static readonly StatusItem infoStatusItem = new StatusItem("LogicOperationalInfo", "BUILDING", string.Empty, StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID)
//    {
//        resolveStringCallback = static (str, data) =>
//        {
//            return
//                ((LogicConduitModeController)data).operational!.GetFlag(LogicOperationalController.LogicOperationalFlag)
//                ? STRINGS.BUILDING.STATUSITEMS.LOGIC.LOGIC_CONTROLLED_ENABLED
//                : STRINGS.BUILDING.STATUSITEMS.LOGIC.LOGIC_CONTROLLED_DISABLED;
//        }
//    };

//    private static readonly SELF_DELEGATE OnLogicValueChangedDelegate = new SELF_DELEGATE(OnLogicValueChangedStatic);
//    private static readonly SELF_DELEGATE OnConduitConnectionChangedDelegate = new SELF_DELEGATE(OnConduitConnectionChangedStatic);

//    private static void OnConduitConnectionChangedStatic(LogicConduitModeController self, object _)
//    {
//        switch (self.currentMode)
//        {
//            case (int)ConduitType.Gas:
//                OnIfConnectedDispenser(ref self._liquidDispenser);
//                OnIfConnectedDispenser(ref self._solidDispenser);
//                break;

//            case (int)ConduitType.Liquid:
//                OnIfConnectedDispenser(ref self._solidDispenser);
//                break;

//            default:
//                break;
//        }
//    }

//    private static void OnLogicValueChangedStatic(LogicConduitModeController self, object data)
//    {
//        if (data is LogicValueChanged)
//            self.OnLogicValueChanged(ref Unsafe.Unbox<LogicValueChanged>(data));
//        else
//            Debug.LogWarning("[Advanced Generator] Unexpected type (on logic value changed)");
//    }

//    // [MethodImpl(MethodImplOptions.AggressiveInlining)]
//    // private static bool isok(CellOffset off) => (off.x | off.y) != int.MinValue;

//    private static ConduitPort SetupConsumer(
//        GameObject parent,
//        CellOffset offset,
//        Storage storage,
//        ConduitType type)
//    {
//        int inputCell = Grid.OffsetCell(Grid.PosToCell(parent), offset);

//        object obj;
//        IUtilityNetworkMgr netMgr;
//        if (type != ConduitType.Solid)
//        {
//            var cc = parent.AddComponent<ConduitConsumer>();
//            obj = cc;

//            cc.conduitType = type;
//            cc.storage = storage;
//            cc.alwaysConsume = false;
//            cc.forceAlwaysSatisfied = false;
//            cc.keepZeroMassObject = false;
//            cc.useSecondaryInput = true;
//            cc.wrongElementResult = ConduitConsumer.WrongElementResult.Dump;

//            if (type == ConduitType.Gas)
//            {
//                cc.capacityKG = GasFlow;
//                cc.capacityTag = GameTags.Gas;
//                netMgr = Game.Instance.gasConduitSystem;
//            }
//            else
//            {
//                cc.capacityKG = LiquidFlow;
//                cc.capacityTag = GameTags.Liquid;
//                netMgr = Game.Instance.liquidConduitSystem;
//            }
//        }
//        else
//        {
//            var scc = parent.AddComponent<SolidConduitConsumer>();
//            obj = scc;

//            scc.storage = storage;
//            scc.capacityTag = GameTags.Any;
//            scc.alwaysConsume = false;
//            scc.useSecondaryInput = true;
//            scc.capacityKG = SolidFlow;

//            netMgr = Game.Instance.solidConduitSystem;
//        }

//        var networkItem = new NetItem(type, Endpoint.Sink, inputCell, parent);
//        // netMgr.AddToNetworks(inputCell, networkItem, true);

//        return new ConduitPort(netMgr, networkItem, obj, inputCell);
//    }

//    private static ConduitPort SetupDispenser(
//        GameObject parent,
//        CellOffset offset,
//        Storage storage,
//        ConduitType type)
//    {
//        int outputCell = Grid.OffsetCell(Grid.PosToCell(parent), offset);

//        object obj;
//        IUtilityNetworkMgr netMgr;
//        if (type != ConduitType.Solid)
//        {
//            var cd = parent.AddComponent<ConduitDispenser>();
//            obj = cd;

//            cd.conduitType = type;
//            cd.storage = storage;
//            cd.alwaysDispense = true;
//            cd.useSecondaryOutput = true;

//            netMgr = 
//                type == ConduitType.Gas 
//                ? Game.Instance.gasConduitSystem 
//                : Game.Instance.liquidConduitSystem;
//        }
//        else
//        {
//            var scd = parent.AddComponent<SolidConduitDispenser>();
//            obj = scd;

//            scd.storage = storage;
//            scd.alwaysDispense = true;
//            scd.solidOnly = true;
//            scd.useSecondaryOutput = true;

//            netMgr = Game.Instance.solidConduitSystem;
//        }

//        var networkItem = new NetItem(type, Endpoint.Source, outputCell, parent);
//        // netMgr.AddToNetworks(outputCell, networkItem, true);

//        return new ConduitPort(netMgr, networkItem, obj, outputCell);
//    }

//    // [Serialize]
//    // public CellOffset gasIn = NullOffset;
//    // [Serialize]
//    // public CellOffset gasOut = NullOffset;
//    // 
//    // [Serialize]
//    // public CellOffset liquidIn = NullOffset;
//    // [Serialize]
//    // public CellOffset liquidOut = NullOffset;
//    // 
//    // [Serialize]
//    // public CellOffset solidIn = NullOffset;
//    // [Serialize]
//    // public CellOffset solidOut = NullOffset;

//    // public Action<LogicConduitModeController>? OnTransferAll;

//    public Storage? InputStorage;
//    public Storage? OutputStorage;

//    public CellOffset InputOffset;
//    public CellOffset OutputOffset;

//    private ConduitPort _gasConsumer;
//    private ConduitPort _liquidConsumer;
//    private ConduitPort _solidConsumer;

//    private ConduitPort _gasDispenser;
//    private ConduitPort _liquidDispenser;
//    private ConduitPort _solidDispenser;

//    // private ConduitPort _currentConsumer;

//    [Serialize]
//    private int currentValue;
//    // [Serialize]
//    private int currentMode;

//    private KSelectable? selectable;
//    private Operational? operational;
//    // private LogicPorts? logicPorts;
//    // private LogicCircuitNetwork? logicNetwork;

//    [Serialize]
//    private float _prevMassStored;

//    protected override void OnSpawn()
//    {
//        Subscribe((int)GameHashes.LogicEvent, OnLogicValueChangedDelegate);
//        Subscribe((int)GameHashes.ConduitConnectionChanged, OnConduitConnectionChangedDelegate);

//        selectable = GetComponent<KSelectable>();
//        operational = GetComponent<Operational>();

//        var go = gameObject;
//        var inSt = InputStorage!;
//        var otSt = OutputStorage!;

//        var inCell = InputOffset;
//        var otCell = OutputOffset;

//        _gasConsumer = SetupConsumer(go, inCell, inSt, ConduitType.Gas);
//        _liquidConsumer = SetupConsumer(go, inCell, inSt, ConduitType.Liquid);
//        _solidConsumer = SetupConsumer(go, inCell, inSt, ConduitType.Solid);

//        _gasDispenser = SetupDispenser(go, otCell, otSt, ConduitType.Gas);
//        _liquidDispenser = SetupDispenser(go, otCell, otSt, ConduitType.Liquid);
//        _solidDispenser = SetupDispenser(go, otCell, otSt, ConduitType.Solid);

//        // enalbe all dispenser
//        // On(ref _gasDispenser);
//        // On(ref _liquidDispenser);
//        // On(ref _solidDispenser);

//        currentMode = -1; // un-set
//        UpdateModeImmediate();
//    }

//    protected override void OnCleanUp()
//    {
//        // var go = gameObject;
//        Off(ref _gasConsumer);
//        Off(ref _liquidConsumer);
//        Off(ref _solidConsumer);

//        Off(ref _gasDispenser);
//        Off(ref _liquidDispenser);
//        Off(ref _solidDispenser);
//    }

//    private void OnLogicValueChanged(ref LogicValueChanged e)
//    {
//        if (e.portID != PORT_ID) return;

//        Debug.Log("OnLogicValueChanged!");
//        UpdateModeImmediate();
//    }

//    private void UpdateModeImmediate()
//    {
//        var n = Game.Instance.logicCircuitManager.GetNetworkForCell(GetComponent<LogicPorts>()!.GetPortCell(PORT_ID));
//        if (n != null)
//        {
//            selectable!.ToggleStatusItem(infoStatusItem, true, this);

//            var newValue = n.OutputValue;

//            operational!.SetFlag(LogicOperationalController.LogicOperationalFlag, (newValue & 1) != 0);
//            currentValue = newValue;
//        }
//        else
//        {
//            // unNetwork
//            currentValue = 1;

//            selectable!.ToggleStatusItem(infoStatusItem, false, this);
//        }

//        UpdateMode();
//    }

//    private void UpdateMode()
//    {
//        const int LOGIC_MASK = 0x06;
//        const int TYPE_MAP =
//            ((int)ConduitType.Liquid << 0) |
//            ((int)ConduitType.Gas << 2) |
//            ((int)ConduitType.Solid << 4) |
//            ((int)ConduitType.Liquid << 6);

//        //  BIT 1       : Operational control bit (is game default)
//        //  BIT 2 - 3   : Using this area
//        int v = (TYPE_MAP >> (currentValue & LOGIC_MASK)) & 0x03;
//        var p = currentMode;

//        if (v != p)
//        {
//            ref var c = ref Unsafe.NullRef<ConduitPort>();
//            switch (v)
//            {
//                case (int)ConduitType.Gas:
//                    Off(ref _liquidConsumer);
//                    Off(ref _solidConsumer);

//                    c = ref _gasConsumer;
//                    On(ref _gasDispenser);
//                    break;

//                case (int)ConduitType.Liquid:
//                    Off(ref _gasConsumer);
//                    Off(ref _solidConsumer);

//                    c = ref _liquidConsumer;
//                    On(ref _liquidDispenser);

//                    Off(ref _gasDispenser);
//                    break;

//                case (int)ConduitType.Solid:
//                    Off(ref _gasConsumer);
//                    Off(ref _liquidConsumer);

//                    c = ref _solidConsumer;
//                    On(ref _solidDispenser);

//                    Off(ref _gasDispenser);
//                    Off(ref _liquidDispenser);
//                    break;

//                default:
//                    goto _SKIP;
//            }

//            if (_prevMassStored == 0) On(ref c);
//            else Off(ref c);

//        _SKIP:
//            currentMode = v;
//        }
//    }

//    [MethodImpl(MethodImplOptions.AggressiveInlining)]
//    private static void On(ref ConduitPort v) => v.On();

//    [MethodImpl(MethodImplOptions.AggressiveInlining)]
//    private static void Off(ref ConduitPort v) => v.Off();

//    bool ISecondaryOutput.HasSecondaryConduitType(ConduitType type) => true;
//    bool ISecondaryInput.HasSecondaryConduitType(ConduitType type) => true;

//    CellOffset ISecondaryOutput.GetSecondaryConduitOffset(ConduitType type) => OutputOffset;
//    CellOffset ISecondaryInput.GetSecondaryConduitOffset(ConduitType type) => InputOffset;

//    [MethodImpl(MethodImplOptions.AggressiveInlining)]
//    private static void OnIfConnectedDispenser(ref ConduitPort d)
//    {
//        if (d.As<ConduitDispenser>().IsConnected) On(ref d);
//        else Off(ref d);
//    }

//    public void Sim200ms(float dt)
//    {
//        ref var c = ref _gasDispenser;
//        float maxCapacityKG;
//        switch (currentMode)
//        {
//            case (int)ConduitType.Gas:
//                c = ref _gasConsumer;
//                maxCapacityKG = GasFlow * 2;
//                break;

//            case (int)ConduitType.Liquid:
//                c = ref _liquidConsumer;
//                maxCapacityKG = LiquidFlow * 2;
//                break;

//            case (int)ConduitType.Solid:
//                c = ref _solidConsumer;
//                maxCapacityKG = SolidFlow * 2;
//                break;

//            default:
//                return;
//        }

//        var mass =
//            InputStorage!.ExactMassStored() +
//            OutputStorage!.ExactMassStored();
//        if (mass > maxCapacityKG)
//        {
//            if (_prevMassStored == 0) Off(ref c);

//            _prevMassStored = mass;
//        }
//        else
//        {
//            if (mass < _prevMassStored) On(ref c);

//            _prevMassStored = 0f;
//        }
//    }
//}
