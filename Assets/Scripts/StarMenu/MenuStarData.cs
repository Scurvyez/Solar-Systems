using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MenuStarData : MonoBehaviour
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
    public TextMeshProUGUI MetallicityText;

    private const float SolRadii = 695700000.0f;

    // Start is called before the first frame update
    public void Start()
    {
        //StarProperties = StarProperties.instance;
        SaveManager = SaveManager.instance;
    }

    // Update is called once per frame
    void Update()
    {
        bool isSolarSystemScene = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name == "SolarSystem";

        if (SaveManager.hasLoaded)
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

            if (SaveManager.activeSave.starMetallicity == null)
                MetallicityText.text = "<color=#ff8f8f>Metallicity:</color> Unknown";
            else
                MetallicityText.text = "<color=#ff8f8f>Metallicity:</color> " + string.Join(", ", SaveManager.activeSave.starMetallicity);
        }

        if (SaveManager.hasBeenReset || !SaveManager.hasLoaded)
        {
            if (SaveManager.activeSave.starSystemName == null || StarProperties.SystemName == null)
                SystemNameText.text = "<color=#ff8f8f>System Name:</color> " + "N/A";
            else
                SystemNameText.text = "<color=#ff8f8f>System Name:</color> " + StarProperties.SystemName;

            if (StarProperties.Age >= 1000000000)
                AgeText.text = "<color=#ff8f8f>Age:</color> " + (StarProperties.Age / 1000000000f).ToString("0.00") + " billion years old";
            else
                AgeText.text = "<color=#ff8f8f>Age:</color> " + (StarProperties.Age / 1000000f).ToString("0.00") + " million years old";

            MassText.text = "<color=#ff8f8f>Mass:</color> " + string.Format("{0:0,0.00}", StarProperties.Mass) + " M<sub>O</sub>";
            RadiusText.text = "<color=#ff8f8f>Radius:</color> " + string.Format("{0:0,0.00}", StarProperties.Radius / SolRadii) + " R<sub>O</sub>";
            LuminosityText.text = "<color=#ff8f8f>Luminosity:</color> " + string.Format("{0:0,0.00}", StarProperties.Luminosity) + " L<sub>O</sub>";
            TemperatureText.text = "<color=#ff8f8f>Temperature:</color> " + string.Format("{0:0.00}", StarProperties.Temperature) + " K";
            RotationText.text = "<color=#ff8f8f>Rotation:</color> " + string.Format("{0:0.00}", StarProperties.Rotation) + " km/h";
            MagneticFieldText.text = "<color=#ff8f8f>MagneticField:</color> " + string.Format("{0:0.000000}", StarProperties.MagneticField) + " teslas";

            if (SaveManager.activeSave.starMetallicity == null || StarProperties.Metallicity == null)
                MetallicityText.text = "<color=#ff8f8f>Metallicity:</color> N/A";
            else
                MetallicityText.text = "<color=#ff8f8f>Metallicity:</color> " + string.Join(", ", StarProperties.Metallicity);
        }

        if (isSolarSystemScene)
        {
            SystemNameText.text = "System Name: " + SaveManager.activeSave.starSystemName;

            if (SaveManager.activeSave.starAge >= 1000000000)
                AgeText.text = "Age: " + (SaveManager.activeSave.starAge / 1000000000f).ToString("0.00") + " billion years old";
            else
                AgeText.text = "Age: " + (SaveManager.activeSave.starAge / 1000000f).ToString("0.00") + " million years old";

            MassText.text = "Mass: " + string.Format("{0:0,0.00}", SaveManager.activeSave.starMass) + " M<sub>O</sub>";
            RadiusText.text = "Radius: " + string.Format("{0:0,0.00}", SaveManager.activeSave.starRadius / SolRadii) + " R<sub>O</sub>";
            LuminosityText.text = "Luminosity: " + string.Format("{0:0,0.00}", SaveManager.activeSave.starLuminosity) + " L<sub>O</sub>";
            TemperatureText.text = "Temperature: " + string.Format("{0:0.00}", SaveManager.activeSave.starTemperature) + " K";
            RotationText.text = "Rotation: " + string.Format("{0:0.00}", SaveManager.activeSave.starRotation) + " km/h";
            MagneticFieldText.text = "MagneticField: " + string.Format("{0:0.000000}", SaveManager.activeSave.starMagneticField) + " teslas";

            if (SaveManager.activeSave.starMetallicity == null)
                MetallicityText.text = "Metallicity: Unknown";
            else
                MetallicityText.text = "Metallicity: " + string.Join(", ", SaveManager.activeSave.starMetallicity);
        }
    }
}
