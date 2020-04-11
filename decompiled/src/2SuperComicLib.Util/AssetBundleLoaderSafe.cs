using System;
using System.IO;
using UnityEngine;

namespace SuperComicLib.Util
{
	// Token: 0x02000003 RID: 3
	public sealed class AssetBundleLoaderSafe
	{
		// Token: 0x06000003 RID: 3 RVA: 0x0000209D File Offset: 0x0000029D
		public AssetBundleLoaderSafe(string path)
		{
			if (path == null)
			{
				throw new ArgumentNullException("path");
			}
			this.local = path;
		}

		// Token: 0x06000004 RID: 4 RVA: 0x000020BB File Offset: 0x000002BB
		public void LoadAutomatic(string setname, string asset_file_url)
		{
			AssetBundleLoader.LoadFromFile(setname, Path.Combine(this.local, asset_file_url));
		}

		// Token: 0x06000005 RID: 5 RVA: 0x000020CF File Offset: 0x000002CF
		public Object[] Load(string asset_file_url)
		{
			return AssetBundleLoader.LoadFromFile(Path.Combine(this.local, asset_file_url));
		}

		// Token: 0x04000001 RID: 1
		private string local;
	}
}
