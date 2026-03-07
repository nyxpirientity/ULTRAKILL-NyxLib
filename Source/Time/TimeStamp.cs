using UnityEngine;

namespace Nyxpiri.ULTRAKILL.NyxLib
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

    /* represents a timestamp in fixedupdate time */
    public struct FixedTimeStamp
    {
        public double? TimeStamp { get; set; }
        public readonly double TimeSince { get => Time.fixedTime - TimeStamp.GetValueOrDefault(0.0); }

        public void UpdateToNow()
        {
            TimeStamp = Time.fixedTime;
        }
    }

    public struct GlobalTimeStamp
    {
        public double? TimeStamp { get; set; }
        public readonly double TimeSince { get => Time.timeAsDouble - TimeStamp.GetValueOrDefault(0.0); }

        public void UpdateToNow()
        {
            TimeStamp = Time.timeAsDouble;
        }
    }
}