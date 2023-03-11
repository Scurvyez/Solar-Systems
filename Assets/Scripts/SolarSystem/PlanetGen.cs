using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class PlanetGen : MonoBehaviour
{
    public GameObject starPrefab;
    public GameObject axialTiltMarkerPrefab;
    public GameObject spinDirectionMarkerPrefab;

    public List<GameObject> planets = new ();

    public void Start()
    {
        Material testMat = Resources.Load<Material>("Star");

        for (int i = 0; i < SaveManager.instance.activeSave.rockyPlanets.Count; i++)
        {
            // planet properties
            float pMass = SaveManager.instance.activeSave.rockyPlanets[i].Mass;
            float pRadius = SaveManager.instance.activeSave.rockyPlanets[i].Radius;
            float pOrbitalPeriod = SaveManager.instance.activeSave.rockyPlanets[i].OrbitalPeriod;
            float pRotationPeriod = SaveManager.instance.activeSave.rockyPlanets[i].RotationPeriod;
            float pAxialTilt = SaveManager.instance.activeSave.rockyPlanets[i].AxialTilt;
            float pSurfaceTemperature = SaveManager.instance.activeSave.rockyPlanets[i].SurfaceTemperature;
            float pMeanDensity = SaveManager.instance.activeSave.rockyPlanets[i].MeanDensity;
            float pSurfacePressure = SaveManager.instance.activeSave.rockyPlanets[i].SurfacePressure;
            float pSurfaceGravity = SaveManager.instance.activeSave.rockyPlanets[i].SurfaceGravity;
            float pEscapeVelocity = SaveManager.instance.activeSave.rockyPlanets[i].EscapeVelocity;
            float pAlbedo = SaveManager.instance.activeSave.rockyPlanets[i].Albedo;
            float pMagneticFieldStrength = SaveManager.instance.activeSave.rockyPlanets[i].MagneticFieldStrength;
            bool pHasAtmosphere = SaveManager.instance.activeSave.rockyPlanets[i].HasAtmosphere;
            bool pHasRings = SaveManager.instance.activeSave.rockyPlanets[i].HasRings;
            bool pIsHabitable = SaveManager.instance.activeSave.rockyPlanets[i].IsHabitable;

            // star properties
            float sMass = (float)SaveManager.instance.activeSave.starMass;
            float gravitationalConstant = 6.6743e-11f;

            // Make a rocky planet prefab and set its comps and props
            GameObject planet = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            planet.AddComponent<SphereCollider>();
            planet.AddComponent<TrailRenderer>();
            planet.AddComponent<Rigidbody>();
            planet.AddComponent<PlanetController>();

            //float distanceFromStar = (Mathf.Pow(pOrbitalPeriod, 2) * gravitationalConstant * sMass) / Mathf.Pow(4.0f * Mathf.Pow(Mathf.PI, 2), 1 / 3);
            //distanceFromStar *= 149597870.7f / 500.0f; // (1 AU / half) / a factor of 500 for scaling purposes

            // Populate all components fields
            planet.name = SaveManager.instance.activeSave.starSystemName + "-" + ToRomanNumeral(i + 1);
            planet.transform.localScale = new Vector3(pRadius, pRadius, pRadius);
            planet.transform.localPosition = new Vector3(pOrbitalPeriod, 0, 0);
            planet.GetComponent<MeshRenderer>().material = testMat;
            planet.GetComponent<SphereCollider>().isTrigger = true;
            planet.GetComponent<Rigidbody>().mass = pMass;
            planet.GetComponent<Renderer>().material.color = Color.red;
            planet.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePositionY;
            planet.GetComponent<Rigidbody>().useGravity = true;
            planet.GetComponent<PlanetController>().SaveManager = SaveManager.instance;
            planet.GetComponent<PlanetController>().starPrefab = starPrefab;
            planet.GetComponent<PlanetController>().OrbitalPeriod = pOrbitalPeriod;
            planet.GetComponent<PlanetController>().RotationPeriod = pRotationPeriod;
            planet.GetComponent<PlanetController>().AxialTilt = pAxialTilt;
            planet.GetComponent<PlanetController>().SurfaceTemperature = pSurfaceTemperature;
            planet.GetComponent<PlanetController>().MeanDensity = pMeanDensity;
            planet.GetComponent<PlanetController>().SurfacePressure = pSurfacePressure;
            planet.GetComponent<PlanetController>().SurfaceGravity = pSurfaceGravity;
            planet.GetComponent<PlanetController>().EscapeVelocity = pEscapeVelocity;
            planet.GetComponent<PlanetController>().Albedo = pAlbedo;
            planet.GetComponent<PlanetController>().MagneticFieldStrength = pMagneticFieldStrength;
            planet.GetComponent<PlanetController>().HasAtmosphere = pHasAtmosphere;
            planet.GetComponent<PlanetController>().HasRings = pHasRings;
            planet.GetComponent<PlanetController>().IsHabitable = pIsHabitable;
            planet.GetComponent<PlanetController>().axialTiltMarkerPrefab = axialTiltMarkerPrefab;
            planet.GetComponent<PlanetController>().spinDirectionMarkerPrefab = spinDirectionMarkerPrefab;

            // Trail Renderer
            planet.GetComponent<TrailRenderer>().startWidth = 1.0f;
            planet.GetComponent<TrailRenderer>().endWidth = 0.0f;
            planet.GetComponent<TrailRenderer>().time = 7.0f;
            planet.GetComponent<TrailRenderer>().material = new Material(Shader.Find("Sprites/Default"));
            planet.GetComponent<TrailRenderer>().startColor = Color.white;
            planet.GetComponent<TrailRenderer>().endColor = Color.clear;

            planets.Add(planet);
        }
    }

    private string ToRomanNumeral(int number)
    {
        // Define a dictionary of Roman numeral values
        var numeralMap = new Dictionary<int, string>
        {
            {1000, "M"},
            {900, "CM"},
            {500, "D"},
            {400, "CD"},
            {100, "C"},
            {90, "XC"},
            {50, "L"},
            {40, "XL"},
            {10, "X"},
            {9, "IX"},
            {5, "V"},
            {4, "IV"},
            {1, "I"}
        };

        // Convert the number to a Roman numeral string
        var result = new StringBuilder();
        foreach (var pair in numeralMap)
        {
            while (number >= pair.Key)
            {
                result.Append(pair.Value);
                number -= pair.Key;
            }
        }

        return result.ToString();
    }
}
