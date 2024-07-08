using UnityEngine;

namespace Utils
{
    public static class TrailTimersUtil
    {
        public static float PlanetTrailTime(float curGameSpeed)
        {
            return CalculateTrailTime(curGameSpeed);
        }

        public static float MoonTrailTime(float curGameSpeed)
        {
            return CalculateTrailTime(curGameSpeed);
        }

        private static float CalculateTrailTime(float curGameSpeed)
        {
            float baseTrailTime = curGameSpeed <= 1f ? 5f : 0.5f;

            float adjustedTrailTime = baseTrailTime / Mathf.Max(curGameSpeed / 200f, 1f); // Prevent division by zero or extremely high values
            return Mathf.Clamp(adjustedTrailTime, 0f, 5f);
        }
    }
}