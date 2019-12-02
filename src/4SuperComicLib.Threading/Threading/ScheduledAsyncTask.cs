using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;

namespace SuperComicLib.Threading
{
    public class ScheduledAsyncTask : IDisposable
    {
        protected bool workThreadBlocking;
        protected bool working;
        protected ManualResetEvent revent = new ManualResetEvent(false);
        protected Action action;
        protected Queue<AwaitibleAsyncMethod> targets = new Queue<AwaitibleAsyncMethod>();

        public virtual IEnumerator Start()
        {
            workThreadBlocking = true;
            working = true;
            new Thread(SubThreadWork).Start(); // 이거 왜 이슈 안들어옴? 아무도 이거 안쓰나 ㅋㅋ
            while (working)
                if (workThreadBlocking) yield return null;
                else
                {
                    workThreadBlocking = true;
                    action.Invoke();
                    revent.Set();
                    revent.Reset();
                }
            Dispose();
            yield break;
        }

        public virtual void Scheduled(AwaitibleAsyncMethod method)
        {
            if (working)
                throw new InvalidOperationException("이미 메인 스레드가 작업을 실행하고 있습니다.");
            if (method == null)
                throw new ArgumentNullException(nameof(method));
            targets.Enqueue(method);
        }

        protected virtual void SubThreadWork()
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

        #region IDisposable Support
        private bool disposedValue = false; // 중복 호출을 검색하려면

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: 관리되는 상태(관리되는 개체)를 삭제합니다.
                    revent.Close();
                    targets.Clear();
                    revent = null;
                    targets = null;
                }

                // TODO: 관리되지 않는 리소스(관리되지 않는 개체)를 해제하고 아래의 종료자를 재정의합니다.
                // TODO: 큰 필드를 null로 설정합니다.
                disposedValue = true;
            }
        }

        // TODO: 위의 Dispose(bool disposing)에 관리되지 않는 리소스를 해제하는 코드가 포함되어 있는 경우에만 종료자를 재정의합니다.
        // ~ScheduledAsyncTask()
        // {
        //   // 이 코드를 변경하지 마세요. 위의 Dispose(bool disposing)에 정리 코드를 입력하세요.
        //   Dispose(false);
        // }

        // 삭제 가능한 패턴을 올바르게 구현하기 위해 추가된 코드입니다.
        // 이 코드를 변경하지 마세요. 위의 Dispose(bool disposing)에 정리 코드를 입력하세요.
        //
        // TODO: 위의 종료자가 재정의된 경우 다음 코드를 추가합니다.
        // GC.SuppressFinalize(this);
        public void Dispose() => Dispose(true);
        #endregion
    }
}
