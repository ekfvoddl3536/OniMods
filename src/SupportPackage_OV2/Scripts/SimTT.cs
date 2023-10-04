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

using System;
using System.Collections.Generic;
using UnityEngine;

namespace SupportPackage
{
    public sealed class SimTT
    {
        private readonly Storage storage;
        private readonly float temperature;
        private readonly float heatCapacity;
        private readonly float thermalConductivity;
        private bool active;

        public SimTT(float sim_temp, float heat_capa, float thermal_cond, Storage storage)
        {
            temperature = sim_temp;
            heatCapacity = heat_capa;
            thermalConductivity = thermal_cond;

            this.storage = storage;

            storage.gameObject.Subscribe((int)GameHashes.OnStorageChange, OnStorageChanged);
        }

        public void SetActive(bool active)
        {
            var list = storage.items;
            var cnt = list.Count;

            this.active = active;
            if (active)
                for (int i = 0; i < cnt; ++i)
                {
                    var go = list[i];
                    if (go != null)
                        OnItemSimRegistered(go.GetComponent<SimTemperatureTransfer>());
                }
            else
                for (int i = 0; i < cnt; ++i)
                {
                    var go = list[i];
                    if (go != null)
                        Unregister(go.GetComponent<SimTemperatureTransfer>());
                }
        }

        private void OnStorageChanged(object data)
        {
            var go = (GameObject)data;
            var tt = go.GetComponent<SimTemperatureTransfer>();
            
            if (tt == null)
                return;

            var pick = go.GetComponent<Pickupable>();
            if (pick != null)
            {
                if (active && pick.storage == (object)storage)
                    Register(tt);
                else
                    Unregister(tt);
            }
        }

        private void Register(SimTemperatureTransfer tt)
        {
            var act = new Action<SimTemperatureTransfer>(OnItemSimRegistered);
            tt.onSimRegistered -= act;
            tt.onSimRegistered += act;

            if (Sim.IsValidHandle(tt.SimHandle))
                OnItemSimRegistered(tt);
        }

        private void Unregister(SimTemperatureTransfer tt)
        {
            tt.onSimRegistered -= OnItemSimRegistered;

            if (Sim.IsValidHandle(tt.SimHandle))
                SimMessages.ModifyElementChunkTemperatureAdjuster(tt.SimHandle, 0, 0, 0);
        }

        private void OnItemSimRegistered(SimTemperatureTransfer tt)
        {
            if (tt != null && Sim.IsValidHandle(tt.SimHandle))
            {
                var n = temperature;
                var h = heatCapacity;
                var t = thermalConductivity;
                
                if (!active)
                    n = h = t = 0;

                SimMessages.ModifyElementChunkTemperatureAdjuster(tt.SimHandle, n, h, t);
            }
        }

        public void CleanUp()
        {
            storage.gameObject.Unsubscribe((int)GameHashes.OnStorageChange, OnStorageChanged);

            var list = storage.items;
            for (int i = 0, cnt = list.Count; i < cnt; ++i)
            {
                var item = list[i];
                if (item != null)
                    Unregister(item.GetComponent<SimTemperatureTransfer>());
            }
        }
    }
}
