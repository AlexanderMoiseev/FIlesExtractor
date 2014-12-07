using System;

namespace FilesExtractor.Code.Extensions {
    /// <summary>
    /// safe object operations
    /// </summary>
    static class SafeOperations {
        public static TOut With<T, TOut>(this T obj, Func<T, TOut> f) where T : class where TOut : class {
            return obj == null ? null : f(obj);
        }

        public static void Do<T>(this T obj, Action<T> action) where T : class {
            if (obj != null)  {
                 action(obj);
            }
        }
    }
}