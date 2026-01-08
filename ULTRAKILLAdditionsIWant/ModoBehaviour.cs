using System;
using System.Collections.Generic;
using System.Linq;
using UKAIW;
using UKAIW.Diagnostics.Debug;
using UnityEngine;


[Serializable]
public abstract class ModoBehaviour : ScriptableObject
{
    protected ModoBehaviour() {}

    [SerializeReference] public ModoBehaviourManager Mono = null;
    public GameObject GameObject { get => Mono.gameObject; }
    public Transform Transform { get => Mono.transform; }

    public T GetComponent<T>() where T : Component
    {
        Assert.IsNotNull(Mono);
        Assert.IsNotNull(GameObject);
        return GameObject.GetComponent<T>();
    }

    protected abstract void ModoAwake();
    protected abstract void ModoStart();
    public abstract void ModoUpdate();
    public abstract void ModoLateUpdate();
    public abstract void ModoFixedUpdate();
    public abstract void ModoOnDestroy();
    public abstract void ModoOnDisable();
    public abstract void ModoOnEnable();

    public abstract void OnModRemoved();

    public abstract void OnClonedFrom(ModoBehaviour ClonedFrom);

    private bool HasAwaken = false;
    internal void Wake()
    {
        if (HasAwaken)
        {
            return;
        }

        TryLog.Action(() => { ModoAwake(); });
        HasAwaken = true;
    }

    private bool HasStarted = false;
    internal void Begin()
    {
        if (HasStarted)
        {
            return;
        }

        TryLog.Action(() => { ModoStart(); });
        
        HasStarted = true;
    }
}

/* Manages Monobehaviour modules, or ModoBehaviours because when measuring performance I've mysteriously noticed that MonoBehaviours made by mods are significantly slower than ones that aren't. No clue why. */
[Serializable]
public class ModoBehaviourManager : MonoBehaviour
{
    [SerializeReference] private List<ModoBehaviour> _Mods = new List<ModoBehaviour>(8);
    public IReadOnlyList<ModoBehaviour> Mods { get => _Mods; }

    public T GetMod<T>() where T : ModoBehaviour
    {
        return (T)_Mods.Find((mod) => { return mod is T; });
    }

    public T AddMod<T>() where T : ModoBehaviour
    {
        T mod = ScriptableObject.CreateInstance<T>();

        AddMod(mod);

        return mod;
    }

    public void RemoveMod<T>() where T : ModoBehaviour
    {
        _Mods.RemoveAll((mod) => 
        { 
            if (mod is T)
            {
                TryLog.Action(() => { mod.OnModRemoved(); });
                
                mod.Mono = null;
                return true;
            }

            return false; 
        });
    }

    public void AddMod(ModoBehaviour mod)
    {
        _Mods.Add(mod);
        mod.Mono = this;

        if (HasAwaken)
        {
            mod.Wake();
        }
    }

    public void RemoveMod(ModoBehaviour mod)
    {
        mod.Mono = null;
        TryLog.Action(() => { mod.OnModRemoved(); });
        _Mods.Remove(mod);
    }

    private bool HasAwaken = false;
    protected void Awake()
    {
        try
        {
            UnsafeAwake();
        }
        catch (System.Exception e)
        {
            Log.Error($"{name}.ModoBehaviourManager awake failed!\n {e}");            
        }
    }
    protected void UnsafeAwake()
    {
        List<ModoBehaviour> cloneMods = new List<ModoBehaviour>(_Mods.Count);
        _Mods.RemoveAll((mod) =>
        {
            Assert.IsNotNull(mod);

            if (mod.Mono != this)
            {
                cloneMods.Add(mod);
                return true;
            }

            return false;
        });

        foreach (var mod in cloneMods)
        {
            var modClone = UnityEngine.Object.Instantiate(mod);
            modClone.OnClonedFrom(mod);

            AddMod(modClone);
        }

        foreach (var mod in Mods)
        {
            mod.Wake();
        }

        HasAwaken = true;
    }

    private bool HasStarted = false;
    protected void Start()
    {
        foreach (var mod in Mods)
        {
            TryLog.Action(() => { mod.Begin(); });
        }

        HasStarted = true;
    }

    protected void Update()
    {
        foreach (var mod in Mods)
        {
            TryLog.Action(() => { mod.Begin(); });
            TryLog.Action(() => { mod.ModoUpdate(); });
        }
    }

    protected void LateUpdate()
    {
        foreach (var mod in Mods)
        {
            TryLog.Action(() => { mod.ModoLateUpdate(); });
        }
    }

    protected void FixedUpdate()
    {
        foreach (var mod in Mods)
        {
            TryLog.Action(() => { mod.ModoFixedUpdate(); });
        }
    }

    protected void OnDestroy()
    {
        ModoBehaviour[] mods = Mods.ToArray();
        foreach (var mod in mods)
        {
            TryLog.Action(() => { mod.ModoOnDestroy(); });
            RemoveMod(mod);
        }
    }

    protected void OnDisable()
    {
        foreach (var mod in Mods)
        {
            TryLog.Action(() => { mod.ModoOnDisable(); });
        }
    }

    protected void OnEnable()
    {
        foreach (var mod in Mods)
        {
            TryLog.Action(() => { mod.ModoOnEnable(); });
        }
    }
}