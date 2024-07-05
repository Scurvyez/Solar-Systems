using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;
using Saving;
using SolarSystem;
using SolarSystemUI;

public class CelestialGen : MonoBehaviour
{
    public GameObject StarPrefab;
    public GameObject PlanetPrefab;
    public GameObject PlanetaryRingsPrefab;
    public GameObject AxialTiltMarkerPrefab;
    public GameObject SpinDirectionMarkerPrefab;
    public StaticAngledCamera StaticAngledCamera;
    public Transform ScrollViewContent;
    public Button StarButtonPrefab;
    public Button PlanetButtonPrefab;
    public Button MoonButtonPrefab;
    public Color StarUIColor;
    public Color PlanetUIColor;
    public Color PlanetUIColorHabitable;
    public Color MoonUIColor;
    public List<GameObject> Stars = new();
    public List<GameObject> Planets = new();
    public List<GameObject> Moons = new();
    public float MOrbitalDistanceX;
    
    private const string STAR_NAME = "Star";

    public void Start()
    {
        StarGen();
        PlanetGen();
        MoonGen();
    }

    private void StarGen()
    {
        List<GameObject> starObjects = FindObjectsOfType<GameObject>().Where(go => go.name == STAR_NAME).ToList();

        for (int i = 0; i < starObjects.Count; i++)
        {
            string sName = SaveManager.Instance.ActiveSave.starSystemName;
            Stars.Add(StarPrefab);

            // Store a reference to the star game object
            GameObject starObject = Stars[i];

            Button starButton = Instantiate(StarButtonPrefab, ScrollViewContent);
            starButton.GetComponentInChildren<TextMeshProUGUI>().text = sName;
            starButton.GetComponentInChildren<TextMeshProUGUI>().rectTransform.anchoredPosition = new Vector2(0.0f, -25.0f);
            starButton.GetComponentInChildren<TextMeshProUGUI>().fontSize = 12.0f;
            starButton.GetComponentInChildren<TextMeshProUGUI>().color = StarUIColor;

            starButton.onClick.AddListener(() => StaticAngledCamera.SetFocus(starObject));
        }
    }

    private void PlanetGen()
    {
        Material testMat = Resources.Load<Material>(STAR_NAME);

        for (int index = 0; index < SaveManager.Instance.ActiveSave.rockyPlanets.Count; index++)
        {
            RockyPlanet rP = SaveManager.Instance.ActiveSave.rockyPlanets[index];

            float radius = 300f * (index + 1);
            float angle = Random.Range(0f, Mathf.PI * 2);
            float xComponent = Mathf.Cos(angle) * radius;
            float zComponent = Mathf.Sin(angle) * radius;
            
            // planet properties
            Vector3 pStartingPos = new(xComponent, 0f, zComponent);

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
            pObject.GetComponent<PlanetController>().starPrefab = StarPrefab;
            pObject.GetComponent<PlanetController>().planetaryRingsPrefab = PlanetaryRingsPrefab;
            pObject.GetComponent<PlanetController>().Radius = rP.Radius;
            pObject.GetComponent<PlanetController>().StartingPosition = pStartingPos;
            pObject.GetComponent<PlanetController>().OrbitalPeriod = rP.OrbitalPeriod;
            pObject.GetComponent<PlanetController>().RotationPeriod = rP.RotationalPeriod;
            pObject.GetComponent<PlanetController>().AxialTilt = rP.AxialTilt;
            pObject.GetComponent<PlanetController>().IsHabitable = rP.IsHabitable;
            pObject.GetComponent<PlanetController>().HasRings = rP.HasRings;
            pObject.GetComponent<PlanetController>().InnerRingRadius = rP.InnerRingRadius;
            pObject.GetComponent<PlanetController>().OuterRingRadius = rP.OuterRingRadius;
            pObject.GetComponent<PlanetController>().axialTiltMarkerPrefab = AxialTiltMarkerPrefab;
            pObject.GetComponent<PlanetController>().spinDirectionMarkerPrefab = SpinDirectionMarkerPrefab;

            // Planet Info
            pObject.GetComponent<PlanetInfo>().Name = rP.Name;
            pObject.GetComponent<PlanetInfo>().Mass = rP.Mass;
            pObject.GetComponent<PlanetInfo>().Radius = rP.Radius;
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
            pObject.GetComponent<TrailRenderer>().time = 0.5f;
            pObject.GetComponent<TrailRenderer>().material = new Material(Shader.Find("Sprites/Default"));
            pObject.GetComponent<TrailRenderer>().startColor = PlanetUIColor;
            pObject.GetComponent<TrailRenderer>().endColor = Color.clear;

            Planets.Add(pObject);
        }

        foreach (GameObject planet in Planets)
        {
            GameObject planetObject = planet;
            planetObject.transform.SetParent(StarPrefab.transform);

            Button planetButton = Instantiate(PlanetButtonPrefab, ScrollViewContent);
            planetButton.GetComponentInChildren<TextMeshProUGUI>().text = planet.name;
            planetButton.GetComponentInChildren<TextMeshProUGUI>().rectTransform.anchoredPosition = new Vector2(0.0f, -25.0f);
            planetButton.GetComponentInChildren<TextMeshProUGUI>().fontSize = 12.0f;
            
            planetButton.GetComponentInChildren<TextMeshProUGUI>().color = planet.GetComponent<PlanetController>().IsHabitable ? PlanetUIColorHabitable : PlanetUIColor;
            planetButton.onClick.AddListener(() => StaticAngledCamera.SetFocus(planetObject));
        }
    }
    
    private void MoonGen()
    {
        // CHANGE THIS LATER
        Material testMat = Resources.Load<Material>(STAR_NAME);

        foreach (Moon moon in SaveManager.Instance.ActiveSave.moons)
        {
            // Make a moon prefab and set its comps and props
            GameObject mObject = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            mObject.AddComponent<TrailRenderer>();
            mObject.AddComponent<Rigidbody>();
            mObject.AddComponent<MoonController>();

            // Populate all components fields
            mObject.name = moon.Name;
            mObject.GetComponent<MeshRenderer>().material = testMat;
            mObject.GetComponent<SphereCollider>().isTrigger = false;
            mObject.GetComponent<Rigidbody>().mass = moon.Mass;
            mObject.GetComponent<Renderer>().material.color = Color.white;
            mObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePosition;
            mObject.GetComponent<Rigidbody>().useGravity = false;
            mObject.GetComponent<MoonController>().MoonPrefab = PlanetPrefab;
            mObject.GetComponent<MoonController>().Index = moon.Index;
            mObject.GetComponent<MoonController>().Radius = moon.Radius;
            mObject.GetComponent<MoonController>().OrbitalDistanceFrom = moon.OrbitalDistanceFrom;
            mObject.GetComponent<MoonController>().OrbitalPeriod = moon.OrbitalPeriod;
            mObject.GetComponent<MoonController>().RotationPeriod = moon.RotationPeriod;
            mObject.GetComponent<MoonController>().AxialTilt = moon.AxialTilt;
            mObject.GetComponent<MoonController>().SurfaceTemperature = moon.SurfaceTemperature;
            mObject.GetComponent<MoonController>().MeanDensity = moon.MeanDensity;
            mObject.GetComponent<MoonController>().SurfaceGravity = moon.SurfaceGravity;
            mObject.GetComponent<MoonController>().EscapeVelocity = moon.EscapeVelocity;
            mObject.GetComponent<MoonController>().Albedo = moon.Albedo;
            mObject.GetComponent<MoonController>().HasAtmosphere = moon.HasAtmosphere;
            mObject.GetComponent<MoonController>().IsHabitable = moon.IsHabitable;
            mObject.GetComponent<MoonController>().AxialTiltMarkerPrefab = AxialTiltMarkerPrefab;
            mObject.GetComponent<MoonController>().SpinDirectionMarkerPrefab = SpinDirectionMarkerPrefab;

            // Trail Renderer
            mObject.GetComponent<TrailRenderer>().startWidth = 1.0f;
            mObject.GetComponent<TrailRenderer>().endWidth = 0.0f;
            mObject.GetComponent<TrailRenderer>().time = 0.1f;
            mObject.GetComponent<TrailRenderer>().material = new Material(Shader.Find("Sprites/Default"));
            mObject.GetComponent<TrailRenderer>().startColor = MoonUIColor;
            mObject.GetComponent<TrailRenderer>().endColor = Color.clear;

            Moons.Add(mObject);
        }

        for (int i = 0; i < Moons.Count; i++)
        {
            // Store a reference to the moon game object
            GameObject moonObject = Moons[i];

            // Find the parent planet object by name
            string parentPlanetName = moonObject.name;
            foreach (GameObject planetObject in Planets)
            {
                if (planetObject.name == parentPlanetName)
                {
                    // Set the moon's parent to the planet object
                    moonObject.transform.SetParent(planetObject.transform);
                }
            }

            Button moonButton = Instantiate(MoonButtonPrefab, ScrollViewContent);
            moonButton.GetComponentInChildren<TextMeshProUGUI>().text = Moons[i].name;
            moonButton.GetComponentInChildren<TextMeshProUGUI>().rectTransform.anchoredPosition = new Vector2(0.0f, -25.0f);
            moonButton.GetComponentInChildren<TextMeshProUGUI>().fontSize = 12.0f;
            moonButton.GetComponentInChildren<TextMeshProUGUI>().color = MoonUIColor;
            moonButton.onClick.AddListener(() => StaticAngledCamera.SetFocus(moonObject));
        }
    }
}
