using System;
using System.Reflection;
using HarmonyLib;

namespace LazySaver
{
    using static SharedData;
    [HarmonyPatch(typeof(Sim), nameof(Sim.Save))]
    public static unsafe class SimSave_PATCH
    {
        private static ExternSIM_BeginSave m_simBeginSave;
        private static ExternSIM_EndSave m_simEndSave;

        public static void Prepare()
        {
            const BindingFlags STATIC = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static;

            // Sim
            var t0 = typeof(Sim);

            var m1 = t0.GetMethod("SIM_BeginSave", STATIC);
            m_simBeginSave = (ExternSIM_BeginSave)Delegate.CreateDelegate(typeof(ExternSIM_BeginSave), m1);

            m1 = t0.GetMethod("SIM_EndSave", STATIC);
            m_simEndSave = (ExternSIM_EndSave)Delegate.CreateDelegate(typeof(ExternSIM_EndSave), m1);
        }

        public static bool Prefix(int x, int y)
        {
            int length;
            byte* source = m_simBeginSave.Invoke(&length, x, y);

            m_writer.Write(length);
            m_writer.m_stream.Write(source, length);

            m_simEndSave.Invoke();

            return false;
        }
    }
}