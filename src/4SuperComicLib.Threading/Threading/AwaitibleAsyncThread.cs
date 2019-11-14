using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;

namespace SuperComicLib.Threading
{
    public delegate IEnumerator<Action> AwaitibleAsyncMethod();

    public sealed class AwaitibleAsyncThread
    {
        private bool skipframe;
        private bool working;
        private ManualResetEvent revent = new ManualResetEvent(false);
        private Action action;

        public IEnumerator StartThread(AwaitibleAsyncMethod asyncMethod)
        {
            skipframe = true;
            working = true;
            new Thread(() => SubThreadWork(asyncMethod)).Start();
            while (working)
                if (skipframe) yield return null;
                else
                {
                    skipframe = true;
                    action.Invoke();
                    revent.Set();
                }
            yield break;
        }

        private void SubThreadWork(AwaitibleAsyncMethod method)
        {
            IEnumerator<Action> enumerator = method.Invoke();
            while (enumerator.MoveNext())
                if (enumerator.Current != null)
                {
                    action = enumerator.Current;
                    skipframe = false;
                    revent.WaitOne();
                }
            working = false;
        }
    }
}
