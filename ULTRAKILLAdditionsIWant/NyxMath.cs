namespace UKAIW
{
    public static class NyxMath
    {
        public static float NormalizeToRange(float a, float minimum, float maximum)
        {
            if (maximum == minimum)
            {
                return 1.0f;
            }

            a -= minimum;
            a /= maximum - minimum;

            return a;
        }

        public static float InverseNormalizeToRange(float a, float minimum, float maximum)
        {
            if (maximum == minimum)
            {
                return 0.0f;
            }

            a -= maximum;
            a /= minimum - maximum;

            return a;
        }
    }
}