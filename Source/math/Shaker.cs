using System;
using UnityEngine;

namespace NyxpiriOS // made for my godot game, yoinked it and converted it to this version of C# for this project AND to unity, and to stop using the many convenience libraries I wrote for godot and maths...
{
    public class Shaker2D
    {
        public float _MinScale = 0.0f;
        public float MinScale
        {
            get => _MinScale;
            set
            {
                _MinScale = value;
            }
        }

        public float _MaxScale = 1.0f;
        public float MaxScale
        {
            get => _MaxScale;
            set
            {
                _MaxScale = value;
            }
        }

        private float _MinDistance = 1.0f;
        public float MinDistance
        {
            get => _MinDistance;
            set
            {
                _MinDistance = value;
            }
        }

        public float _Rate = 50.0f;
        public float Rate
        {
            get => _Rate;
            set
            {
                _Rate = Mathf.Max(0.0f, value);
            }
        }

        public Vector2 PositionA { get; private set; } = Vector2.zero;
        public Vector2 PositionB { get; private set; } = Vector2.zero;
        public float Alpha { get; private set; } = 0.0f;

        public Vector2 Position
        {
            get
            {
                float processedAlpha = Alpha;
                
                processedAlpha = Mathf.Sin((processedAlpha + 0.5f) * ((float)Math.PI));

                if (Alpha < 0.5f)
                {
                    processedAlpha = (1.0f - processedAlpha) * 0.5f;
                }
                else
                {
                    processedAlpha = 0.5f + (Mathf.Abs(processedAlpha) * 0.5f);
                }

                return Vector3.Lerp(PositionA, PositionB, processedAlpha);
            }
        }

        public void NextPosition()
        {
            PositionA = PositionB;

            for (int i = 0; (i < 10 && Vector3.Distance(PositionB, PositionA) < MinDistance) || i < 1; i++)
            {
                Vector2 randomUnitVec2 = UnityEngine.Random.insideUnitCircle.normalized;
                
                randomUnitVec2 = randomUnitVec2 == Vector2.zero ? Vector2.right : randomUnitVec2;
                PositionB = randomUnitVec2 * UnityEngine.Random.Range(MinScale, MaxScale);

                if (Vector2.Distance(PositionB, PositionA) < MinDistance)
                {
                    PositionB +=  (PositionB - PositionA).normalized * (MinDistance - Vector2.Distance(PositionB, PositionA));

                    if (PositionB.magnitude > MaxScale || PositionB.magnitude < MinScale)
                    {
                        PositionB = Vector2.zero;
                        continue;
                    }
                    else
                    {
                        break;
                    }
                }
            }
        }

        public void Process(float delta)
        {
            Alpha += (delta * (Rate));

            if (Alpha > 1.0f)
            {
                Alpha %= 1.0f;
                NextPosition();
            }
        }
    }
}