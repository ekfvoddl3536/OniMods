using System;
using System.Collections.Generic;
using UnityEngine;

namespace SupportPackages
{
	// Token: 0x02000007 RID: 7
	public sealed class ShareStorage : Storage
	{
		// Token: 0x0600000F RID: 15 RVA: 0x000022B4 File Offset: 0x000004B4
		public void Deserialize(IReader reader)
		{
			foreach (GameObject gameObject in this.items)
			{
				TracesExtesions.DeleteObject(gameObject);
			}
			int cap = reader.ReadInt32();
			this.items.Clear();
			this.items.Alloc(cap);
			for (int x = 0; x < cap; x++)
			{
				Tag mtag = TagManager.Create(reader.ReadKleiString());
				SaveLoadRoot svlr = SaveLoadRoot.Load(mtag, reader);
				if (svlr != null)
				{
					KBatchedAnimController compo = svlr.GetComponent<KBatchedAnimController>();
					if (compo != null)
					{
						compo.enabled = false;
					}
					svlr.SetRegistered(false);
					GameObject gm = base.Store(svlr.gameObject, true, true, false, true);
					if (gm != null)
					{
						gm.GetComponent<Pickupable>().OnStore(this);
						if (this.dropOnLoad)
						{
							base.Drop(svlr.gameObject, true);
						}
					}
				}
				else
				{
					Debug.LogWarning("Tried to deserialize " + mtag.ToString() + " into storage but failed", base.gameObject);
				}
			}
		}

		// Token: 0x04000001 RID: 1
		public List<GameObject> items;
	}
}
