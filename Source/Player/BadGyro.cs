using NyxpiriOS;
using UnityEngine;

namespace Nyxpiri.ULTRAKILL.NyxLib
{
    public class BadGyro : MonoBehaviour
    {
        public NewMovement player { get; private set; } = null;
        Shaker2D Shaker = new Shaker2D();
        Vector2 Rotation = Vector2.zero;

        protected void Start()
        {
            player = NewMovement.Instance;
        }

        protected void OnDestroy()
        {
        }

        protected void FixedUpdate()
        {
        }

        protected void Update()
        {
            if (Cheats.IsCheatDisabled(Cheats.BadGyro))
            {
                return;
            }

            Shaker.MaxScale = 45.0f;
            Shaker.MinScale = 0.0f;
            Shaker.MinDistance = 20.0f;
            Shaker.Rate = 1.0f;
            Shaker.Process(Time.deltaTime);

            Vector2 additional2d = Rotation;
            Rotation = NyxMath.EaseInterpTo(Rotation, Shaker.Position, Vector2.Distance(Shaker.PositionA, Shaker.PositionB) * 0.1f, Time.deltaTime);
            additional2d = Rotation - additional2d;

            player.cc.rotationX += additional2d.x;
            player.cc.rotationY += additional2d.y;
        }

        protected void LateUpdate()
        {
        }
    }
}