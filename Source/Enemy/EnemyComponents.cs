using System;
using System.Collections.Generic;
using Nyxpiri.ULTRAKILL.NyxLib;
using Nyxpiri.ULTRAKILL.NyxLib.Diagnostics.Debug;
using UnityEngine;

/* as of making it, mostly just meant to be inheritted from so our own enemy addition monobehaviours can be identified. */
public class EnemyModifier : MonoBehaviour
{
}

public class EnemyComponents : MonoBehaviour
{
    public static MonoRegistrar MonoRegistrar = new MonoRegistrar();

    [SerializeField] private EnemyPrefabStore _PrefabStore = null;
    public EnemyPrefabStore PrefabStore { get => _PrefabStore; private set => _PrefabStore = value; }

    [SerializeField] private EnemyFriendIdentifier _FriendID = null;
    public EnemyFriendIdentifier FriendID { get => _FriendID; private set => _FriendID = value; }

    [SerializeField] private EnemyRadiance _Radiance = null;
    public EnemyRadiance Radiance { get => _Radiance; private set => _Radiance = value; }

    [SerializeField] private EnemyFeedbacker _Feedbacker = null;
    public EnemyFeedbacker Feedbacker { get => _Feedbacker; private set => _Feedbacker = value; }

    public bool UniquelySolo { get; private set; } = false;

    // params: (GameObject target, Vector3 force, Vector3? hitPoint, float multiplier, bool tryForExplode, float critMultiplier, GameObject sourceWeapon, bool ignoreTotalDamageTakenMultiplier, bool fromExplosion)
    public event Action<GameObject, Vector3, Vector3?, float, bool, float, GameObject, bool, bool> PreHurt = null;
    // params: (GameObject target, Vector3 force, Vector3? hitPoint, float multiplier, bool tryForExplode, float critMultiplier, GameObject sourceWeapon, bool ignoreTotalDamageTakenMultiplier, bool fromExplosion)
    public event Action<GameObject, Vector3, Vector3?, float, bool, float, GameObject, bool, bool> PostHurt = null;

    public Action PreEnrage = null;
    public Action PreUnEnrage = null;
    
    // params: (bool instakill)
    public event Action<bool> PreDeath = null;
    // params: (bool instakill)
    public event Action<bool> PostDeath = null;

    [SerializeField] public float InitialHealth { get; private set; } = -1.0f;

    public float Health 
    { 
        get 
        {
            if (Enemy != null)
            {
                return Enemy.health;
            }

            return Eid.health;
        } 
        set
        {   
            if (Enemy != null)
            {
                Enemy.health = value;
                return;
            }

            Eid.health = value;
        } 
    }

    public GameObject RootGameObject { get => Eid.enemyType == EnemyType.MaliciousFace ? transform.parent.gameObject : gameObject; }

    public bool InstaKilled { get; private set; } = false;

    [NonSerialized] public bool QueuedForDestruction = false;
    
    [NonSerialized] public EnemyIdentifier Eid = null;
    [NonSerialized] public Enemy Enemy = null;
    public IReadOnlyList<Collider> Colliders { get => _colliders; }

    public T GetMonoByIndex<T>(int idx) where T : MonoBehaviour
    {
        if (idx < 0)
        {
            throw new IndexOutOfRangeException($"Index {idx} was less than 0");
        }

        if (idx > _monoBehaviours.Count)
        {
            return null;
        }

        return _monoBehaviours[idx] as T;
    }

    public void MarkAsUniquelySolo()
    {
        UniquelySolo = true;
    }

    [SerializeField] private Collider[] _colliders = null; 
    [SerializeField] private List<MonoBehaviour> _monoBehaviours = null;

    private void Awake()
    {
        Log.TraceExpectedInfo($"enemy '{name}:{gameObject.GetInstanceID()}' awakens...");

        Eid = GetComponent<EnemyIdentifier>();
        Enemy = GetComponent<Enemy>();
        RootGameObject.AddComponent<EnemyRoot>();
        Assert.IsNotNull(Eid);
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
            _colliders = GetComponentsInChildren<Collider>();
            Eid.ForceGetHealth();
            CreateMods();
        }
    }

    /* to allow patching lol */
    private void OnDestroy()
    {
        Log.TraceExpectedInfo($"enemy '{name}:{gameObject.GetInstanceID()}' gets instantaneously obliterated (destroyed)...");

        if (Eid.enemyType == EnemyType.Mindflayer)
        {
            var mindflayer = GetComponent<Mindflayer>();
            
            if (mindflayer.tempBeam != null)
            {
                mindflayer.tempBeam.DetachAndTurnOff();
            }
        }
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

        _monoBehaviours = new List<MonoBehaviour>(MonoRegistrar.RegisteredTypes.Count);

        foreach (var type in MonoRegistrar.RegisteredTypes)
        {
            _monoBehaviours.Add((MonoBehaviour)(gameObject.AddComponent(type)));
        }

        FriendID = gameObject.AddComponent<EnemyFriendIdentifier>();
        Radiance = gameObject.AddComponent<EnemyRadiance>();
        Feedbacker = gameObject.AddComponent<EnemyFeedbacker>();
        FriendID.IsLeader = false;
        PrefabStore = gameObject.AddComponent<EnemyPrefabStore>();
        PrefabStore.StorePrefab();
        FriendID.IsLeader = true;
    }

    private void AssertModsNotNull()
    {
        Assert.IsNotNull(PrefabStore);
        Assert.IsNotNull(FriendID);
        Assert.IsNotNull(Radiance);
        Assert.IsNotNull(Feedbacker);
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
        EnemyEvents.PreDeath?.Invoke(this, InstaKilled);
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
        EnemyEvents.PostDeath?.Invoke(this, InstaKilled);
    }
    
    private bool DeathCalled = false;
    internal void TryCallDeath()
    {
        if (DeathCalled)
        {
            return;
        }

        DeathCalled = true;
        EnemyEvents.Death?.Invoke(this);
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