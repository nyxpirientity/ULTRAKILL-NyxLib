using System;
using System.Collections.Generic;
using MelonLoader;
using SettingsMenu.Components.Pages;
using TMPro;
using UnityEngine;

namespace UKAIW
{
    public class PlayerAttackParryability
    {
        int NumParries = 0;
    }

    public class ParryabilityCalculator : MonoBehaviour
    {
        public static ParryabilityCalculator Instance { get; private set; } = null;

        Dictionary<Type, PlayerAttackParryability> AttackParryability = new Dictionary<Type, PlayerAttackParryability>(32);

        protected void Start()
        {
            Instance = this;
        }

        protected void OnDestroy()
        {
        }

        protected void FixedUpdate()
        {

        }

        protected void LateUpdate()
        {

        }
    }
}