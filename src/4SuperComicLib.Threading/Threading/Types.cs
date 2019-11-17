using System;
using System.Collections.Generic;

namespace SuperComicLib.Threading
{
    public delegate void AwaitibleAsyncTaskMethod(AwaitibleAsyncTasks.Instance instance);

    public delegate IEnumerator<Action> AwaitibleAsyncMethod();
}
