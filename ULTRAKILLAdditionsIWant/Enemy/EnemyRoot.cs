using System;
using System.Collections.Generic;
using UKAIW;
using UKAIW.Diagnostics.Debug;
using UnityEngine;
using UnityEngine.AI;

/* component to be added to the root/top-level game object for all enemies */
public class EnemyRoot : MonoBehaviour
{
    public EnemyAdditions Eadd { get; private set; } = null;

    protected void Awake()
    {
        Eadd = GetComponent<EnemyAdditions>() ?? GetComponentInChildren<EnemyAdditions>();
        Assert.IsNotNull(Eadd, $"EnemyRoot object '{gameObject}' could not find it's EnemyAdditions object!");
    }
}