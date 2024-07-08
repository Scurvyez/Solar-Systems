using System.Linq;
using UnityEngine;

namespace SolarSystem
{
    public class StarInfo : MonoBehaviour
    {
        public string Class;
        public string Name;
        public double Age;
        public float Mass;
        public float Radius;
        public float Luminosity;
        public float SurfaceTemperature;
        public float RotationalPeriod;
        public float MagneticFieldStrength;
        public bool HasIntrinsicVariability;
        public bool HasExtrinsicVariability;
        public float Variability;
        public int PlanetCount;
        
        // New fields to store string representations
        public string CompositionString;
            
        public SerializableDictionary<string, float> Composition;
        
        private void Update()
        {
            CompositionString = DictionaryToString(Composition);
        }

        private static string DictionaryToString(SerializableDictionary<string, float> dictionary)
        {
            if (dictionary == null || dictionary.Count == 0)
                return string.Empty;

            return dictionary.Aggregate("", (current, kvp) => current + $"{kvp.Key}: {kvp.Value:F0}, ");
        }
    }
}