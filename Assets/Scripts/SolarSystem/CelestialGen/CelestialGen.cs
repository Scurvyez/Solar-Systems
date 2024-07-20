using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using TMPro;
using System.Linq;
using Saving;
using SolarSystem;
using Unity.VisualScripting;

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
    public Color CelestialBodyUIColor;
    public Color StarUIColor;
    public Color RockyPlanetUIColor;
    public Color GasGiantPlanetUIColor;
    public Color PlanetUIColorHabitable;
    public Color MoonUIColor;
    
    public List<GameObject> Stars = new();
    public List<GameObject> Planets = new();
    public List<GameObject> Moons = new();
    
    public PlanetMesh PlanetMesh;
    public PlanetMesh MoonMesh;
    public Material RockyPlanetMat;
    public Material GasGiantPlanetMat;
    public Material MoonMaterial;
    
    private int _moonCountPerPlanet;
    
    public void Start()
    {
        TryGenerateFinalStars();

        PlanetMesh = this.AddComponent<PlanetMesh>();
        MoonMesh = this.AddComponent<PlanetMesh>();
        if (PlanetMesh == null || MoonMesh == null)
        {
            Debug.LogError("PlanetMesh or MoonMesh component not found on CelestialGen.");
            return;
        }
        
        if (Stars == null || Stars.Count == 0) return;
        TryGenerateFinalPlanets();
        
        if (Planets != null && Planets.Count != 0)
        {
            TryGenerateFinalMoons();
        }
    }

    private void TryGenerateFinalStars()
    {
        for (int index = 0; index < 1; index++)
        {
            GameObject starInstance = Instantiate(StarPrefab);
            starInstance.name = "Star";
            Stars.Add(starInstance);
            TryPopulateStarInfo(starInstance);
            TryGenerateStarUIButton(starInstance);
            StaticAngledCamera.SelectedObject = starInstance;
        }
    }

    private void TryGenerateFinalPlanets()
    {
        var savedPlanet = SaveManager.Instance.ActiveSave.planets;
        int layerIndex = 8;
        
        foreach (Planet planetData in savedPlanet)
        {
            GameObject planetInstance;
            Button planetButtonPrefab;
            string planetName = planetData.Info_Name;
            MeshFilter combinedMesh = PlanetMesh.meshFilter;
            
            switch (planetData)
            {
                case RockyPlanet:
                    planetInstance = Instantiate(RockyPlanetPrefab);
                    planetInstance.AddComponent<MeshFilter>().mesh = combinedMesh.mesh;
                    planetInstance.AddComponent<MeshRenderer>().material = RockyPlanetMat;
                    planetInstance.name = planetName;
                    planetButtonPrefab = RockyPlanetButtonPrefab;
                    break;
                case GasGiant:
                    planetInstance = Instantiate(GasGiantPlanetPrefab);
                    planetInstance.AddComponent<MeshFilter>().mesh = combinedMesh.mesh;
                    planetInstance.AddComponent<MeshRenderer>().material = GasGiantPlanetMat;
                    planetInstance.name = planetName;
                    planetButtonPrefab = GasGiantPlanetButtonPrefab;
                    break;
                default:
                    Debug.Log($"[CelestialGen.TryGenerateFinalPlanets] Unknown planet type.");
                    continue;
            }
            
            Planets.Add(planetInstance);
            TryPopulatePlanetInfo(planetInstance, planetData);
            TryGeneratePlanetUIButton(planetInstance, planetButtonPrefab);
            GameObject starObject = GameObject.Find("Star");
            if (starObject == null) return;
            planetInstance.transform.SetParent(starObject.transform);
            
            // Assign a new and unique layer to the planet
            if (layerIndex < 32) // Ensure we do not exceed the maximum number of layers
            {
                string layerName = $"PlanetLayer{layerIndex}";
                planetInstance.GetComponent<PlanetInfo>().LayerName = layerName;
                CreateLayer(layerName);
                planetInstance.layer = LayerMask.NameToLayer(layerName);
                layerIndex++;
            }
            else
            {
                Debug.LogWarning("Maximum number of layers exceeded. Not all planets will have unique layers.");
            }
        }
    }
    
    private static void CreateLayer(string layerName)
    {
        SerializedObject tagManager = new (AssetDatabase.LoadAllAssetsAtPath("ProjectSettings/TagManager.asset")[0]);
        SerializedProperty layersProp = tagManager.FindProperty("layers");

        for (int i = 8; i < layersProp.arraySize; i++)
        {
            SerializedProperty layerSP = layersProp.GetArrayElementAtIndex(i);
            if (layerSP.stringValue == layerName)
            {
                return; // Layer already exists
            }

            if (!string.IsNullOrEmpty(layerSP.stringValue)) continue;
            layerSP.stringValue = layerName;
            tagManager.ApplyModifiedProperties();
            return;
        }

        Debug.LogWarning("Maximum number of layers exceeded. Cannot create new layer.");
    }
    
    private static void TryPopulateStarInfo(GameObject starGO)
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
        starGO.GetComponent<StarInfo>().Variability = SaveManager.Instance.ActiveSave.starVariability;
        starGO.GetComponent<StarInfo>().Composition = SaveManager.Instance.ActiveSave.starMetallicity;
    }

    private void TryGenerateStarUIButton(GameObject starGO)
    {
        Button starButton = Instantiate(StarButtonPrefab, ScrollViewContent);
        
        TextMeshProUGUI nameTextMesh = starButton.transform.Find("TEXT_Name").GetComponent<TextMeshProUGUI>();
        TextMeshProUGUI typeTextMesh = starButton.transform.Find("TEXT_S").GetComponent<TextMeshProUGUI>();
        
        nameTextMesh.text = SaveManager.Instance.ActiveSave.starSystemName;
        nameTextMesh.color = CelestialBodyUIColor;
        nameTextMesh.fontSize = 16.0f;
        typeTextMesh.color = StarUIColor;
        
        starButton.onClick.AddListener(() => StaticAngledCamera.SetFocus(starGO));
        starButton.onClick.AddListener(AudioSource.Play);
    }
    
    private static void TryPopulatePlanetInfo(GameObject planetGO, Planet planetData)
    {
        PlanetInfo planetInfo = planetGO.GetComponent<PlanetInfo>();
        planetInfo.PlanetType = planetData.PlanetType;
        planetInfo.Index = planetData.GO_OrbitPosition;
        planetInfo.Name = planetData.Info_Name;
        planetInfo.Mass = planetData.Info_Mass;
        planetInfo.GO_Radius = planetData.GO_Radius;
        planetInfo.Info_Radius = planetData.Info_Radius;
        planetInfo.RotationalPeriod = planetData.Info_RotationalPeriod;
        planetInfo.OrbitalPeriod = planetData.Info_OrbitalPeriod;
        planetInfo.FocusPoint = planetData.Info_FocusPoint;
        planetInfo.SurfaceTemperature = planetData.Info_SurfaceTemperature;
        planetInfo.SurfacePressure = planetData.Info_SurfacePressure;
        planetInfo.SurfaceGravity = planetData.Info_SurfaceGravity;
        planetInfo.EscapeVelocity = planetData.Info_EscapeVelocity;
        planetInfo.MagneticFieldStrength = planetData.Info_MagneticFieldStrength;
        planetInfo.MeanDensity = planetData.Info_MeanDensity;
        planetInfo.AxialTilt = planetData.Info_AxialTilt;
        planetInfo.HasAtmosphere = planetData.Info_HasAtmosphere;
        planetInfo.IsHabitable = planetData.Info_IsHabitable;
        planetInfo.HasRings = planetData.Info_HasRings;
        planetInfo.InnerRingRadius = planetData.GO_InnerRingRadius;
        planetInfo.OuterRingRadius = planetData.GO_OuterRingRadius;
        planetInfo.Composition = planetData.Info_Composition;
        planetInfo.AtmosphereComposition = planetData.Info_AtmosphereComposition;
    }

    private Color TryGetPlanetUIColor(PlanetInfo planetInfo)
    {
        if (planetInfo.IsHabitable) return PlanetUIColorHabitable;
        
        switch (planetInfo.PlanetType)
        {
            case "RockyPlanet":
                return RockyPlanetUIColor;
            case "GasGiant":
                return GasGiantPlanetUIColor;
            default:
                Debug.Log($"[CelestialGen.TryGenPlanetUIColor] Unknown planet type.");
                return Color.white;
        }
    }
    
    private void TryGeneratePlanetUIButton(GameObject planetGO, Button planetButtonPrefab)
    {
        Button planetButton = Instantiate(planetButtonPrefab, ScrollViewContent);
        
        TextMeshProUGUI nameTextMesh = planetButton.transform.Find("TEXT_Name").GetComponent<TextMeshProUGUI>();
        TextMeshProUGUI typeTextMesh = planetButton.transform.Find("TEXT_P").GetComponent<TextMeshProUGUI>();
        
        nameTextMesh.text = planetGO.name;
        nameTextMesh.color = CelestialBodyUIColor;
        nameTextMesh.fontSize = 16.0f;
        
        PlanetInfo planetInfo = planetGO.GetComponent<PlanetInfo>();
        typeTextMesh.color = TryGetPlanetUIColor(planetInfo);
        
        planetButton.onClick.AddListener(() => StaticAngledCamera.SetFocus(planetGO));
        planetButton.onClick.AddListener(AudioSource.Play);
    }
    
    private void TryGenerateFinalMoons()
    {
        for (int index = 0; index < SaveManager.Instance.ActiveSave.moons.Count; index++)
        {
            Moon moon = SaveManager.Instance.ActiveSave.moons[index];
            MeshFilter combinedMesh = MoonMesh.meshFilter;
            GameObject moonInstance = Instantiate(MoonPrefab);
            moonInstance.AddComponent<MeshFilter>().mesh = combinedMesh.mesh;
            moonInstance.AddComponent<MeshRenderer>().material = MoonMaterial;
            Moons.Add(moonInstance);
            GameObject moonObject = Moons[index];

            TryPopulateMoonInfo(moonObject, moon, index);
            TryGenerateMoonUIButton(moonObject);
            
            GameObject parentPlanet = Planets.FirstOrDefault(p => p.name == moon.ParentPlanetName);
            
            if (parentPlanet == null) continue;
            moonObject.transform.SetParent(parentPlanet.transform);
            PlanetInfo planetInfo = parentPlanet.GetComponent<PlanetInfo>();
            
            if (planetInfo == null) return;
            planetInfo.NumberOfMoons++;
        }
    }

    private static void TryPopulateMoonInfo(GameObject moonGO, Moon moon, int index)
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
        
        TextMeshProUGUI nameTextMesh = moonButton.transform.Find("TEXT_Name").GetComponent<TextMeshProUGUI>();
        TextMeshProUGUI typeTextMesh = moonButton.transform.Find("TEXT_M").GetComponent<TextMeshProUGUI>();
        
        nameTextMesh.text = moonGO.name;
        nameTextMesh.color = CelestialBodyUIColor;
        nameTextMesh.fontSize = 16.0f;
        typeTextMesh.color = MoonUIColor;
        
        moonButton.onClick.AddListener(() => StaticAngledCamera.SetFocus(moonGO));
        moonButton.onClick.AddListener(AudioSource.Play);
    }
}
