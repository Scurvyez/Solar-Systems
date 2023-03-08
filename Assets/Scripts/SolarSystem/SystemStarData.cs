using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;

public class SystemStarData : MonoBehaviour
{
    public StarProperties StarProperties;
    public SaveManager SaveManager;

    public TextMeshProUGUI SystemNameText;
    public TextMeshProUGUI AgeText;
    public TextMeshProUGUI MassText;
    public TextMeshProUGUI RadiusText;
    public TextMeshProUGUI LuminosityText;
    public TextMeshProUGUI TemperatureText;
    public TextMeshProUGUI RotationText;
    public TextMeshProUGUI MagneticFieldText;
    public TextMeshProUGUI VariabilityText;
    public TextMeshProUGUI MetallicityText;
    private TextMeshProUGUI[] textElements;
    
    private const float SolRadii = 695700000.0f;
    
    // Start is called before the first frame update
    private void Start()
    {
        SaveManager = SaveManager.instance;
        SetSavedStarValues();
        PopulateTextElementsArray();
        //SelectionRing.transform.localRotation = Quaternion.Euler(newX, SelectionRing.transform.localRotation.y, SelectionRing.transform.localRotation.z);
        //var sP = transform.localScale;
        //SelectionRing.transform.localScale = new Vector3(sP.x * 2.5f, sP.y * 2.5f, sP.z * 2.5f);
    }

    private void Update()
    {
        bool isSolarSystemScene = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name == "SolarSystem";
        Renderer parentMaterial = transform.GetComponent<Renderer>();

        if (SaveManager.hasLoaded && isSolarSystemScene)
        {
            RotationText.text = "<color=#ff8f8f>Rotation:</color> " + string.Format("{0:0.00}", transform.rotation) + " km/h";
            VariabilityText.text = "<color=#ff8f8f>Variability:</color> " + parentMaterial.material.GetFloat("_SolarFlare");
        }
    }

    private void SetSavedStarValues()
    {
        bool isSolarSystemScene = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name == "SolarSystem";

        if (SaveManager.hasLoaded && isSolarSystemScene)
        {
            SystemNameText.text = "<color=#ff8f8f>System Name:</color> " + SaveManager.activeSave.starSystemName;

            if (SaveManager.activeSave.starAge >= 1000000000)
                AgeText.text = "<color=#ff8f8f>Age:</color> " + (SaveManager.activeSave.starAge / 1000000000f).ToString("0.00") + " billion years old";
            else
                AgeText.text = "<color=#ff8f8f>Age:</color> " + (SaveManager.activeSave.starAge / 1000000f).ToString("0.00") + " million years old";

            MassText.text = "<color=#ff8f8f>Mass:</color> " + string.Format("{0:0,0.00}", SaveManager.activeSave.starMass) + " M<sub>O</sub>";
            RadiusText.text = "<color=#ff8f8f>Radius:</color> " + string.Format("{0:0,0.00}", SaveManager.activeSave.starRadius / SolRadii) + " R<sub>O</sub>";
            LuminosityText.text = "<color=#ff8f8f>Luminosity:</color> " + string.Format("{0:0,0.00}", SaveManager.activeSave.starLuminosity) + " L<sub>O</sub>";
            TemperatureText.text = "<color=#ff8f8f>Temperature:</color> " + string.Format("{0:0.00}", SaveManager.activeSave.starTemperature) + " K";
            RotationText.text = "<color=#ff8f8f>Rotation:</color> " + string.Format("{0:0.00}", SaveManager.activeSave.starRotation) + " km/h";
            MagneticFieldText.text = "<color=#ff8f8f>MagneticField:</color> " + string.Format("{0:0.000000}", SaveManager.activeSave.starMagneticField) + " teslas";
            MetallicityText.text = "<color=#ff8f8f>Metallicity:</color> " + string.Join(", ", SaveManager.activeSave.starMetallicity);
        }
    }

    private void PopulateTextElementsArray()
    {
        textElements = new TextMeshProUGUI[] 
        { 
            SystemNameText, // 0
            AgeText, // 1
            MassText, // 2
            RadiusText, // 3
            LuminosityText, // 4
            TemperatureText, // 5
            RotationText, // 6
            MagneticFieldText, // 7
            VariabilityText, // 8
            MetallicityText // 9
        };
    }
}
