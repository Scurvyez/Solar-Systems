using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Utils;
using Random = UnityEngine.Random;

public class SpectralTypeButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public StarProperties StarProperties;
    public StarDescriptions StarDescriptions;
    public int Index;
    public Button Button;
    public TextMeshProUGUI StarDescriptionText;
	public Vector3 DefaultScale = new (1f, 1f, 1f);
    public Vector3 HighlightScale = new (2.0f, 2.0f, 2.0f);
    private bool IsMouseOver = false;
    private const float SCALE_LERP_SPEED = 0.025f;
    public string starClass;
    private RomanNumConverter romanNumConverter;

    public List<RockyPlanet> planets = new ();
    public List<Moon> moons = new ();

    public void OnPointerEnter(PointerEventData eventData)
    {
        IsMouseOver = true;
        StarProperties.SpectralType spectralType = (StarProperties.SpectralType)Index;
        StarDescriptionText.text = StarDescriptions.StarDescription[spectralType];
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        IsMouseOver = false;
        StarDescriptionText.text = "";
    }

    private IEnumerator ScaleButton(Transform button, Vector3 startScale, Vector3 endScale, float duration)
    {
        float startTime = Time.deltaTime;
        while (Time.deltaTime < startTime + duration)
        {
            button.localScale = Vector3.Lerp(startScale, endScale, (Time.time - startTime) / duration);
            yield return null;
        }
        button.localScale = endScale;
    }

    public void Update()
    {
        transform.localScale = Vector3.Lerp(transform.localScale, IsMouseOver ? HighlightScale : DefaultScale, SCALE_LERP_SPEED);
    }

    public void Start()
    {
        romanNumConverter = new RomanNumConverter();
        Button.onClick.AddListener(PlaySound);
        Button.onClick.AddListener(GenerateStar);
        Button.onClick.AddListener(GeneratePlanets);
        Button.onClick.AddListener(GenerateMoons);
    }

    private static void PlaySound()
    {
        FindObjectOfType<AudioManager>().Play("MenuButtonClick");
    }

    private void GenerateStar()
    {
        if (StarProperties == null)
        {
            Debug.LogError("StarPhysicalProperties is null");
            return;
        }

        GrabFinalizedStarData();

        if (SaveManager.instance.hasLoaded)
        {
            SystemNamingUtil.Info_SystemName = SaveManager.instance.activeSave.starSystemName;
            starClass = SaveManager.instance.activeSave.starClassAsString;

            StarProperties.Info_Age = SaveManager.instance.activeSave.starAge;
            StarProperties.Info_Radius = SaveManager.instance.activeSave.starRadius;
            StarProperties.Info_Temperature = SaveManager.instance.activeSave.starTemperature;
            StarProperties.Info_Rotation = SaveManager.instance.activeSave.starRotation;
            StarProperties.Info_MagneticField = SaveManager.instance.activeSave.starMagneticField;
            StarProperties.Info_Metallicity = SaveManager.instance.activeSave.starMetallicity;
            StarProperties.Info_Variability = SaveManager.instance.activeSave.starVariability;
            StarProperties.Info_Luminosity = SaveManager.instance.activeSave.starLuminosity;
            StarProperties.Info_Mass = SaveManager.instance.activeSave.starMass;
            
            StarProperties.ExtrinsicVariability = SaveManager.instance.activeSave.hasExtrinsicVariability;
            StarProperties.IntrinsicVariability = SaveManager.instance.activeSave.hasIntrinsicVariability;
            
            StarProperties.GO_Size = SaveManager.instance.activeSave.starSize;
            StarProperties.GO_Position = SaveManager.instance.activeSave.starPosition;
            StarProperties.GO_Radius = SaveManager.instance.activeSave.starActualRadius;
            StarProperties.GO_Chromaticity = SaveManager.instance.activeSave.starChromaticity;
            StarProperties.GO_CellColor = SaveManager.instance.activeSave.starCellColor;
            StarProperties.GO_HabitableRangeInner = SaveManager.instance.activeSave.habitableRangeInner;
            StarProperties.GO_HabitableRangeOuter = SaveManager.instance.activeSave.habitableRangeOuter;
        }
        else
        {
            SaveManager.instance.activeSave.starSystemName = SystemNamingUtil.Info_SystemName;
            SaveManager.instance.activeSave.starClassAsString = DetermineStarClass();

            SaveManager.instance.activeSave.starAge = StarProperties.Info_Age;
            SaveManager.instance.activeSave.starRadius = StarProperties.Info_Radius;
            SaveManager.instance.activeSave.starTemperature = StarProperties.Info_Temperature;
            SaveManager.instance.activeSave.starRotation = StarProperties.Info_Rotation;
            SaveManager.instance.activeSave.starMagneticField = StarProperties.Info_MagneticField;
            SaveManager.instance.activeSave.starMetallicity = StarProperties.Info_Metallicity;
            SaveManager.instance.activeSave.starVariability = StarProperties.Info_Variability;
            SaveManager.instance.activeSave.starLuminosity = StarProperties.Info_Luminosity;
            SaveManager.instance.activeSave.starMass = StarProperties.Info_Mass;
            
            SaveManager.instance.activeSave.hasExtrinsicVariability = StarProperties.ExtrinsicVariability;
            SaveManager.instance.activeSave.hasIntrinsicVariability = StarProperties.IntrinsicVariability;
            
            SaveManager.instance.activeSave.starSize = StarProperties.GO_Size;
            SaveManager.instance.activeSave.starPosition = StarProperties.GO_Position;
            SaveManager.instance.activeSave.starActualRadius = StarProperties.GO_Radius;
            SaveManager.instance.activeSave.starChromaticity = StarProperties.GO_Chromaticity;
            SaveManager.instance.activeSave.starCellColor = StarProperties.GO_CellColor;
            SaveManager.instance.activeSave.habitableRangeInner = StarProperties.GO_HabitableRangeInner;
            SaveManager.instance.activeSave.habitableRangeOuter = StarProperties.GO_HabitableRangeOuter;
        }
    }

    private void GeneratePlanets()
    {
        GrabFinalizedPlanetData();

        if (SaveManager.instance.hasLoaded)
        {
            foreach (RockyPlanet t in SaveManager.instance.activeSave.rockyPlanets)
            {
                planets = SaveManager.instance.activeSave.rockyPlanets;
            }
        }
        else
        {
            SaveManager.instance.activeSave.rockyPlanets = planets;
        }
    }

    private void GenerateMoons()
    {
        GrabFinalizedMoonData();

        if (SaveManager.instance.hasLoaded)
        {
            foreach (Moon t in SaveManager.instance.activeSave.moons)
            {
                moons = SaveManager.instance.activeSave.moons;
            }
        }
        else
        {
            SaveManager.instance.activeSave.moons = moons;
        }
    }

    private string DetermineStarClass()
    {
        StarProperties.SpectralType spectralType = StarProperties.SpectralClass;
        starClass = spectralType switch
        {
            StarProperties.SpectralType.O => spectralType.ToString(),
            StarProperties.SpectralType.B => spectralType.ToString(),
            StarProperties.SpectralType.A => spectralType.ToString(),
            StarProperties.SpectralType.F => spectralType.ToString(),
            StarProperties.SpectralType.G => spectralType.ToString(),
            StarProperties.SpectralType.K => spectralType.ToString(),
            StarProperties.SpectralType.M => spectralType.ToString(),
            _ => starClass
        };
        return starClass;
    }

    private void GrabFinalizedStarData()
    {
        StarProperties.SpectralType spectralType = (StarProperties.SpectralType)Index;
        StarProperties.SpectralClass = spectralType;

        SystemNamingUtil.PickNamingMethodAndGenerate();
        
        StarProperties.GenerateInfoAge(spectralType);
        StarProperties.GenerateInfoRadius(spectralType);
        StarProperties.GenerateInfoTemperature(spectralType);
        StarProperties.GenerateInfoRotation(spectralType);
        StarProperties.GenerateInfoMagneticField(spectralType);
        StarProperties.Info_Metallicity = new SerializableDictionary<string, float>();
        StarProperties.GenerateInfoMetallicity(spectralType);
        StarProperties.GenerateInfoIntrinsicVariability(spectralType);
        StarProperties.GenerateInfoLuminosity(StarProperties.Info_Radius, StarProperties.Info_Temperature);
        StarProperties.GenerateInfoMass(StarProperties.Info_Luminosity);
        
        StarProperties.GenerateGOStartingPosition();
        StarProperties.GenerateGORadius();
        StarProperties.GenerateGOChromaticity(spectralType);
        StarProperties.GenerateGOCellColor();
        StarProperties.GenerateGOHabitableRangeInner(spectralType);
        StarProperties.GenerateGOHabitableRangeOuter(spectralType);
    }

    private void GrabFinalizedPlanetData()
    {
        SystemSaveData saveData = SaveManager.instance.activeSave;
        int numPlanets = Random.Range(1, 13); // generate a random number of planets between 1 and 12

        for (int i = 0; i < numPlanets; i++)
        {
            // create a new rocky planet object
            RockyPlanet rP = new ()
            {
                Name = SystemNamingUtil.Info_SystemName + "-" + RomanNumConverter.ToRomanNumeral(i + 1)
            };
            
            rP.GenerateGORadius();
            
            rP.HasRandomRings();
            rP.HasRandomAtmosphere();
            rP.AtmosphereHeight(rP.HasAtmosphere, rP.Radius);
            rP.GenerateMagneticFieldStrength();
            rP.GetColor();
            rP.GenerateAlbedo();
            rP.GenerateOrbitalPeriod();
            rP.GenerateFocusPoint();
            rP.GenerateRotationalPeriod();
            rP.GenerateAxialTilt();
            rP.GenerateSurfacePressure();
            rP.GenerateMass(saveData.starMass, rP.FocusPoint, rP.OrbitalPeriod);
            rP.GenerateSurfaceGravity(rP.Mass, rP.Radius);
            rP.GenerateEscapeVelocity(rP.Mass, rP.Radius);
            rP.GenerateSurfaceTemperature();
            rP.HasLiquidWater(rP.SurfaceTemperature, rP.SurfacePressure, rP.HasAtmosphere);
            rP.IsRandomlyHabitable(saveData.habitableRangeInner, saveData.habitableRangeOuter, rP.FocusPoint);
            rP.GenerateAtmosphereComposition(rP.HasAtmosphere, saveData.habitableRangeInner, saveData.habitableRangeOuter, rP.SurfaceTemperature);
            rP.GenerateComposition();
            rP.GenerateMeanDensity(rP.Composition);
            
            rP.GenerateInnerRingRadius();
            rP.GenerateOuterRingRadius();
            
            planets.Add(rP);
        }
    }

    private void GrabFinalizedMoonData()
    {
        foreach (RockyPlanet planet in planets)
        {
            int numMoons = Random.Range(0, 4); // generate a random number of moons for each planet

            for (int i = 0; i < numMoons; i++)
            {
                Moon m = new ()
                {
                    Name = planet.Name
                };
                m.GenerateMass();
                m.GenerateRadius();
                m.GenerateOrbitalPeriod();
                m.GenerateOrbitalDistanceX();
                m.GenerateRotationPeriod();
                m.GenerateAxialTilt();
                m.GenerateSurfaceTemperature();
                m.HasRandomAtmosphere();
                m.IsRandomlyHabitable();
                m.GenerateMeanDensity();
                m.GenerateSurfaceGravity();
                m.GenerateEscapeVelocity();
                m.GenerateAlbedo();
                moons.Add(m);
            }
        }
    }
}
