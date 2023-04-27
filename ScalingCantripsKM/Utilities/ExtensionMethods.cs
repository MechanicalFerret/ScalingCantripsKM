using System;
using System.Linq;

namespace ScalingCantripsKM.Utilities
{
    static class ExtensionMethods
    {
        public static T[] AppendToArray<T>(this T[] array, T value)
        {
            var len = array.Length;
            var result = new T[len + 1];
            Array.Copy(array, result, len);
            result[len] = value;
            return result;
        }

        public static T[] AppendToArrayIfMissing<T>(this T[] array, T value)
        {
            if (!array.Contains(value)) return AppendToArray<T>(array, value);
            return array;
        }
    }
}
