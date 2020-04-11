using System;
using System.Collections.Generic;
using System.IO;

namespace MoreSpecialGeysers
{
	// Token: 0x02000002 RID: 2
	internal static class CustomGeysers
	{
		// Token: 0x06000001 RID: 1 RVA: 0x00002050 File Offset: 0x00000250
		public static void Load(string directory)
		{
			if (Directory.Exists(directory))
			{
				CustomGeysers.parsers = new List<GeyserSandfileParser>();
				CustomGeysers.Load(Directory.GetFiles(directory, "*.sndf", SearchOption.AllDirectories), CustomGeysers.parsers);
				CustomGeysers.Load(Directory.GetFiles(directory, "*.txt", SearchOption.AllDirectories), CustomGeysers.parsers);
			}
		}

		// Token: 0x06000002 RID: 2 RVA: 0x00002090 File Offset: 0x00000290
		private static void Load(string[] files, List<GeyserSandfileParser> parsers)
		{
			int x = 0;
			int max = files.Length;
			while (x < max)
			{
				parsers.Add(GeyserSandfileParser.LoadStatic(files[x]));
				if (parsers[x].Element == null || parsers[x].Element == -1456075980 || parsers[x].GetStringValue("KANIM") == null || parsers[x].GetStringValue("NAME") == null || parsers[x].GetStringValue("DESCRIPTION") == null)
				{
					Debug.LogWarning("Error " + files[x]);
					Debug.LogWarning("Disable all custom geysers");
					CustomGeysers.Dispose();
					return;
				}
				EntityConfMgrPath.Strp(parsers[x].ID.ToUpper(), parsers[x].GetStringValue("NAME"), parsers[x].GetStringValue("DESCRIPTION"));
				x++;
			}
		}

		// Token: 0x06000003 RID: 3 RVA: 0x00002178 File Offset: 0x00000378
		public static void GetList(ref List<GeyserGenericConfig.GeyserPrefabParams> list, bool autoDisposal = true)
		{
			if (CustomGeysers.parsers == null)
			{
				return;
			}
			for (int i = 0; i < CustomGeysers.parsers.Count; i++)
			{
				list.Add(GeyserGenPath.MakeGeyserPrefab(CustomGeysers.parsers[i].GetStringValue("KANIM"), (int)CustomGeysers.parsers[i].GetFValue("WIDTH"), (int)CustomGeysers.parsers[i].GetFValue("HEIGHT"), GeyserGenPath.MakeGeyserCnf(CustomGeysers.parsers[i].ID, CustomGeysers.parsers[i].Element, CustomGeysers.parsers[i].GetFValue("TEMPERATURE"), CustomGeysers.parsers[i].GetFValue("MINRATEKG"), CustomGeysers.parsers[i].GetFValue("MAXRATEKG"), CustomGeysers.parsers[i].GetFValue("MAXPRESSURE"), CustomGeysers.parsers[i].GetFValue("MINACTIVEPERIOD"), CustomGeysers.parsers[i].GetFValue("MAXACTIVEPERIOD"), CustomGeysers.parsers[i].GetFValue("MINACTIVEPERCENT"), CustomGeysers.parsers[i].GetFValue("MAXACTIVEPERCENT"), CustomGeysers.parsers[i].GetFValue("MINYEARLENGTH"), CustomGeysers.parsers[i].GetFValue("MAXYEARLENGTH"), CustomGeysers.parsers[i].GetFValue("MINYEARPERCENT"), CustomGeysers.parsers[i].GetFValue("MAXYEARPERCENT"))));
			}
			if (autoDisposal)
			{
				CustomGeysers.Dispose();
			}
		}

		// Token: 0x06000004 RID: 4 RVA: 0x0000231E File Offset: 0x0000051E
		private static void Dispose()
		{
			CustomGeysers.parsers.Clear();
			CustomGeysers.parsers = null;
		}

		// Token: 0x04000001 RID: 1
		private static List<GeyserSandfileParser> parsers;
	}
}
