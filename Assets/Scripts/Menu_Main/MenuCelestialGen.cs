using SolarSystem;
using UnityEngine;

namespace Menu_Main
{
    public class MenuCelestialGen :MonoBehaviour
    {
        public GameObject PlanetPrefab;

        private void Start()
        {
            TryGenerateFinalPlanet();
        }

        private void TryGenerateFinalPlanet()
        {
            GameObject planetInstance = Instantiate(PlanetPrefab);
            planetInstance.layer = LayerMask.NameToLayer("Planet");
            
            Planet planet = new();
            PlanetInfo planetInfo = planetInstance.GetComponent<PlanetInfo>();
            planetInfo.PlanetType = "RockyPlanet";
            planetInfo.Index = 1;
            planetInfo.Name = " ";
            planetInfo.Mass = 1f;
            planetInfo.GO_Radius = 2f;
            planetInfo.Info_Radius = 1f;
            planetInfo.RotationalPeriod = 1f;
            planetInfo.OrbitalPeriod = 1f;
            planetInfo.FocusPoint = new Vector3(1f, 1f, 1f);
            planetInfo.SurfaceTemperature = 1f;
            planetInfo.SurfacePressure = 1f;
            planetInfo.SurfaceGravity = 1f;
            planetInfo.EscapeVelocity = 1f;
            planetInfo.MagneticFieldStrength = 1f;
            planetInfo.MeanDensity = 1f;
            planetInfo.AxialTilt = 1f;
            planetInfo.HasAtmosphere = true;
            planetInfo.IsHabitable = false;
            planetInfo.HasRings = false;
            planetInfo.InnerRingRadius = 1f;
            planetInfo.OuterRingRadius = 1f;
            planetInfo.Composition = planet.GenerateComposition();
            planetInfo.AtmosphereComposition = planet.GenerateAtmosphereComposition(true);
        }
    }
}