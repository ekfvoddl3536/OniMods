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

namespace AdvancedGenerators
{
    using static EventSystem;
    public abstract class AdvancedGeneratorBase : Generator
    {
        protected static readonly IntraObjectHandler<AdvancedGeneratorBase> OnActivateChangeDelegate = 
            new IntraObjectHandler<AdvancedGeneratorBase>(OnActivateChangedStatic);

        public bool HasMeter = true;
        public Meter.Offset MeterOffset;
        protected MeterController meter;

        private static void OnActivateChangedStatic(AdvancedGeneratorBase gen, object data) => 
            gen.OnActivateChanged((Operational)data);

        protected virtual void OnActivateChanged(Operational op)
        {
            var db = Db.Get();

            selectable.SetStatusItem(
                db.StatusItemCategories.Power,
                op.IsActive 
                ? db.BuildingStatusItems.Wattage
                : db.BuildingStatusItems.GeneratorOffline,
                this);
        }

        protected override void OnPrefabInit()
        {
            base.OnPrefabInit();
            Subscribe((int)GameHashes.ActiveChanged, OnActivateChangeDelegate);
        }

        protected override void OnSpawn()
        {
            base.OnSpawn();
            OnSpawnPost();
        }

        protected virtual void OnSpawnPost()
        {
            if (HasMeter)
                meter = new MeterController(GetComponent<KBatchedAnimController>(), "meter_target", "meter", MeterOffset, Grid.SceneLayer.NoLayer, new[]
                {
                    "meter_target",
                    "meter_fill",
                    "meter_frame",
                    "meter_OL"
                });
        }
    }
}