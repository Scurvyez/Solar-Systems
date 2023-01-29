using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DisplayStarData : MonoBehaviour
{
    public StarProperties StarProperties;

    public TextMeshProUGUI AgeText;
    public TextMeshProUGUI MassText;
    public TextMeshProUGUI RadiusText;
    public TextMeshProUGUI LuminosityText;
    public TextMeshProUGUI TemperatureText;
    public TextMeshProUGUI RotationText;
    public TextMeshProUGUI MagneticFieldText;
    public TextMeshProUGUI VariabilityText;
    public TextMeshProUGUI BinaryText;
    public TextMeshProUGUI CompositionText;

    // Start is called before the first frame update
    public void Start()
    {
        StarProperties = StarProperties.instance;
    }

    // Update is called once per frame
    void Update()
    {
        if (StarProperties.Age >= 1000000000)
            AgeText.text = "Age: " + (StarProperties.Age / 1000000000f).ToString("0.00") + " billion years old";
        else
            AgeText.text = "Age: " + (StarProperties.Age / 1000000f).ToString("0.00") + " million years old";

        MassText.text = "Mass: " + string.Format("{0:0,0.00}", StarProperties.Mass) + " kg";
        RadiusText.text = "Radius: " + string.Format("{0:0,0.00}", StarProperties.Radius) + " km";
        LuminosityText.text = "Luminosity: " + string.Format("{0:0,0.00}", StarProperties.Luminosity) + " watts";
        TemperatureText.text = "Temperature: " + string.Format("{0:0.00}", StarProperties.Temperature) + " K";
        RotationText.text = "Rotation: " + string.Format("{0:0.00}", StarProperties.Rotation) + " km/h";
        MagneticFieldText.text = "MagneticField: " + string.Format("{0:0.000000}", StarProperties.MagneticField) + " teslas";
        VariabilityText.text = "Variability: " + StarProperties.Variability;
        BinaryText.text = "Binary: " + StarProperties.Binary;

        if (StarProperties.Composition == null)
            CompositionText.text = "Composition: Unknown";
        else
            CompositionText.text = "Composition: " + string.Join(", ", StarProperties.Composition);
    }
}
