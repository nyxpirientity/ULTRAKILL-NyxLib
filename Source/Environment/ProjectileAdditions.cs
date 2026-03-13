using System;
using UnityEngine;

namespace Nyxpiri.ULTRAKILL.NyxLib
{
    public class ProjectileAdditions : MonoBehaviour
    {
        // (bool viaCreateFromExplosion)
        Action<bool> PreExplode = null;
        // (bool viaCreateFromExplosion)
        Action<bool> PostExplode = null;

        internal void InvokePreExplode(bool viaCreateFromExplosion)
        {
            PreExplode?.Invoke(viaCreateFromExplosion);
        }

        internal void InvokePostExplode(bool viaCreateFromExplosion)
        {
            PostExplode?.Invoke(viaCreateFromExplosion);
        }
    }
}