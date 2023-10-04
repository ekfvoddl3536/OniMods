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

#pragma warning disable IDE0031 // Null 전파 사용
using KSerialization;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace EcoFriendlyToilet
{
    [SerializationConfig(MemberSerialization.OptIn)]
    public sealed unsafe class WildPlantablePlot : PlantablePlot, ISaveLoadable, IGameObjectEffectDescriptor
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Ref<KPrefabID> GetPlantRef(PlantablePlot pp) =>
            Unsafe.As<byte, Ref<KPrefabID>>(ref Unsafe.AsRef<byte>((byte*)Unsafe.AsPointer(ref pp.occupyingObjectVisualOffset) - 0x10));

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

            if (plant_ != null)
            {
                plant = plant_;
                SetOccupyInternal(plant_.gameObject);
            }
        }

        protected override GameObject SpawnOccupyingObject(GameObject go)
        {
            var seed = go.GetComponent<PlantableSeed>();
            if (seed == null)
            {
                destroyEntityOnDeposit = false;
                return go;
            }

            var poscbc = Grid.CellToPosCBC(Grid.PosToCell(this), plantLayer);
            var g1 = GameUtil.KInstantiate(Assets.GetPrefab(seed.PlantID), poscbc, plantLayer);
            
            var c1 = g1.GetComponent<MutantPlant>();
            if (c1 != null)
                go.GetComponent<MutantPlant>().CopyMutationsTo(c1);

            g1.SetActive(true);

            destroyEntityOnDeposit = true;

            return g1;
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
            GetPlantRef(this).Set(newPlant.GetComponent<KPrefabID>());

            PlantSettingInternal(newPlant);
        }

        private void SetOccupyInternal(GameObject go)
        {
            occupyingObject = go;
            
            var comp = go.GetComponent<HarvestDesignatable>();
            if (comp != null)
                comp.SetHarvestWhenReady(true);
        }
    }
}
