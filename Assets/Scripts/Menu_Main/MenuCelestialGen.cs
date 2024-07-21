using SolarSystem;
using Unity.VisualScripting;
using UnityEngine;

namespace Menu_Main
{
    public class MenuCelestialGen :MonoBehaviour
    {
        public GameObject PlanetPrefab;
        public PlanetMesh PlanetMesh;
        public Material PlanetMat;

        private void Start()
        {
            PlanetMesh = this.AddComponent<PlanetMesh>();
            if (PlanetMesh == null)
            {
                Debug.LogError("PlanetMesh or MoonMesh component not found on CelestialGen.");
                return;
            }
            
            TryGenerateFinalPlanet();
        }

        private void TryGenerateFinalPlanet()
        {
            MeshFilter combinedMesh = PlanetMesh.meshFilter;
            
            GameObject planetInstance = Instantiate(PlanetPrefab);
            planetInstance.AddComponent<MeshFilter>().mesh = combinedMesh.mesh;
            planetInstance.AddComponent<MeshRenderer>().material = PlanetMat;
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