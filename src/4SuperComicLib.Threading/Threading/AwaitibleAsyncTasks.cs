using System;
using System.Collections;
using System.Threading;

namespace SuperComicLib.Threading
{
    public sealed class AwaitibleAsyncTasks
    {
        private bool skipframe;
        private bool working;
        private QueuedAction queuedAct;

        // 대기하거나 안하거나 할 수 있도록
        // 또는 ContinueWith을 사용하기 위해
        public struct Awaiter
        {
            private ManualResetEvent mpe;
            private bool autoclosing;

            internal Awaiter(ManualResetEvent mpe)
            {
                this.mpe = mpe;
                autoclosing = true;
            }

            public void Wait() => mpe.WaitOne();
            
            // 굉장히 위험하기 때문에, 경우에 따라서는 이 코드를 사용하지 못하도록 하세요
            public void Close() => mpe.Close();

            public Awaiter ContinueWith(Action<Awaiter> action)
            {
                autoclosing = false;
                mpe.WaitOne();
                mpe.Reset();
                action.Invoke(this);
                return new Awaiter(mpe);
            }

            internal void Complete()
            {
                mpe.Set();
                if (autoclosing)
                    mpe.Close();
            }
        }

        // Awaiter 와 Action을 한쌍으로 관리하기 위한 구조체
        private struct QueuedAction
        {
            public Awaiter GetAwaiter;
            public Action GetAction;
        }

        // AwaitibleAsyncTasks 개체가 쉽게 노출되지 않도록 합니다.
        // StartTask가 중복으로 호출되는것을 막아보기 위한 시도...
        public struct Instance
        {
            internal AwaitibleAsyncTasks tasks;

            public Instance(AwaitibleAsyncTasks instance) => tasks = instance;

            public Awaiter Invoke(Action action)
            {
                Awaiter awaiter = new Awaiter(new ManualResetEvent(true));
                tasks.queuedAct = new QueuedAction { GetAction = action, GetAwaiter = awaiter };
                tasks.skipframe = false;
                return awaiter;
            }
        }

        public IEnumerator StartTask(AwaitibleAsyncTaskMethod asyncMethod)
        {
            skipframe = true;
            working = true;
            new Thread(() => SubThreadWork(asyncMethod)).Start();
            while (working)
                if (skipframe) yield return null;
                else
                {
                    skipframe = true;
                    queuedAct.GetAction.Invoke();
                    queuedAct.GetAwaiter.Complete();
                }
            yield break;
        }

        private void SubThreadWork(AwaitibleAsyncTaskMethod method)
        {
            method.Invoke(new Instance(this));
            working = false;
        }
    }
}
