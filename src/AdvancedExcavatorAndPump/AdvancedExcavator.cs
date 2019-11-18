/*
MIT License

Copyright (c) 2019 SuperComic <ekfvoddl3535@naver.com>

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.
*/

using System;
using System.Collections.Generic;
using KSerialization;
using UnityEngine;

namespace AdvancedExcavatorAndPump
{
    [SerializationConfig(MemberSerialization.OptIn)]
    public class AdvancedExcavator : KMonoBehaviour, ISim1000ms
    {
        protected const int OnOperChangedHash = -592767678;
        protected static readonly EventSystem.IntraObjectHandler<AdvancedExcavator> OnOperChangeDelegate = new EventSystem.IntraObjectHandler<AdvancedExcavator>(OnOperChangeStatic);

        private static void OnOperChangeStatic(AdvancedExcavator arg1, object arg2) => arg1.OnOperChanged((bool)arg2);

        #region 매우 잘 작동함
        public float consumptionRate = 5;
        [MyCmpGet]
        protected Operational operational;
        [MyCmpGet]
        protected ElementConsumer consumer;
        [MyCmpReq]
        protected MiningSounds miningsd;
        [MyCmpGet]
        protected KBatchedAnimController controller;
        public byte hardnessLv = 150;
        public int maxLength = 50;
        public sbyte sidemax = 1;
        [Serialize]
        protected int currentLength;
        protected int trcell;
        [Serialize]
        protected Queue<int> reservedCells = new Queue<int>();

        public bool HasDigCell => reservedCells.Count > 0;

        protected override void OnPrefabInit()
        {
            base.OnPrefabInit();
            simRenderLoadBalance = true;
        }

        protected override void OnSpawn()
        {
            base.OnSpawn();
            Subscribe(OnOperChangedHash, OnOperChangeDelegate);
            trcell = Grid.PosToCell(transform.localPosition);
            consumer.sampleCellOffset = new Vector3(0, -1 * Math.Max(1, currentLength));
        }

        protected virtual int GetSampleCell() => GetSampleCell(currentLength);

        protected int GetSampleCell(int len) => Grid.OffsetCell(trcell, new CellOffset(0, -1 * len));

        protected virtual void OnOperChanged(bool data)
        {
            if (data ^ operational.IsActive)
                operational.SetActive(data);
        }

        protected virtual void RefreshDiggableCell(int cell)
        {
            if (IsDiggableCell(cell) && Grid.NavValidatorMasks[cell] == 0) // buildingBack -> buildingFront
            {
                reservedCells.Enqueue(cell);
                Grid.CellToXY(cell, out int x, out int y);
                RefreshDiggableCell(x, y, -sidemax, -1);
                RefreshDiggableCell(x, y, 1, sidemax);
            }
        }

        protected void RefreshDiggableCell(int x, int y, int start, int end)
        {
            for (; start <= end; start++)
            {
                int ncell = Grid.XYToCell(x + start, y);
                if (Grid.IsValidCell(ncell) && IsDiggableCell(ncell))
                    reservedCells.Enqueue(ncell);
            }
        }

        protected bool IsDiggableCell(int cell) => 
            Grid.Solid[cell] && !Grid.Foundation[cell] && Grid.Element[cell].hardness <= hardnessLv;

        public void UpdateDig(float dt)
        {
            int digcell = reservedCells.Peek();
            Diggable.DoDigTick(digcell, dt);
            miningsd.SetPercentComplete(Grid.Damage[digcell]);
            if (!Grid.Solid[digcell])
                reservedCells.Dequeue();
        }

        public virtual void Sim1000ms(float dt)
        {
            if (operational.IsActive)
            {
                if (HasDigCell)
                    UpdateDig(dt);
                else if (currentLength <= maxLength)
                {
                    int ncell = GetSampleCell(currentLength + 1);
                    if (Grid.IsValidCell(ncell))
                    {
                        RefreshDiggableCell(ncell);
                        OnUpdateNextCell(ncell);
                    }
                }
                else
                {
                    operational.SetActive(false); // 최대 깊이에 도달, 더 이상의 계산은 불필요함
                    EnableConsum(false);
                }
            }
        }

        protected virtual void OnUpdateNextCell(int ncell)
        {
            Element.State temp = Grid.Element[ncell].state & Element.State.Solid;
            if (temp == Element.State.Liquid)
                EnableConsum(true);
            else
            {
                EnableConsum(false);
                if (HasDigCell ^ temp <= Element.State.Gas)
                {
                    currentLength++;
                    consumer.sampleCellOffset += Vector3.down;
                    controller.Play("working_loop", KAnim.PlayMode.Loop);
                }
                else
                    controller.Play("off", KAnim.PlayMode.Once);
            }
        }

        public void EnableConsum(bool enable)
        {
            consumer.EnableConsumption(enable);
            consumer.showDescriptor = enable;
        }
        #endregion
    }
}
