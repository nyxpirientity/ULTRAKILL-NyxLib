using System;
using UnityEngine;

namespace Nyxpiri.ULTRAKILL.NyxLib
{
    public static class EnemyEvents
    {
        public static Action<EnemyComponents> PreStart = null;
        public static Action<EnemyComponents> PostStart = null;
        public static Action<EnemyComponents> PreDisabled = null;
        public static Action<EnemyComponents> PreDestroy = null;
        
        public static Action<EnemyComponents, bool> PreDeath = null;
        public static Action<EnemyComponents, bool> PostDeath = null;
        public static Action<EnemyComponents> Death = null;
    }
}