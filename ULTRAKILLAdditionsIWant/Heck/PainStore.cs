using UnityEngine;

namespace UKAIW
{
    /* where heck itself stores its absorbed pain */
    public class PainStore : MonoBehaviour
    {
        public Heck Heck { get; private set; } = null;
        public float Pain { get; private set; } = 0.0f;
        private GameObject CheckpointDetector = null;

        public void AddPain(float amount)
        {
            if (amount > 0.0f)
            {
                amount = (amount / Mathf.Max((Pain - 100.0f) / 100.0f, 1.0f));
            }
            
            Pain = Mathf.Max(0.0f, Pain + amount);
        }

        protected void Awake()
        {
            Heck = Heck.Instance;
        }

        protected void Start()
        {
            NewCheckpointDetector();
        }

        private void NewCheckpointDetector()
        {
            CheckpointDetector = new GameObject();
            CheckpointDetector.AddComponent<DestroyOnCheckpointRestart>();
        }

        protected void Update()
        {
            if (CheckpointDetector == null)
            {
                NewCheckpointDetector();
                Pain = 0.0f;
            }
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