using System.Threading.Tasks;

namespace XCommon.Threading
{
    public static class TaskHelpersExtensions
    {
        // Methods
        public static async Task<object> CastToObject(this Task task)
        {
            await task;
            return null;
        }

        public static async Task<object> CastToObject<T>(this Task<T> task)
        {
            return await task;
        }

        public static void ThrowIfFaulted(this Task task)
        {
            task.GetAwaiter().GetResult();
        }

        public static bool TryGetResult<TResult>(this Task<TResult> task, out TResult result)
        {
            if (task.Status == TaskStatus.RanToCompletion)
            {
                result = task.Result;
                return true;
            }

            result = default(TResult);
            return false;
        }
    }
}