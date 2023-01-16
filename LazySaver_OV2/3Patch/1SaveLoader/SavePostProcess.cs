// MIT License
//
// Copyright (c) 2023. Super Comic (ekfvoddl3535@naver.com)
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in all
// copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
// SOFTWARE.

using System;

namespace LazySaver
{
    using static GlobalHeader;
    internal static class SavePostProcess
    {
        public static void Complete()
        {
            var msg = res_errMsg;
            if (msg != null)
            {
                if (res_exceptionUnhandled)
                    throw new InvalidOperationException(msg);

                var gmScrinst = GameScreenManager.Instance;

                var dialogScrnObj = ScreenPrefabs.Instance.ConfirmDialogScreen.gameObject;

                var screen = (ConfirmDialogScreen)gmScrinst.StartScreen(dialogScrnObj, gmScrinst.ssOverlayCanvas.gameObject);
                screen.PopupConfirmDialog(msg, null, null);
            }
            else
            {
                DebugUtil.LogArgs("Saved to", "[" + arg_filename + "]");

                SpeedControlScreen.Instance.Unpause(false);
            }

            GC.Collect(0, GCCollectionMode.Default, false, false);
        }
    }
}
