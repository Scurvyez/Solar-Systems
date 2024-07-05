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
    public StarProperties StarProperties;
    public StarDescriptions StarDescriptions;
    public int Index;
    public Button Button;
    public TextMeshProUGUI StarDescriptionText;
	public Vector3 DefaultScale = new (1f, 1f, 1f);
    public Vector3 HighlightScale = new (2.0f, 2.0f, 2.0f);
    public List<RockyPlanet> Planets = new ();
    public List<Moon> Moons = new ();
    
    private bool _isMouseOver = false;
    
    public void OnPointerEnter(PointerEventData eventData)
    {
        _isMouseOver = true;
        StarProperties.SpectralType spectralType = (StarProperties.SpectralType)Index;
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

    public void Update()
    {
        transform.localScale = Vector3.Lerp(transform.localScale, _isMouseOver ? HighlightScale : DefaultScale, ConstantsUtil.BUTTON_SCALE_LERP_SPEED);
    }

    public void Start()
    {
        Button.onClick.AddListener(PlaySound);
        Button.onClick.AddListener(StarCreation);
        Button.onClick.AddListener(PlanetCreation);
        Button.onClick.AddListener(MoonCreation);
    }
    
    private static void PlaySound()
    {
        FindObjectOfType<AudioManager>().Play("MenuButtonClick");
    }

    private void StarCreation()
    {
        StarGenerator.GenerateStar(Index, StarProperties);
    }

    private void PlanetCreation()
    {
        PlanetGenerator.GeneratePlanets(Planets);
    }

    private void MoonCreation()
    {
        MoonGenerator.GenerateMoons(Planets, Moons);
    }
}