using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Harmony;
using SuperComicLib.Sandfile;

namespace MoreSpecialGeysers
{
	// Token: 0x02000006 RID: 6
	[HarmonyPatch(typeof(EntityConfigManager))]
	[HarmonyPatch("LoadGeneratedEntities")]
	[HarmonyPatch(new Type[]
	{
		typeof(List<Type>)
	})]
	public class EntityConfMgrPath
	{
		// Token: 0x06000011 RID: 17 RVA: 0x000024C0 File Offset: 0x000006C0
		public static void Prefix()
		{
			string directoryName = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
			string __path = Path.Combine(directoryName, "textpatch.txt");
			string __path2 = Path.Combine(directoryName, "textpatch.sndf");
			if (File.Exists(__path))
			{
				EntityConfMgrPath.UpdateString(__path);
			}
			else if (File.Exists(__path2))
			{
				EntityConfMgrPath.UpdateString(__path2);
			}
			else
			{
				EntityConfMgrPath.DefualtPatch();
			}
			CustomGeysers.Load(Path.Combine(directoryName, "custom_geysers"));
		}

		// Token: 0x06000012 RID: 18 RVA: 0x00002528 File Offset: 0x00000728
		private static void UpdateString(string path)
		{
			using (SandfileParser sp = new SandfileParser(path))
			{
				sp.Start();
				EntityConfMgrPath.SetString(sp, MSG_CONST.TUNGSTEN.ID_UPPER + ".NAME", MSG_CONST.TUNGSTEN.NAME);
				EntityConfMgrPath.SetString(sp, MSG_CONST.TUNGSTEN.ID_UPPER + ".DESC", MSG_CONST.TUNGSTEN.DESC);
				EntityConfMgrPath.SetString(sp, MSG_CONST.COOLWATER.ID_UPPER + ".NAME", MSG_CONST.COOLWATER.NAME);
				EntityConfMgrPath.SetString(sp, MSG_CONST.COOLWATER.ID_UPPER + ".DESC", MSG_CONST.COOLWATER.DESC);
				EntityConfMgrPath.SetString(sp, MSG_CONST.LIQOXYGEN.ID_UPPER + ".NAME", MSG_CONST.LIQOXYGEN.NAME);
				EntityConfMgrPath.SetString(sp, MSG_CONST.LIQOXYGEN.ID_UPPER + ".DESC", MSG_CONST.LIQOXYGEN.DESC);
				EntityConfMgrPath.SetString(sp, MSG_CONST.LIQHELIUM.ID_UPPER + ".NAME", MSG_CONST.LIQHELIUM.NAME);
				EntityConfMgrPath.SetString(sp, MSG_CONST.LIQHELIUM.ID_UPPER + ".DESC", MSG_CONST.LIQHELIUM.DESC);
			}
		}

		// Token: 0x06000013 RID: 19 RVA: 0x00002664 File Offset: 0x00000864
		public static void DefualtPatch()
		{
			EntityConfMgrPath.Strp(MSG_CONST.TUNGSTEN.ID_UPPER, MSG_CONST.TUNGSTEN.NAME, MSG_CONST.TUNGSTEN.DESC);
			EntityConfMgrPath.Strp(MSG_CONST.COOLWATER.ID_UPPER, MSG_CONST.COOLWATER.NAME, MSG_CONST.COOLWATER.DESC);
			EntityConfMgrPath.Strp(MSG_CONST.LIQOXYGEN.ID_UPPER, MSG_CONST.LIQOXYGEN.NAME, MSG_CONST.LIQOXYGEN.DESC);
			EntityConfMgrPath.Strp(MSG_CONST.LIQHELIUM.ID_UPPER, MSG_CONST.LIQHELIUM.NAME, MSG_CONST.LIQHELIUM.DESC);
		}

		// Token: 0x06000014 RID: 20 RVA: 0x000026EC File Offset: 0x000008EC
		private static void SetString(SandfileParser sp, string key, string def)
		{
			string temp = sp.GetStringValue(key);
			if (temp == null)
			{
				Debug.Log("NULL KEY -> " + key);
				temp = def;
			}
			Strings.Add(new string[]
			{
				"STRINGS.CREATURES.SPECIES.GEYSER." + key,
				temp
			});
		}

		// Token: 0x06000015 RID: 21 RVA: 0x00002734 File Offset: 0x00000934
		public static void Strp(string idup, string name, string desc)
		{
			Strings.Add(new string[]
			{
				"STRINGS.CREATURES.SPECIES.GEYSER." + idup + ".NAME",
				name
			});
			Strings.Add(new string[]
			{
				"STRINGS.CREATURES.SPECIES.GEYSER." + idup + ".DESC",
				desc
			});
		}
	}
}
