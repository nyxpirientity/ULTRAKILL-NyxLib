using System;
using System.Collections.Generic;
using UnityEngine;

namespace Nyxpiri.ULTRAKILL.NyxLib;

public struct ExplosionOverrides()
{
    public bool? FriendlyFire = null;
    public bool? Enemy = null;
    public bool? IsFup = null;
    public bool? Ultrabooster = null;
    public bool? RocketExplosion = null;

    public bool? Ignite = null;
    public bool? Eletric = null;

    public bool? Unblockable = null;
    public bool? Halved = null;

    public int? PlayerDamageOverride = null;
    public float? EnemyDamageMultiplier = null;
    public float? PushScale = null;

    public string HitterWeapon = null;
    public GameObject SourceWeapon = null;
    public EnemyIdentifier OriginEid = null;

    public AffectedSubjects? CanHit = null;
    public List<EnemyType> ToIgnore = null;
}