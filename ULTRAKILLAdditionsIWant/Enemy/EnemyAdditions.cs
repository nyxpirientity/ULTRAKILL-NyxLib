using System;
using System.Collections.Generic;
using UKAIW;
using UKAIW.Diagnostics.Debug;
using UnityEngine;
using UnityEngine.AI;

/* as of making it, mostly just meant to be inheritted from so our own enemy addition monobehaviours can be identified. */
public class EnemyModifier : MonoBehaviour
{
}

public class EnemyAdditions : MonoBehaviour
{
    [SerializeField] public EnemyHydra Hydra { get; private set; }
    [SerializeField] public EnemyPrefabStore PrefabStore { get; private set; }
    [SerializeField] public EnemyFriendIdentifier FriendID { get; private set; } = null;
    [SerializeField] public EnemyBloodFuel BloodFuel { get; private set; } = null;
    [SerializeField] public SaltyEnemy Salt { get; private set; } = null;
    [SerializeField] public HeckPuppet HeckPuppet { get; private set; } = null;
    [SerializeField] public HeckPuppetLeader HeckPuppetLeader { get; private set; } = null;
    [SerializeField] public EnemyRadiance Radiance { get; private set; } = null;
    [SerializeField] public EnemyFeedbacker Feedbacker { get; private set; } = null;
    [SerializeField] public EnemyPain Pain { get; private set; } = null;

    public bool UniquelySolo { get; private set; } = false;

    // params: (GameObject target, Vector3 force, Vector3? hitPoint, float multiplier, bool tryForExplode, float critMultiplier, GameObject sourceWeapon, bool ignoreTotalDamageTakenMultiplier, bool fromExplosion)
    public Action<GameObject, Vector3, Vector3?, float, bool, float, GameObject, bool, bool> PreHurt = null;
    // params: (GameObject target, Vector3 force, Vector3? hitPoint, float multiplier, bool tryForExplode, float critMultiplier, GameObject sourceWeapon, bool ignoreTotalDamageTakenMultiplier, bool fromExplosion)
    public Action<GameObject, Vector3, Vector3?, float, bool, float, GameObject, bool, bool> PostHurt = null;

    public Action PreEnrage = null;
    public Action PreUnEnrage = null;
    
    // params: (bool instakill)
    public Action<bool> PreDeath = null;
    // params: (bool instakill)
    public Action<bool> PostDeath = null;

    [SerializeField] public float InitialHealth { get; private set; } = -1.0f;

    public float Health 
    { 
        get => Eid.Health; 
        set
        {   
            Enemy.health = value;
        } 
    }

    public GameObject RootGameObject { get => Eid.enemyType == EnemyType.MaliciousFace ? transform.parent.gameObject : gameObject; }

    public bool InstaKilled { get; private set; } = false;

    [NonSerialized] public bool QueuedForDestruction = false;
    
    [NonSerialized] public EnemyIdentifier Eid = null;
    [NonSerialized] public Enemy Enemy = null;

    private void Awake()
    {
        Log.TraceExpectedInfo($"enemy '{name}:{gameObject.GetInstanceID()}' awakens...");

        Eid = GetComponent<EnemyIdentifier>() ?? GetComponentInChildren<EnemyIdentifier>();
        Enemy = GetComponent<Enemy>();
    }

    private void Start()
    {
        Log.TraceExpectedInfo($"enemy '{name}:{gameObject.GetInstanceID()}' starts...");

        if (InitialHealth <= 0.0f)
        {
            InitialHealth = Health;
        }

        if (Radiance == null)
        {
            Eid.ForceGetHealth();
            CreateMods();
        }
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
    }

    protected void FixedUpdate()
    {
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

    private void CreateMods()
    {
        Log.TraceExpectedInfo($"{name}.EnemyAdditions is creating new modules...");
        Hydra = gameObject.AddComponent<EnemyHydra>();
        Hydra.InitializeAsNew();
        FriendID = gameObject.AddComponent<EnemyFriendIdentifier>();
        BloodFuel = gameObject.AddComponent<EnemyBloodFuel>();
        Salt = gameObject.AddComponent<SaltyEnemy>();
        HeckPuppetLeader = gameObject.AddComponent<HeckPuppetLeader>();
        Radiance = gameObject.AddComponent<EnemyRadiance>();
        Feedbacker = gameObject.AddComponent<EnemyFeedbacker>();
        Pain = gameObject.AddComponent<EnemyPain>();
        FriendID.IsLeader = false;
        PrefabStore = gameObject.AddComponent<EnemyPrefabStore>();
        PrefabStore.StorePrefab();
        Hydra.PassPrefabToShared();
        FriendID.IsLeader = true;
    }

    private void AssertModsNotNull()
    {
        Assert.IsNotNull(Hydra);
        Assert.IsNotNull(PrefabStore);
        Assert.IsNotNull(FriendID);
        Assert.IsNotNull(BloodFuel);
        Assert.IsNotNull(Radiance);
        Assert.IsNotNull(Feedbacker);
        Assert.IsNotNull(Pain);
    }

    public void MarkAsUniquelySolo()
    {
        UniquelySolo = true;
    }
    
    object DeathPatchCallerObject = null;
    private bool PreDeathCalled = false;
    internal void TryCallPreDeath(bool instakill, object patchCallerObject)
    {
        if (PreDeathCalled)
        {
            return;
        }

        DeathPatchCallerObject = patchCallerObject;
        PreDeathCalled = true;
        InstaKilled = instakill;
        PreDeath?.Invoke(InstaKilled);
        EnemyEvents.PreDeath?.Invoke(Enemy, InstaKilled);
    }

    private bool PostDeathCalled = false;
    internal void TryCallPostDeath(object patchCallerObject)
    {
        if (PostDeathCalled)
        {
            return;
        }

        if (DeathPatchCallerObject != patchCallerObject)
        {
            return;
        }

        PostDeathCalled = true;
        PostDeath?.Invoke(InstaKilled);
        EnemyEvents.PostDeath?.Invoke(Enemy, InstaKilled);
    }
    
    private bool DeathCalled = false;
    internal void TryCallDeath()
    {
        if (DeathCalled)
        {
            return;
        }

        DeathCalled = true;
        EnemyEvents.Death?.Invoke(Enemy);
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
        
        HurtPatchCallerObject = hurtPatchCallerObject;
        InTheProcessOfHurting = true;
    }

    internal void NotifyOfPostHurt(GameObject target, Vector3 force, Vector3? hitPoint, float multiplier, float critMultiplier, GameObject sourceWeapon, bool tryForExplode, bool ignoreTotalDamageTakenMultiplier, bool fromExplosion, object hurtPatchCallerObject)
    {
        if (!InTheProcessOfHurting)
        {
            Log.TraceExpectedInfo($"{name}: had NotifyOfPostHurt called when we were NOT in the process of hurting already, ignoring");
            return;
        }

        if (HurtPatchCallerObject != hurtPatchCallerObject)
        {
            Log.TraceExpectedInfo($"{name}: had NotifyOfPostHurt called when we were in the process of hurting already BUT the hurtPatchCallerObject did not match, ignoring");
            return;
        }


        PostHurt?.Invoke(target, force, hitPoint, multiplier, tryForExplode, critMultiplier, sourceWeapon, ignoreTotalDamageTakenMultiplier, fromExplosion);
        EnemyEvents.PostHurt?.Invoke(this, target, force, hitPoint, multiplier, tryForExplode, critMultiplier, sourceWeapon, ignoreTotalDamageTakenMultiplier, fromExplosion);
        
        HurtPatchCallerObject = null;
        InTheProcessOfHurting = false;
    }
}