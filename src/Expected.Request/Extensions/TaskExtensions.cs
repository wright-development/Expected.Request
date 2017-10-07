using System.Threading.Tasks;
using System;

namespace Expected.Request
{
    public static class TaskExtensions
    {
        public static async Task<TResult> Next<T, TResult>(this Task<T> task, Func<T, Task<TResult>> nextTask)
        {
            var result = await task;
            var nextTaskResult = await nextTask(result);
            return nextTaskResult;
        }

        public static async Task Done(this Task<IExpectRequest> expect)
        {
            var result = await expect;
            await result.Done();
        }
    }
}
