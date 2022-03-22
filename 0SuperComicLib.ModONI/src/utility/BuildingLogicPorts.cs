#region LICENSE
/*
MIT License

Copyright (c) 2022. Super Comic (ekfvoddl3535@naver.com)

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.
*/
#endregion
using UnityEngine;
using LPORT = LogicPorts.Port;

namespace SuperComicLib.ModONI
{
    using static LogicOperationalController;
    using static STRINGS.UI.LOGIC_PORTS;
    public static class BuildingLogicPorts
    {
        public static void RegisterSingleInput(GameObject go, CellOffset offset = default)
        {
            var ports = go.AddOrGet<LogicPorts>();
            ports.inputPortInfo = new LPORT[] 
            {
                LPORT.InputPort(PORT_ID, offset, CONTROL_OPERATIONAL, CONTROL_OPERATIONAL_ACTIVE, CONTROL_OPERATIONAL_INACTIVE)
            };
            ports.outputPorts = null;
        }
    }
}
