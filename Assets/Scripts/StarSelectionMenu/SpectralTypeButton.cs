using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using TMPro;

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
        Button.onClick.AddListener(GenerateStar);
        StarProperties = StarProperties.instance;
    }

    private void GenerateStar()
    {
        if (StarProperties == null)
        {
            Debug.LogError("StarPhysicalProperties is null");
            return;
        }

        GrabAndSaveFinalizedStarData();

        //Debug.Log("Distance from Earth to star: " + string.Format("{0:0,0.00}", StarDistance.GenerateDistanceToStar()) + " parsecs (pc).");
        //Debug.Log("Distance from Earth to star: " + string.Format("{0:0,0.00}", StarDistance.GenerateDistanceToStar() * 3.26156) + " light years (ly).");
        //Debug.Log("Distance from Earth to star: " + string.Format("{0:0,0.00}", StarDistance.GenerateDistanceToStar() * 3.086e16) + " meters (m).");
        //Debug.Log(string.Format("Probability of the generated distance: {0:0.00}%", Mathf.Round((float)(StarDistance.DistanceToStarProbability() * 100 * 100)) / 100));

        //Debug.Log("Star size: " + string.Format("{0:0,0.00}", StarProperties.GenerateStarSize()) + " (km).");
    }

    private void GrabAndSaveFinalizedStarData()
    {
        StarProperties.SpectralType spectralType = (StarProperties.SpectralType)Index;
        StarProperties.SpectralClass = spectralType;

        StarProperties.GenerateMass(spectralType);
        StarProperties.GenerateRadius(spectralType);
        StarProperties.GenerateStarSize();
        StarProperties.GenerateLuminosity(spectralType);
        StarProperties.GenerateTemperature(spectralType);
        StarProperties.GenerateAge(spectralType);
        StarProperties.GenerateRotation(spectralType);
        StarProperties.GenerateMagneticField(spectralType);
        StarProperties.Variability = Random.value < 0.1f;
        StarProperties.Binary = Random.value < 0.1f;
        StarProperties.Composition = new Dictionary<string, float>();
        StarProperties.GenerateComposition(spectralType);
        StarProperties.GenerateChromaticity(spectralType);
    }
}
