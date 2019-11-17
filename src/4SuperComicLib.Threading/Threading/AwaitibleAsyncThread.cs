using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;

namespace SuperComicLib.Threading
{
    public class AwaitibleAsyncThread
    {
        protected bool skipframe;
        protected bool working;
        protected ManualResetEvent revent = new ManualResetEvent(false);
        protected Action action;

        public virtual IEnumerator StartThread(AwaitibleAsyncMethod asyncMethod)
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
                    revent.Reset();
                }
            revent.Close();
            yield break;
        }

        protected virtual void SubThreadWork(AwaitibleAsyncMethod method)
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
