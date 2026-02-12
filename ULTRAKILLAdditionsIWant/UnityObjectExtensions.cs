using System;
using UnityEngine;

namespace UKAIW
{
public static class UnityEngineObjectExtensions
{      
    /* 
        Returns null if (self == null), thus using Unity's equality overload that can check for invalid references (as in, a reference to a destroyed UnityEngine.Object)
        there may be better ways to do this, but the purpose of this is to allow things like ?. that uses Unity's == overload (to check for invalidity OR null equality)
        example: SomeObject.NullInvalid()?.DoSomethingGoodMaybe();
        from my understanding, 
    */
    public static T NullInvalid<T>(this T self) where T : UnityEngine.Object
    {
        if (self == null)
        {
            return null;
        }
        
        return self;
    }
}
}