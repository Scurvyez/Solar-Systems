using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class StarProperties : MonoBehaviour
{
    public enum SpectralType { O, B, A, F, G, K, M }
    public SpectralType SpectralClass { get; set; }
    public double Age { get; set; }
    public double Mass { get; set; }
    public double Radius { get; set; }
    public double Luminosity { get; set; }
    public double Temperature { get; set; }
    public double Rotation { get; set; }
    public double MagneticField { get; set; }
    public bool Variability { get; set; }
    public bool Binary { get; set; }
    public Dictionary<string, float> Composition { get; set; }
    public Color Chromaticity { get; set; }
    public Vector3 Size { get; set; }

    private const double SolLuminosity = 3.8e26;
    private const double SolMassKG = 1.9881e30;
    private const double SolRadiusM = 6.96342E8;
    private const double StefanBoltzmannConstant = 5.670373E-8;

    public StarProperties instance;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        if (SaveManager.instance.hasLoaded)
        {
            instance.Age = SaveManager.instance.activeSave.savedStarAge;
            instance.Mass = SaveManager.instance.activeSave.savedStarMass;
            instance.Radius = SaveManager.instance.activeSave.savedStarRadius;
            instance.Luminosity = SaveManager.instance.activeSave.savedStarLuminosity;
            instance.Temperature = SaveManager.instance.activeSave.savedStarTemperature;
            instance.Rotation = SaveManager.instance.activeSave.savedStarRotation;
            instance.MagneticField = SaveManager.instance.activeSave.savedStarMagneticField;
            instance.Variability = SaveManager.instance.activeSave.savedStarVariability;
            instance.Binary = SaveManager.instance.activeSave.savedStarBinary;
        }
        else
        {
            SaveManager.instance.activeSave.savedStarAge = instance.Age;
            SaveManager.instance.activeSave.savedStarMass = instance.Mass;
            SaveManager.instance.activeSave.savedStarRadius = instance.Radius;
            SaveManager.instance.activeSave.savedStarLuminosity = instance.Luminosity;
            SaveManager.instance.activeSave.savedStarTemperature = instance.Temperature;
            SaveManager.instance.activeSave.savedStarRotation = instance.Rotation;
            SaveManager.instance.activeSave.savedStarMagneticField = instance.MagneticField;
            SaveManager.instance.activeSave.savedStarVariability = instance.Variability;
            SaveManager.instance.activeSave.savedStarBinary = instance.Binary;
        }
    }

    public double GenerateRadius(SpectralType spectralType)
    {
        switch (spectralType)
        {
            case SpectralType.O:
                Radius = Random.Range(6.6f, 10f);
                break;
            case SpectralType.B:
                Radius = Random.Range(1.8f, 6.6f);
                break;
            case SpectralType.A:
                Radius = Random.Range(1.4f, 1.8f);
                break;
            case SpectralType.F:
                Radius = Random.Range(1.15f, 1.4f);
                break;
            case SpectralType.G:
                Radius = Random.Range(0.96f, 1.15f);
                break;
            case SpectralType.K:
                Radius = Random.Range(0.7f, 0.96f);
                break;
            case SpectralType.M:
                Radius = Random.Range(0.08f, 0.7f);
                break;
        }

        // convert to kilometers before returning
        Radius *= SolRadiusM / 1000;

        return Radius;
    }

    public Vector3 GenerateStarSize()
    {
        Size = new Vector3((float)Radius, (float)Radius, (float)Radius);

        return Size;
    }

    public double GenerateTemperature(SpectralType spectralType)
    {
        switch (spectralType)
        {
            case SpectralType.O:
                Temperature = Random.Range(30000f, 60000f);
                break;
            case SpectralType.B:
                Temperature = Random.Range(10000f, 30000f);
                break;
            case SpectralType.A:
                Temperature = Random.Range(7500f, 10000f);
                break;
            case SpectralType.F:
                Temperature = Random.Range(6000f, 7500f);
                break;
            case SpectralType.G:
                Temperature = Random.Range(5200f, 6000f);
                break;
            case SpectralType.K:
                Temperature = Random.Range(3700f, 5200f);
                break;
            case SpectralType.M:
                Temperature = Random.Range(2400f, 3700f);
                break;
        }
        return Temperature;
    }

    public double GenerateLuminosity(SpectralType spectralType)
    {
        if (GenerateTemperature(spectralType) != 0 && GenerateRadius(spectralType) != 0)
        {
            double starRadius = GenerateRadius(spectralType);
            double starTemperature = GenerateTemperature(spectralType);

            Luminosity = ((float)StefanBoltzmannConstant * (4 * Mathf.PI * Mathf.Pow((float)starRadius, 2)) * Mathf.Pow((float)starTemperature, 4));
        }
        return Luminosity;
    }

    public double GenerateMass(SpectralType spectralType)
    {
        if (GenerateLuminosity(spectralType) != 0)
        {
            double starLuminosity = GenerateLuminosity(spectralType);

            Mass = Mathf.Pow((float)starLuminosity / (float)SolLuminosity, 3f / 4f);
            Mass *= (float)SolMassKG;
        }
        return Mass;
    }

    public double GenerateAge(SpectralType spectralType)
    {
        Age = 0f;

        switch (spectralType)
        {
            case SpectralType.O:
                Age = Random.Range(5000000, 10000000); // 5M - 10M
                break;
            case SpectralType.B:
                Age = Random.Range(50000000, 100000000); // 50M - 100M
                break;
            case SpectralType.A:
                Age = Random.Range(500000000, 1000000000); // 500M - 1B
                break;
            case SpectralType.F:
                Age = Random.Range(2500000000, 5000000000); // 2.5B - 5B
                break;
            case SpectralType.G:
                Age = Random.Range(5000000000, 10000000000); // 5B - 10B
                break;
            case SpectralType.K:
                Age = Random.Range(25000000000, 50000000000); // 25B - 50B
                break;
            case SpectralType.M:
                Age = Random.Range(50000000000, 100000000000); // 50B - 100B
                break;
        }
        return Age;
    }

    public double GenerateRotation(SpectralType spectralType)
    {
        switch (spectralType)
        {
            case SpectralType.O:
                Rotation = Random.Range(20f, 30f);
                break;
            case SpectralType.B:
                Rotation = Random.Range(5f, 15f);
                break;
            case SpectralType.A:
                Rotation = Random.Range(1f, 10f);
                break;
            case SpectralType.F:
                Rotation = Random.Range(0.5f, 5f);
                break;
            case SpectralType.G:
                Rotation = Random.Range(0.1f, 1f);
                break;
            case SpectralType.K:
                Rotation = Random.Range(0.05f, 0.5f);
                break;
            case SpectralType.M:
                Rotation = Random.Range(0.01f, 0.05f);
                break;
        }
        return Rotation;
    }

    public double GenerateMagneticField(SpectralType spectralType)
    {
        switch (spectralType)
        {
            case SpectralType.O:
                MagneticField = Random.Range(0.05f, 1.0f);
                break;
            case SpectralType.B:
                MagneticField = Random.Range(0.03f, 0.05f);
                break;
            case SpectralType.A:
                MagneticField = Random.Range(0.01f, 0.03f);
                break;
            case SpectralType.F:
                MagneticField = Random.Range(0.003f, 0.01f);
                break;
            case SpectralType.G:
                MagneticField = Random.Range(0.001f, 0.003f);
                break;
            case SpectralType.K:
                MagneticField = Random.Range(0.0005f, 0.001f);
                break;
            case SpectralType.M:
                MagneticField = Random.Range(0.0001f, 0.0005f);
                break;
        }
        return MagneticField;
    }


    public Color GenerateChromaticity(SpectralType spectralType)
    {
        switch (spectralType)
        {
            case SpectralType.O:
                Chromaticity = new Color(146, 181, 255);
                break;
            case SpectralType.B:
                Chromaticity = new Color(162, 192, 255);
                break;
            case SpectralType.A:
                Chromaticity = new Color(213, 224, 255);
                break;
            case SpectralType.F:
                Chromaticity = new Color(249, 245, 255);
                break;
            case SpectralType.G:
                Chromaticity = new Color(255, 237, 227);
                break;
            case SpectralType.K:
                Chromaticity = new Color(255, 218, 181);
                break;
            case SpectralType.M:
                Chromaticity = new Color(255, 181, 108);
                break;
        }
        return Chromaticity;
    }

    public void GenerateComposition(SpectralType spectralType)
    {
        switch (spectralType)
        {
            case SpectralType.O:
                Composition.Add("Hydrogen", Random.Range(74f, 76f));
                Composition.Add("Helium", Random.Range(24f, 26f));
                break;
            case SpectralType.B:
                Composition.Add("Hydrogen", Random.Range(58f, 70f));
                Composition.Add("Helium", Random.Range(28f, 42f));
                Composition.Add("Carbon", Random.Range(0.1f, 2f));
                Composition.Add("Nitrogen", Random.Range(0.1f, 2f));
                Composition.Add("Oxygen", Random.Range(0.1f, 2f));
                break;
            case SpectralType.A:
                Composition.Add("Hydrogen", Random.Range(71f, 74f));
                Composition.Add("Helium", Random.Range(25f, 28f));
                Composition.Add("Carbon", Random.Range(0.1f, 2f));
                Composition.Add("Nitrogen", Random.Range(0.1f, 2f));
                Composition.Add("Oxygen", Random.Range(0.1f, 2f));
                Composition.Add("Neon", Random.Range(0.1f, 2f));
                break;
            case SpectralType.F:
                Composition.Add("Hydrogen", Random.Range(54f, 64f));
                Composition.Add("Helium", Random.Range(35f, 45f));
                Composition.Add("Carbon", Random.Range(0.1f, 2f));
                Composition.Add("Nitrogen", Random.Range(0.1f, 2f));
                Composition.Add("Oxygen", Random.Range(0.1f, 2f));
                Composition.Add("Neon", Random.Range(0.1f, 2f));
                Composition.Add("Iron", Random.Range(0.1f, 2f));
                break;
            case SpectralType.G:
                Composition.Add("Hydrogen", Random.Range(74f, 84f));
                Composition.Add("Helium", Random.Range(14f, 24f));
                Composition.Add("Carbon", Random.Range(0.1f, 2f));
                Composition.Add("Nitrogen", Random.Range(0.1f, 2f));
                Composition.Add("Oxygen", Random.Range(0.1f, 2f));
                Composition.Add("Neon", Random.Range(0.1f, 2f));
                Composition.Add("Iron", Random.Range(0.1f, 2f));
                break;
            case SpectralType.K:
                Composition.Add("Hydrogen", Random.Range(56f, 64f));
                Composition.Add("Helium", Random.Range(36f, 44f));
                Composition.Add("Carbon", Random.Range(0.1f, 2f));
                Composition.Add("Nitrogen", Random.Range(0.1f, 2f));
                Composition.Add("Oxygen", Random.Range(0.1f, 2f));
                Composition.Add("Neon", Random.Range(0.1f, 2f));
                Composition.Add("Iron", Random.Range(0.1f, 2f));
                Composition.Add("Silicon", Random.Range(0.1f, 2f));
                Composition.Add("Magnesium", Random.Range(0.1f, 2f));
                break;
            case SpectralType.M:
                Composition.Add("Hydrogen", Random.Range(36f, 56f));
                Composition.Add("Helium", Random.Range(44f, 64f));
                Composition.Add("Carbon", Random.Range(0.1f, 2f));
                Composition.Add("Nitrogen", Random.Range(0.1f, 2f));
                Composition.Add("Oxygen", Random.Range(0.1f, 2f));
                Composition.Add("Neon", Random.Range(0.1f, 2f));
                Composition.Add("Iron", Random.Range(0.1f, 2f));
                Composition.Add("Silicon", Random.Range(0.1f, 2f));
                Composition.Add("Magnesium", Random.Range(0.1f, 2f));
                Composition.Add("Sulfur", Random.Range(0.1f, 2f));
                Composition.Add("Chlorine", Random.Range(0.1f, 2f));
                Composition.Add("Potassium", Random.Range(0.1f, 2f));
                break;
            default:
                break;
        }
    }
}
