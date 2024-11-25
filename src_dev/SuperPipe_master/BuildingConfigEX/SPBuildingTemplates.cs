using UnityEngine;

namespace SuperPipe
{
    public static class SPBuildingTemplates
    {
        public static SPBuildingDef CreateDef(
            string id, 
            int width,
            int height,
            string anim, 
            int hitpoints, 
            float construction_time,
            float[] construction_mass,
            string[] construction_materials,
            float melting_point, 
            BuildLocationRule buildLocationRule, 
            EffectorValues decor,
            EffectorValues noise, 
            float temperature_modification_mass_scale = 0.2f)
        {
            SPBuildingDef res = ScriptableObject.CreateInstance<SPBuildingDef>();

            res.PrefabID = id;
            
            res.InitDef();
            
            res.name = id;
            
            res.Mass = construction_mass;
            res.MassForTemperatureModification = construction_mass[0] * temperature_modification_mass_scale;

            res.WidthInCells = width;
            res.HeightInCells = height;

            res.HitPoints = hitpoints;

            res.ConstructionTime = construction_time;

            res.SceneLayer = Grid.SceneLayer.Building;

            res.MaterialCategory = construction_materials;

            res.BaseMeltingPoint = melting_point;

            switch (buildLocationRule)
            {
                case BuildLocationRule.Tile:
                case BuildLocationRule.Conduit:
                case BuildLocationRule.LogicBridge:
                case BuildLocationRule.WireBridge:
                case BuildLocationRule.Anywhere:
                    res.ContinuouslyCheckFoundation = false;
                    break;

                default:
                    res.ContinuouslyCheckFoundation = true;
                    break;
            }

            res.BuildLocationRule = buildLocationRule;

            res.ObjectLayer = ObjectLayer.Building;

            res.AnimFiles = new KAnimFile[1] { Assets.GetAnim(anim) };

            res.GenerateOffsets();

            res.BaseDecor = decor.amount;
            res.BaseDecorRadius = decor.radius;

            res.BaseNoisePollution = noise.amount;
            res.BaseNoisePollutionRadius = noise.radius;

            return res;
        }
    }
}
