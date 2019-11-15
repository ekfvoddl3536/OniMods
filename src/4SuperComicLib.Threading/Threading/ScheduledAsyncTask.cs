using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;

namespace SuperComicLib.Threading
{
    public sealed class ScheduledAsyncTask : IDisposable
    {
        private bool workThreadBlocking;
        private bool working;
        private ManualResetEvent revent = new ManualResetEvent(false);
        private Action action;
        private Queue<AwaitibleAsyncMethod> targets = new Queue<AwaitibleAsyncMethod>();

        public IEnumerator Start()
        {
            workThreadBlocking = true;
            working = true;
            new Thread(SubThreadWork);
            while (working)
                if (workThreadBlocking) yield return null;
                else
                {
                    workThreadBlocking = true;
                    action.Invoke();
                    revent.Set();
                    revent.Reset(); // 생각할 수록 화나네 ㅋㅋ 아놔 ㅋㅋ
                }
            Dispose();
            yield break;
        }

        public void Scheduled(AwaitibleAsyncMethod method)
        {
            if (method == null)
                throw new ArgumentNullException(nameof(method));
            targets.Enqueue(method);
        }

        private void SubThreadWork()
        {
            while (targets.Count > 0)
            {
                IEnumerator<Action> temp = targets.Dequeue().Invoke();
                while (temp.MoveNext())
                {
                    workThreadBlocking = false;
                    if (temp.Current != null)
                    {
                        action = temp.Current;
                        revent.WaitOne();
                    }
                }
            }
            working = false;
        }

        public void Dispose()
        {
            revent.Close();
            targets.Clear();
            targets = null;
        }
    }
}
