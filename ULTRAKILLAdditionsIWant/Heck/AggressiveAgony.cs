using System;
using UnityEngine;
using UnityEngine.UI;

namespace UKAIW
{
    public class AggressiveAgony : MonoBehaviour
    {
        public Heck Heck { get; private set; } = null;
        public PainStore PainStore { get; private set; } = null;

        protected void Awake()
        {
            Heck = Heck.Instance;
            PainStore = Heck.Instance.PainStore;
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