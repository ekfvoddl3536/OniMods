using System;
using System.Collections.Generic;
using Klei.AI;
using STRINGS;
using UnityEngine;

namespace SINT_SINT_Is_Not_Toilet
{
	// Token: 0x02000005 RID: 5
	public class FerzToilet : StateMachineComponent<FerzToilet.StatesInstance>, ISaveLoadable, IUsable, IEffectDescriptor, IGameObjectEffectDescriptor
	{
		// Token: 0x0600000B RID: 11 RVA: 0x00002540 File Offset: 0x00000740
		protected bool IsCanReadyUse()
		{
			return !this.storage.IsEmpty() && this.needSrcElems != null && this.storage.Has(this.needSrcElems[0]) && this.storage.Has(this.needSrcElems[1]);
		}

		// Token: 0x17000001 RID: 1
		// (get) Token: 0x0600000C RID: 12 RVA: 0x00002594 File Offset: 0x00000794
		// (set) Token: 0x0600000D RID: 13 RVA: 0x0000259C File Offset: 0x0000079C
		public int FlushesUsed
		{
			get
			{
				return this._flushesUsed;
			}
			set
			{
				this._flushesUsed = value;
				base.smi.sm.flushes.Set(value, base.smi);
			}
		}

		// Token: 0x0600000E RID: 14 RVA: 0x000025C4 File Offset: 0x000007C4
		protected override void OnSpawn()
		{
			base.OnSpawn();
			Components.Toilets.Add(this);
			base.smi.StartSM();
			base.GetComponent<ToiletWorkableUse>().trackUses = true;
			this.meter = FerzToilet.<OnSpawn>g__create|17_0(base.GetComponent<KBatchedAnimController>(), 1, -2, new string[]
			{
				"meter_target",
				"meter_arrow",
				"meter_scale"
			});
			this.meter.SetPositionPercent((float)this._flushesUsed / (float)this.maxFlushes);
			this.FlushesUsed = this._flushesUsed;
			base.Subscribe<FerzToilet>(493375141, FerzToilet.OnRefreshUserMenuDelegate);
		}

		// Token: 0x0600000F RID: 15 RVA: 0x00002662 File Offset: 0x00000862
		protected override void OnCleanUp()
		{
			base.OnCleanUp();
			Components.Toilets.Remove(this);
		}

		// Token: 0x06000010 RID: 16 RVA: 0x00002678 File Offset: 0x00000878
		public void Flush(Worker w)
		{
			float temp = base.GetComponent<PrimaryElement>().Temperature;
			Element ebh = ElementLoader.FindElementByHash(this.solidWastePerUse.elementID);
			byte idx = Db.Get().Diseases.GetIndex(this.diseaseId);
			if (this.UseDisease)
			{
				GameObject kpp = ebh.substance.SpawnResource(TransformExtensions.GetPosition(base.transform), this.solidWastePerUse.mass, temp, idx, this.diseasePerFlush, true, false, false);
				this.storage.Store(kpp, false, false, true, false);
				w.GetComponent<PrimaryElement>().AddDisease(idx, this.diseaseOnDupePerFlush, "FerzToilet.Flush");
				PopFXManager.Instance.SpawnFX(PopFXManager.Instance.sprite_Resource, string.Format(DUPLICANTS.DISEASES.ADDED_POPFX, Db.Get().Diseases[(int)idx].Name, this.diseasePerFlush + this.diseaseOnDupePerFlush), base.transform, Vector3.up, 1.5f, false, false);
				Tutorial.Instance.TutorialMessage(10, true);
			}
			else
			{
				this.storage.Store(ebh.substance.SpawnResource(TransformExtensions.GetPosition(base.transform), base.smi.MassPerFlush(), temp, idx, 0, true, false, false), false, false, true, false);
			}
			int flushesUsed = this.FlushesUsed;
			this.FlushesUsed = flushesUsed + 1;
			this.meter.SetPositionPercent((float)this._flushesUsed / (float)this.maxFlushes);
		}

		// Token: 0x06000011 RID: 17 RVA: 0x000027ED File Offset: 0x000009ED
		protected void SpawnMonster()
		{
			if (this.IsCanRot)
			{
				GameUtil.KInstantiate(Assets.GetPrefab("Glom"), TransformExtensions.GetPosition(base.smi.transform), 24, null, 0).SetActive(true);
			}
		}

		// Token: 0x06000012 RID: 18 RVA: 0x00002828 File Offset: 0x00000A28
		public List<Descriptor> RequirementDescriptors()
		{
			List<Descriptor> descriptorList = new List<Descriptor>();
			ManualDeliveryKG[] xstr = base.GetComponents<ManualDeliveryKG>();
			int x = 0;
			int max = xstr.Length;
			while (x < max)
			{
				string str = GameTagExtensions.ProperName(xstr[x].requestedItemTag);
				descriptorList.Add(new Descriptor(this.GetTooltipStr(UI.BUILDINGEFFECTS.ELEMENTCONSUMEDPERUSE, str), this.GetTooltipStr(UI.BUILDINGEFFECTS.TOOLTIPS.ELEMENTCONSUMEDPERUSE, str), 0, false));
				x++;
			}
			return descriptorList;
		}

		// Token: 0x06000013 RID: 19 RVA: 0x00002894 File Offset: 0x00000A94
		public List<Descriptor> EffectDescriptors()
		{
			Descriptor[] descriptorList = new Descriptor[2];
			try
			{
				string str = GameTagExtensions.ProperName(ElementLoader.FindElementByName(this.solidWastePerUse.elementID.ToString()).tag);
				descriptorList[0] = new Descriptor(this.GetTooltipStr(UI.BUILDINGEFFECTS.ELEMENTEMITTEDPERUSE, str), this.GetTooltipStr(UI.BUILDINGEFFECTS.TOOLTIPS.ELEMENTEMITTEDPERUSE, str), 1, false);
				Disease di = Db.Get().Diseases.Get(this.diseaseId);
				str = GameUtil.GetFormattedDiseaseAmount(this.diseasePerFlush + this.diseaseOnDupePerFlush);
				descriptorList[1] = new Descriptor(string.Format(UI.BUILDINGEFFECTS.DISEASEEMITTEDPERUSE, di.Name, str), string.Format(UI.BUILDINGEFFECTS.TOOLTIPS.DISEASEEMITTEDPERUSE, di.Name, str), 4, false);
			}
			catch
			{
			}
			return new List<Descriptor>(descriptorList);
		}

		// Token: 0x06000014 RID: 20 RVA: 0x0000297C File Offset: 0x00000B7C
		private string GetTooltipStr(string target, string _str)
		{
			return string.Format(target, _str, GameUtil.GetFormattedMass(base.smi.MassPerFlush(), 0, 0, true, "{0:0.##}"));
		}

		// Token: 0x06000015 RID: 21 RVA: 0x0000299D File Offset: 0x00000B9D
		public List<Descriptor> GetDescriptors(BuildingDef def)
		{
			List<Descriptor> list = new List<Descriptor>();
			list.AddRange(this.RequirementDescriptors());
			list.AddRange(this.EffectDescriptors());
			return list;
		}

		// Token: 0x06000016 RID: 22 RVA: 0x000029BC File Offset: 0x00000BBC
		public List<Descriptor> GetDescriptors(GameObject go)
		{
			List<Descriptor> list = new List<Descriptor>();
			list.AddRange(this.RequirementDescriptors());
			list.AddRange(this.EffectDescriptors());
			return list;
		}

		// Token: 0x06000017 RID: 23 RVA: 0x000029DB File Offset: 0x00000BDB
		public static void EventAction(FerzToilet component, object data)
		{
			component.OnRefreshUserMenu(data);
		}

		// Token: 0x06000018 RID: 24 RVA: 0x000029E4 File Offset: 0x00000BE4
		private void OnRefreshUserMenu(object data)
		{
			if (base.smi.GetCurrentState() == base.smi.sm.full || !base.smi.IsSoiled || base.smi.cleanChore != null)
			{
				return;
			}
			Game.Instance.userMenu.AddButton(base.gameObject, new KIconButtonMenu.ButtonInfo("status_item_toilet_needs_emptying", UI.USERMENUACTIONS.CLEANTOILET.NAME, delegate()
			{
				base.smi.GoTo(base.smi.sm.earlyclean);
			}, 250, null, null, null, UI.USERMENUACTIONS.CLEANTOILET.TOOLTIP, true), 1f);
		}

		// Token: 0x06000019 RID: 25 RVA: 0x00002A76 File Offset: 0x00000C76
		public bool IsUsable()
		{
			return base.smi.HasTag(GameTags.Usable);
		}

		// Token: 0x04000004 RID: 4
		private static readonly EventSystem.IntraObjectHandler<FerzToilet> OnRefreshUserMenuDelegate = new EventSystem.IntraObjectHandler<FerzToilet>(new Action<FerzToilet, object>(FerzToilet.EventAction));

		// Token: 0x04000005 RID: 5
		public bool UseDisease = true;

		// Token: 0x04000006 RID: 6
		public bool IsCanRot = true;

		// Token: 0x04000007 RID: 7
		[SerializeField]
		public int maxFlushes = 24;

		// Token: 0x04000008 RID: 8
		[SerializeField]
		public int _flushesUsed;

		// Token: 0x04000009 RID: 9
		[SerializeField]
		public Toilet.SpawnInfo solidWastePerUse;

		// Token: 0x0400000A RID: 10
		[SerializeField]
		public Toilet.SpawnInfo gasWasteWhenFull;

		// Token: 0x0400000B RID: 11
		[SerializeField]
		public string diseaseId = string.Empty;

		// Token: 0x0400000C RID: 12
		[SerializeField]
		public int diseasePerFlush;

		// Token: 0x0400000D RID: 13
		[SerializeField]
		public int diseaseOnDupePerFlush;

		// Token: 0x0400000E RID: 14
		[SerializeField]
		public Tag[] needSrcElems;

		// Token: 0x0400000F RID: 15
		[MyCmpReq]
		public Storage storage;

		// Token: 0x04000010 RID: 16
		private MeterController meter;

		// Token: 0x0200000B RID: 11
		public class StatesInstance : GameStateMachine<FerzToilet.States, FerzToilet.StatesInstance, FerzToilet, object>.GameInstance
		{
			// Token: 0x0600002F RID: 47 RVA: 0x0000308F File Offset: 0x0000128F
			public StatesInstance(FerzToilet master) : base(master)
			{
				this.activeUseChores = new List<Chore>();
			}

			// Token: 0x17000003 RID: 3
			// (get) Token: 0x06000030 RID: 48 RVA: 0x000030AE File Offset: 0x000012AE
			public bool IsSoiled
			{
				get
				{
					return base.master.FlushesUsed > 0;
				}
			}

			// Token: 0x06000031 RID: 49 RVA: 0x000030BE File Offset: 0x000012BE
			public int GetFlushesRemaining()
			{
				return base.master.maxFlushes - base.master.FlushesUsed;
			}

			// Token: 0x06000032 RID: 50 RVA: 0x000030D7 File Offset: 0x000012D7
			public bool HasDirt()
			{
				return base.master.IsCanReadyUse();
			}

			// Token: 0x06000033 RID: 51 RVA: 0x000030E4 File Offset: 0x000012E4
			public float MassPerFlush()
			{
				return base.master.solidWastePerUse.mass;
			}

			// Token: 0x06000034 RID: 52 RVA: 0x000030F6 File Offset: 0x000012F6
			public bool IsToxicSandRemoved()
			{
				return base.master.storage.FindFirst(GameTagExtensions.Create(base.master.solidWastePerUse.elementID)) == null;
			}

			// Token: 0x06000035 RID: 53 RVA: 0x00003124 File Offset: 0x00001324
			public void CreateCleanChore()
			{
				if (this.cleanChore != null)
				{
					this.cleanChore.Cancel("dupe");
				}
				this.cleanChore = new WorkChore<ToiletWorkableClean>(Db.Get().ChoreTypes.CleanToilet, base.master.GetComponent<ToiletWorkableClean>(), null, true, new Action<Chore>(this.OnCleanComplete), null, null, true, null, false, true, null, false, true, true, 0, 5, true, true);
			}

			// Token: 0x06000036 RID: 54 RVA: 0x0000318A File Offset: 0x0000138A
			public void CancelCleanChore()
			{
				if (this.cleanChore == null)
				{
					return;
				}
				this.cleanChore.Cancel("Cancelled");
				this.cleanChore = null;
			}

			// Token: 0x06000037 RID: 55 RVA: 0x000031AC File Offset: 0x000013AC
			private void OnCleanComplete(Chore chore)
			{
				this.cleanChore = null;
				Tag tag = GameTagExtensions.Create(base.master.solidWastePerUse.elementID);
				ListPool<GameObject, FerzToilet>.PooledList pooledList = ListPool<GameObject, FerzToilet>.Allocate();
				base.master.storage.Find(tag, pooledList);
				for (int x = 0; x < pooledList.Count; x++)
				{
					base.master.storage.Drop(pooledList[x], true);
				}
				pooledList.Recycle();
				base.master.meter.SetPositionPercent((float)base.master.FlushesUsed / (float)base.master.maxFlushes);
			}

			// Token: 0x06000038 RID: 56 RVA: 0x00003248 File Offset: 0x00001448
			public void Flush()
			{
				base.master.Flush(base.master.GetComponent<ToiletWorkableUse>().worker);
			}

			// Token: 0x04000047 RID: 71
			public float monsterSpawnTime = 1200f;

			// Token: 0x04000048 RID: 72
			public Chore cleanChore;

			// Token: 0x04000049 RID: 73
			public List<Chore> activeUseChores;
		}

		// Token: 0x0200000C RID: 12
		public class States : GameStateMachine<FerzToilet.States, FerzToilet.StatesInstance, FerzToilet>
		{
			// Token: 0x06000039 RID: 57 RVA: 0x00003268 File Offset: 0x00001468
			public override void InitializeStates(out StateMachine.BaseState default_state)
			{
				default_state = this.needElems;
				this.root.PlayAnim("off").EventTransition(-1697596308, this.needElems, (FerzToilet.StatesInstance smi) => !smi.HasDirt()).EventTransition(-592767678, this.notoperational, (FerzToilet.StatesInstance smi) => !smi.Get<Operational>().IsOperational);
				this.needElems.ToggleMainStatusItem(Db.Get().BuildingStatusItems.Unusable, null).EventTransition(-1697596308, this.ready, (FerzToilet.StatesInstance smi) => smi.HasDirt());
				this.ready.ParamTransition<int>(this.flushes, this.full, (FerzToilet.StatesInstance smi, int p) => smi.GetFlushesRemaining() <= 0).ToggleMainStatusItem(Consts.FerzToilet, null).ToggleRecurringChore(new Func<FerzToilet.StatesInstance, Chore>(this.CreateUrgentUseChore), null).ToggleRecurringChore(new Func<FerzToilet.StatesInstance, Chore>(this.CreateBreakUseChore), null).ToggleTag(GameTags.Usable).EventHandler(-350347868, delegate(FerzToilet.StatesInstance smi, object p)
				{
					smi.Flush();
				});
				this.earlyclean.PlayAnims((FerzToilet.StatesInstance smi) => FerzToilet.States.FULL_ANIMS, 1).OnAnimQueueComplete(this.earlyWaitingForClean);
				this.earlyWaitingForClean.Enter(delegate(FerzToilet.StatesInstance smi)
				{
					smi.CreateCleanChore();
				}).Exit(delegate(FerzToilet.StatesInstance smi)
				{
					smi.CancelCleanChore();
				}).ToggleStatusItem(Db.Get().BuildingStatusItems.ToiletNeedsEmptying, null).ToggleMainStatusItem(Db.Get().BuildingStatusItems.Unusable, null).EventTransition(-1697596308, this.empty, (FerzToilet.StatesInstance smi) => smi.IsToxicSandRemoved());
				this.full.PlayAnims((FerzToilet.StatesInstance smi) => FerzToilet.States.FULL_ANIMS, 1).OnAnimQueueComplete(this.fullWaitingForClean);
				this.fullWaitingForClean.Enter(delegate(FerzToilet.StatesInstance smi)
				{
					smi.CreateCleanChore();
				}).Exit(delegate(FerzToilet.StatesInstance smi)
				{
					smi.CancelCleanChore();
				}).ToggleStatusItem(Db.Get().BuildingStatusItems.ToiletNeedsEmptying, null).ToggleMainStatusItem(Db.Get().BuildingStatusItems.Unusable, null).EventTransition(-1697596308, this.empty, (FerzToilet.StatesInstance smi) => smi.IsToxicSandRemoved()).Enter(delegate(FerzToilet.StatesInstance smi)
				{
					smi.Schedule(smi.monsterSpawnTime, delegate(object _parm)
					{
						smi.master.SpawnMonster();
					}, null);
				});
				this.empty.PlayAnim("off").Enter("ClearFlushes", delegate(FerzToilet.StatesInstance smi)
				{
					smi.master.FlushesUsed = 0;
				}).Enter("ClearDirt", delegate(FerzToilet.StatesInstance smi)
				{
					smi.master.storage.ConsumeAllIgnoringDisease();
				}).GoTo(this.needElems);
				this.notoperational.EventTransition(-592767678, this.needElems, (FerzToilet.StatesInstance smi) => smi.Get<Operational>().IsOperational).ToggleMainStatusItem(Db.Get().BuildingStatusItems.Unusable, null);
			}

			// Token: 0x0600003A RID: 58 RVA: 0x0000366B File Offset: 0x0000186B
			private Chore CreateUrgentUseChore(FerzToilet.StatesInstance smi)
			{
				Chore chore = this.CreateUseChore(smi, Db.Get().ChoreTypes.Pee);
				chore.AddPrecondition(ChorePreconditions.instance.IsBladderFull, null);
				chore.AddPrecondition(ChorePreconditions.instance.NotCurrentlyPeeing, null);
				return chore;
			}

			// Token: 0x0600003B RID: 59 RVA: 0x000036A8 File Offset: 0x000018A8
			private Chore CreateUseChore(FerzToilet.StatesInstance smi, ChoreType ct)
			{
				WorkChore<ToiletWorkableUse> wr = new WorkChore<ToiletWorkableUse>(ct, smi.master, null, true, null, null, null, false, null, true, true, null, false, true, false, 2, 5, false, false);
				smi.activeUseChores.Add(wr);
				WorkChore<ToiletWorkableUse> workChore = wr;
				workChore.onExit = (Action<Chore>)Delegate.Combine(workChore.onExit, new Action<Chore>(delegate(Chore ec)
				{
					smi.activeUseChores.Remove(ec);
				}));
				wr.AddPrecondition(ChorePreconditions.instance.IsPreferredAssignableOrUrgentBladder, smi.master.GetComponent<Assignable>());
				wr.AddPrecondition(ChorePreconditions.instance.IsExclusivelyAvailableWithOtherChores, smi.activeUseChores);
				return wr;
			}

			// Token: 0x0600003C RID: 60 RVA: 0x00003754 File Offset: 0x00001954
			private Chore CreateBreakUseChore(FerzToilet.StatesInstance smi)
			{
				Chore chore = this.CreateUseChore(smi, Db.Get().ChoreTypes.BreakPee);
				chore.AddPrecondition(ChorePreconditions.instance.IsBladderFull, null);
				chore.AddPrecondition(ChorePreconditions.instance.IsScheduledTime, Db.Get().ScheduleBlockTypes.Recreation);
				return chore;
			}

			// Token: 0x0400004A RID: 74
			private static readonly HashedString[] FULL_ANIMS = new HashedString[]
			{
				"full_pre",
				"full"
			};

			// Token: 0x0400004B RID: 75
			public StateMachine<FerzToilet.States, FerzToilet.StatesInstance, FerzToilet, object>.IntParameter flushes = new StateMachine<FerzToilet.States, FerzToilet.StatesInstance, FerzToilet, object>.IntParameter(0);

			// Token: 0x0400004C RID: 76
			public GameStateMachine<FerzToilet.States, FerzToilet.StatesInstance, FerzToilet, object>.State needElems;

			// Token: 0x0400004D RID: 77
			public GameStateMachine<FerzToilet.States, FerzToilet.StatesInstance, FerzToilet, object>.State empty;

			// Token: 0x0400004E RID: 78
			public GameStateMachine<FerzToilet.States, FerzToilet.StatesInstance, FerzToilet, object>.State notoperational;

			// Token: 0x0400004F RID: 79
			public GameStateMachine<FerzToilet.States, FerzToilet.StatesInstance, FerzToilet, object>.State ready;

			// Token: 0x04000050 RID: 80
			public GameStateMachine<FerzToilet.States, FerzToilet.StatesInstance, FerzToilet, object>.State earlyclean;

			// Token: 0x04000051 RID: 81
			public GameStateMachine<FerzToilet.States, FerzToilet.StatesInstance, FerzToilet, object>.State earlyWaitingForClean;

			// Token: 0x04000052 RID: 82
			public GameStateMachine<FerzToilet.States, FerzToilet.StatesInstance, FerzToilet, object>.State full;

			// Token: 0x04000053 RID: 83
			public GameStateMachine<FerzToilet.States, FerzToilet.StatesInstance, FerzToilet, object>.State fullWaitingForClean;

			// Token: 0x0200000D RID: 13
			public class ReadyStates : GameStateMachine<FerzToilet.States, FerzToilet.StatesInstance, FerzToilet, object>.State
			{
				// Token: 0x04000054 RID: 84
				public GameStateMachine<FerzToilet.States, FerzToilet.StatesInstance, FerzToilet, object>.State idle;

				// Token: 0x04000055 RID: 85
				public GameStateMachine<FerzToilet.States, FerzToilet.StatesInstance, FerzToilet, object>.State inuse;

				// Token: 0x04000056 RID: 86
				public GameStateMachine<FerzToilet.States, FerzToilet.StatesInstance, FerzToilet, object>.State flush;
			}
		}
	}
}
