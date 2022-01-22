﻿using System;
using System.Threading;
using System.Threading.Tasks;

namespace DesignPatternsLibrary.BackgroundWorkerQueue
{
    public class BackgroundWorkerQueue
    {
        private Task previousTask = Task.FromResult(true);
        private object key = new object();

        public Task QueueTask(Action action)
        {
            lock (key)
            {
                previousTask = previousTask.ContinueWith(
                  t => action(),
                  CancellationToken.None,
                  TaskContinuationOptions.None,
                  TaskScheduler.Default);
                return previousTask;
            }
        }

        public Task<T> QueueTask<T>(Func<T> work)
        {
            lock (key)
            {
                var task = previousTask.ContinueWith(
                  t => work(),
                  CancellationToken.None,
                  TaskContinuationOptions.None,
                  TaskScheduler.Default);
                previousTask = task;
                return task;
            }
        }
    }
}