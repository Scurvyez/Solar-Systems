using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;

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

        foreach (RockyPlanet rP in SaveManager.instance.activeSave.rockyPlanets)
        {
            // planet properties
            string pName = rP.Name;
            float pMass = rP.Mass;
            float pRadius = rP.Radius;
            Vector3 pStartingPos = rP.StartingPosition;
            float pOrbitalPeriod = rP.OrbitalPeriod;
            float pRotationPeriod = rP.RotationPeriod;
            float pAxialTilt = rP.AxialTilt;
            float pSurfaceTemperature = rP.SurfaceTemperature;
            float pMeanDensity = rP.MeanDensity;
            float pSurfaceGravity = rP.SurfaceGravity;
            float pEscapeVelocity = rP.EscapeVelocity;
            float pAlbedo = rP.Albedo;
            bool pHasAtmosphere = rP.HasAtmosphere;
            bool pIsHabitable = rP.IsHabitable;
            bool pHasRings = rP.HasRings;
            float pInnerRingRadius = rP.InnerRingRadius;
            float pOuterRingRadius = rP.OuterRingRadius;

            // Make a planet prefab and set its comps and props
            GameObject pObject = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            pObject.AddComponent<TrailRenderer>();
            pObject.AddComponent<Rigidbody>();
            pObject.AddComponent<PlanetController>();

            // Populate all components fields
            pObject.name = pName;
            pObject.transform.localScale = new Vector3(pRadius, pRadius, pRadius);
            pObject.transform.localPosition = new Vector3(pStartingPos.x, pStartingPos.y, pStartingPos.z);
            pObject.GetComponent<MeshRenderer>().material = testMat;
            pObject.GetComponent<SphereCollider>().isTrigger = true;
            pObject.GetComponent<Rigidbody>().mass = pMass;
            pObject.GetComponent<Renderer>().material.color = Color.white;
            pObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePositionY;
            pObject.GetComponent<Rigidbody>().useGravity = true;
            pObject.GetComponent<PlanetController>().SaveManager = SaveManager.instance;
            pObject.GetComponent<PlanetController>().starPrefab = starPrefab;
            pObject.GetComponent<PlanetController>().planetaryRingsPrefab = planetaryRingsPrefab;
            pObject.GetComponent<PlanetController>().Radius = pRadius;
            pObject.GetComponent<PlanetController>().StartingPosition = pStartingPos;
            pObject.GetComponent<PlanetController>().OrbitalPeriod = pOrbitalPeriod;
            pObject.GetComponent<PlanetController>().RotationPeriod = pRotationPeriod;
            pObject.GetComponent<PlanetController>().AxialTilt = pAxialTilt;
            pObject.GetComponent<PlanetController>().IsHabitable = pIsHabitable;
            pObject.GetComponent<PlanetController>().HasRings = pHasRings;
            pObject.GetComponent<PlanetController>().InnerRingRadius = pInnerRingRadius;
            pObject.GetComponent<PlanetController>().OuterRingRadius = pOuterRingRadius;
            pObject.GetComponent<PlanetController>().axialTiltMarkerPrefab = axialTiltMarkerPrefab;
            pObject.GetComponent<PlanetController>().spinDirectionMarkerPrefab = spinDirectionMarkerPrefab;

            // Trail Renderer
            pObject.GetComponent<TrailRenderer>().startWidth = 3.0f;
            pObject.GetComponent<TrailRenderer>().endWidth = 0.0f;
            pObject.GetComponent<TrailRenderer>().time = 10f;
            pObject.GetComponent<TrailRenderer>().material = new Material(Shader.Find("Sprites/Default"));
            pObject.GetComponent<TrailRenderer>().startColor = planetUIColor;
            pObject.GetComponent<TrailRenderer>().endColor = Color.clear;

            planets.Add(pObject);
        }

        for (int i = 0; i < planets.Count; i++)
        {
            // Store a reference to the planet game object
            GameObject planetObject = planets[i];
            planetObject.transform.SetParent(starPrefab.transform);

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
