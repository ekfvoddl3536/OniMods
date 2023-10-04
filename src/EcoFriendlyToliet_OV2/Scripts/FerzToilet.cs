// MIT License
//
// Copyright (c) 2022-2023. SuperComic (ekfvoddl3535@naver.com)
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in all
// copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
// SOFTWARE.

using KSerialization;
using System.Collections.Generic;
using UnityEngine;
using static STRINGS.UI.USERMENUACTIONS;
using static STRINGS.DUPLICANTS.DISEASES;
using static STRINGS.UI.BUILDINGEFFECTS;
using KBUTTON = KIconButtonMenu.ButtonInfo;

namespace EcoFriendlyToilet
{
    using static GlobalConsts.EcoFriendlyToilet;
    public class FerzToilet : StateMachineComponent<FerzToilet.StatesInstance>, ISaveLoadable, IUsable, IGameObjectEffectDescriptor, IBasicBuilding
    {
        private static readonly EventSystem.IntraObjectHandler<FerzToilet> OnRefreshUserMenuDelegate = new EventSystem.IntraObjectHandler<FerzToilet>(STATIC_EVENT_ACTION);

        private static void STATIC_EVENT_ACTION(FerzToilet c, object _) => c.OnRefreshUserMenu();

        [SerializeField]
        public Toilet.SpawnInfo solidWastePerUse;

        [Serialize]
        public int _flushesUsed;

        [SerializeField]
        public Tag[] needSrcElems;

        private MeterController meter;

        private Storage storage;
        private byte disease_db_idx;

        protected override void OnPrefabInit()
        {
            Debug.Assert(needSrcElems != null);

            base.OnPrefabInit();

            storage = GetComponent<Storage>();
            disease_db_idx = Db.Get().Diseases.GetIndex(GlobalConsts.DISEASE_ID);
        }

        protected override void OnSpawn()
        {
            base.OnSpawn();

            Components.Toilets.Add(this);

            var smi = base.smi;
            smi.StartSM();

            GetComponent<ToiletWorkableUse>().trackUses = true;

            meter = new MeterController(GetComponent<KBatchedAnimController>(), "meter_target", "meter", Meter.Offset.Behind, Grid.SceneLayer.NoLayer, new string[]
            {
                "meter_target",
                "meter_arrow",
                "meter_scale"
            });
            meter.SetPositionPercent((float)_flushesUsed / MAX_FLUSHES);

            smi.sm.flushes.Set(_flushesUsed, smi);

            Subscribe((int)GameHashes.RefreshUserMenu, OnRefreshUserMenuDelegate);
        }

        protected override void OnCleanUp()
        {
            base.OnCleanUp();

            Components.Toilets.Remove(this);
        }

        private void OnRefreshUserMenu()
        {
            var smi = base.smi;

            if (smi.GetCurrentState() == smi.sm.full || _flushesUsed <= 0 || smi.cleanChore != null)
                return;

            const string ICON = "status_item_toilet_needs_emptying";
            Game.Instance.userMenu.AddButton(gameObject, 
                new KBUTTON(
                    ICON, 
                    CLEANTOILET.NAME, 
                    BUTTON_CB, 
                    Action.CinemaZoomOut,
                    null,
                    null,
                    null,
                    CLEANTOILET.TOOLTIP));
        }

        private void BUTTON_CB()
        {
            var smi = base.smi;
            smi.GoTo(smi.sm.earlyclean);
        }

        public bool IsUsable() => smi.HasTag(GameTags.Usable);

        private static string GetTooltipStr(string target, string _str, float mass) =>
            string.Format(target, _str, GameUtil.GetFormattedMass(mass, 0, 0, true, "{0:0.##}"));

        private IEnumerable<Descriptor> RequirementDescriptors()
        {
            var res = new Descriptor[2];
            
            var arr = GetComponents<ManualDeliveryKG>();

            var mass = solidWastePerUse.mass;

            for (int x = arr.Length; --x >= 0;)
            {
                string str = GameTagExtensions.ProperName(arr[x].RequestedItemTag);
                res[x] =
                    new Descriptor(
                        GetTooltipStr(ELEMENTCONSUMEDPERUSE, str, mass),
                        GetTooltipStr(TOOLTIPS.ELEMENTCONSUMEDPERUSE, str, mass),
                        Descriptor.DescriptorType.Requirement);
            }

            return res;
        }

        private IEnumerable<Descriptor> EffectDescriptors()
        {
            var res = new Descriptor[2];

            var mass = solidWastePerUse.mass;

            try
            {
                string str = GameTagExtensions.ProperName(ElementLoader.FindElementByHash(solidWastePerUse.elementID).tag);
                res[0] =
                    new Descriptor(
                        GetTooltipStr(ELEMENTEMITTEDPERUSE, str, mass),
                        GetTooltipStr(TOOLTIPS.ELEMENTEMITTEDPERUSE, str, mass),
                        Descriptor.DescriptorType.Effect);

                var di = Db.Get().Diseases[disease_db_idx];
                str = GameUtil.GetFormattedDiseaseAmount(DISEASE_PER_FLUSH << 1);

                res[1] =
                    new Descriptor(
                        string.Format(DISEASEEMITTEDPERUSE, di.Name, str),
                        string.Format(TOOLTIPS.DISEASEEMITTEDPERUSE, di.Name, str),
                        Descriptor.DescriptorType.DiseaseSource);
            }
            catch
            {
            }

            return res;
        }

        public List<Descriptor> GetDescriptors(GameObject go)
        {
            var res = new List<Descriptor>();
            res.AddRange(RequirementDescriptors());
            res.AddRange(EffectDescriptors());
            return res;
        }

        private void Flush(Worker w)
        {
            var ebh = ElementLoader.FindElementByHash(solidWastePerUse.elementID);

            var idx = disease_db_idx;

            var kpp = 
                ebh.substance.SpawnResource(
                    TransformExtensions.GetPosition(transform), 
                    solidWastePerUse.mass, 
                    SOLID_WASTE_TEMP, 
                    0,
                    0, true);

            storage.Store(kpp);
            
            w.GetComponent<PrimaryElement>().AddDisease(idx, DISEASE_PER_FLUSH, "FerzToilet.Flush");

            var pop_inst = PopFXManager.Instance;
            pop_inst.SpawnFX(
                pop_inst.sprite_Resource,
                string.Format(ADDED_POPFX, Db.Get().Diseases[idx].Name, DISEASE_PER_FLUSH << 1), transform,
                Vector3.up);

            Tutorial.Instance.TutorialMessage(Tutorial.TutorialMessages.TM_LotsOfGerms, true);

            var smi = base.smi;
            smi.sm.flushes.Set(++_flushesUsed, smi);

            meter.SetPositionPercent((float)_flushesUsed / MAX_FLUSHES);
        }

        private bool IsCanReadyUse()
        {
            var st = storage;
            var es = needSrcElems;

            return
                st.IsEmpty() == false &&
                st.Has(es[0]) && st.Has(es[1]);
        }

        public class StatesInstance : GameStateMachine<States, StatesInstance, FerzToilet, object>.GameInstance
        {
            public List<Chore> activeUseChores;
            public Chore cleanChore;

            public StatesInstance(FerzToilet master) : base(master) =>
                activeUseChores = new List<Chore>();

            public bool IsToxicSandRemoved() =>
                (object)master.storage.FindFirst(GameTagExtensions.Create(master.solidWastePerUse.elementID)) is null;

            public void CreateCleanChore()
            {
                cleanChore?.Cancel("dupe");

                cleanChore = 
                    new WorkChore<ToiletWorkableClean>(
                        Db.Get().ChoreTypes.CleanToilet,
                        master.GetComponent<ToiletWorkableClean>(), 
                        on_complete: OnCleanComplete, 
                        ignore_building_assignment: true);
            }

            public void CancelCleanChore()
            {
                cleanChore?.Cancel("Cancelled");
                cleanChore = null;
            }

            private void DropFromStorage(Tag tag)
            {
                ListPool<GameObject, FerzToilet>.PooledList result = ListPool<GameObject, FerzToilet>.Allocate();

                var st = master.storage;
                st.Find(tag, result);

                var list = (List<GameObject>)result;
                for (int i = list.Count; --i >= 0;)
                    st.Drop(list[i], true);

                result.Recycle();
            }

            public void OnCleanComplete(Chore chore)
            {
                cleanChore = null;

                DropFromStorage(GameTagExtensions.Create(master.solidWastePerUse.elementID));
                DropFromStorage(ElementLoader.FindElementByHash(SimHashes.Dirt).tag);

                var m = master;
                m.meter.SetPositionPercent((float)m._flushesUsed / MAX_FLUSHES);
            }
        }

        public class States : GameStateMachine<States, StatesInstance, FerzToilet>
        {
            private static readonly HashedString[] FULL_ANIMS = new HashedString[]
            {
                "full_pre",
                "full"
            };

            public State needElems;
            public State empty;
            public State notoperational;
            public State ready;
            public State earlyclean;
            public State earlyWaitingForClean;
            public State full;
            public State fullWaitingForClean;
            public IntParameter flushes = new IntParameter(0);

            public override void InitializeStates(out BaseState default_state)
            {
                default_state = needElems;

                root.PlayAnim("off")
                    .EventTransition(GameHashes.OnStorageChange, needElems, smi => !smi.master.IsCanReadyUse())
                    .EventTransition(GameHashes.OperationalChanged, notoperational, smi => !smi.Get<Operational>().IsOperational);

                var db = Db.Get();
                needElems.ToggleMainStatusItem(db.BuildingStatusItems.Unusable)
                    .EventTransition(GameHashes.OnStorageChange, ready, smi => smi.master.IsCanReadyUse());

                ready.ParamTransition(flushes, full, (smi, p) => MAX_FLUSHES - smi.master._flushesUsed <= 0)
                    .ToggleMainStatusItem(GlobalVars.FerzToilet_)
                    .ToggleRecurringChore(CreateUrgentUseChore)
                    .ToggleRecurringChore(CreateBreakUseChore)
                    .ToggleTag(GameTags.Usable)
                    .EventHandler(GameHashes.Flush, (smi, p) =>
                    {
                        var m = smi.master;
                        m.Flush(m.GetComponent<ToiletWorkableUse>().worker);
                    });

                earlyclean.PlayAnims(smi => FULL_ANIMS, KAnim.PlayMode.Once)
                    .OnAnimQueueComplete(earlyWaitingForClean);

                earlyWaitingForClean
                    .Enter(smi => smi.CreateCleanChore())
                    .Exit(smi => smi.CancelCleanChore())
                    .ToggleStatusItem(db.BuildingStatusItems.ToiletNeedsEmptying)
                    .ToggleMainStatusItem(db.BuildingStatusItems.Unusable)
                    .EventTransition(GameHashes.OnStorageChange, empty, smi => smi.IsToxicSandRemoved());

                full.PlayAnims(smi => FULL_ANIMS, KAnim.PlayMode.Once)
                    .OnAnimQueueComplete(fullWaitingForClean);

                fullWaitingForClean
                    .Enter(smi => smi.CreateCleanChore())
                    .Exit(smi => smi.CancelCleanChore())
                    .ToggleStatusItem(db.BuildingStatusItems.ToiletNeedsEmptying)
                    .ToggleMainStatusItem(db.BuildingStatusItems.Unusable)
                    .EventTransition(GameHashes.OnStorageChange, empty, smi => smi.IsToxicSandRemoved());

                empty.PlayAnim("off")
                    .Enter("ClearFlushes", smi =>
                    {
                        smi.master._flushesUsed = 0;
                        flushes.Set(0, smi);
                    })
                    .Enter("ClearDirt", smi => smi.master.storage.ConsumeAllIgnoringDisease())
                    .GoTo(needElems);

                notoperational.EventTransition(GameHashes.OperationalChanged, needElems, smi => smi.Get<Operational>().IsOperational)
                    .ToggleMainStatusItem(db.BuildingStatusItems.Unusable);
            }

            private static Chore CreateBreakUseChore(StatesInstance smi)
            {
                var inst = ChorePreconditions.instance;
                var db = Db.Get();

                var res = CreateUseChore(smi, db.ChoreTypes.BreakPee, inst);

                res.AddPrecondition(inst.IsScheduledTime, db.ScheduleBlockTypes.Hygiene);

                return res;
            }

            private static Chore CreateUrgentUseChore(StatesInstance smi)
            {
                var inst = ChorePreconditions.instance;

                var res = CreateUseChore(smi, Db.Get().ChoreTypes.Pee, inst);

                res.AddPrecondition(inst.NotCurrentlyPeeing);

                return res;
            }

            private static Chore CreateUseChore(StatesInstance smi, ChoreType ct, ChorePreconditions inst)
            {
                var wr = new WorkChore<ToiletWorkableUse>(
                    ct,
                    smi.master,
                    allow_in_red_alert: false,
                    ignore_schedule_block: true,
                    allow_prioritization: false,
                    priority_class: PriorityScreen.PriorityClass.personalNeeds,
                    add_to_daily_report: false);

                smi.activeUseChores.Add(wr);

                wr.onExit += (ec) =>
                {
                    smi.activeUseChores.Remove(ec);
                };

                wr.AddPrecondition(inst.IsPreferredAssignableOrUrgentBladder, smi.master.GetComponent<Assignable>());
                wr.AddPrecondition(inst.IsExclusivelyAvailableWithOtherChores, smi.activeUseChores);
                
                // common
                wr.AddPrecondition(inst.IsBladderFull);

                return wr;
            }

            public class ReadyStates : State
            {
                public State idle;
                public State inuse;
                public State flush;
            }
        }
    }
}
