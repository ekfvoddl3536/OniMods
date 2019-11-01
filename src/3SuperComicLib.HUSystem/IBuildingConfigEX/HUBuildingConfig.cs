using UnityEngine;

namespace SuperComicLib.HUSystem
{
    public abstract class HUBuildingConfig : IBuildingConfig
    {
        public override void DoPostConfigureComplete(GameObject go) => go.AddOrGet<BuildingCellVisualizer>();
        public override void DoPostConfigureUnderConstruction(GameObject go) => go.AddOrGet<BuildingCellVisualizer>();
        public override void DoPostConfigurePreview(BuildingDef def, GameObject go) => go.AddOrGet<BuildingCellVisualizer>();
    }
}
