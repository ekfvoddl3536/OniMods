/*
Copyright (c) 2019. Super Comic

This software was developed by the Computer Systems Engineering group
at Lawrence Berkeley Laboratory under DARPA contract BG 91-66 and
contributed to Berkeley.

All advertising materials mentioning features or use of this software
must display the following acknowledgement: This product includes
software developed by the University of California, Lawrence Berkeley
Laboratory.

Redistribution and use in source and binary forms, with or without
modification, are permitted provided that the following conditions are
met:

    1. Redistributions of source code must retain the above copyright
    notice, this list of conditions and the following disclaimer.

    2. Redistributions in binary form must reproduce the above copyright
    notice, this list of conditions and the following disclaimer in
    the documentation and/or other materials provided with the
    distribution.

    3. All advertising materials mentioning features or use of this
    software must display the following acknowledgement: This product
    includes software developed by the University of California,
    Berkeley and its contributors.

    4. Neither the name of the University nor the names of its
    contributors may be used to endorse or promote products derived
    from this software without specific prior written permission.

THIS SOFTWARE IS PROVIDED BY THE REGENTS AND CONTRIBUTORS ``AS IS''
AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO,
THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR
PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL THE REGENTS OR CONTRIBUTORS
BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR
CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF
SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR PROFITS; OR
BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY,
WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE
OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN
IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
*/

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
