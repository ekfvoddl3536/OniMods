using UnityEngine;

namespace SuperComicLib.HUSystem
{
    public static class HUBuildingTemplates
    {
        public static HUBuildingDef CreateDef(string id, int width, int height, string anim, int hitpoints, float construction_time, float[] construction_mass, string[] construction_materials, float melting_point, BuildLocationRule buildLocationRule, EffectorValues decor, EffectorValues noise, float temperature_modification_mass_scale = 0.2f)
        {
            HUBuildingDef instance = ScriptableObject.CreateInstance<HUBuildingDef>();
            instance.PrefabID = id;
            instance.InitDef();
            instance.name = id;
            instance.Mass = construction_mass;
            instance.MassForTemperatureModification = construction_mass[0] * temperature_modification_mass_scale;
            instance.WidthInCells = width;
            instance.HeightInCells = height;
            instance.HitPoints = hitpoints;
            instance.ConstructionTime = construction_time;
            instance.SceneLayer = Grid.SceneLayer.Building;
            instance.MaterialCategory = construction_materials;
            instance.BaseMeltingPoint = melting_point;
            switch (buildLocationRule)
            {
                case BuildLocationRule.Tile:
                case BuildLocationRule.Conduit:
                case BuildLocationRule.LogicBridge:
                case BuildLocationRule.WireBridge:
                case BuildLocationRule.Anywhere:
                    instance.ContinuouslyCheckFoundation = false;
                    break;
                default:
                    instance.ContinuouslyCheckFoundation = true;
                    break;
            }
            instance.BuildLocationRule = buildLocationRule;
            instance.ObjectLayer = ObjectLayer.Building;
            instance.AnimFiles = new KAnimFile[] { Assets.GetAnim(anim) };
            instance.GenerateOffsets();
            instance.BaseDecor = decor.amount;
            instance.BaseDecorRadius = decor.radius;
            instance.BaseNoisePollution = noise.amount;
            instance.BaseNoisePollutionRadius = noise.radius;
            return instance;
        }
    }
}
