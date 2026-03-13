using System.Reflection;

namespace Nyxpiri.ULTRAKILL.NyxLib
{
    public static class GrenadeExtensions
    {
        private static FieldInfo _explodedFi = typeof(Grenade).GetField("exploded", BindingFlags.Instance | BindingFlags.NonPublic);
        public static bool IsExploded(this Grenade self)
        {
            return (bool)_explodedFi.GetValue(self);
        }
    }
}