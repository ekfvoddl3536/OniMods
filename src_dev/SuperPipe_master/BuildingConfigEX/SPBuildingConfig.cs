using UnityEngine;

namespace SuperPipe
{
    public abstract class SPBuildingConfig : IBuildingConfig
    {
        public override void DoPostConfigureComplete(GameObject go) =>
            go.AddOrGet<BuildingCellVisualizer>();

        public override void DoPostConfigureUnderConstruction(GameObject go) =>
            go.AddOrGet<BuildingCellVisualizer>();

        public override void DoPostConfigurePreview(BuildingDef def, GameObject go) =>
            go.AddOrGet<BuildingCellVisualizer>();
    }
}
