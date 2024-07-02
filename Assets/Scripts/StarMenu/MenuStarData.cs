using UnityEngine;
using TMPro;
using Utils;

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

    private const float SOL_RADII = 695700000.0f;

    // Start is called before the first frame update
    public void Start()
    {
        //StarProperties = StarProperties.instance;
        SaveManager = SaveManager.instance;
    }

    // Update is called once per frame
    private void Update()
    {
        bool isSolarSystemScene = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name == "SolarSystem";

        if (SaveManager.hasLoaded)
        {
            SystemNameText.text = "<color=#ff8f8f>System Name:</color> " + SaveManager.activeSave.starSystemName;

            if (SaveManager.activeSave.starAge >= 1000000000)
                AgeText.text = "<color=#ff8f8f>Age:</color> " + (SaveManager.activeSave.starAge / 1000000000f).ToString("0.00") + " billion years old";
            else
                AgeText.text = "<color=#ff8f8f>Age:</color> " + (SaveManager.activeSave.starAge / 1000000f).ToString("0.00") + " million years old";

            MassText.text = "<color=#ff8f8f>Mass:</color> " + $"{SaveManager.activeSave.starMass:0,0.00}" + " M<sub>O</sub>";
            RadiusText.text = "<color=#ff8f8f>Radius:</color> " + $"{SaveManager.activeSave.starRadius / SOL_RADII:0,0.00}" + " R<sub>O</sub>";
            LuminosityText.text = "<color=#ff8f8f>Luminosity:</color> " + $"{SaveManager.activeSave.starLuminosity:0,0.00}" + " L<sub>O</sub>";
            TemperatureText.text = "<color=#ff8f8f>Temperature:</color> " + $"{SaveManager.activeSave.starTemperature:0.00}" + " K";
            RotationText.text = "<color=#ff8f8f>Rotation:</color> " + $"{SaveManager.activeSave.starRotation:0.00}" + " km/h";
            MagneticFieldText.text = "<color=#ff8f8f>MagneticField:</color> " + $"{SaveManager.activeSave.starMagneticField:0.000000}" + " teslas";

            if (SaveManager.activeSave.starMetallicity == null)
                MetallicityText.text = "<color=#ff8f8f>Metallicity:</color> Unknown";
            else
                MetallicityText.text = "<color=#ff8f8f>Metallicity:</color> " + string.Join(", ", SaveManager.activeSave.starMetallicity);
        }

        if (SaveManager.hasBeenReset || !SaveManager.hasLoaded)
        {
            if (SaveManager.activeSave.starSystemName == null || SystemNamingUtil.Info_SystemName == null)
                SystemNameText.text = "<color=#ff8f8f>System Name:</color> " + "N/A";
            else
                SystemNameText.text = "<color=#ff8f8f>System Name:</color> " + SystemNamingUtil.Info_SystemName;

            if (StarProperties.Info_Age >= 1000000000)
                AgeText.text = "<color=#ff8f8f>Age:</color> " + (StarProperties.Info_Age / 1000000000f).ToString("0.00") + " billion years old";
            else
                AgeText.text = "<color=#ff8f8f>Age:</color> " + (StarProperties.Info_Age / 1000000f).ToString("0.00") + " million years old";

            MassText.text = "<color=#ff8f8f>Mass:</color> " + $"{StarProperties.Info_Mass:0,0.00}" + " M<sub>O</sub>";
            RadiusText.text = "<color=#ff8f8f>Radius:</color> " + $"{StarProperties.Info_Radius / SOL_RADII:0,0.00}" + " R<sub>O</sub>";
            LuminosityText.text = "<color=#ff8f8f>Luminosity:</color> " + $"{StarProperties.Info_Luminosity:0,0.00}" + " L<sub>O</sub>";
            TemperatureText.text = "<color=#ff8f8f>Temperature:</color> " + $"{StarProperties.Info_Temperature:0.00}" + " K";
            RotationText.text = "<color=#ff8f8f>Rotation:</color> " + $"{StarProperties.Info_Rotation:0.00}" + " km/h";
            MagneticFieldText.text = "<color=#ff8f8f>MagneticField:</color> " + $"{StarProperties.Info_MagneticField:0.000000}" + " teslas";

            if (SaveManager.activeSave.starMetallicity == null || StarProperties.Info_Metallicity == null)
                MetallicityText.text = "<color=#ff8f8f>Metallicity:</color> N/A";
            else
                MetallicityText.text = "<color=#ff8f8f>Metallicity:</color> " + string.Join(", ", StarProperties.Info_Metallicity);
        }

        if (!isSolarSystemScene) return;
        SystemNameText.text = "System Name: " + SaveManager.activeSave.starSystemName;

        if (SaveManager.activeSave.starAge >= 1000000000)
            AgeText.text = "Age: " + (SaveManager.activeSave.starAge / 1000000000f).ToString("0.00") + " billion years old";
        else
            AgeText.text = "Age: " + (SaveManager.activeSave.starAge / 1000000f).ToString("0.00") + " million years old";

        MassText.text = "Mass: " + $"{SaveManager.activeSave.starMass:0,0.00}" + " M<sub>O</sub>";
        RadiusText.text = "Radius: " + $"{SaveManager.activeSave.starRadius / SOL_RADII:0,0.00}" + " R<sub>O</sub>";
        LuminosityText.text = "Luminosity: " + $"{SaveManager.activeSave.starLuminosity:0,0.00}" + " L<sub>O</sub>";
        TemperatureText.text = "Temperature: " + $"{SaveManager.activeSave.starTemperature:0.00}" + " K";
        RotationText.text = "Rotation: " + $"{SaveManager.activeSave.starRotation:0.00}" + " km/h";
        MagneticFieldText.text = "MagneticField: " + $"{SaveManager.activeSave.starMagneticField:0.000000}" + " teslas";

        if (SaveManager.activeSave.starMetallicity == null)
            MetallicityText.text = "Metallicity: Unknown";
        else
            MetallicityText.text = "Metallicity: " + string.Join(", ", SaveManager.activeSave.starMetallicity);
    }
}
