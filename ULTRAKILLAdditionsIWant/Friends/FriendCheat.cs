using System;
using System.Collections.Generic;
using MelonLoader;
using UKAIW;

public class FriendCheat : ICheat
{
    public string LongName { get; private set; } = "Give all Enemies A Friend c:";

    public string Identifier { get; private set; } = "ukaiw.give-all-enemies-a-friend-0";

    public string ButtonEnabledOverride { get; set; } = null;

    public string ButtonDisabledOverride { get; set; } = null;

    public string Icon => null;

    public bool DefaultState { get; private set; } = false;

    public StatePersistenceMode PersistenceMode => StatePersistenceMode.Persistent;

    public int Idx = -1;

    public FriendCheat(bool another = false)
    {
        Idx = FriendCheats.Count;
        FriendCheats.Add(this);

        if (another)
        {
            LongName = "Give all Enemies ANOTHER friend! c:";
        }

        Identifier = $"ukaiw.give-all-enemies-a-friend-{Idx}";

        CheatsManager.Instance.RegisterCheat(this, "THOUGHFULNESS AND CARING");
        CheatsManager.Instance.RebuildMenu();
    }

    public bool IsActive { get => Cheats.FriendCount > Idx; } 

    public void Disable()
    {
        Cheats.FriendCount -= 1;
        RefreshRegistration();
    }

    public void Enable(CheatsManager manager)
    {
        Cheats.FriendCount += 1;
        RefreshRegistration();
    }

    public static void Reset()
    {
        if (CheatsManager.Instance != null)
        {
            FriendCheats.Clear();
            RefreshRegistration();
        }
    }

    public static void RefreshRegistration()
    {
        if (FriendCheats.Count == 0)
        {
            new FriendCheat();
        }

        if (Cheats.FriendCount + 1 > FriendCheats.Count)
        {
            for (int i = FriendCheats.Count; i < Cheats.FriendCount + 1; i++)
            {
                new FriendCheat(true);
            }
        }
    }

    private static List<FriendCheat> FriendCheats = new List<FriendCheat>(); 
}