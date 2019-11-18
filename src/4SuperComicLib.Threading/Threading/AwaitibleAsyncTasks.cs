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
        // ContinueWith은 반환이 있는경우에만 쓸모가 있을것 같아서 삭제하였습니다
        public struct Awaiter
        {
            private ManualResetEvent mpe;

            internal Awaiter(ManualResetEvent mpe) => this.mpe = mpe;
            
            // Close 후 WaitOne 하게되면 오류가 발생하기 때문에 급하게 코드 수정
            public void Wait() 
            {
                mpe.WaitOne();
                mpe.Close();
            }
            
            internal void Complete() => mpe.Set();
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
                Awaiter awaiter = new Awaiter(new ManualResetEvent(false)); // 초기값은 항상 false
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
