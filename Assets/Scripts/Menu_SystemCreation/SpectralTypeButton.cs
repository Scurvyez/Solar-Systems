using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using SolarSystem;
using Utils;

public class SpectralTypeButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public int Index;
    public Button Button;
    public StarDescriptions StarDescriptions;
    public TextMeshProUGUI StarDescriptionText;
    public Vector3 DefaultScale = new (1f, 1f, 1f);
    public Vector3 HighlightScale = new (2.0f, 2.0f, 2.0f);
    
    public Star _star;
    private bool _isMouseOver = false;
    private List<Planet> _planets = new ();
    private List<Moon> _moons = new ();
    private int _currentPlanetNumber = 1;

    public void Start()
    {
        Button.onClick.AddListener(CelestialCreation);
    }
    
    private void CelestialCreation()
    {
        if (_star != null)
        {
            Destroy(_star);
        }
        
        _star = gameObject.AddComponent<Star>();
        StarGenerator.GenerateStar(Index, _star);
        PlanetGenerator.CurrentOrbitPosition = 25;
        
        int totalPlanets = _star.PlanetCount;
        float rockyPlanetsRatio = PlanetRatioUtil.GetRockyPlanetsRatio(_star.SpectralClass);
        int rockyPlanetsCount = Mathf.FloorToInt(totalPlanets * rockyPlanetsRatio);
        int gasGiantsCount = totalPlanets - rockyPlanetsCount;
        
        _planets.Clear();
        _moons.Clear();
        _currentPlanetNumber = 1;

        List<Planet> rockyPlanets = GeneratePlanetList<RockyPlanet>(rockyPlanetsCount);
        List<Planet> gasGiants = GeneratePlanetList<GasGiant>(gasGiantsCount);
        List<Planet> allPlanets = new List<Planet>(rockyPlanets);
        
        allPlanets.AddRange(gasGiants);
        ShufflePlanets(allPlanets);
        GeneratePlanets(allPlanets);
        PlanetGenerator.SavePlanetData(_planets);
        
        GenerateMoons();
        MoonGenerator.SaveMoonData(_moons);
    }
    
    private List<Planet> GeneratePlanetList<T>(int planetCount) where T : Planet, new()
    {
        List<Planet> planets = new List<Planet>();
        for (int i = 0; i < planetCount; i++)
        {
            T planet = new T
            {
                Info_Name = SystemNamingUtil.Info_SystemName + "-" + RomanNumConverter.ToRomanNumeral(_currentPlanetNumber),
                PlanetType = typeof(T).Name
            };

            planets.Add(planet);
            _currentPlanetNumber++;
        }
        return planets;
    }

    private static void ShufflePlanets(List<Planet> planets)
    {
        System.Random rng = new ();
        int n = planets.Count;
        while (n > 1)
        {
            int k = rng.Next(n);
            n--;
            (planets[n], planets[k]) = (planets[k], planets[n]);
        }
    }
    
    private void GeneratePlanets(List<Planet> planets)
    {
        foreach (Planet planet in planets)
        {
            planet.GO_OrbitPosition = PlanetGenerator.CurrentOrbitPosition;
            PlanetGenerator.CurrentOrbitPosition += 15;
            PlanetGenerator.GeneratePlanetData(planet);
            _planets.Add(planet);
        }
    }
    
    private void GenerateMoons()
    {
        foreach (Planet planet in _planets)
        {
            int moonCount = planet.Info_Moons;
            for (int i = 0; i < moonCount; i++)
            {
                Moon moon = new ()
                {
                    Info_Name = planet.Info_Name + "-" + (i + 1),
                    Info_ParentPlanetName = planet.Info_Name
                };
                MoonGenerator.GenerateMoonData(moon);
                _moons.Add(moon);
            }
        }
    }

    public void Update()
    {
        transform.localScale = Vector3.Lerp(transform.localScale, _isMouseOver ? HighlightScale : DefaultScale, ConstantsUtil.BUTTON_SCALE_LERP_SPEED);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        _isMouseOver = true;
        Star.SpectralType spectralType = (Star.SpectralType)Index;
        StarDescriptionText.text = StarDescriptions.StarDescription[spectralType];
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        _isMouseOver = false;
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
}
