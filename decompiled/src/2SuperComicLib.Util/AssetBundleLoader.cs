using System;
using UnityEngine;

namespace SuperComicLib.Util
{
	// Token: 0x02000002 RID: 2
	public static class AssetBundleLoader
	{
		// Token: 0x06000001 RID: 1 RVA: 0x00002050 File Offset: 0x00000250
		public static void LoadFromFile(string setname, string asset_file_path_with_name)
		{
			Object[] vs = AssetBundle.LoadFromFile(asset_file_path_with_name).LoadAllAssets();
			ModUtil.AddKAnim(setname, (TextAsset)vs[vs.Length - 2], (TextAsset)vs[vs.Length - 1], (Texture2D)vs[0]);
		}

		// Token: 0x06000002 RID: 2 RVA: 0x00002090 File Offset: 0x00000290
		public static Object[] LoadFromFile(string asset_file_path_with_name)
		{
			return AssetBundle.LoadFromFile(asset_file_path_with_name).LoadAllAssets();
		}
	}
}
