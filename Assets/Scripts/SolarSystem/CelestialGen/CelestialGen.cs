using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;
using Saving;
using SolarSystem;

public class CelestialGen : MonoBehaviour
{
    public GameObject StarPrefab;
    public GameObject RockyPlanetPrefab;
    public GameObject GasGiantPlanetPrefab;
    public GameObject MoonPrefab;
    public StaticAngledCamera StaticAngledCamera;
    public AudioSource AudioSource;
    public Transform ScrollViewContent;
    public Button StarButtonPrefab;
    public Button RockyPlanetButtonPrefab;
    public Button GasGiantPlanetButtonPrefab;
    public Button MoonButtonPrefab;
    public Color StarUIColor;
    public Color PlanetUIColor;
    public Color PlanetUIColorHabitable;
    public Color MoonUIColor;
    public List<GameObject> Stars = new();
    public List<GameObject> Planets = new();
    public List<GameObject> Moons = new();

    private int _moonCountPerPlanet;
    
    public void Start()
    {
        TryGenerateFinalStars();
        TryGenerateFinalRockyPlanets();
        TryGenerateFinalGasGiantPlanets();
        TryGenerateFinalMoons();
    }

    private void TryGenerateFinalStars()
    {
        List<GameObject> starObjects = FindObjectsOfType<GameObject>().Where(go => go.name == "Star").ToList();

        if (starObjects.Count == 0)
        {
            GameObject starInstance = Instantiate(StarPrefab);
            Stars.Add(starInstance);
            TryPopulateStarInfo(starInstance);
            TryGenerateStarUIButton(starInstance);
        }
        else
        {
            foreach (GameObject starObject in starObjects)
            {
                TryPopulateStarInfo(starObject);
                TryGenerateStarUIButton(starObject);
            }
        }
    }

    private void TryGenerateFinalRockyPlanets()
    {
        for (int index = 0; index < SaveManager.Instance.ActiveSave.rockyPlanets.Count; index++)
        {
            RockyPlanet rockyPlanet = SaveManager.Instance.ActiveSave.rockyPlanets[index];
            GameObject planetInstance = Instantiate(RockyPlanetPrefab);
            Planets.Add(planetInstance);
            GameObject planetObject = Planets[index];

            TryPopulateRockyPlanetInfo(planetObject, rockyPlanet, index);
            TryGenerateRockyPlanetUIButton(planetObject);
            planetObject.transform.SetParent(StarPrefab.transform);
        }
    }
    
    private void TryGenerateFinalGasGiantPlanets()
    {
        for (int index = 0; index < SaveManager.Instance.ActiveSave.gasGiantPlanets.Count; index++)
        {
            GasGiant gasGiantPlanet = SaveManager.Instance.ActiveSave.gasGiantPlanets[index];
            GameObject planetInstance = Instantiate(GasGiantPlanetPrefab);
            Planets.Add(planetInstance);
            GameObject planetObject = Planets[index];

            TryPopulateGasGiantPlanetInfo(planetObject, gasGiantPlanet, index);
            TryGenerateGasGiantPlanetUIButton(planetObject);
            planetObject.transform.SetParent(StarPrefab.transform);
        }
    }
    
    private void TryGenerateFinalMoons()
    {
        for (int index = 0; index < SaveManager.Instance.ActiveSave.moons.Count; index++)
        {
            Moon moon = SaveManager.Instance.ActiveSave.moons[index];
            GameObject moonInstance = Instantiate(MoonPrefab);
            Moons.Add(moonInstance);
            GameObject moonObject = Moons[index];

            TryPopulateMoonInfo(moonObject, moon, index);
            TryGenerateMoonUIButton(moonObject);
            
            GameObject parentPlanet = Planets.FirstOrDefault(p => p.name == moon.ParentPlanetName); // TODO: change back to ParentPlanetName
            
            if (parentPlanet == null) continue;
            moonObject.transform.SetParent(parentPlanet.transform);
            PlanetInfo planetInfo = parentPlanet.GetComponent<PlanetInfo>();
            
            if (planetInfo == null) return;
            planetInfo.NumberOfMoons++;
        }
    }

    private void TryPopulateStarInfo(GameObject starGO)
    {
        starGO.GetComponent<StarInfo>().Class = SaveManager.Instance.ActiveSave.starClassAsString;
        starGO.GetComponent<StarInfo>().Name = SaveManager.Instance.ActiveSave.starSystemName;
        starGO.GetComponent<StarInfo>().Age = SaveManager.Instance.ActiveSave.starAge;
        starGO.GetComponent<StarInfo>().Mass = SaveManager.Instance.ActiveSave.starMass;
        starGO.GetComponent<StarInfo>().Radius = SaveManager.Instance.ActiveSave.starRadius;
        starGO.GetComponent<StarInfo>().Luminosity = SaveManager.Instance.ActiveSave.starLuminosity;
        starGO.GetComponent<StarInfo>().SurfaceTemperature = SaveManager.Instance.ActiveSave.starTemperature;
        starGO.GetComponent<StarInfo>().RotationalPeriod = SaveManager.Instance.ActiveSave.starRotation;
        starGO.GetComponent<StarInfo>().MagneticFieldStrength = SaveManager.Instance.ActiveSave.starMagneticField;
        starGO.GetComponent<StarInfo>().HasIntrinsicVariability = SaveManager.Instance.ActiveSave.hasIntrinsicVariability;
        starGO.GetComponent<StarInfo>().HasExtrinsicVariability = SaveManager.Instance.ActiveSave.hasExtrinsicVariability;
        starGO.GetComponent<StarInfo>().Variability = SaveManager.Instance.ActiveSave.starVariability;
        starGO.GetComponent<StarInfo>().Composition = SaveManager.Instance.ActiveSave.starMetallicity;
    }

    private void TryGenerateStarUIButton(GameObject starGO)
    {
        Button starButton = Instantiate(StarButtonPrefab, ScrollViewContent);
        starButton.GetComponentInChildren<TextMeshProUGUI>().text = SaveManager.Instance.ActiveSave.starSystemName;
        starButton.GetComponentInChildren<TextMeshProUGUI>().rectTransform.anchoredPosition = new Vector2(0.0f, -25.0f);
        starButton.GetComponentInChildren<TextMeshProUGUI>().fontSize = 12.0f;
        starButton.GetComponentInChildren<TextMeshProUGUI>().color = StarUIColor;
        starButton.onClick.AddListener(() => StaticAngledCamera.SetFocus(starGO));
        starButton.onClick.AddListener(AudioSource.Play);
    }

    private void TryPopulateRockyPlanetInfo(GameObject planetGO, RockyPlanet rockyPlanet, int index)
    {
        planetGO.name = rockyPlanet.Name;
        planetGO.GetComponent<PlanetController>().StarPrefab = StarPrefab;
        planetGO.GetComponent<PlanetInfo>().Index = rockyPlanet.OrbitPosition;
        planetGO.GetComponent<PlanetInfo>().Name = rockyPlanet.Name;
        planetGO.GetComponent<PlanetInfo>().Mass = rockyPlanet.Mass;
        planetGO.GetComponent<PlanetInfo>().Radius = rockyPlanet.Radius;
        planetGO.GetComponent<PlanetInfo>().RotationalPeriod = rockyPlanet.RotationalPeriod;
        planetGO.GetComponent<PlanetInfo>().OrbitalPeriod = rockyPlanet.OrbitalPeriod;
        planetGO.GetComponent<PlanetInfo>().FocusPoint = rockyPlanet.FocusPoint;
        planetGO.GetComponent<PlanetInfo>().SurfaceTemperature = rockyPlanet.SurfaceTemperature;
        planetGO.GetComponent<PlanetInfo>().SurfacePressure = rockyPlanet.SurfacePressure;
        planetGO.GetComponent<PlanetInfo>().SurfaceGravity = rockyPlanet.SurfaceGravity;
        planetGO.GetComponent<PlanetInfo>().EscapeVelocity = rockyPlanet.EscapeVelocity;
        planetGO.GetComponent<PlanetInfo>().MagneticFieldStrength = rockyPlanet.MagneticFieldStrength;
        planetGO.GetComponent<PlanetInfo>().MeanDensity = rockyPlanet.MeanDensity;
        planetGO.GetComponent<PlanetInfo>().AxialTilt = rockyPlanet.AxialTilt;
        planetGO.GetComponent<PlanetInfo>().HasAtmosphere = rockyPlanet.HasAtmosphere;
        planetGO.GetComponent<PlanetInfo>().IsHabitable = rockyPlanet.IsHabitable;
        planetGO.GetComponent<PlanetInfo>().HasRings = rockyPlanet.HasRings;
        planetGO.GetComponent<PlanetInfo>().InnerRingRadius = rockyPlanet.InnerRingRadius;
        planetGO.GetComponent<PlanetInfo>().OuterRingRadius = rockyPlanet.OuterRingRadius;
        planetGO.GetComponent<PlanetInfo>().Composition = rockyPlanet.Composition;
        planetGO.GetComponent<PlanetInfo>().AtmosphereComposition = rockyPlanet.AtmosphereComposition;
    }
    
    private void TryPopulateGasGiantPlanetInfo(GameObject planetGO, GasGiant gasGiantPlanet, int index)
    {
        planetGO.name = gasGiantPlanet.Name;
        planetGO.GetComponent<PlanetController>().StarPrefab = StarPrefab;
        planetGO.GetComponent<PlanetInfo>().Index = gasGiantPlanet.OrbitPosition;
        planetGO.GetComponent<PlanetInfo>().Name = gasGiantPlanet.Name;
        planetGO.GetComponent<PlanetInfo>().Mass = gasGiantPlanet.Mass;
        planetGO.GetComponent<PlanetInfo>().Radius = gasGiantPlanet.Radius;
        planetGO.GetComponent<PlanetInfo>().RotationalPeriod = gasGiantPlanet.RotationalPeriod;
        planetGO.GetComponent<PlanetInfo>().OrbitalPeriod = gasGiantPlanet.OrbitalPeriod;
        planetGO.GetComponent<PlanetInfo>().FocusPoint = gasGiantPlanet.FocusPoint;
        planetGO.GetComponent<PlanetInfo>().SurfaceTemperature = gasGiantPlanet.SurfaceTemperature;
        planetGO.GetComponent<PlanetInfo>().SurfacePressure = gasGiantPlanet.SurfacePressure;
        planetGO.GetComponent<PlanetInfo>().SurfaceGravity = gasGiantPlanet.SurfaceGravity;
        planetGO.GetComponent<PlanetInfo>().EscapeVelocity = gasGiantPlanet.EscapeVelocity;
        planetGO.GetComponent<PlanetInfo>().MagneticFieldStrength = gasGiantPlanet.MagneticFieldStrength;
        planetGO.GetComponent<PlanetInfo>().MeanDensity = gasGiantPlanet.MeanDensity;
        planetGO.GetComponent<PlanetInfo>().AxialTilt = gasGiantPlanet.AxialTilt;
        planetGO.GetComponent<PlanetInfo>().HasAtmosphere = gasGiantPlanet.HasAtmosphere;
        planetGO.GetComponent<PlanetInfo>().IsHabitable = gasGiantPlanet.IsHabitable;
        planetGO.GetComponent<PlanetInfo>().HasRings = gasGiantPlanet.HasRings;
        planetGO.GetComponent<PlanetInfo>().InnerRingRadius = gasGiantPlanet.InnerRingRadius;
        planetGO.GetComponent<PlanetInfo>().OuterRingRadius = gasGiantPlanet.OuterRingRadius;
        planetGO.GetComponent<PlanetInfo>().Composition = gasGiantPlanet.Composition;
        planetGO.GetComponent<PlanetInfo>().AtmosphereComposition = gasGiantPlanet.AtmosphereComposition;
    }

    private void TryGenerateRockyPlanetUIButton(GameObject rockyPlanetGO)
    {
        Button planetButton = Instantiate(RockyPlanetButtonPrefab, ScrollViewContent);
        planetButton.GetComponentInChildren<TextMeshProUGUI>().text = rockyPlanetGO.name;
        planetButton.GetComponentInChildren<TextMeshProUGUI>().rectTransform.anchoredPosition = new Vector2(0.0f, -25.0f);
        planetButton.GetComponentInChildren<TextMeshProUGUI>().fontSize = 12.0f;
        planetButton.GetComponentInChildren<TextMeshProUGUI>().color = rockyPlanetGO.GetComponent<PlanetInfo>().IsHabitable ? PlanetUIColorHabitable : PlanetUIColor;
        planetButton.onClick.AddListener(() => StaticAngledCamera.SetFocus(rockyPlanetGO));
        planetButton.onClick.AddListener(AudioSource.Play);
    }
    
    private void TryGenerateGasGiantPlanetUIButton(GameObject gasGiantPlanetGO)
    {
        Button planetButton = Instantiate(GasGiantPlanetButtonPrefab, ScrollViewContent);
        planetButton.GetComponentInChildren<TextMeshProUGUI>().text = gasGiantPlanetGO.name;
        planetButton.GetComponentInChildren<TextMeshProUGUI>().rectTransform.anchoredPosition = new Vector2(0.0f, -25.0f);
        planetButton.GetComponentInChildren<TextMeshProUGUI>().fontSize = 12.0f;
        planetButton.GetComponentInChildren<TextMeshProUGUI>().color = gasGiantPlanetGO.GetComponent<PlanetInfo>().IsHabitable ? PlanetUIColorHabitable : PlanetUIColor;
        planetButton.onClick.AddListener(() => StaticAngledCamera.SetFocus(gasGiantPlanetGO));
        planetButton.onClick.AddListener(AudioSource.Play);
    }

    private void TryPopulateMoonInfo(GameObject moonGO, Moon moon, int index)
    {
        moonGO.name = moon.Name;
        moonGO.GetComponent<MoonInfo>().ParentPlanetName = moon.ParentPlanetName;
        moonGO.GetComponent<MoonInfo>().Name = moon.Name;
        moonGO.GetComponent<MoonInfo>().Index = index + 1;
        moonGO.GetComponent<MoonInfo>().Radius = moon.Radius;
        moonGO.GetComponent<MoonInfo>().OrbitalPeriod = moon.OrbitalPeriod;
        moonGO.GetComponent<MoonInfo>().RotationalPeriod = moon.RotationPeriod;
        moonGO.GetComponent<MoonInfo>().AxialTilt = moon.AxialTilt;
        moonGO.GetComponent<MoonInfo>().SurfaceTemperature = moon.SurfaceTemperature;
        moonGO.GetComponent<MoonInfo>().MeanDensity = moon.MeanDensity;
        moonGO.GetComponent<MoonInfo>().SurfaceGravity = moon.SurfaceGravity;
        moonGO.GetComponent<MoonInfo>().EscapeVelocity = moon.EscapeVelocity;
        moonGO.GetComponent<MoonInfo>().HasAtmosphere = moon.HasAtmosphere;
        moonGO.GetComponent<MoonInfo>().IsHabitable = moon.IsHabitable;
    }

    private void TryGenerateMoonUIButton(GameObject moonGO)
    {
        Button moonButton = Instantiate(MoonButtonPrefab, ScrollViewContent);
        moonButton.GetComponentInChildren<TextMeshProUGUI>().text = moonGO.name;
        moonButton.GetComponentInChildren<TextMeshProUGUI>().rectTransform.anchoredPosition = new Vector2(0.0f, -25.0f);
        moonButton.GetComponentInChildren<TextMeshProUGUI>().fontSize = 12.0f;
        moonButton.GetComponentInChildren<TextMeshProUGUI>().color = MoonUIColor;
        moonButton.onClick.AddListener(() => StaticAngledCamera.SetFocus(moonGO));
        moonButton.onClick.AddListener(AudioSource.Play);
    }
}
