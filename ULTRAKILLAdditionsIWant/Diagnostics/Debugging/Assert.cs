using System;
using UnityEngine.Assertions;

namespace UKAIW
{
    public static class Assert
    {
        public static void IsTrue(bool condition, string additionalMsg = "")
        {
            if (!condition)
            {
                throw new AssertionException($"Assertion Failed: Condition was false :c; {additionalMsg}", $"Assertion Failed: Condition was false :c; {additionalMsg}");
            }
        }

        public static void IsFalse(bool condition, string additionalMsg = "")
        {
            if (condition)
            {
                throw new AssertionException($"Assertion Failed: Condition was false :c; {additionalMsg}", $"Assertion Failed: Condition was false :c; {additionalMsg}");
            }
        }

        public static void IsNotNull(object obj, string additionalMsg = "")
        {
            if (obj == null)
            {
                if (obj is null)
                {
                    throw new AssertionException($"Assertion Failed: Object was null :c; {additionalMsg}", $"Assertion Failed: Object was null :c; {additionalMsg}"); 
                }

                throw new AssertionException($"Assertion Failed: Object equals null but *'is' not* null :c; {additionalMsg}", $"Assertion Failed: Object was null :c; {additionalMsg}");
            }
        }
        
        public static void IsNotNull(UnityEngine.Object obj, string additionalMsg = "")
        {
            if (obj == null)
            {
                if (obj is null)
                {
                    throw new AssertionException($"Assertion Failed: Object was null :c; {additionalMsg}", $"Assertion Failed: Object was null :c; {additionalMsg}"); 
                }

                throw new AssertionException($"Assertion Failed: Object equals null but *'is' not* null :c; {additionalMsg}", $"Assertion Failed: Object was null :c; {additionalMsg}");
            }
        }        
    }
}