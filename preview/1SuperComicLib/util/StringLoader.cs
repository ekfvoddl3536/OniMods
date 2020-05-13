using System;
using System.Reflection;

namespace SuperComicLib.Oni
{
	public static class StringLoader
	{
		private const BindingFlags bf = BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Static;
		public static readonly string defaultBuilding = "STRINGS.BUILDINGS.PREFABS.";
		public static readonly string defaultGeyser = "STRINGS.CREATURES.SPECIES.GEYSER.";

		/// <summary>
		/// 최상위 class 하위에 속한 모든 static class의 building 정보를 로드합니다
		/// 모든 하위 클래스에는 string 또는 LocString 형식의 NAME, DESC, EFFECT 또는 EFFC 그리고 string 형식의 ID_UPPER가 필요합니다
		/// </summary>
		public static void BuildAll(Type value)
		{
			Type[] vs = value.GetNestedTypes(bf);
			for (int x = vs.Length; x > 0;)
			{
				x--;
				
				Type t = vs[x];
				FieldInfo effect = t.GetField("EFFECT", bf) ?? t.GetField("EFFC", bf);
				if (effect != null)
				{
					object val = effect.GetValue(null);
					Set(defaultBuilding, t.FindIDUpper(), t.FindField("NAME"), t.FindField("DESC"), val as string ?? ((LocString)val).text);
				}
				else
					Set(defaultBuilding, t.FindIDUpper(), t.FindField("NAME"), t.FindField("DESC"));
			}
		}

		public static void Build(Type type) => SetSmart(type, defaultBuilding, true);

		public static void Geyser(Type type) => SetSmart(type, defaultGeyser, false);

		/// <summary>
		/// (string) ID_UPPER, (LocString | string) NAME, (LocString | string) DESC, (LocString | string) EFFECT static 필드를 찾습니다.
		/// root 는 STRINGS.BUILDINGS.PREFABS. 과 같은 것입니다.
		/// </summary>
		public static void SetSmart(Type type, string root, bool includeEffect)
		{
			if (type == null)
				return;
			
			if (includeEffect)
				Set(root, type.FindIDUpper(), type.FindField("NAME"), type.FindField("DESC"), type.FindField("EFFECT"));
			else
				Set(root, type.FindIDUpper(), type.FindField("NAME"), type.FindField("DESC"));
		}

		public static void Set(string root, string id_upper, string name, string desc, string effect)
		{
			Strings.Add($"{root}{id_upper}.NAME", name);
			Strings.Add($"{root}{id_upper}.DESC", desc);
			Strings.Add($"{root}{id_upper}.EFFECT", effect);
		}

		public static void Set(string root, string id_upper, string name, string desc)
		{
			Strings.Add($"{root}{id_upper}.NAME", name);
			Strings.Add($"{root}{id_upper}.DESC", desc);
		}
	}
}
