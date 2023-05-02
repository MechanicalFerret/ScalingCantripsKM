using System;

namespace ScalingCantripsKM.Extensions
{
    static class UnityEngineObjectExtensions
    {
        public static T CreateCopy<T>(this T original, Action<T> action = null) where T : UnityEngine.Object
        {
            var clone = UnityEngine.Object.Instantiate(original);
            if (action != null)
            {
                action(clone);
            }
            return clone;
        }
    }
}
