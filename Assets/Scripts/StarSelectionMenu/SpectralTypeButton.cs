using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using System;

public class SpectralTypeButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public StarDistance StarDistance;
    public StarProperties StarProperties;
    public StarDescriptions StarDescriptions;
    public int Index;
    public Button Button;
    public TextMeshProUGUI StarDescriptionText;
	public Vector3 DefaultScale = new (1f, 1f, 1f);
    public Vector3 HighlightScale = new (2.0f, 2.0f, 2.0f);
    private bool IsMouseOver = false;
    private float ScaleLerpSpeed = 0.025f;
    public string starClass;

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
        if (IsMouseOver)
        {
            transform.localScale = Vector3.Lerp(transform.localScale, HighlightScale, ScaleLerpSpeed);
        }
        else
        {
            transform.localScale = Vector3.Lerp(transform.localScale, DefaultScale, ScaleLerpSpeed);
        }
    }

    public void Start()
    {
        Button.onClick.AddListener(PlaySound);
        Button.onClick.AddListener(GenerateStar);
        StarProperties = StarProperties.instance;
    }

    private void PlaySound()
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
        double distanceToStarFromEarth = StarDistance.GenerateDistanceToStar();

        double size = StarProperties.Radius;
        StarProperties.Size = new Vector3((float)size, (float)size, (float)size);

        if (SaveManager.instance.hasLoaded)
        {
            StarProperties.SystemName = SaveManager.instance.activeSave.starSystemName;
            starClass = SaveManager.instance.activeSave.starClassAsString;

            StarProperties.Age = SaveManager.instance.activeSave.starAge;
            StarProperties.Mass = SaveManager.instance.activeSave.starMass;
            StarProperties.Radius = SaveManager.instance.activeSave.starRadius;
            StarProperties.Luminosity = SaveManager.instance.activeSave.starLuminosity;
            StarProperties.Temperature = SaveManager.instance.activeSave.starTemperature;
            StarProperties.Rotation = SaveManager.instance.activeSave.starRotation;
            StarProperties.MagneticField = SaveManager.instance.activeSave.starMagneticField;
            StarProperties.Chromaticity = SaveManager.instance.activeSave.starChromaticity;
            StarProperties.CellColor = SaveManager.instance.activeSave.starCellColor;
            StarProperties.Size = SaveManager.instance.activeSave.starSize;
            StarProperties.ExtrinsicVariability = SaveManager.instance.activeSave.hasExtrinsicVariability;
            StarProperties.IntrinsicVariability = SaveManager.instance.activeSave.hasIntrinsicVariability;
            StarProperties.Variability = SaveManager.instance.activeSave.starVariability;
            distanceToStarFromEarth = SaveManager.instance.activeSave.starDistance;

            StarProperties.Metallicity = SaveManager.instance.activeSave.starMetallicity;
        }
        else
        {
            SaveManager.instance.activeSave.starSystemName = StarProperties.SystemName;
            SaveManager.instance.activeSave.starClassAsString = DetermineStarClass();

            SaveManager.instance.activeSave.starAge = StarProperties.Age;
            SaveManager.instance.activeSave.starMass = StarProperties.Mass;
            SaveManager.instance.activeSave.starRadius = StarProperties.Radius;
            SaveManager.instance.activeSave.starLuminosity = StarProperties.Luminosity;
            SaveManager.instance.activeSave.starTemperature = StarProperties.Temperature;
            SaveManager.instance.activeSave.starRotation = StarProperties.Rotation;
            SaveManager.instance.activeSave.starMagneticField = StarProperties.MagneticField;
            SaveManager.instance.activeSave.starChromaticity = StarProperties.Chromaticity;
            SaveManager.instance.activeSave.starCellColor = StarProperties.CellColor;
            SaveManager.instance.activeSave.starSize = StarProperties.Size;
            SaveManager.instance.activeSave.hasExtrinsicVariability = StarProperties.ExtrinsicVariability;
            SaveManager.instance.activeSave.hasIntrinsicVariability = StarProperties.IntrinsicVariability;
            SaveManager.instance.activeSave.starVariability = StarProperties.Variability;
            SaveManager.instance.activeSave.starDistance = distanceToStarFromEarth;

            SaveManager.instance.activeSave.starMetallicity = StarProperties.Metallicity;
        }

        //Debug.Log("Distance from Earth to star: " + string.Format("{0:0,0.00}", StarDistance.GenerateDistanceToStar()) + " parsecs (pc).");
        //Debug.Log("Distance from Earth to star: " + string.Format("{0:0,0.00}", StarDistance.GenerateDistanceToStar() * 3.26156) + " light years (ly).");
        //Debug.Log("Distance from Earth to star: " + string.Format("{0:0,0.00}", StarDistance.GenerateDistanceToStar() * 3.086e16) + " meters (m).");
        //Debug.Log(string.Format("Probability of the generated distance: {0:0.00}%", Mathf.Round((float)(StarDistance.DistanceToStarProbability() * 100 * 100)) / 100));

        //Debug.Log("Star size: " + string.Format("{0:0,0.00}", StarProperties.GenerateStarSize()) + " (km).");
    }

    private string DetermineStarClass()
    {
        StarProperties.SpectralType spectralType = StarProperties.SpectralClass;
        switch (spectralType)
        {
            case StarProperties.SpectralType.O:
                starClass = spectralType.ToString();
                break;
            case StarProperties.SpectralType.B:
                starClass = spectralType.ToString();
                break;
            case StarProperties.SpectralType.A:
                starClass = spectralType.ToString();
                break;
            case StarProperties.SpectralType.F:
                starClass = spectralType.ToString();
                break;
            case StarProperties.SpectralType.G:
                starClass = spectralType.ToString();
                break;
            case StarProperties.SpectralType.K:
                starClass = spectralType.ToString();
                break;
            case StarProperties.SpectralType.M:
                starClass = spectralType.ToString();
                break;
        }
        return starClass;
    }

    private void GrabFinalizedStarData()
    {
        StarProperties.SpectralType spectralType = (StarProperties.SpectralType)Index;
        StarProperties.SpectralClass = spectralType;

        StarProperties.GenerateStarSystemName();
        StarProperties.GenerateMass(spectralType);
        StarProperties.GenerateRadius(spectralType);
        StarProperties.GenerateLuminosity(spectralType);
        StarProperties.GenerateTemperature(spectralType);
        StarProperties.GenerateAge(spectralType);
        StarProperties.GenerateRotation(spectralType);
        StarProperties.GenerateMagneticField(spectralType);
        StarProperties.Metallicity = new SerializableDictionary<string, float>();
        StarProperties.GenerateComposition(spectralType);
        StarProperties.GenerateChromaticity(spectralType);
        StarProperties.RandomCellColorGenerator();
        StarProperties.GenerateIntrinsicVariability(spectralType);
    }
}
