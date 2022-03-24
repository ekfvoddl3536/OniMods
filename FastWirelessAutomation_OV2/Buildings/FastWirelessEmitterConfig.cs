#region LICENSE
/*
MIT License

Copyright (c) 2022. Super Comic (ekfvoddl3535@naver.com)

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
#endregion
using UnityEngine;
using TUNING;
using System.Collections.Generic;

namespace FastWirelessAutomation
{
    using static GlobalConsts;
    using static GlobalConsts.FastWirelessEmitter;
    using static STRINGS.UI.LOGIC_PORTS;
    using LPORT = LogicPorts.Port;
    public class FastWirelessEmitterConfig : IBuildingConfig
    {
        public override BuildingDef CreateBuildingDef()
        {
            BuildingDef bd = BuildingTemplates.CreateBuildingDef(ID, 1, 1, ANISTR, HITPT, CONTIME,
                TMass, TMate, MELTPT, BuildLocationRule.Anywhere, DECOR.NONE, NOISE_POLLUTION.NONE);

            bd.Overheatable = bd.Floodable = bd.Entombable = false;
            
            bd.RequiresPowerInput = true;

            bd.ViewMode = OverlayModes.Logic.ID;

            bd.AudioCategory = AU_CATE;
            bd.SceneLayer = Grid.SceneLayer.Building;

            bd.EnergyConsumptionWhenActive = USE_POWER;
            bd.SelfHeatKilowattsWhenActive = bd.ExhaustKilowattsWhenActive = 0;
            
            bd.PowerInputOffset = default; // (0, 0)

            bd.DragBuild = true;

            bd.LogicInputPorts = new List<LPORT>(1)
            {
                new LPORT(
                    LogicOperationalController.PORT_ID, 
                    default, 
                    INPUT_PORTS, 
                    GATE_SINGLE_INPUT_ONE_ACTIVE, 
                    GATE_SINGLE_INPUT_ONE_INACTIVE, 
                    false, 
                    LogicPortSpriteType.Input)
            };

            var inst = SoundEventVolumeCache.instance;
            inst.AddVolume("switchgaspressure_kanim", "PowerSwitch_on", NOISE_POLLUTION.NOISY.TIER3);
            inst.AddVolume("switchgaspressure_kanim", "PowerSwitch_off", NOISE_POLLUTION.NOISY.TIER3);

            GeneratedBuildings.RegisterWithOverlay(OverlayModes.Logic.HighlightItemIDs, ID);

            return bd;
        }

        public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag) =>
            go.AddOrGet<BuildingComplete>().isManuallyOperated = false;

        public override void DoPostConfigureComplete(GameObject go)
        {
            go.AddOrGet<CopyBuildingSettings>();

            go.AddOrGet<LogicOperationalController>().unNetworkedValue = 0;

            go.AddOrGet<SignalEmitter>();

            go.GetComponent<KPrefabID>().AddTag(GameTags.OverlayInFrontOfConduits);
        }
    }
}
