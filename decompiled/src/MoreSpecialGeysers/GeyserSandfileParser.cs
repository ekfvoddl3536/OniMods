using System;
using System.Collections.Generic;
using System.IO;
using SuperComicLib.Sandfile;

namespace MoreSpecialGeysers
{
	// Token: 0x02000004 RID: 4
	internal sealed class GeyserSandfileParser : SandfileParser
	{
		// Token: 0x06000006 RID: 6 RVA: 0x00002338 File Offset: 0x00000538
		public GeyserSandfileParser(string _path) : base(_path)
		{
			this.floats = new Dictionary<string, float>();
			this.ID = Path.GetFileNameWithoutExtension(_path);
		}

		// Token: 0x17000001 RID: 1
		// (get) Token: 0x06000007 RID: 7 RVA: 0x00002358 File Offset: 0x00000558
		// (set) Token: 0x06000008 RID: 8 RVA: 0x00002360 File Offset: 0x00000560
		public string ID { get; private set; }

		// Token: 0x17000002 RID: 2
		// (get) Token: 0x06000009 RID: 9 RVA: 0x00002369 File Offset: 0x00000569
		// (set) Token: 0x0600000A RID: 10 RVA: 0x00002371 File Offset: 0x00000571
		public SimHashes Element { get; private set; }

		// Token: 0x0600000B RID: 11 RVA: 0x0000237C File Offset: 0x0000057C
		public float GetFValue(string id)
		{
			float res;
			if (!this.floats.TryGetValue(id, out res))
			{
				return 0f;
			}
			return res;
		}

		// Token: 0x0600000C RID: 12 RVA: 0x000023A0 File Offset: 0x000005A0
		protected override void UndetectString(string id, string val)
		{
			id = id.ToUpper();
			if (val.StartsWith("SimHashes."))
			{
				this.Element = (SimHashes)Enum.Parse(typeof(SimHashes), val.Remove(0, 10));
				return;
			}
			float _res;
			if (float.TryParse(val, out _res))
			{
				this.floats.Add(id, _res);
				return;
			}
			Debug.LogWarning("Unknown data-type detected!");
		}

		// Token: 0x0600000D RID: 13 RVA: 0x00002408 File Offset: 0x00000608
		protected override void DetectString(string id, string val)
		{
			id = id.ToUpper();
			if (id == "ID")
			{
				this.ID = val;
				return;
			}
			if (id == "KANIM")
			{
				if (!val.StartsWith("geyser_"))
				{
					val = "geyser_" + val;
				}
				if (!val.EndsWith("_kanim"))
				{
					val += "_kanim";
				}
			}
			if (id == "DESCRIPTION")
			{
				val = val.Replace("\\n", "\n");
			}
			this.strDatas.Add(id, val);
		}

		// Token: 0x0600000E RID: 14 RVA: 0x0000249F File Offset: 0x0000069F
		public static GeyserSandfileParser LoadStatic(string _path)
		{
			GeyserSandfileParser geyserSandfileParser = new GeyserSandfileParser(_path);
			geyserSandfileParser.Start();
			return geyserSandfileParser;
		}

		// Token: 0x04000015 RID: 21
		private Dictionary<string, float> floats;
	}
}
