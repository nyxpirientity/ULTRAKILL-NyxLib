using HarmonyLib;

namespace Nyxpiri.ULTRAKILL.NyxLib
{
    public static class PlayerPunchEvents
    {
        public delegate void PreParryProjectileEventHandler(EventMethodCanceler canceler, Punch punch, Projectile projectile);
        public static event PreParryProjectileEventHandler PreParryProjectile;

        public delegate void PostParryProjectileEventHandler(EventMethodCancelInfo cancelInfo, Punch punch, Projectile projectile);
        public static event PostParryProjectileEventHandler PostParryProjectile;

        [HarmonyPatch(typeof(Punch), "ParryProjectile")]
        static class PunchParryProjectilePatch
        {
            private static EventMethodCancellationTracker _cancellationTracker = new EventMethodCancellationTracker();

            public static bool Prefix(Punch __instance, Projectile proj)
            {
                _cancellationTracker.Reset();
                PreParryProjectile?.Invoke(_cancellationTracker.GetCanceler(), __instance, proj);
                _cancellationTracker.TryInvokeReimplementation();
                
                return !_cancellationTracker.Cancelled;
            }

            public static void Postfix(Punch __instance, Projectile proj)
            {
                PostParryProjectile?.Invoke(_cancellationTracker.GetCancelInfo(), __instance, proj);
            }
        }
    }
    

}