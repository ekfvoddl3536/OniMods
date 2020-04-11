using System;
using STRINGS;

namespace SuperComicLib.HUSystem
{
	// Token: 0x02000020 RID: 32
	public static class UIEX
	{
		// Token: 0x04000058 RID: 88
		public static readonly LocString HUPERSEC_PRODUCE = string.Concat(new string[]
		{
			"연결된 ",
			UI.PRE_KEYWORD,
			"히트파이프",
			UI.PST_KEYWORD,
			"에 ",
			UI.PRE_RATE_POSITIVE,
			"{0}/s",
			UI.PST_RATE,
			" 속도로 <b>HU</b>를 공급합니다."
		});

		// Token: 0x04000059 RID: 89
		public static readonly LocString HUPERSEC_CONSUMED = UI.PRE_KEYWORD + "{0} {1}/s" + UI.PST_KEYWORD + " 속도로 <b>HU</b>를 소비";
	}
}
