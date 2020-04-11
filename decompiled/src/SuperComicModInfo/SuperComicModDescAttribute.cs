using System;

namespace SuperComicModInfo
{
	// Token: 0x02000004 RID: 4
	[AttributeUsage(AttributeTargets.Assembly, AllowMultiple = false, Inherited = false)]
	public sealed class SuperComicModDescAttribute : Attribute
	{
		// Token: 0x06000004 RID: 4 RVA: 0x000020DD File Offset: 0x000002DD
		public SuperComicModDescAttribute(string name, string desc, ModAttributes att)
		{
			this.ModName = name;
			this.ModDesc = desc;
			this.ModFlags = att;
		}

		// Token: 0x06000005 RID: 5 RVA: 0x000020FC File Offset: 0x000002FC
		public SuperComicModDescAttribute(string name, ModAttributes att) : this(name, string.Empty, att)
		{
		}

		// Token: 0x06000006 RID: 6 RVA: 0x0000210D File Offset: 0x0000030D
		public SuperComicModDescAttribute(string name, string desc) : this(name, desc, ModAttributes.None)
		{
		}

		// Token: 0x06000007 RID: 7 RVA: 0x0000211A File Offset: 0x0000031A
		public SuperComicModDescAttribute(string name) : this(name, string.Empty, ModAttributes.None)
		{
		}

		// Token: 0x06000008 RID: 8 RVA: 0x0000212B File Offset: 0x0000032B
		public SuperComicModDescAttribute() : this(null, null, ModAttributes.None)
		{
		}

		// Token: 0x17000001 RID: 1
		// (get) Token: 0x06000009 RID: 9 RVA: 0x00002138 File Offset: 0x00000338
		public bool IsInvalid
		{
			get
			{
				return string.IsNullOrEmpty(this.ModName) && string.IsNullOrEmpty(this.ModDesc) && this.ModFlags == ModAttributes.None;
			}
		}

		// Token: 0x17000002 RID: 2
		// (get) Token: 0x0600000A RID: 10 RVA: 0x00002160 File Offset: 0x00000360
		public bool IsHelpful
		{
			get
			{
				return this.ModFlags.IsEquals(ModAttributes.Helpful);
			}
		}

		// Token: 0x17000003 RID: 3
		// (get) Token: 0x0600000B RID: 11 RVA: 0x0000216E File Offset: 0x0000036E
		public bool IsGenerator
		{
			get
			{
				return this.ModFlags.IsEquals(ModAttributes.Generator);
			}
		}

		// Token: 0x04000019 RID: 25
		public readonly string ModName;

		// Token: 0x0400001A RID: 26
		public readonly string ModDesc;

		// Token: 0x0400001B RID: 27
		public readonly ModAttributes ModFlags;
	}
}
