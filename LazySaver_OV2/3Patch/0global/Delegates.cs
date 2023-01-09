using System.IO;
using System.Security;
using System.Collections.Generic;

namespace LazySaver
{
    //
    // SaveLoader
    //
    internal delegate void MethodReportSaveMetrics(SaveLoader @this, bool isAutoSave);
    internal delegate void MethodSave(SaveLoader @this, BinaryWriter writer);

    // 
    // Sim (DLLImport + Unmanaged source)
    //
    [SuppressUnmanagedCodeSecurity]
    internal unsafe delegate byte* ExternSIM_BeginSave(int* size, int x, int y);
    [SuppressUnmanagedCodeSecurity]
    internal unsafe delegate void ExternSIM_EndSave();

    //
    // SaveManager
    //
    internal delegate void MethodWrite(SaveManager @this, Tag key, List<SaveLoadRoot> value, BinaryWriter writer);
}
