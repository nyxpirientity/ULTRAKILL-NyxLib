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

    [SerializeField] private EnemyRadiance _Radiance = null;
    public EnemyRadiance Radiance { get => _Radiance; private set => _Radiance = value; }

    [SerializeField] private bool _hasDoneSetup = false;
    [SerializeField] private EnemyRoot _enemyRootMono;
    public EnemyRoot EnemyRootMono { get => _enemyRootMono; }

    public bool HasDoneSetup { get => _hasDoneSetup; }

    public bool UniquelySolo { get; private set; } = false;

    public delegate void PreHurtEventHandler(EventMethodCanceler canceler, GameObject target, Vector3 force, Vector3? hitPoint, float multiplier, bool tryForExplode, float critMultiplier, GameObject sourceWeapon, bool ignoreTotalDamageTakenMultiplier, bool fromExplosion);
    public event PreHurtEventHandler PreHurt;

    public delegate void PostHurtEventHandler(EventMethodCancelInfo cancelInfo, GameObject target, Vector3 force, Vector3? hitPoint, float multiplier, bool tryForExplode, float critMultiplier, GameObject sourceWeapon, bool ignoreTotalDamageTakenMultiplier, bool fromExplosion);
    public event PostHurtEventHandler PostHurt;

    public delegate void PreAnyEnemyHurtEventHandler(EventMethodCanceler canceler, EnemyComponents enemy, GameObject target, Vector3 force, Vector3? hitPoint, float multiplier, bool tryForExplode, float critMultiplier, GameObject sourceWeapon, bool ignoreTotalDamageTakenMultiplier, bool fromExplosion);
    public static event PreAnyEnemyHurtEventHandler PreAnyEnemyHurt = null;

    public delegate void PostAnyEnemyHurtEventHandler(EventMethodCancelInfo cancelInfo, EnemyComponents enemy, GameObject target, Vector3 force, Vector3? hitPoint, float multiplier, bool tryForExplode, float critMultiplier, GameObject sourceWeapon, bool ignoreTotalDamageTakenMultiplier, bool fromExplosion);
    public static event PostAnyEnemyHurtEventHandler PostAnyEnemyHurt = null;

    public delegate void PreEnrageEventHandler(EventMethodCanceler canceler);
    public event PreEnrageEventHandler PreEnrage = null;
    public delegate void PostEnrageEventHandler(EventMethodCancelInfo cancelInfo);
    public event PostEnrageEventHandler PostEnrage  = null;

    public delegate void PreUnEnrageEventHandler(EventMethodCanceler canceler);
    public event PreUnEnrageEventHandler PreUnEnrage = null;
    public delegate void PostUnEnrageEventHandler(EventMethodCancelInfo cancelInfo);
    public event PostUnEnrageEventHandler PostUnEnrage = null;
    
    public delegate void PreDeathEventHandler(EventMethodCanceler canceler, bool instakill);
    public event PreDeathEventHandler PreDeath;
    public delegate void PostDeathEventHandler(EventMethodCancelInfo cancelInfo, bool instakill);
    public event PostDeathEventHandler PostDeath;

    public bool IsEnemyCompInitializer { get => _isEnemyCompInitializer; }

    public bool AvoidHealthBasedSlowDown = false;

    [SerializeField] public float InitialHealth { get; private set; } = -1.0f;
    [SerializeField] public float HighestHealth { get; private set; } = -1.0f;

    public float Health 
    { 
        get 
        {
            if (Enemy != null)
            {
                return Enemy.health;
            }
            
            Eid.ForceGetHealth();

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
    
    public EnemyIdentifier Eid = null;
    public Enemy Enemy = null;
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

    public void InstaDestroy()
    {
        Destroy(RootGameObject);
    }

    [SerializeField] private Collider[] _colliders = null; 
    [SerializeField] private List<MonoBehaviour> _monoBehaviours = null;
    private bool _isEnemyCompInitializer = false;

    private void Awake()
    {
        Log.TraceExpectedInfo($"EnemyComponents '{name}:{gameObject.GetInstanceID()}' awakens...");
        Setup();
        EnemyRootMono.Enemy = this;
    }

    internal void Setup()
    {
        if (HasDoneSetup)
        {
            return;
        }

        Log.TraceExpectedInfo($"EnemyComponents '{name}:{gameObject.GetInstanceID()}' is setting up...");
        Eid = GetComponent<EnemyIdentifier>();
        Enemy = GetComponent<Enemy>();
        Assert.IsNotNull(Eid);
        
        _isEnemyCompInitializer = true;
        _hasDoneSetup = true;
        
        _enemyRootMono = RootGameObject.GetOrAddComponent<EnemyRoot>();
        CreateMods();

        Eid.ForceGetHealth();
        InitialHealth = Health;
        UpdateHighestHealth();
        
        _colliders = GetComponentsInChildren<Collider>(true);
    }

    private void UpdateHighestHealth()
    {
        if (Health > HighestHealth)
        {
            HighestHealth = Health;
        }
    }

    private void Start()
    {
        Log.TraceExpectedInfo($"enemy '{name}:{gameObject.GetInstanceID()}' starts...");
        
        PrefabStore.StorePrefab();
        
        if (_isEnemyCompInitializer)
        {
            _colliders = GetComponentsInChildren<Collider>();
        }

        if (InitialHealth <= 0.0f)
        {
            Eid.ForceGetHealth();
            InitialHealth = Health;
        }
        
        PrefabStore.StorePrefab();
        UpdateHighestHealth();
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
        UpdateHighestHealth();
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

        Radiance = gameObject.GetOrAddComponent<EnemyRadiance>();
        PrefabStore = gameObject.AddComponent<EnemyPrefabStore>();
    }

    private void AssertModsNotNull()
    {
        Assert.IsNotNull(PrefabStore);
        Assert.IsNotNull(Radiance);
    }
    
    object DeathPatchCallerObject = null;
    private bool PreDeathCalled = false;
    internal void TryCallPreDeath(EventMethodCanceler canceler, bool instakill, object patchCallerObject)
    {
        if (PreDeathCalled)
        {
            return;
        }

        DeathPatchCallerObject = patchCallerObject;
        PreDeathCalled = true;
        InstaKilled = instakill;
        PreDeath?.Invoke(canceler, InstaKilled);
        EnemyEvents.PreDeath?.Invoke(this, InstaKilled);
    }

    private bool PostDeathCalled = false;
    internal void TryCallPostDeath(EventMethodCancelInfo cancelInfo, object patchCallerObject)
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
        PostDeath?.Invoke(cancelInfo, InstaKilled);
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
    internal void NotifyOfPreHurt(EventMethodCanceler canceler, GameObject target, Vector3 force, Vector3? hitPoint, float multiplier, float critMultiplier, GameObject sourceWeapon, bool tryForExplode, bool ignoreTotalDamageTakenMultiplier, bool fromExplosion, object hurtPatchCallerObject)
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

        PreHurt?.Invoke(canceler, target, force, hitPoint, multiplier, tryForExplode, critMultiplier, sourceWeapon, ignoreTotalDamageTakenMultiplier, fromExplosion);
        PreAnyEnemyHurt?.Invoke(canceler, this, target, force, hitPoint, multiplier, tryForExplode, critMultiplier, sourceWeapon, ignoreTotalDamageTakenMultiplier, fromExplosion);
        
        HurtPatchCallerObject = hurtPatchCallerObject;
        InTheProcessOfHurting = true;
    }

    internal void NotifyOfPostHurt(EventMethodCancelInfo cancellationInfo, GameObject target, Vector3 force, Vector3? hitPoint, float multiplier, float critMultiplier, GameObject sourceWeapon, bool tryForExplode, bool ignoreTotalDamageTakenMultiplier, bool fromExplosion, object hurtPatchCallerObject)
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


        PostHurt?.Invoke(cancellationInfo, target, force, hitPoint, multiplier, tryForExplode, critMultiplier, sourceWeapon, ignoreTotalDamageTakenMultiplier, fromExplosion);
        PostAnyEnemyHurt?.Invoke(cancellationInfo, this, target, force, hitPoint, multiplier, tryForExplode, critMultiplier, sourceWeapon, ignoreTotalDamageTakenMultiplier, fromExplosion);
        
        HurtPatchCallerObject = null;
        InTheProcessOfHurting = false;
    }

    internal bool CallPreEnrage(EventMethodCancellationTracker cancellationTracker)
    {
        cancellationTracker.Reset();
        PreEnrage?.Invoke(cancellationTracker.GetCanceler());
        cancellationTracker.TryInvokeReimplementation();
        return cancellationTracker.ShouldRunMethod;
    }

    internal void CallPostEnrage(EventMethodCancellationTracker cancellationTracker)
    {
        PostEnrage?.Invoke(cancellationTracker.GetCancelInfo());
    }
    
    internal bool CallPreUnEnrage(EventMethodCancellationTracker cancellationTracker)
    {
        cancellationTracker.Reset();
        PreUnEnrage?.Invoke(cancellationTracker.GetCanceler());
        cancellationTracker.TryInvokeReimplementation();
        return cancellationTracker.ShouldRunMethod;
    }

    internal void CallPostUnEnrage(EventMethodCancellationTracker cancellationTracker)
    {
        PostUnEnrage?.Invoke(cancellationTracker.GetCancelInfo());
    }
    
}