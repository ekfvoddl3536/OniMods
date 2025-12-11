// The MIT License (MIT)
// Copyright (c) 2019-2025. Super Comic (ekfvoddl3535@naver.com)

//namespace SupportPackage;

//public sealed class ElectricFoodDehydratorController : GameStateMachine<ElectricFoodDehydratorController, ElectricFoodDehydratorController.StatesInstance, IStateMachineTarget, ElectricFoodDehydratorController.Def>
//{
//    private static readonly Operational.Flag foodDehydratorFlag = new Operational.Flag("food_dehydrator", Operational.Flag.Type.Requirement);

//    public State off;
//    public State working;
//    public State requestEmpty;

//    public override void InitializeStates(out BaseState default_state)
//    {
//        default_state = off;

//        off.PlayAnim("off")
//            .EventTransition(GameHashes.OperationalChanged, working, smi => smi.IsPoweredActive);

//        working.Enter(smi =>
//        {
//            smi.complexFabricator.SetQueueDirty();
//            smi.operational.SetFlag(foodDehydratorFlag, true);
//        })
//        .EventTransition(GameHashes.OperationalChanged, off, smi => !smi.IsPoweredActive)
//        .EventHandler(GameHashes.FabricatorOrdersUpdated, smi => smi.UpdateFoodSymbol())
//        .EnterTransition(requestEmpty, smi => smi.IsEmptyOutStorage)
//        .EventHandlerTransition(GameHashes.FabricatorOrderCompleted, requestEmpty, (smi, data) => !smi.IsEmptyOutStorage)
//        .EventHandler(GameHashes.FabricatorOrderStarted, smi => smi.UpdateFoodSymbol());

//        requestEmpty
//            .ToggleRecurringChore(CreateChore, smi => smi.IsEmptyOutStorage)
//            .EventHandlerTransition(GameHashes.OnStorageChange, working, (smi, data) => smi.IsEmptyOutStorage)
//            .Enter(smi =>
//            {
//                smi.operational.SetFlag(foodDehydratorFlag, false);
//                smi.UpdateFoodSymbol();
//            })
//            .ToggleStatusItem(Db.Get().BuildingStatusItems.AwaitingEmptyBuilding);
//    }

//    private Chore CreateChore(StatesInstance smi)
//    {
//        var chore = new WorkChore<FoodDehydratorWorkableEmpty>(Db.Get().ChoreTypes.FoodFetch, smi.master.GetComponent<FoodDehydratorWorkableEmpty>(), on_complete: smi.OnEmptyComplete, only_when_operational: false);
//        chore.AddPrecondition(ChorePreconditions.instance.IsNotARobot, null);
//        return chore;
//    }

//    public sealed class StatesInstance : GameInstance
//    {
//        private const string HASH_FOOD = "food";

//        [MyCmpReq]
//        public Operational operational;
//        [MyCmpReq]
//        public ComplexFabricator complexFabricator;
//        private readonly KBatchedAnimController foodKBAC;

//        public StatesInstance(IStateMachineTarget master, Def def) : base(master, def)
//        {
//            var go = Util.NewGameObject(master.gameObject, "food_symbol");
//            go.SetActive(false);

//            var anim = GetComponent<KBatchedAnimController>();

//            var position = anim.GetSymbolTransform(HASH_FOOD, out _).GetColumn(3);
//            position.z = Grid.GetLayerZ(Grid.SceneLayer.BuildingUse);
//            go.transform.SetPosition(position);

//            var kbac = go.AddComponent<KBatchedAnimController>();
//            foodKBAC = kbac;
//            kbac.AnimFiles = [Assets.GetAnim("mushbar_kanim")];
//            kbac.initialAnim = "object";

//            anim.SetSymbolVisiblity(HASH_FOOD, is_visible: false);
//            kbac.sceneLayer = Grid.SceneLayer.BuildingUse;

//            var tracker = go.AddComponent<KBatchedAnimTracker>();
//            tracker.symbol = new HashedString(HASH_FOOD);
//            tracker.offset = default;
//        }

//        public bool IsEmptyOutStorage => complexFabricator.outStorage.items.Count == 0;

//        public bool IsPoweredActive => operational.IsOperational && operational.IsActive;

//        public void OnEmptyComplete(Chore _)
//        {
//            var position = Grid.CellToPosLCC(Grid.PosToCell(this), Grid.SceneLayer.Ore);
//            complexFabricator.outStorage.DropAll(position, false, false);
//        }

//        public void UpdateFoodSymbol()
//        {
//            var kbac = foodKBAC;

//            UnityEngine.GameObject animTarget;
//            var workingOrder = complexFabricator.CurrentWorkingOrder;
//            if (workingOrder != null)
//            {
//                animTarget = Assets.GetPrefab(workingOrder.ingredients[0].material);
//            }
//            else if (complexFabricator.outStorage.items.Count > 0)
//            {
//                animTarget = complexFabricator.outStorage.items[0];
//            }
//            else
//            {
//                kbac.gameObject.SetActive(false);
//                return;
//            }

//            kbac.gameObject.SetActive(true);
//            kbac.SwapAnims(animTarget.GetComponent<KBatchedAnimController>().AnimFiles);
//            kbac.Play("object", KAnim.PlayMode.Loop);
//        }
//    }

//    public sealed class Def : BaseDef { }
//}
