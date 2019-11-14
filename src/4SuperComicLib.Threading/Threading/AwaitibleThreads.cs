using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SuperComicLib.Threading
{
    public static class AwaitibleThreads
    {
        public static ScheduledAsyncTask Scheduled(AwaitibleAsyncMethod method)
        {
            ScheduledAsyncTask task = new ScheduledAsyncTask();
            task.Scheduled(method);
            return task;
        }

        public static ScheduledAsyncTask Scheduled(IEnumerable<AwaitibleAsyncMethod> methods)
        {
            ScheduledAsyncTask task = new ScheduledAsyncTask();
            if (methods == null)
                throw new ArgumentNullException(nameof(methods));
            foreach (AwaitibleAsyncMethod i in methods)
                task.Scheduled(i);
            return task;
        }

        public static IEnumerator Create(AwaitibleAsyncMethod method) => new AwaitibleAsyncThread().StartThread(method);

        public static GameObject Create(AwaitibleAsyncMethod method, string gameObjectName)
        {
            GameObject go = new GameObject(gameObjectName);
            go.AddComponent<MonoBehaviour>().StartCoroutine(Create(method));
            go.SetActive(true);
            return go;
        }
    }
}
