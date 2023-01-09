using LazySaver.MemoryIO;
using System.Threading.Tasks;

namespace LazySaver
{
    internal static class SharedData
    {
        //
        //  ==  saveloader save PATCH  ==
        //
        internal static UnmanagedWriter m_writer;
        internal static Task m_previousTask;

        // saveloader.save task
        internal static bool arg_isAutoSave;
        internal static string arg_filename;
        internal static string res_errMsg;
        internal static bool res_exceptionUnhandled;
    }
}