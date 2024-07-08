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
    private int _starPlanetCount;
    private List<RockyPlanet> _rockyPlanets = new ();
    private List<GasGiant> _gasGiants = new ();
    private List<Moon> _moons = new ();
    
    public void Start()
    {
        _star = gameObject.AddComponent<Star>();
        Button.onClick.AddListener(StarCreation);
        Button.onClick.AddListener(PlanetCreation);
        Button.onClick.AddListener(MoonCreation);
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
    
    private void StarCreation()
    {
        StarGenerator.GenerateStar(Index, _star);
        _starPlanetCount = _star.PlanetCount;
    }

    private void PlanetCreation()
    {
        int totalPlanets = _starPlanetCount;
        int rockyPlanetsCount = Mathf.FloorToInt(totalPlanets * 0.7f); // 70% rocky planets
        int gasGiantsCount = totalPlanets - rockyPlanetsCount;

        PlanetGenerator.GeneratePlanets(_rockyPlanets, rockyPlanetsCount);
        PlanetGenerator.GeneratePlanets(_gasGiants, gasGiantsCount);
    }

    private void MoonCreation()
    {
        MoonGenerator.GenerateRockyPlanetMoons(_rockyPlanets, _moons);
    }
}