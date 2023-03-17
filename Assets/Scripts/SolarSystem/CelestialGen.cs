using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;
using System.Linq;
using UnityEngine.Rendering.Universal;

public class CelestialGen : MonoBehaviour
{
    public GameObject starPrefab;
    public GameObject axialTiltMarkerPrefab;
    public GameObject spinDirectionMarkerPrefab;
    public StaticAngledCamera StaticAngledCamera;

    public Transform scrollViewContent;
    public Button starButtonPrefab;
    public Button planetButtonPrefab;
    public Button moonButtonPrefab;

    private float minDistance = 1.0f;
    private float maxDistance = 5.75f;

    public Color starUIColor;
    public Color planetUIColor;
    public Color planetUIColorHabitable;
    public Color moonUIColor;

    public List<GameObject> stars = new();
    public List<GameObject> planets = new();
    public List<GameObject> moons = new();

    public void Start()
    {
        StarGen();
        PlanetGen();
        MoonGen();
    }

    private void StarGen()
    {
        string starName = "Star";
        List<GameObject> starObjects = GameObject.FindObjectsOfType<GameObject>().Where(go => go.name == starName).ToList();

        for (int i = 0; i < starObjects.Count; i++)
        {
            string sName = SaveManager.instance.activeSave.starSystemName;
            stars.Add(starPrefab);

            // Store a reference to the star game object
            GameObject starObject = stars[i];

            Button starButton = Instantiate(starButtonPrefab, scrollViewContent);
            starButton.GetComponentInChildren<TextMeshProUGUI>().text = sName;
            starButton.GetComponentInChildren<TextMeshProUGUI>().rectTransform.anchoredPosition = new Vector2(0.0f, -25.0f);
            starButton.GetComponentInChildren<TextMeshProUGUI>().fontSize = 12.0f;
            starButton.GetComponentInChildren<TextMeshProUGUI>().color = starUIColor;

            starButton.onClick.AddListener(() => StaticAngledCamera.SetFocus(starObject));
        }
    }

    private void PlanetGen()
    {
        // CHANGE THIS LATER
        // DIFF TEXTURES FOR DIFF PLANET TYPES
        Material testMat = Resources.Load<Material>("Star");

        for (int i = 0; i < SaveManager.instance.activeSave.rockyPlanets.Count; i++)
        {
            // planet properties
            string pName = SaveManager.instance.activeSave.rockyPlanets[i].Name;
            float pMass = SaveManager.instance.activeSave.rockyPlanets[i].Mass;
            float pRadius = SaveManager.instance.activeSave.rockyPlanets[i].Radius;
            float pRotationPeriod = SaveManager.instance.activeSave.rockyPlanets[i].RotationPeriod;
            float pOrbitalPeriod = SaveManager.instance.activeSave.rockyPlanets[i].OrbitalPeriod;
            float pSemiMajorAxis = SaveManager.instance.activeSave.rockyPlanets[i].SemiMajorAxis;
            float pEccentricity = SaveManager.instance.activeSave.rockyPlanets[i].Eccentricity;
            Vector3 pFocusPoint = SaveManager.instance.activeSave.rockyPlanets[i].FocusPoint;
            float pSemiMinorAxis = SaveManager.instance.activeSave.rockyPlanets[i].SemiMinorAxis;
            float pAxialTilt = SaveManager.instance.activeSave.rockyPlanets[i].AxialTilt;
            bool pIsHabitable = SaveManager.instance.activeSave.rockyPlanets[i].IsHabitable;
            List<Moon> pMoons = SaveManager.instance.activeSave.rockyPlanets[i].Moons;

            // Make a rocky planet prefab and set its comps and props
            GameObject planet = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            planet.AddComponent<TrailRenderer>();
            planet.AddComponent<Rigidbody>();
            planet.AddComponent<PlanetController>();

            // Populate all components fields
            planet.name = pName;
            planet.transform.localScale = new Vector3(pRadius, pRadius, pRadius);
            planet.transform.localPosition = pFocusPoint;
            planet.GetComponent<MeshRenderer>().material = testMat;
            planet.GetComponent<SphereCollider>().isTrigger = true;
            planet.GetComponent<Rigidbody>().mass = pMass;
            planet.GetComponent<Renderer>().material.color = Color.green;
            planet.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePositionY;
            planet.GetComponent<Rigidbody>().useGravity = true;
            planet.GetComponent<PlanetController>().SaveManager = SaveManager.instance;
            planet.GetComponent<PlanetController>().starPrefab = starPrefab;
            planet.GetComponent<PlanetController>().RotationPeriod = pRotationPeriod;
            planet.GetComponent<PlanetController>().OrbitalPeriod = pOrbitalPeriod;
            planet.GetComponent<PlanetController>().SemiMajorAxis = pSemiMajorAxis;
            planet.GetComponent<PlanetController>().FocusPoint = pFocusPoint;
            planet.GetComponent<PlanetController>().Eccentricity = pEccentricity;
            planet.GetComponent<PlanetController>().SemiMinorAxis = pSemiMinorAxis;
            planet.GetComponent<PlanetController>().AxialTilt = pAxialTilt;
            planet.GetComponent<PlanetController>().IsHabitable = pIsHabitable;
            planet.GetComponent<PlanetController>().axialTiltMarkerPrefab = axialTiltMarkerPrefab;
            planet.GetComponent<PlanetController>().spinDirectionMarkerPrefab = spinDirectionMarkerPrefab;

            // Trail Renderer
            planet.GetComponent<TrailRenderer>().startWidth = 30.0f;
            planet.GetComponent<TrailRenderer>().endWidth = 0.0f;
            planet.GetComponent<TrailRenderer>().time = 100.0f;
            planet.GetComponent<TrailRenderer>().material = testMat;
            if (planet.GetComponent<PlanetController>().IsHabitable)
            {
                planet.GetComponent<TrailRenderer>().startColor = planetUIColorHabitable;
            }
            else
            {
                planet.GetComponent<TrailRenderer>().startColor = planetUIColor;
            }
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
            if (planets[i].GetComponent<PlanetController>().IsHabitable == true)
            {
                planetButton.GetComponentInChildren<TextMeshProUGUI>().color = planetUIColorHabitable;
            }
            else
            {
                planetButton.GetComponentInChildren<TextMeshProUGUI>().color = planetUIColor;
            }

            planetButton.onClick.AddListener(() => StaticAngledCamera.SetFocus(planetObject));
        }
    }
    
    private void MoonGen()
    {
        // CHANGE THIS LATER
        Material testMat = Resources.Load<Material>("Star");

        foreach (RockyPlanet rP in SaveManager.instance.activeSave.rockyPlanets)
        {
            if (rP.Moons.Count > 0)
            {
                foreach (Moon moon in rP.Moons)
                {
                    // moon properties
                    string pName = moon.Name;
                    float mMass = moon.Mass;
                    float mRadius = moon.Radius;
                    float mOrbitalPeriod = moon.OrbitalPeriod;
                    float mRotationPeriod = moon.RotationPeriod;
                    float mAxialTilt = moon.AxialTilt;
                    float mSurfaceTemperature = moon.SurfaceTemperature;
                    float mMeanDensity = moon.MeanDensity;
                    float mSurfaceGravity = moon.SurfaceGravity;
                    float mEscapeVelocity = moon.EscapeVelocity;
                    float mAlbedo = moon.Albedo;
                    bool mHasAtmosphere = moon.HasAtmosphere;
                    bool mIsHabitable = moon.IsHabitable;

                    // Make a moon prefab and set its comps and props
                    GameObject mObject = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                    mObject.AddComponent<TrailRenderer>();
                    mObject.AddComponent<Rigidbody>();
                    mObject.AddComponent<MoonController>();

                    // Populate all components fields
                    mObject.name = pName;
                    mObject.transform.localScale = new Vector3(mRadius, mRadius, mRadius);
                    mObject.transform.localPosition = new Vector3(mOrbitalPeriod, 0, 0);
                    mObject.GetComponent<MeshRenderer>().material = testMat;
                    mObject.GetComponent<SphereCollider>().isTrigger = true;
                    mObject.GetComponent<Rigidbody>().mass = mMass;
                    mObject.GetComponent<Renderer>().material.color = Color.white;
                    mObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePositionY;
                    mObject.GetComponent<Rigidbody>().useGravity = true;
                    mObject.GetComponent<MoonController>().SaveManager = SaveManager.instance;
                    mObject.GetComponent<MoonController>().OrbitalPeriod = mOrbitalPeriod;
                    mObject.GetComponent<MoonController>().RotationPeriod = mRotationPeriod;
                    mObject.GetComponent<MoonController>().AxialTilt = mAxialTilt;
                    mObject.GetComponent<MoonController>().SurfaceTemperature = mSurfaceTemperature;
                    mObject.GetComponent<MoonController>().MeanDensity = mMeanDensity;
                    mObject.GetComponent<MoonController>().SurfaceGravity = mSurfaceGravity;
                    mObject.GetComponent<MoonController>().EscapeVelocity = mEscapeVelocity;
                    mObject.GetComponent<MoonController>().Albedo = mAlbedo;
                    mObject.GetComponent<MoonController>().HasAtmosphere = mHasAtmosphere;
                    mObject.GetComponent<MoonController>().IsHabitable = mIsHabitable;
                    mObject.GetComponent<MoonController>().axialTiltMarkerPrefab = axialTiltMarkerPrefab;
                    mObject.GetComponent<MoonController>().spinDirectionMarkerPrefab = spinDirectionMarkerPrefab;

                    // Trail Renderer
                    mObject.GetComponent<TrailRenderer>().startWidth = 1.0f;
                    mObject.GetComponent<TrailRenderer>().endWidth = 0.0f;
                    mObject.GetComponent<TrailRenderer>().time = 2.5f;
                    mObject.GetComponent<TrailRenderer>().material = new Material(Shader.Find("Sprites/Default"));
                    mObject.GetComponent<TrailRenderer>().startColor = moonUIColor;
                    mObject.GetComponent<TrailRenderer>().endColor = Color.clear;

                    moons.Add(mObject);
                }
            }
        }

        for (int i = 0; i < moons.Count; i++)
        {
            // Store a reference to the moon game object
            GameObject moonObject = moons[i];

            // Find the parent planet object by name
            string parentPlanetName = moonObject.name;
            foreach (GameObject planetObject in planets)
            {
                if (planetObject.name == parentPlanetName)
                {
                    // Set the moon's parent to the planet object
                    moonObject.transform.SetParent(planetObject.transform);

                    // Set the moon's position relative to its parent planet
                    moonObject.transform.localPosition = new Vector3(Random.Range(minDistance, maxDistance), 0f, Random.Range(minDistance, maxDistance));
                }
            }

            Button moonButton = Instantiate(moonButtonPrefab, scrollViewContent);
            moonButton.GetComponentInChildren<TextMeshProUGUI>().text = moons[i].name;
            moonButton.GetComponentInChildren<TextMeshProUGUI>().rectTransform.anchoredPosition = new Vector2(0.0f, -25.0f);
            moonButton.GetComponentInChildren<TextMeshProUGUI>().fontSize = 12.0f;
            moonButton.GetComponentInChildren<TextMeshProUGUI>().color = moonUIColor;
            moonButton.onClick.AddListener(() => StaticAngledCamera.SetFocus(moonObject));
        }
    }
}
