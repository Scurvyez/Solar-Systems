using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;
using SolarSystem;

public class CelestialGen : MonoBehaviour
{
    public GameObject starPrefab;
    public GameObject planetPrefab;
    public GameObject planetaryRingsPrefab;
    public GameObject axialTiltMarkerPrefab;
    public GameObject spinDirectionMarkerPrefab;
    public StaticAngledCamera StaticAngledCamera;
    
    public Transform scrollViewContent;
    public Button starButtonPrefab;
    public Button planetButtonPrefab;
    public Button moonButtonPrefab;

    public Color starUIColor;
    public Color planetUIColor;
    public Color planetUIColorHabitable;
    public Color moonUIColor;

    public List<GameObject> stars = new();
    public List<GameObject> planets = new();
    public List<GameObject> moons = new();

    public float mOrbitalDistanceX;

    public void Start()
    {
        StarGen();
        PlanetGen();
        //MoonGen();
    }

    private void StarGen()
    {
        string starName = "Star";
        List<GameObject> starObjects = FindObjectsOfType<GameObject>().Where(go => go.name == starName).ToList();

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
        Material testMat = Resources.Load<Material>("Star");

        for (int index = 0; index < SaveManager.instance.activeSave.rockyPlanets.Count; index++)
        {
            RockyPlanet rP = SaveManager.instance.activeSave.rockyPlanets[index];

            float startPosXZ = 300f * (index + 1);
            
            // planet properties
            Vector3 pStartingPos = new(startPosXZ, 0f, startPosXZ);

            // Make a planet prefab and set its comps and props
            GameObject pObject = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            pObject.AddComponent<TrailRenderer>();
            pObject.AddComponent<Rigidbody>();
            pObject.AddComponent<PlanetController>();
            pObject.AddComponent<PlanetInfo>();

            // Populate all components fields
            pObject.name = rP.Name;
            pObject.GetComponent<MeshRenderer>().material = testMat;
            pObject.GetComponent<SphereCollider>().isTrigger = false;
            pObject.GetComponent<Rigidbody>().mass = rP.Mass;
            pObject.GetComponent<Renderer>().material.color = Color.white;
            pObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePositionY;
            pObject.GetComponent<Rigidbody>().useGravity = false;
            pObject.GetComponent<PlanetController>().SaveManager = SaveManager.instance;
            pObject.GetComponent<PlanetController>().starPrefab = starPrefab;
            pObject.GetComponent<PlanetController>().planetaryRingsPrefab = planetaryRingsPrefab;
            pObject.GetComponent<PlanetController>().Radius = rP.Radius;
            pObject.GetComponent<PlanetController>().StartingPosition = pStartingPos;
            pObject.GetComponent<PlanetController>().OrbitalPeriod = rP.OrbitalPeriod;
            pObject.GetComponent<PlanetController>().RotationPeriod = rP.RotationalPeriod;
            pObject.GetComponent<PlanetController>().AxialTilt = rP.AxialTilt;
            pObject.GetComponent<PlanetController>().IsHabitable = rP.IsHabitable;
            pObject.GetComponent<PlanetController>().HasRings = rP.HasRings;
            pObject.GetComponent<PlanetController>().InnerRingRadius = rP.InnerRingRadius;
            pObject.GetComponent<PlanetController>().OuterRingRadius = rP.OuterRingRadius;
            pObject.GetComponent<PlanetController>().axialTiltMarkerPrefab = axialTiltMarkerPrefab;
            pObject.GetComponent<PlanetController>().spinDirectionMarkerPrefab = spinDirectionMarkerPrefab;

            // Planet Info
            pObject.GetComponent<PlanetInfo>().Name = rP.Name;
            pObject.GetComponent<PlanetInfo>().Mass = rP.Mass;
            pObject.GetComponent<PlanetInfo>().RotationalPeriod = rP.RotationalPeriod;
            pObject.GetComponent<PlanetInfo>().OrbitalPeriod = rP.OrbitalPeriod;
            pObject.GetComponent<PlanetInfo>().FocusPoint = rP.FocusPoint;
            pObject.GetComponent<PlanetInfo>().SurfaceTemperature = rP.SurfaceTemperature;
            pObject.GetComponent<PlanetInfo>().SurfacePressure = rP.SurfacePressure;
            pObject.GetComponent<PlanetInfo>().SurfaceGravity = rP.SurfaceGravity;
            pObject.GetComponent<PlanetInfo>().EscapeVelocity = rP.EscapeVelocity;
            pObject.GetComponent<PlanetInfo>().MagneticFieldStrength = rP.MagneticFieldStrength;
            pObject.GetComponent<PlanetInfo>().MeanDensity = rP.MeanDensity;
            pObject.GetComponent<PlanetInfo>().AxialTilt = rP.AxialTilt;
            pObject.GetComponent<PlanetInfo>().HasAtmosphere = rP.HasAtmosphere;
            pObject.GetComponent<PlanetInfo>().IsHabitable = rP.IsHabitable;
            pObject.GetComponent<PlanetInfo>().HasRings = rP.HasRings;
            pObject.GetComponent<PlanetInfo>().Composition = rP.Composition;
            pObject.GetComponent<PlanetInfo>().AtmosphereComposition = rP.AtmosphereComposition;
            
            // Trail Renderer
            pObject.GetComponent<TrailRenderer>().startWidth = 3.0f;
            pObject.GetComponent<TrailRenderer>().endWidth = 0.0f;
            pObject.GetComponent<TrailRenderer>().time = 10f;
            pObject.GetComponent<TrailRenderer>().material = new Material(Shader.Find("Sprites/Default"));
            pObject.GetComponent<TrailRenderer>().startColor = planetUIColor;
            pObject.GetComponent<TrailRenderer>().endColor = Color.clear;

            planets.Add(pObject);
        }

        foreach (GameObject planet in planets)
        {
            GameObject planetObject = planet;
            planetObject.transform.SetParent(starPrefab.transform);

            Button planetButton = Instantiate(planetButtonPrefab, scrollViewContent);
            planetButton.GetComponentInChildren<TextMeshProUGUI>().text = planet.name;
            planetButton.GetComponentInChildren<TextMeshProUGUI>().rectTransform.anchoredPosition = new Vector2(0.0f, -25.0f);
            planetButton.GetComponentInChildren<TextMeshProUGUI>().fontSize = 12.0f;
            
            planetButton.GetComponentInChildren<TextMeshProUGUI>().color = planet.GetComponent<PlanetController>().IsHabitable ? planetUIColorHabitable : planetUIColor;
            planetButton.onClick.AddListener(() => StaticAngledCamera.SetFocus(planetObject));
        }
    }
    
    private void MoonGen()
    {
        // CHANGE THIS LATER
        Material testMat = Resources.Load<Material>("Star");

        foreach (Moon moon in SaveManager.instance.activeSave.moons)
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
            mOrbitalDistanceX = moon.OrbitalDistanceX;

            // Make a moon prefab and set its comps and props
            GameObject mObject = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            mObject.AddComponent<TrailRenderer>();
            mObject.AddComponent<Rigidbody>();
            mObject.AddComponent<MoonController>();

            // Populate all components fields
            mObject.name = pName;
            mObject.transform.localScale = new Vector3(mRadius, mRadius, mRadius);
            mObject.GetComponent<MeshRenderer>().material = testMat;
            mObject.GetComponent<SphereCollider>().isTrigger = true;
            mObject.GetComponent<Rigidbody>().mass = mMass;
            mObject.GetComponent<Renderer>().material.color = Color.white;
            mObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePositionY;
            mObject.GetComponent<Rigidbody>().useGravity = true;
            mObject.GetComponent<MoonController>().SaveManager = SaveManager.instance;
            mObject.GetComponent<MoonController>().planetPrefab = planetPrefab;
            mObject.GetComponent<MoonController>().OrbitalDistanceX = mOrbitalDistanceX;
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
            mObject.GetComponent<TrailRenderer>().time = 0.5f;
            mObject.GetComponent<TrailRenderer>().material = new Material(Shader.Find("Sprites/Default"));
            mObject.GetComponent<TrailRenderer>().startColor = moonUIColor;
            mObject.GetComponent<TrailRenderer>().endColor = Color.clear;

            moons.Add(mObject);
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
