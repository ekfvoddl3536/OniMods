using System;
using System.Collections.Generic;
using UnityEngine;

namespace SuperComicLib.HUSystem
{
	// Token: 0x02000023 RID: 35
	public interface IHUOverlayUpdate : IHaveHUCell
	{
		// Token: 0x0600008C RID: 140
		bool IfAddLabel(ICollection<SaveLoadRoot> roots, Vector2I min, Vector2I max);

		// Token: 0x0600008D RID: 141
		void UpdateVisualizer(LocText label, LocText units, Color32 generator_color, Color32 consumer_color, byte labelHandle);

		// Token: 0x1700000E RID: 14
		// (get) Token: 0x0600008E RID: 142
		string ObjectName { get; }

		// Token: 0x1700000F RID: 15
		// (get) Token: 0x0600008F RID: 143
		string ToolTipText { get; }
	}
}
