using System.IO;
using HarmonyLib;
using TMPro;
using Nyxpiri.ULTRAKILL.NyxLib.Diagnostics.Debug;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.AddressableAssets;
using System.Collections.Generic;
using System;

namespace Nyxpiri.ULTRAKILL.NyxLib.Assets;

[ConfigureSingleton(SingletonFlags.NoAutoInstance)]
public class Explosions : MonoSingleton<Explosions>
{
    public static class Core
    {
        public static PrefabAsset<ExplosionRoot> Normal { get; private set; } = new PrefabAsset<ExplosionRoot>(() => Instance?._coreNormal);
        public static PrefabAsset<ExplosionRoot> Super { get; private set; } = new PrefabAsset<ExplosionRoot>(() => Instance?._coreSuper);
    }

    public static class Rocket
    {
        public static PrefabAsset<ExplosionRoot> Harmless { get; private set; } = new PrefabAsset<ExplosionRoot>(() => Instance?._rocketHarmless);
        public static PrefabAsset<ExplosionRoot> Normal { get; private set; } = new PrefabAsset<ExplosionRoot>(() => Instance?._rocketNormal);
        public static PrefabAsset<ExplosionRoot> Super { get; private set; } = new PrefabAsset<ExplosionRoot>(() => Instance?._rocketSuper);
    }

    public static ExplosionRoot InstantiateHarmless(bool active, Vector3 position, float size, float speed, ExplosionOverrides overrides = default, Transform parent = null)
    {
        return InstantiateExplosion(Harmless, active, position, size, speed, 0, overrides, parent);
    }

    public static ExplosionRoot InstantiateNormal(bool active, Vector3 position, float size, float speed, int damage, ExplosionOverrides overrides = default, Transform parent = null)
    {
        overrides.Ignite = overrides.Ignite.GetValueOrDefault(true);
        return InstantiateExplosion(Normal, active, position, size, speed, damage, overrides, parent);
    }

    public static ExplosionRoot InstantiateSuper(bool active, Vector3 position, float size, float speed, int damage, ExplosionOverrides overrides = default, Transform parent = null)
    {
        overrides.Ignite = overrides.Ignite.GetValueOrDefault(true);
        return InstantiateExplosion(Super, active, position, size, speed, damage, overrides, parent);
    }

    public static PrefabAsset<ExplosionRoot> Harmless { get; private set; } = new PrefabAsset<ExplosionRoot>(() => Instance?._harmless);
    public static PrefabAsset<ExplosionRoot> Normal { get; private set; } = new PrefabAsset<ExplosionRoot>(() => Instance?._normal);
    public static PrefabAsset<ExplosionRoot> Super { get; private set; } = new PrefabAsset<ExplosionRoot>(() => Instance?._super);

    [SerializeField] private ExplosionRoot _harmless = null;
    [SerializeField] private ExplosionRoot _normal = null;
    [SerializeField] private ExplosionRoot _super = null;

    [SerializeField] private ExplosionRoot _rocketHarmless = null;
    [SerializeField] private ExplosionRoot _rocketNormal = null;
    [SerializeField] private ExplosionRoot _rocketSuper = null;

    [SerializeField] private ExplosionRoot _coreNormal = null;
    [SerializeField] private ExplosionRoot _coreSuper = null;

    private void Awake()
    {
        SceneEvents.OnSceneStart += OnNewSceneStart;
    }

    private void OnNewSceneStart(Scene scene, string levelName, string unitySceneName)
    {
        if (_harmless != null || !Gear.AssetsLoaded)
        {
            return;
        }

        Log.ExpectedInfo($"Getting explosions...");

        var fs = Gear.Firestarter.DirectPrefab.GetComponent<RocketLauncher>();
        var ce = Gear.CoreEject.DirectPrefab.GetComponent<Shotgun>();
        var rocket = fs.rocket.GetComponent<Grenade>();
        var grenade = ce.grenade.GetComponent<Grenade>();

        _coreNormal = Instantiate(grenade.explosion, AssetsRoot.Holder).AddComponent<ExplosionRoot>();
        _coreSuper = Instantiate(grenade.superExplosion, AssetsRoot.Holder).AddComponent<ExplosionRoot>();

        _rocketHarmless = Instantiate(rocket.harmlessExplosion, AssetsRoot.Holder).AddComponent<ExplosionRoot>();
        _rocketNormal = Instantiate(rocket.explosion, AssetsRoot.Holder).AddComponent<ExplosionRoot>();
        _rocketSuper = Instantiate(rocket.superExplosion, AssetsRoot.Holder).AddComponent<ExplosionRoot>();

        _harmless = Instantiate(_rocketHarmless, AssetsRoot.Holder.transform);
        _harmless.SetMaxSize(1.0f);
        _harmless.SetMaxSpeed(1.0f);
        _harmless.SetMaxPlayerDamageOverride(-1);
        _harmless.SetMaxPushForce(1.0f);
        _harmless.RocketExplosion = false;

        _normal = Instantiate(_coreNormal, AssetsRoot.Holder.transform);
        _normal.RemoveExplosions((e) => e.harmless || e.damage == 0);
        _normal.SetMaxDamage(1);
        _normal.SetMaxSize(1.0f);
        _normal.SetMaxSpeed(1.0f);
        _normal.SetMaxPlayerDamageOverride(-1);
        _normal.SetMaxPushForce(1.0f);

        _super = Instantiate(_coreSuper, AssetsRoot.Holder.transform);
        _super.RemoveExplosions((e) => e.harmless || e.damage == 0);
        _super.SetMaxDamage(1);
        _super.SetMaxSize(1.0f);
        _super.SetMaxSpeed(1.0f);
        _super.SetMaxPlayerDamageOverride(-1);
        _super.SetMaxPushForce(1.0f);
    }

    private static ExplosionRoot InstantiateExplosion(PrefabAsset<ExplosionRoot> prefab, bool active, Vector3 position, float size, float speed, int damage, ExplosionOverrides overrides = default, Transform parent = null)
    {
        var newExplosion = prefab.Instantiate(active: false, position, Quaternion.identity, parent);

        newExplosion.ScaleDamage(damage);
        newExplosion.ScaleSize(size);
        newExplosion.ScaleSpeed(speed);

        newExplosion.FriendlyFire = overrides.FriendlyFire.GetValueOrDefault(false);
        newExplosion.Enemy = overrides.Enemy.GetValueOrDefault(false);
        newExplosion.IsFup = overrides.IsFup.GetValueOrDefault(false);
        newExplosion.Ultrabooster = overrides.Ultrabooster.GetValueOrDefault(false);
        newExplosion.RocketExplosion = overrides.RocketExplosion.GetValueOrDefault(false);
        newExplosion.Ignite = overrides.Ignite.GetValueOrDefault(false);
        newExplosion.Eletric = overrides.Eletric.GetValueOrDefault(false);
        newExplosion.Unblockable = overrides.Unblockable.GetValueOrDefault(false);
        newExplosion.Halved = overrides.Halved.GetValueOrDefault(false);
        newExplosion.HitterWeapon = overrides.HitterWeapon ?? "";
        newExplosion.SourceWeapon = overrides.SourceWeapon;
        newExplosion.OriginEid = overrides.OriginEid;
        newExplosion.CanHit = overrides.CanHit ?? AffectedSubjects.All;
        newExplosion.ToIgnore = overrides.ToIgnore ?? [];
        newExplosion.SetPlayerDamageOverride(overrides.PlayerDamageOverride.GetValueOrDefault(-1));
        newExplosion.ScalePushForce(overrides.PushScale.GetValueOrDefault(1.0f));
        newExplosion.ScaleEnemyDamageMultiplier(overrides.EnemyDamageMultiplier.GetValueOrDefault(1.0f));

        if (active)
        {
            newExplosion.gameObject.SetActive(true);
        }

        return newExplosion;
    }
}