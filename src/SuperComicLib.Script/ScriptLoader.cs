using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace SuperComicLib.Script
{
    public static class ScriptLoader
    {
        internal static Dictionary<string, IDynamicLinker> linkers;
        private static readonly Type ifces = typeof(IDynamicLinker);
        private static readonly Type atsearch = typeof(PublicSearchableAttribute);
        private static readonly Type atci = typeof(CreateInstanceAttribute);
        private const string SEARCH_NAME = "CreateInstance";
        private const BindingFlags flags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static;

        public static void Load(string diretory_path)
        {
            if (linkers != null)
                return;
            linkers = new Dictionary<string, IDynamicLinker>();
            Internal_Load_Dlls(Directory.GetFiles(diretory_path, "*.dll", SearchOption.AllDirectories));
        }

        private static void Internal_Load_Dlls(string[] files)
        {
            Type[] tps;
            MethodInfo minfo;
            foreach (string x in files)
            {
                if (x.Contains("SuperComicLib"))
                    continue;
                try
                {
                    tps = Assembly.LoadFile(x).GetTypes();
                    foreach (Type i in tps)
                        if (i.GetCustomAttributes(atsearch, false).Length > 0)
                        {
                            minfo = i.GetMethod(SEARCH_NAME, flags) ?? FindFirst(i.GetMethods(flags), c => c.GetCustomAttributes(atci, false).Length > 0);
                            if (minfo != null && minfo.ReturnType == ifces)
                                linkers.Add(Path.GetFileNameWithoutExtension(x), (IDynamicLinker)minfo.Invoke(null, null));
                        }
                }
                catch
                {
                    continue;
                }
            }
        }

        private static T FindFirst<T>(IEnumerable<T> vs, Func<T, bool> func)
        {
            foreach (T a in vs)
                if (func.Invoke(a))
                    return a;
            return default;
        }

        public static bool GetValue(string name, out IDynamicLinker value) => linkers.TryGetValue(name, out value);

        public static void Add(string name, IDynamicLinker linker) => linkers.Add(name, linker);
    }
}
