using System;
using System.Collections;
using System.Collections.Generic;

namespace FilesExtractor.Code.Extensions {
    static class CollectionExtensions {
        public static bool IsNullOrEmpty(this ICollection collection) {
            return collection == null || collection.Count == 0;
        }

        public static void ForEach<T>(this IEnumerable<T> source, Action<T> action) {
            foreach (T element in source) {
                action(element);
            }
        }
    }
}