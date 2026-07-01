using System.IO;
using HarmonyLib;
using TMPro;
using Nyxpiri.ULTRAKILL.NyxLib.Diagnostics.Debug;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.AddressableAssets;
using System.Collections.Generic;
using System;

namespace Nyxpiri.ULTRAKILL.NyxLib.Assets
{
    [ConfigureSingleton(SingletonFlags.NoAutoInstance)]
    public class Gear : MonoSingleton<Gear>
    {
        /* revolvers :3 */
        public static AssetReference Piercer => GunSetter.Instance.NullInvalid()?.revolverPierce[0];
        public static AssetReference Marksman => GunSetter.Instance.NullInvalid()?.revolverRicochet[0];
        public static AssetReference Sharpshooter => GunSetter.Instance.NullInvalid()?.revolverTwirl[0];

        public static AssetReference AltPiercer => GunSetter.Instance.NullInvalid()?.revolverPierce[1];
        public static AssetReference AltMarksman => GunSetter.Instance.NullInvalid()?.revolverRicochet[1];
        public static AssetReference AltSharpshooter => GunSetter.Instance.NullInvalid()?.revolverTwirl[1];

        /* shotguns :3 */
        public static AssetReference CoreEject => GunSetter.Instance.NullInvalid()?.shotgunGrenade[0];
        public static AssetReference PumpCharge => GunSetter.Instance.NullInvalid()?.shotgunPump[0];
        public static AssetReference SawedOn => GunSetter.Instance.NullInvalid()?.shotgunRed[0];

        public static AssetReference AltCoreEject => GunSetter.Instance.NullInvalid()?.shotgunGrenade[1];
        public static AssetReference AltPumpCharge => GunSetter.Instance.NullInvalid()?.shotgunPump[1];
        public static AssetReference AltSawedOn => GunSetter.Instance.NullInvalid()?.shotgunRed[1];

        /* nailguns :3 */
        public static AssetReference Attractor => GunSetter.Instance.NullInvalid()?.nailMagnet[0];
        public static AssetReference Overheat => GunSetter.Instance.NullInvalid()?.nailOverheat[0];
        public static AssetReference Jumpstart => GunSetter.Instance.NullInvalid()?.nailRed[0];

        public static AssetReference AltAttractor => GunSetter.Instance.NullInvalid()?.nailMagnet[1];
        public static AssetReference AltOverheat => GunSetter.Instance.NullInvalid()?.nailOverheat[1];
        public static AssetReference AltJumpstart => GunSetter.Instance.NullInvalid()?.nailRed[1];

        /* railcannons :3 */
        public static AssetReference ElectricRailCannon => GunSetter.Instance.NullInvalid()?.railCannon[0];
        public static AssetReference Screwdriver => GunSetter.Instance.NullInvalid()?.railHarpoon[0];
        public static AssetReference MaliciousRailCannon => GunSetter.Instance.NullInvalid()?.railMalicious[0];

        /* rocket launchers :3 */
        public static AssetReference FreezeFrame => GunSetter.Instance.NullInvalid()?.rocketBlue[0];
        public static AssetReference SRSCannon => GunSetter.Instance.NullInvalid()?.rocketGreen[0];
        public static AssetReference Firestarter => GunSetter.Instance.NullInvalid()?.rocketRed[0];

        private void Awake()
        {

        }
    }
}