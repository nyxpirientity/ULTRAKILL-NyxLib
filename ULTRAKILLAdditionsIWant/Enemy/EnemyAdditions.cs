using System;
using System.Collections.Generic;
using UKAIW;
using UKAIW.Diagnostics.Debug;
using UnityEngine;
using UnityEngine.AI;

[Serializable]
public class EnemyAdditions : MonoBehaviour
{
    public EnemyHydraMod HydraMod { get; private set; }
    public EnemyPrefabMod PrefabMod { get; private set; }
    public EnemyFriendIdentifier EnemyFriend { get; private set; } = null;
    public EnemyBloodFuel EnemyBloodFuel { get; private set; } = null;
    public SaltyEnemy SaltyEnemy { get; private set; } = null;
    public HeckPuppet HeckPuppet { get; private set; } = null;
    public HeckPuppetLeader HeckPuppetLeader { get; private set; } = null;
    public Radiance EnemyRadiance { get; private set; } = null;
    public EnemyFeedbacker Feedbacker { get; private set; } = null;
    public EnemyPain Agony { get; private set; } = null;
    public bool UniquelySolo { get; private set; } = false;

    // params: (GameObject target, Vector3 force, Vector3? hitPoint, float multiplier, bool tryForExplode, float critMultiplier, GameObject sourceWeapon, bool ignoreTotalDamageTakenMultiplier, bool fromExplosion)
    public Action<GameObject, Vector3, Vector3?, float, bool, float, GameObject, bool, bool> PreHurt = null;
    // params: (GameObject target, Vector3 force, Vector3? hitPoint, float multiplier, bool tryForExplode, float critMultiplier, GameObject sourceWeapon, bool ignoreTotalDamageTakenMultiplier, bool fromExplosion)
    public Action<GameObject, Vector3, Vector3?, float, bool, float, GameObject, bool, bool> PostHurt = null;
    
    // params: (bool instakill)
    public Action<bool> PreDeath = null;
    // params: (bool instakill)
    public Action<bool> PostDeath = null;

    public float Health 
    { 
        get => Eid.Health; 
        set
        {
            if (Eid.zombie != null)
            {
                Eid.zombie.health = value;
            }
            else if (Eid.drone != null)
            {
                Eid.drone.health = value;
            }
            else if (Eid.machine != null)
            {
                Eid.machine.health = value;
            }
            else if (Eid.statue != null)
            {
                Eid.statue.health = value;
            }
            else if (Eid.spider != null)
            {
                Eid.spider.health = value;
            }

            Eid.health = value;
        } 
    }

    public GameObject RootGameObject { get => Eid.enemyType == EnemyType.MaliciousFace ? transform.parent.gameObject : gameObject; }

    public bool InstaKilled { get; private set; } = false;

    [NonSerialized] public bool QueuedForDestruction = false;
    
    [NonSerialized] public EnemyIdentifier Eid = null;

    private void Awake()
    {
        Eid = GetComponent<EnemyIdentifier>() ?? GetComponentInChildren<EnemyIdentifier>();
        FindAndCacheMods(true);
        Log.TraceExpectedInfo($"enemy '{name}:{gameObject.GetInstanceID()}' awakens...");
    }

    private void Start()
    {
        FindAndCacheMods(true);
        Log.TraceExpectedInfo($"enemy '{name}:{gameObject.GetInstanceID()}' starts...");
    }

    /* to allow patching lol */
    private void OnDestroy()
    {
        Log.TraceExpectedInfo($"enemy '{name}:{gameObject.GetInstanceID()}' gets instantaneously obliterated (destroyed)...");
    }

    protected void Update()
    {
        if (QueuedForDestruction)
        {
            Log.TraceExpectedInfo($"enemy '{name}:{gameObject.GetInstanceID()}' was queued for destruction, so its time has come.");
            Destroy(RootGameObject);
        }

        if (InTheProcessOfHurting)
        {
            InTheProcessOfHurting = false;
            Log.UnexpectedInfo($"{name}: InTheProcessOfHurting had to be set to false by Update");
        }

        if (Eid.enemyType == EnemyType.Streetcleaner)
        {
            var sc = Eid.GetComponent<Streetcleaner>();

            if ((sc.NullInvalid()?.isActiveAndEnabled).GetValueOrDefault(false))
            {
                if (!sc.dead && Eid.machine.limp)
                {
                    Log.Error($"Streetcleaner '{gameObject}' is not dead but limp?");
                    sc.dead = true;
                }

                FieldPublisher<Streetcleaner, NavMeshAgent> nma = new FieldPublisher<Streetcleaner, NavMeshAgent>(sc, "nma");
                if (nma.Value == null && !sc.dead)
                {
                    Log.Warning($"invalid nma failsafe caught invalid nma in active, !dead streetcleaner '{gameObject}'");
                    Destroy(RootGameObject);
                }
            }
        }
    }

    protected void FixedUpdate()
    {
        if (Eid.enemyType == EnemyType.Swordsmachine)
        {
            var sm = Eid.GetComponent<SwordsMachine>();

            if ((sm.NullInvalid()?.isActiveAndEnabled).GetValueOrDefault(false))
            {
                if (Eid.machine.limp)
                {
                    Log.Error($"Swordsmachine '{gameObject}' is limp but still active?");
                    Destroy(RootGameObject);
                }

                FieldPublisher<SwordsMachine, Rigidbody> rb = new FieldPublisher<SwordsMachine, Rigidbody>(sm, "rb");
                if (rb.Value == null)
                {
                    Log.Warning($"invalid nma failsafe caught invalid rb in active swordsmachine '{gameObject}'");
                    Destroy(RootGameObject);
                }
            }
        }
    }
    
    private void OnDisable()
    {
        Log.TraceExpectedInfo($"enemy '{name}:{gameObject.GetInstanceID()}' gets disabled...");
        
        if (QueuedForDestruction)
        {
            Log.TraceExpectedInfo($"enemy '{name}:{gameObject.GetInstanceID()}' was queued for destruction, so its time has come.");
            Destroy(RootGameObject);
        }
    }

    private void OnEnable()
    {
        Log.TraceExpectedInfo($"enemy '{name}:{gameObject.GetInstanceID()}' gets enabled...");
    }

    /* if called we were instantiated by the game, most likely */
    public void SetupMods()
    {
        //Log.ExpectedInfo($"{name}.EnemyAdditions is setting up new modules...");
        HydraMod = gameObject.AddComponent<EnemyHydraMod>();
        HydraMod.InitializeAsNew();
        EnemyFriend = gameObject.AddComponent<EnemyFriendIdentifier>();
        EnemyBloodFuel = gameObject.AddComponent<EnemyBloodFuel>();
        SaltyEnemy = gameObject.AddComponent<SaltyEnemy>();
        HeckPuppetLeader = gameObject.AddComponent<HeckPuppetLeader>();
        EnemyRadiance = gameObject.AddComponent<Radiance>();
        Feedbacker = gameObject.AddComponent<EnemyFeedbacker>();
        Agony = gameObject.AddComponent<EnemyPain>();
        EnemyFriend.IsLeader = false;
        PrefabMod = gameObject.AddComponent<EnemyPrefabMod>();
        HydraMod.PassPrefabToShared();
        EnemyFriend.IsLeader = true;
    }

    public void FindAndCacheMods(bool nullAcceptable = false)
    {
        //Log.ExpectedInfo($"{name}.EnemyAdditions is finding and caching modules...");
        // todo: may be faster to just iterate through components manually 
        HydraMod = GetComponent<EnemyHydraMod>();   
        PrefabMod = GetComponent<EnemyPrefabMod>();   
        EnemyFriend = GetComponent<EnemyFriendIdentifier>();
        SaltyEnemy = GetComponent<SaltyEnemy>();
        EnemyBloodFuel = GetComponent<EnemyBloodFuel>();
        EnemyRadiance = GetComponent<Radiance>();
        HeckPuppet = GetComponent<HeckPuppet>();
        HeckPuppetLeader = GetComponent<HeckPuppetLeader>();
        Feedbacker = GetComponent<EnemyFeedbacker>();
        Agony = GetComponent<EnemyPain>();
        
        if (nullAcceptable)
        {
            return;
        }

        Assert.IsNotNull(HydraMod);
        Assert.IsNotNull(PrefabMod);
        Assert.IsNotNull(EnemyFriend);
        Assert.IsNotNull(EnemyBloodFuel);
        Assert.IsNotNull(EnemyRadiance);
        Assert.IsNotNull(Feedbacker);
        Assert.IsNotNull(Agony);
    }

    public void MarkAsUniquelySolo()
    {
        UniquelySolo = true;
    }
    
    private bool PreDeathCalled = false;
    internal void TryCallPreDeath(bool instakill)
    {
        if (PreDeathCalled)
        {
            return;
        }

        PreDeathCalled = true;
        InstaKilled = instakill;
        PreDeath?.Invoke(InstaKilled);
        EnemyEvents.PreDeath?.Invoke(Eid, InstaKilled);
    }

    private bool PostDeathCalled = false;
    internal void TryCallPostDeath()
    {
        if (PostDeathCalled)
        {
            return;
        }

        PostDeathCalled = true;
        PostDeath?.Invoke(InstaKilled);
        EnemyEvents.PostDeath?.Invoke(Eid, InstaKilled);
    }
    
    private bool DeathCalled = false;
    internal void TryCallDeath()
    {
        if (DeathCalled)
        {
            return;
        }

        DeathCalled = true;
        EnemyEvents.Death?.Invoke(Eid);
    }

    private bool InTheProcessOfHurting = false;
    private object HurtPatchCallerObject = null;
    internal void NotifyOfPreHurt(GameObject target, Vector3 force, Vector3? hitPoint, float multiplier, float critMultiplier, GameObject sourceWeapon, bool tryForExplode, bool ignoreTotalDamageTakenMultiplier, bool fromExplosion, object hurtPatchCallerObject)
    {
        if (InTheProcessOfHurting)
        {
            if (HurtPatchCallerObject == hurtPatchCallerObject)
            {
                Log.TraceExpectedInfo($"{name}: had NotifyOfPreHurt called by the same hurtPatchCallerObject when we were in the process of hurting already? ignoring");
            }
            else
            {
                Log.TraceExpectedInfo($"{name}: had NotifyOfPreHurt called by differing hurtPatchCallerObject when we were in the process of hurting already, ignoring");
            }
            return;
        }

        PreHurt?.Invoke(target, force, hitPoint, multiplier, tryForExplode, critMultiplier, sourceWeapon, ignoreTotalDamageTakenMultiplier, fromExplosion);
        EnemyEvents.PreHurt?.Invoke(this, target, force, hitPoint, multiplier, tryForExplode, critMultiplier, sourceWeapon, ignoreTotalDamageTakenMultiplier, fromExplosion);
    }

    internal void NotifyOfPostHurt(GameObject target, Vector3 force, Vector3? hitPoint, float multiplier, float critMultiplier, GameObject sourceWeapon, bool tryForExplode, bool ignoreTotalDamageTakenMultiplier, bool fromExplosion, object hurtPatchCallerObject)
    {
        if (!InTheProcessOfHurting)
        {
            Log.TraceExpectedInfo($"{name}: had NotifyOfPreHurt called when we were NOT in the process of hurting already, ignoring");
            return;
        }

        if (HurtPatchCallerObject != hurtPatchCallerObject)
        {
            Log.TraceExpectedInfo($"{name}: had NotifyOfPreHurt called when we were in the process of hurting already BUT the hurtPatchCallerObject did not match, ignoring");
            return;
        }

        HurtPatchCallerObject = null;

        PostHurt?.Invoke(target, force, hitPoint, multiplier, tryForExplode, critMultiplier, sourceWeapon, ignoreTotalDamageTakenMultiplier, fromExplosion);
        EnemyEvents.PostHurt?.Invoke(this, target, force, hitPoint, multiplier, tryForExplode, critMultiplier, sourceWeapon, ignoreTotalDamageTakenMultiplier, fromExplosion);
    }
}