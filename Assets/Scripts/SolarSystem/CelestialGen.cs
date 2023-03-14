using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class CelestialGen : MonoBehaviour
{
    public GameObject starPrefab;
    //public GameObject planetPrefab;
    public GameObject axialTiltMarkerPrefab;
    public GameObject spinDirectionMarkerPrefab;

    public List<GameObject> planets = new ();
    public List<GameObject> moons = new ();
    public StaticAngledCamera StaticAngledCamera;

    public Transform scrollViewContent;
    public Button planetButtonPrefab;
    public Button moonButtonPrefab;

    public float minDistance = 0.25f;
    public float maxDistance = 2.15f;

    private float starMass;

    public void Start()
    {
        starMass = (float)SaveManager.instance.activeSave.starMass;
        PlanetGen();
        MoonGen();
    }

    private void PlanetGen()
    {
        // CHANGE THIS LATER
        // DIFF TEXTURES FOR DIFF PLANET TYPES
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

        for (int i = 0; i < planets.Count; i++)
        {
            // Store a reference to the planet game object
            GameObject planetObject = planets[i];

            Button planetButton = Instantiate(planetButtonPrefab, scrollViewContent);
            planetButton.GetComponentInChildren<TextMeshProUGUI>().text = planets[i].name;
            planetButton.GetComponentInChildren<TextMeshProUGUI>().rectTransform.anchoredPosition = new Vector2(0.0f, -25.0f);
            planetButton.GetComponentInChildren<TextMeshProUGUI>().fontSize = 12.0f;
            planetButton.GetComponentInChildren<TextMeshProUGUI>().color = Color.white;
            planetButton.onClick.AddListener(() => StaticAngledCamera.SetFocus(planetObject));
        }
    }

    private void MoonGen()
    {
        // CHANGE THIS LATER
        Material testMat = Resources.Load<Material>("Star");

        //foreach (GameObject planet in planets)
        {
            for (int i = 0; i < SaveManager.instance.activeSave.moons.Count; i++)
            {
                // moon properties
                float mMass = SaveManager.instance.activeSave.moons[i].Mass;
                float mRadius = SaveManager.instance.activeSave.moons[i].Radius;
                float mOrbitalPeriod = SaveManager.instance.activeSave.moons[i].OrbitalPeriod;
                float mRotationPeriod = SaveManager.instance.activeSave.moons[i].RotationPeriod;
                float mAxialTilt = SaveManager.instance.activeSave.moons[i].AxialTilt;
                float mSurfaceTemperature = SaveManager.instance.activeSave.moons[i].SurfaceTemperature;
                float mMeanDensity = SaveManager.instance.activeSave.moons[i].MeanDensity;
                float mSurfaceGravity = SaveManager.instance.activeSave.moons[i].SurfaceGravity;
                float mEscapeVelocity = SaveManager.instance.activeSave.moons[i].EscapeVelocity;
                float mAlbedo = SaveManager.instance.activeSave.moons[i].Albedo;
                bool mHasAtmosphere = SaveManager.instance.activeSave.moons[i].HasAtmosphere;
                bool mIsHabitable = SaveManager.instance.activeSave.moons[i].IsHabitable;

                // Make a moon prefab and set its comps and props
                GameObject moon = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                moon.AddComponent<SphereCollider>();
                moon.AddComponent<TrailRenderer>();
                moon.AddComponent<Rigidbody>();
                moon.AddComponent<MoonController>();

                // Populate all components fields
                moon.name = SaveManager.instance.activeSave.starSystemName + "-" + ToRomanNumeral(i + 1);
                moon.transform.localScale = new Vector3(mRadius, mRadius, mRadius);
                moon.transform.localPosition = new Vector3(mOrbitalPeriod, 0, 0);
                moon.GetComponent<MeshRenderer>().material = testMat;
                moon.GetComponent<SphereCollider>().isTrigger = true;
                moon.GetComponent<Rigidbody>().mass = mMass;
                moon.GetComponent<Renderer>().material.color = Color.red;
                moon.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePositionY;
                moon.GetComponent<Rigidbody>().useGravity = true;
                moon.GetComponent<MoonController>().SaveManager = SaveManager.instance;
                //moon.GetComponent<MoonController>().planetPrefab = planets[i];
                moon.GetComponent<MoonController>().OrbitalPeriod = mOrbitalPeriod;
                moon.GetComponent<MoonController>().RotationPeriod = mRotationPeriod;
                moon.GetComponent<MoonController>().AxialTilt = mAxialTilt;
                moon.GetComponent<MoonController>().SurfaceTemperature = mSurfaceTemperature;
                moon.GetComponent<MoonController>().MeanDensity = mMeanDensity;
                moon.GetComponent<MoonController>().SurfaceGravity = mSurfaceGravity;
                moon.GetComponent<MoonController>().EscapeVelocity = mEscapeVelocity;
                moon.GetComponent<MoonController>().Albedo = mAlbedo;
                moon.GetComponent<MoonController>().HasAtmosphere = mHasAtmosphere;
                moon.GetComponent<MoonController>().IsHabitable = mIsHabitable;
                moon.GetComponent<MoonController>().axialTiltMarkerPrefab = axialTiltMarkerPrefab;
                moon.GetComponent<MoonController>().spinDirectionMarkerPrefab = spinDirectionMarkerPrefab;

                // Trail Renderer
                moon.GetComponent<TrailRenderer>().startWidth = 1.0f;
                moon.GetComponent<TrailRenderer>().endWidth = 0.0f;
                moon.GetComponent<TrailRenderer>().time = 3.5f;
                moon.GetComponent<TrailRenderer>().material = new Material(Shader.Find("Sprites/Default"));
                moon.GetComponent<TrailRenderer>().startColor = Color.cyan;
                moon.GetComponent<TrailRenderer>().endColor = Color.clear;

                moons.Add(moon);
            }

            for (int i = 0; i < moons.Count; i++)
            {
                // Store a reference to the planet game object
                GameObject moonObject = moons[i];
                // Set the moon's parent to a random planet from the planets list
                moonObject.transform.parent = planets[Random.Range(0, planets.Count)].transform;
                // Set the moon's position relative to its parent planet
                moonObject.transform.localPosition = new Vector3(Random.Range(minDistance, maxDistance), 0f, Random.Range(minDistance, maxDistance));

                Button moonButton = Instantiate(moonButtonPrefab, scrollViewContent);
                moonButton.GetComponentInChildren<TextMeshProUGUI>().text = moons[i].name;
                moonButton.GetComponentInChildren<TextMeshProUGUI>().rectTransform.anchoredPosition = new Vector2(0.0f, -25.0f);
                moonButton.GetComponentInChildren<TextMeshProUGUI>().fontSize = 12.0f;
                moonButton.GetComponentInChildren<TextMeshProUGUI>().color = Color.cyan;
                moonButton.onClick.AddListener(() => StaticAngledCamera.SetFocus(moonObject));
            }
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
