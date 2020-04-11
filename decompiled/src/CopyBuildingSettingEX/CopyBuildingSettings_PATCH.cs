using System;
using System.Reflection;
using Harmony;
using STRINGS;
using UnityEngine;

namespace CopyBuildingSettingEX
{
	// Token: 0x02000002 RID: 2
	[HarmonyPatch(typeof(CopyBuildingSettings), "ApplyCopy")]
	public class CopyBuildingSettings_PATCH
	{
		// Token: 0x06000001 RID: 1 RVA: 0x00002050 File Offset: 0x00000250
		public static bool Prefix(int targetCell, GameObject sourceGameObject, ref bool __result)
		{
			int ol = 1;
			BuildingComplete com = sourceGameObject.GetComponent<BuildingComplete>();
			if (com != null)
			{
				ol = com.Def.ObjectLayer;
			}
			GameObject go = Grid.Objects[targetCell, ol];
			if (go != null && go != sourceGameObject)
			{
				KPrefabID kp = sourceGameObject.GetComponent<KPrefabID>();
				if (kp != null)
				{
					KPrefabID kp2 = go.GetComponent<KPrefabID>();
					if (kp2 != null)
					{
						CopyBuildingSettings cp = sourceGameObject.GetComponent<CopyBuildingSettings>();
						if (cp != null)
						{
							CopyBuildingSettings cp2 = go.GetComponent<CopyBuildingSettings>();
							if (cp2 != null)
							{
								if (cp.copyGroupTag != Tag.Invalid)
								{
									if (cp.copyGroupTag != cp2.copyGroupTag)
									{
										return CopyBuildingSettings_PATCH.RetFix(ref __result, false);
									}
								}
								else if (KPrefabIDExtensions.PrefabID(kp) != KPrefabIDExtensions.PrefabID(kp2))
								{
									return CopyBuildingSettings_PATCH.RetFix(ref __result, false);
								}
								kp2.Trigger(-905833192, sourceGameObject);
								AutoDisinfectable ad = sourceGameObject.GetComponent<AutoDisinfectable>();
								if (ad != null)
								{
									AutoDisinfectable ad2 = go.GetComponent<AutoDisinfectable>();
									if (ad2 != null)
									{
										Traverse.Create(ad2).Field<bool>("enableAutoDisinfect").Value = (bool)typeof(AutoDisinfectable).GetField("enableAutoDisinfect", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(ad);
										ad2.RefreshChore();
									}
								}
								PopFXManager.Instance.SpawnFX(PopFXManager.Instance.sprite_Plus, UI.COPIED_SETTINGS, go.transform, new Vector3(0f, 0.5f, 0f), 1.5f, false, false);
								return CopyBuildingSettings_PATCH.RetFix(ref __result, true);
							}
						}
					}
				}
			}
			return CopyBuildingSettings_PATCH.RetFix(ref __result, false);
		}

		// Token: 0x06000002 RID: 2 RVA: 0x000021CD File Offset: 0x000003CD
		private static bool RetFix(ref bool _res, bool val)
		{
			_res = val;
			return false;
		}
	}
}
