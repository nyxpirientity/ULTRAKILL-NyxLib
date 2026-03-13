using System;
using System.Collections.Generic;

public class ToggleCheat : ICheat
{
    public string LongName { get; private set; } = "Unnamed ToggleCheat";

    public string Identifier { get; private set; } = "ukaiw.unnamed-toggle-cheat";

    public string ButtonEnabledOverride { get; set; } = null;

    public string ButtonDisabledOverride { get; set; } = null;

    public string Icon => null;

    public bool DefaultState { get; private set; } = false;

    public StatePersistenceMode PersistenceMode => StatePersistenceMode.Persistent;

    public Action<ToggleCheat> OnDisable = null;
    public Action<ToggleCheat, CheatsManager> OnEnable = null;

    public ToggleCheat(string longName, string identifier, Action<ToggleCheat> onDisable, Action<ToggleCheat, CheatsManager> onEnable)
    {
        LongName = longName;
        Identifier = identifier;
        DefaultState = ToggleCheatStateStore.GetValueOrDefault(Identifier, false);
        OnDisable = onDisable;
        OnEnable = onEnable;
    }

    public bool IsActive { get => ToggleCheatStateStore.GetValueOrDefault(Identifier, false); } 

    public void Disable()
    {
        ToggleCheatStateStore[Identifier] = false;
        
        OnDisable?.Invoke(this);
    }

    public void Enable(CheatsManager manager)
    {
        ToggleCheatStateStore[Identifier] = true;

        OnEnable?.Invoke(this, manager);
    }

    private static Dictionary<string, bool> ToggleCheatStateStore = new Dictionary<string, bool>(); 
}