#pragma warning disable IDE0041
using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;
using UnityEngine;
using Expr = System.Linq.Expressions.Expression;

namespace LazySaver
{
    using static SharedData;
    [HarmonyPatch(typeof(SaveLoader), "PrepSaveFile")]
    public static class PrepSaveFile_PATCH
    {
        private static Func<object> m_newSaveFileRoot;

        public static void Prepare()
        {
            var t0 = typeof(SaveLoader).Assembly.GetType("Klei.SaveFileRoot");

            m_newSaveFileRoot = Expr.Lambda<Func<object>>(Expr.New(t0)).Compile();
        }

        public static bool Prefix(ref object __result)
        {
            var tmp = m_newSaveFileRoot.Invoke();
            var root = Unsafe.As<P1SaveFileRoot>(tmp);
            
            __result = tmp;

            root.WidthInCells = Grid.WidthInCells;
            root.HeightInCells = Grid.HeightInCells;
            
            root.streamed["GridVisible"] = Grid.Visible;
            root.streamed["GridSpawnable"] = Grid.Spawnable;
            root.streamed["GridDamage"] = FloatToBytes(Grid.Damage);

            var list = new List<KMod.Label>();
            root.active_mods = list;

            var modManager = Global.Instance.modManager;
            modManager.SendMetricsEvent();

            var modList = modManager.mods;
            for (int i = 0, cnt = modList.Count; i < cnt; i++)
            {
                var mod = modList[i];
                if (mod.IsEnabledForActiveDlc())
                    list.Add(mod.label);
            }

            var writer = m_writer;
            var stream = writer.m_stream;

            var beforePos = stream.Position;
            Camera.main.transform.parent.GetComponent<CameraController>().Save(writer);

            root.streamed["Camera"] = stream.ToArray(beforePos);
            stream.Position = beforePos;

            return false;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static byte[] FloatToBytes(float[] vs)
        {
            var dst = new byte[vs.Length << 2];
            Buffer.BlockCopy(vs, 0, dst, 0, dst.Length);
            return dst;
        }
    }
}
