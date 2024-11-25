using System;

namespace SuperPipe
{
    [Flags]
    public enum SuperPipeType : byte
    {
        None, InputOnly, OutputOnly, InOutput
    }
}
