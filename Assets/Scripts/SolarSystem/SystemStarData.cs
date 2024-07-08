using Saving;
using UnityEngine;
using TMPro;

public class SystemStarData : MonoBehaviour
{
    public Star Star;
    public StaticAngledCamera SystemCamera;
    
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
    private TextMeshProUGUI[] _textElements;
    private static readonly int _solarFlare = Shader.PropertyToID("_SolarFlare");
    private Renderer _parentMaterial;
    
    // Start is called before the first frame update
    private void Start()
    {
        _parentMaterial = transform.GetComponent<Renderer>();
        SetSavedStarValues();
        PopulateTextElementsArray();
    }

    private void Update()
    {
        bool isSolarSystemScene = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name == "SolarSystem";

        if (!SaveManager.Instance.HasLoaded || !isSolarSystemScene) return;

        GameObject selectedObject = SystemCamera.SelectedObject;
        if (selectedObject is not null && selectedObject == gameObject)
        {
            ShowUIElements();
            RotationText.text = "<color=#ff8f8f>Rotation:</color> " + $"{transform.rotation:0.00}" + " km/h";
            VariabilityText.text = "<color=#ff8f8f>Variability:</color> " + _parentMaterial.material.GetFloat(_solarFlare);
        }
        else
        {
            HideUIElements();
        }
    }

    private void SetSavedStarValues()
    {
        bool isSolarSystemScene = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name == "SolarSystem";

        if (!SaveManager.Instance.HasLoaded || !isSolarSystemScene) return;
        SystemNameText.text = "<color=#ff8f8f>System Name:</color> " + SaveManager.Instance.ActiveSave.starSystemName;

        if (SaveManager.Instance.ActiveSave.starAge >= 1000000000)
            AgeText.text = "<color=#ff8f8f>Age:</color> " + (SaveManager.Instance.ActiveSave.starAge / 1000000000f).ToString("0.00") + " billion years old";
        else
            AgeText.text = "<color=#ff8f8f>Age:</color> " + (SaveManager.Instance.ActiveSave.starAge / 1000000f).ToString("0.00") + " million years old";

        MassText.text = "<color=#ff8f8f>Mass:</color> " + $"{SaveManager.Instance.ActiveSave.starMass:0,0.00}" + " M<sub>O</sub>";
        RadiusText.text = "<color=#ff8f8f>Radius:</color> " + $"{SaveManager.Instance.ActiveSave.starRadius:0,0.00}" + " R<sub>O</sub>";
        LuminosityText.text = "<color=#ff8f8f>Luminosity:</color> " + $"{SaveManager.Instance.ActiveSave.starLuminosity:0,0.00}" + " L<sub>O</sub>";
        TemperatureText.text = "<color=#ff8f8f>Temperature:</color> " + $"{SaveManager.Instance.ActiveSave.starTemperature:0.00}" + " K";
        RotationText.text = "<color=#ff8f8f>Rotation:</color> " + $"{SaveManager.Instance.ActiveSave.starRotation:0.00}" + " km/h";
        MagneticFieldText.text = "<color=#ff8f8f>MagneticField:</color> " + $"{SaveManager.Instance.ActiveSave.starMagneticField:0.000000}" + " teslas";
        MetallicityText.text = "<color=#ff8f8f>Metallicity:</color> " + string.Join(", ", SaveManager.Instance.ActiveSave.starMetallicity);
    }

    private void PopulateTextElementsArray()
    {
        _textElements = new [] 
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
    
    private void ShowUIElements()
    {
        foreach (TextMeshProUGUI textElement in _textElements)
        {
            textElement.gameObject.SetActive(true);
        }
    }

    private void HideUIElements()
    {
        foreach (TextMeshProUGUI textElement in _textElements)
        {
            textElement.gameObject.SetActive(false);
        }
    }
}
