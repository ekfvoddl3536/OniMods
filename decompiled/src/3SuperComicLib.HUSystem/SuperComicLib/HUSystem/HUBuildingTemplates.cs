using System;
using UnityEngine;

namespace SuperComicLib.HUSystem
{
	// Token: 0x02000005 RID: 5
	public static class HUBuildingTemplates
	{
		// Token: 0x06000004 RID: 4 RVA: 0x00002074 File Offset: 0x00000274
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
			instance.SceneLayer = 19;
			instance.MaterialCategory = construction_materials;
			instance.BaseMeltingPoint = melting_point;
			if (buildLocationRule == null || buildLocationRule == 6 || buildLocationRule - 8 <= 2)
			{
				instance.ContinuouslyCheckFoundation = false;
			}
			else
			{
				instance.ContinuouslyCheckFoundation = true;
			}
			instance.BuildLocationRule = buildLocationRule;
			instance.ObjectLayer = 1;
			instance.AnimFiles = new KAnimFile[]
			{
				Assets.GetAnim(anim)
			};
			instance.GenerateOffsets();
			instance.BaseDecor = (float)decor.amount;
			instance.BaseDecorRadius = (float)decor.radius;
			instance.BaseNoisePollution = noise.amount;
			instance.BaseNoisePollutionRadius = noise.radius;
			return instance;
		}
	}
}
