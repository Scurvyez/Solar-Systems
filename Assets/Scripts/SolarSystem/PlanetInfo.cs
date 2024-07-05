using System.Linq;
using UnityEngine;

namespace SolarSystem
{
    public class PlanetInfo : MonoBehaviour
    {
        public string Name;
        public float Mass;
        public float Radius;
        public float RotationalPeriod;
        public float OrbitalPeriod;
        public Vector3 FocusPoint;
        public float SurfaceTemperature;
        public float SurfacePressure;
        public float SurfaceGravity;
        public float EscapeVelocity;
        public float MagneticFieldStrength;
        public float MeanDensity;
        public float AxialTilt;
        public bool HasAtmosphere;
        public bool IsHabitable;
        public bool HasRings;
        
        // New fields to store string representations
        public string CompositionString;
        public string AtmosphereCompositionString;
            
        public SerializableDictionary<string, float> Composition;
        public SerializableDictionary<string, float> AtmosphereComposition;
        
        private void Update()
        {
            CompositionString = DictionaryToString(Composition);
            AtmosphereCompositionString = DictionaryToString(AtmosphereComposition);
        }

        private static string DictionaryToString(SerializableDictionary<string, float> dictionary)
        {
            if (dictionary == null || dictionary.Count == 0)
                return string.Empty;

            return dictionary.Aggregate("", (current, kvp) => current + $"{kvp.Key}: {kvp.Value:F0}, ");
        }
    }
}
