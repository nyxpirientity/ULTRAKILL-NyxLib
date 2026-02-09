using UnityEngine;

namespace UKAIW
{
    /* represents a timestamp relative to the time the current scene was loaded */
    public struct SceneTimeStamp
    {
        public double? TimeStamp { get; set; }
        public readonly double TimeSince { get => Time.timeSinceLevelLoadAsDouble - TimeStamp.GetValueOrDefault(0.0); }

        public void UpdateToNow()
        {
            TimeStamp = Time.timeSinceLevelLoadAsDouble;
        }
    }
}