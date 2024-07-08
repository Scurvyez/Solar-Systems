using UnityEngine;

namespace SolarSystem
{
    public class MoonInfo : MonoBehaviour
    {
        public int Index;
        public string ParentPlanetName;
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
        public float InnerRingRadius;
        public float OuterRingRadius;
    }
}