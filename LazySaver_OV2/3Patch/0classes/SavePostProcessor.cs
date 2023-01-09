using System;
using System.Collections;

namespace LazySaver
{
    using static SharedData;
    internal readonly struct SLSavePostProcessor : IEnumerator
    {
        public static readonly IEnumerator Default = new SLSavePostProcessor();

        public object Current => null;

        public void Reset() { }

        public bool MoveNext()
        {
            if (!m_previousTask.IsCompleted)
                return true;

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
                DebugUtil.LogArgs("Saved to", "[" + arg_filename + "]");

            SpeedControlScreen.Instance.Unpause();
            GC.Collect(0, GCCollectionMode.Optimized, false, false);

            return false;
        }
    }
}