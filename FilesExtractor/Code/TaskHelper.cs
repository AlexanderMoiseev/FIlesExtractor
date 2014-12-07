using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FilesExtractor.Code.Extensions;

namespace FilesExtractor.Code {
    static class TaskHelper {
        /// <summary>
        /// Processes all tasks from pendingTaskQueue and waits for completion
        /// </summary>
        /// <param name="taskPool"></param>
        /// <param name="pendingTaskQueue"></param>
        public static void ProcessTaskPoolToCompletion(Task[] taskPool, Queue<Task> pendingTaskQueue) {
            while(pendingTaskQueue.Count > 0) {
                ProcessTaskPool(taskPool, pendingTaskQueue);
                Task.WaitAny(taskPool);
            }
            Task.WaitAll(taskPool);
        }

        /// <summary>
        /// Fills taskpool with tasks from pendingTaskQueue
        /// </summary>
        /// <param name="taskPool"></param>
        /// <param name="pendingTaskQueue"></param>
        private static void ProcessTaskPool(Task[] taskPool, Queue<Task> pendingTaskQueue) {
            for(int i = 0; i < taskPool.Length; ++i) {
                taskPool[i].With(t => t.Exception).Do(ex => Console.WriteLine(ex.Message)); // Observe exception

                if(pendingTaskQueue.Any() &&
                    ( taskPool[i] == null || taskPool[i].IsCanceled || taskPool[i].IsFaulted || taskPool[i].IsCompleted )) {
                    Task t = pendingTaskQueue.Dequeue();
                    taskPool[i] = t;
                    t.Start();
                }
            }
        }
    }
}