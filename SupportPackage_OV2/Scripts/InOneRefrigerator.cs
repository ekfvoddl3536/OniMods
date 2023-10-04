// MIT License
//
// Copyright (c) 2022-2023. Super Comic (ekfvoddl3535@naver.com)
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
using UnityEngine;

namespace SupportPackage
{
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
            filteredStorage = new FilteredStorage(this, new Tag[1]
            {
                GameTags.Compostable
            }, this, false, Db.Get().ChoreTypes.FoodFetch);

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
}
