using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Utils
{
    public static class ColorUtil
    {
        /// <summary>
        /// Returns a Color with RGB values clamped between a min and max value (0-255).
        /// </summary>
        public static Color GetRandomColor(int minValue, int maxValue)
        {
            // Ensure the values are within the valid range (0 to 255)
            minValue = Mathf.Clamp(minValue, 0, 255);
            maxValue = Mathf.Clamp(maxValue, 0, 255);

            // Convert to 0-1 range for Unity's Color
            float minComponentValue = minValue / 255f;
            float maxComponentValue = maxValue / 255f;

            // Ensure min is not greater than max
            if (minComponentValue > maxComponentValue)
            {
                (minComponentValue, maxComponentValue) = (maxComponentValue, minComponentValue);
            }

            float r = Random.Range(minComponentValue, maxComponentValue);
            float g = Random.Range(minComponentValue, maxComponentValue);
            float b = Random.Range(minComponentValue, maxComponentValue);

            return new Color(r, g, b);
        }

        /// <summary>
        /// Returns a Color representing the atmosphere color based on the chemical composition.
        /// </summary>
        public static Color GetAtmosphereColor(SerializableDictionary<string, float> atmosphereComposition)
        {
            // Define colors for different chemical compositions
            Dictionary<string, Color> elementColors = new Dictionary<string, Color>
            {
                { "O", new Color(0.5f, 0.7f, 1.0f) }, // oxygen
                { "N", new Color(0.5f, 0.7f, 1.0f) }, // nitrogen
                { "CO2", new Color(0.9f, 0.5f, 0.5f) }, // carbon dioxide
                { "CH4", new Color(0.5f, 0.9f, 1.0f) }, // methane
                { "SO2", new Color(1.0f, 0.9f, 0.5f) }, // sulfur dioxide
                { "NH3", new Color(0.7f, 1.0f, 0.7f) }, // ammonia
                { "H", new Color(1.0f, 1.0f, 1.0f) }, // hydrogen
                { "He", new Color(1.0f, 1.0f, 1.0f) }, // helium
                { "Ne", new Color(1.0f, 0.7f, 1.0f) }, // neon
                { "Ar", new Color(1.0f, 0.7f, 1.0f) }, // argon
                { "Kr", new Color(1.0f, 0.7f, 1.0f) }, // krypton
                { "Xe", new Color(1.0f, 0.7f, 1.0f) }, // xenon
                { "NO", new Color(0.7f, 0.7f, 1.0f) }, // nitrogen oxides
                { "O3", new Color(0.4f, 0.6f, 1.0f) }, // ozone
                { "H2O", new Color(0.8f, 0.8f, 1.0f) } // water vapor
            };

            float totalProbability = atmosphereComposition.Values.Sum();
            if (totalProbability == 0) return Color.black;

            Color blendedColor = new Color(0, 0, 0);
            foreach (var element in atmosphereComposition)
            {
                if (!elementColors.TryGetValue(element.Key, out Color elementColor)) continue;
                float weight = element.Value / totalProbability;
                blendedColor += elementColor * weight;
            }

            // Normalize the resulting color
            blendedColor.r = Mathf.Clamp01(blendedColor.r);
            blendedColor.g = Mathf.Clamp01(blendedColor.g);
            blendedColor.b = Mathf.Clamp01(blendedColor.b);

            return blendedColor;
        }
    }
}