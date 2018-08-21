using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace XCommon.Threading
{
    public static class TaskHelpers
    {
        // Fields
        private static readonly Task<object> completedTaskReturningNull = Task.FromResult<object>(null);
        private static readonly Task defaultCompleted = Task.FromResult<AsyncVoid>(new AsyncVoid());

        // Methods
        public static Task Canceled()
        {
            return CancelCache<AsyncVoid>.CanceledResult;
        }

        public static Task<TResult> Canceled<TResult>()
        {
            return CancelCache<TResult>.CanceledResult;
        }

        public static Task Completed()
        {
            return defaultCompleted;
        }

        public static Task FromError(System.Exception exception)
        {
            return FromError<AsyncVoid>(exception);
        }

        public static Task<TResult> FromError<TResult>(System.Exception exception)
        {
            TaskCompletionSource<TResult> source = new TaskCompletionSource<TResult>();
            source.SetException(exception);
            return source.Task;
        }

        public static Task<object> NullResult()
        {
            return completedTaskReturningNull;
        }

        // Nested Types
        [StructLayout(LayoutKind.Sequential, Size = 1)]
        private struct AsyncVoid
        {
        }

        private static class CancelCache<TResult>
        {
            // Fields
            public static readonly Task<TResult> CanceledResult;

            // Methods
            static CancelCache()
            {
                CanceledResult = GetCancelledTask();
            }

            private static Task<TResult> GetCancelledTask()
            {
                TaskCompletionSource<TResult> source = new TaskCompletionSource<TResult>();
                source.SetCanceled();
                return source.Task;
            }
        }
    }
}