using System;
using UnityEngine;
using UnityEngine.UI;

namespace UKAIW
{
    /* where heck itself stores its absorbed pain */
    public class PainStore : MonoBehaviour
    {
        public Heck Heck { get; private set; } = null;
        public float Agony { get; private set; } = 0.0f;

        public void AddPain(float amount)
        {
            Agony += amount;
        }

        protected void Awake()
        {
            Heck = Heck.Instance;
        }

        protected void Start()
        {
            
        }

        protected void Update()
        {
            
        }

        protected void FixedUpdate()
        {
        }

        protected void OnDestroy()
        {
            
        }

        protected void OnEnable()
        {
            
        }
        
        protected void OnDisable()
        {
            
        }
    }
}