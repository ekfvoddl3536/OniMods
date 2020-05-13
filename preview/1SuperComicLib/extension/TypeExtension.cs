using System;
using System.Reflection;

namespace SuperComicLib.Oni
{
    internal static class TypeExtension
	{
		private const BindingFlags bf = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static;

		internal static string FindField(this Type t, string fname)
		{
			var v = t.GetField(fname, bf).GetValue(null);
			return
				v == null
				? "MISSING FIELD: " + fname
				: v as string ?? ((LocString)v).text;
		}

		internal static string FindID(this Type t) =>
			t.GetField("ID", bf).GetValue(null) as string
			??
			t.Name;

		internal static string FindIDUpper(this Type t) =>
			t.GetField("ID_UPPER", bf).GetValue(null) as string
			??
			t.Name.ToUpper();
	}
}
