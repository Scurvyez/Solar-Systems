using System;
using UnityEngine;
using Utils;
using Random = UnityEngine.Random;

public class Star : MonoBehaviour
{
    public enum SpectralType { O, B, A, F, G, K, M, Unknown }
    public SpectralType SpectralClass { get; set; }
    public double Info_Age { get; set; }
    public float Info_Radius { get; set; }
    public float Info_Temperature { get; set; }
    public float Info_Rotation { get; set; }
    public float Info_MagneticField { get; set; }
    public SerializableDictionary<string, float> Info_Metallicity { get; set; }
    public float Info_Variability { get; set; }
    public float Info_Luminosity { get; set; }
    public float Info_Mass { get; set; }
    public Vector3 GO_Size { get; set; }
    public Vector3 GO_Position { get; set; }
    public float GO_Radius { get; set; }
    public Color GO_Chromaticity { get; set; }
    public Color GO_CellColor { get; set; }
    public int PlanetCount { get; set; }
    public float GO_HabitableRangeInner { get; set; }
    public float GO_HabitableRangeOuter { get; set; }
    
    public bool ExtrinsicVariability;
    public bool IntrinsicVariability;

    /// <summary>
    /// Measured/returned in solar radii.
    /// </summary>
    public float GenerateInfoRadius(SpectralType spectralType)
    {
        float randomNumber = Random.Range(0, 250);

        Info_Radius = spectralType switch
        {
            SpectralType.O when Mathf.Approximately(randomNumber, 250) => Random.Range(1250f, 1500f),
            SpectralType.O when randomNumber is <= 249 and >= 240 => Random.Range(1000f, 1250f),
            SpectralType.O when randomNumber is <= 239 and >= 230 => Random.Range(800f, 1000f),
            SpectralType.O when randomNumber is <= 229 and >= 220 => Random.Range(500f, 800f),
            SpectralType.O when randomNumber is <= 219 and >= 200 => Random.Range(100f, 500f),
            SpectralType.O when randomNumber is <= 199 and >= 175 => Random.Range(30f, 100f),
            SpectralType.O when randomNumber is <= 174 and >= 125 => Random.Range(10f, 30f),
            SpectralType.O => Random.Range(6.6f, 10f),
            SpectralType.B => Random.Range(1.8f, 6.6f),
            SpectralType.A => Random.Range(1.4f, 1.8f),
            SpectralType.F => Random.Range(1.15f, 1.4f),
            SpectralType.G => Random.Range(0.96f, 1.15f),
            SpectralType.K => Random.Range(0.7f, 0.96f),
            SpectralType.M => Random.Range(0.08f, 0.7f),
            _ => Info_Radius,
        };
        return Info_Radius;
    }
    
    /// <summary>
    /// Origin point for our star(s) and the rest of the system.
    /// </summary>
    public Vector3 GenerateGOStartingPosition()
    {
        GO_Position = Vector3.zero;
        return GO_Position;
    }

    /// <summary>
    /// Physical radius of star game object.
    /// </summary>
    public float GenerateGORadius()
    {
        GO_Radius = 50.0f;
        return GO_Radius;
    }

    /// <summary>
    /// Measured in kelvins.
    /// </summary>
    public float GenerateInfoTemperature(SpectralType spectralType)
    {
        Info_Temperature = spectralType switch
        {
            SpectralType.O => Random.Range(30000f, 60000f),
            SpectralType.B => Random.Range(10000f, 30000f),
            SpectralType.A => Random.Range(7500f, 10000f),
            SpectralType.F => Random.Range(6000f, 7500f),
            SpectralType.G => Random.Range(5200f, 6000f),
            SpectralType.K => Random.Range(3700f, 5200f),
            SpectralType.M => Random.Range(2400f, 3700f),
            _ => Info_Temperature,
        };
        return Info_Temperature;
    }

    /// <summary>
    /// Measured/returned in solar luminosities.
    /// </summary>
    public float GenerateInfoLuminosity(float starRadius, float starTemperature)
    {
        float starRadiusInMeters = starRadius * ConstantsUtil.SOL_RADII_METERS;
        float luminosity = ConstantsUtil.STEFAN_BOLTZMANN_CONSTANT * (4 * Mathf.PI * Mathf.Pow(starRadiusInMeters, 2)) * Mathf.Pow(starTemperature, 4);

        // Convert luminosity to solar luminosities
        Info_Luminosity = luminosity / ConstantsUtil.SOL_LUMINOSITY;

        return Info_Luminosity;
    }

    /// <summary>
    /// Measured/returned in solar masses.
    /// </summary>
    public float GenerateInfoMass(float starLuminosity)
    {
        if (starLuminosity <= 0)
        {
            throw new ArgumentException("solarLuminosity must be a positive value.");
        }
        
        Info_Mass = Mathf.Pow(starLuminosity, 3f / 4f);
        return Info_Mass;
    }

    /// <summary>
    /// Bigger stars (O class) burn hotter and faster, so shorter lives.
    /// Smaller stars (M class) burn much slower and lower, so they have longer lives.
    /// Measured in Earth years.
    /// </summary>
    public double GenerateInfoAge(SpectralType spectralType)
    {
        Info_Age = spectralType switch
        {
            SpectralType.O => Random.Range(5000000, 10000000),
            SpectralType.B => Random.Range(50000000, 100000000),
            SpectralType.A => Random.Range(500000000, 1000000000),
            SpectralType.F => Random.Range(2500000000, 5000000000),
            SpectralType.G => Random.Range(5000000000, 10000000000),
            SpectralType.K => Random.Range(25000000000, 50000000000),
            SpectralType.M => Random.Range(50000000000, 100000000000),
            _ => Info_Age,
        };
        return Info_Age;
    }

    /// <summary>
    /// Measured/returned in kilometers / second.
    /// </summary>
    public float GenerateInfoRotation(SpectralType spectralType)
    {
        Info_Rotation = spectralType switch
        {
            SpectralType.O => Random.Range(20f, 30f),
            SpectralType.B => Random.Range(5f, 15f),
            SpectralType.A => Random.Range(1f, 10f),
            SpectralType.F => Random.Range(0.5f, 5f),
            SpectralType.G => Random.Range(0.1f, 1f),
            SpectralType.K => Random.Range(0.05f, 0.5f),
            SpectralType.M => Random.Range(0.01f, 0.05f),
            _ => Info_Rotation,
        };
        return Info_Rotation;
    }

    /// <summary>
    /// Measured/returned in teslas.
    /// </summary>
    public float GenerateInfoMagneticField(SpectralType spectralType)
    {
        Info_MagneticField = spectralType switch
        {
            SpectralType.O => Random.Range(0.05f, 1.0f),
            SpectralType.B => Random.Range(0.03f, 0.05f),
            SpectralType.A => Random.Range(0.01f, 0.03f),
            SpectralType.F => Random.Range(0.003f, 0.01f),
            SpectralType.G => Random.Range(0.001f, 0.003f),
            SpectralType.K => Random.Range(0.0005f, 0.001f),
            SpectralType.M => Random.Range(0.0001f, 0.0005f),
            _ => Info_MagneticField,
        };
        return Info_MagneticField;
    }

    /// <summary>
    /// Used as the base color in the star's shader graph.
    /// (update to include hydrogen lines later as a factor?)
    /// </summary>
    public Color GenerateGOChromaticity(SpectralType spectralType)
    {
        GO_Chromaticity = spectralType switch
        {
            SpectralType.O => new Color(146, 181, 255, 255),
            SpectralType.B => new Color(162, 192, 255, 255),
            SpectralType.A => new Color(213, 224, 255, 255),
            SpectralType.F => new Color(249, 245, 255, 255),
            SpectralType.G => new Color(255, 237, 227, 255),
            SpectralType.K => new Color(255, 218, 181, 255),
            SpectralType.M => new Color(255, 181, 108, 255),
            _ => GO_Chromaticity,
        };
        return GO_Chromaticity;
    }

    /// <summary>
    /// Generates a new random color to be used in the star objects _CellColor shader property.
    /// Said number is generated within a range of values based on the stars' chromaticity.
    /// </summary>
    public Color GenerateGOCellColor()
    {
        // the range, -/+ the chromaticity,
        // to be filtered through for r, g, and b values of our output Color
        const float RGB_RANGE = 255;

        GO_CellColor = new Color(
            GO_Chromaticity.r + Random.Range(-RGB_RANGE, RGB_RANGE),
            GO_Chromaticity.g + Random.Range(-RGB_RANGE, RGB_RANGE),
            GO_Chromaticity.b + Random.Range(-RGB_RANGE, RGB_RANGE),
            GO_Chromaticity.a);
        return GO_CellColor;
    }

    /// <summary>
    /// Returns a random number of planets for the star system.
    /// </summary>
    public int GenerateNumPlanets()
    {
        int numPlanets = Random.Range(0, 16);
        PlanetCount = numPlanets;
        return PlanetCount;
    }

    /// <summary>
    /// Generates variability values for the star.
    /// Rolls to see if star will have variability at all.
    /// Can be intrinsic or extrinsic.
    /// Or none, in which case output = 0f.
    /// </summary>
    public float GenerateInfoIntrinsicVariability(SpectralType spectralType)
    {
        // ~60% of all known stars have some variability 
        // choose what type of variability the star will have, if it is variable
        if (Random.Range(0, 2) == 0)
        {
            ExtrinsicVariability = true;
            IntrinsicVariability = false;
        }
        else
        {
            ExtrinsicVariability = false;
            IntrinsicVariability = true;
        }

        // other ~40% of known stars, no variability at all
        if (Random.Range(0f, 1f) < 0.4f)
        {
            ExtrinsicVariability = false;
            IntrinsicVariability = false;
            Info_Variability = 0f;
        }
        else
        {
            Info_Variability = spectralType switch
            {
                SpectralType.O => Random.Range(0.05f, 0.15f),
                SpectralType.B => Random.Range(0.03f, 0.10f),
                SpectralType.A => Random.Range(0.02f, 0.08f),
                SpectralType.F => Random.Range(0.01f, 0.06f),
                SpectralType.G => Random.Range(0.01f, 0.05f),
                SpectralType.K => Random.Range(0.01f, 0.03f),
                SpectralType.M => Random.Range(0.01f, 0.02f),
                _ => 0f,
            };
        }
        return Info_Variability;
    }

    /// <summary>
    /// Each star class contains set compounds. (not random)
    /// Each stars' compounds have a random % though.
    /// </summary>
    public void GenerateInfoMetallicity(SpectralType spectralType)
    {
        switch (spectralType)
        {
            case SpectralType.O:
                Info_Metallicity.Add("H", Random.Range(74f, 76f));
                Info_Metallicity.Add("He", Random.Range(24f, 26f));
                break;
            case SpectralType.B:
                Info_Metallicity.Add("H", Random.Range(58f, 70f));
                Info_Metallicity.Add("He", Random.Range(28f, 42f));
                Info_Metallicity.Add("C", Random.Range(0.1f, 2f));
                Info_Metallicity.Add("N", Random.Range(0.1f, 2f));
                Info_Metallicity.Add("O", Random.Range(0.1f, 2f));
                break;
            case SpectralType.A:
                Info_Metallicity.Add("H", Random.Range(71f, 74f));
                Info_Metallicity.Add("He", Random.Range(25f, 28f));
                Info_Metallicity.Add("C", Random.Range(0.1f, 2f));
                Info_Metallicity.Add("N", Random.Range(0.1f, 2f));
                Info_Metallicity.Add("O", Random.Range(0.1f, 2f));
                Info_Metallicity.Add("Ne", Random.Range(0.1f, 2f));
                break;
            case SpectralType.F:
                Info_Metallicity.Add("H", Random.Range(54f, 64f));
                Info_Metallicity.Add("He", Random.Range(35f, 45f));
                Info_Metallicity.Add("C", Random.Range(0.1f, 2f));
                Info_Metallicity.Add("N", Random.Range(0.1f, 2f));
                Info_Metallicity.Add("O", Random.Range(0.1f, 2f));
                Info_Metallicity.Add("Ne", Random.Range(0.1f, 2f));
                Info_Metallicity.Add("Fe", Random.Range(0.1f, 2f));
                break;
            case SpectralType.G:
                Info_Metallicity.Add("H", Random.Range(74f, 84f));
                Info_Metallicity.Add("He", Random.Range(14f, 24f));
                Info_Metallicity.Add("C", Random.Range(0.1f, 2f));
                Info_Metallicity.Add("N", Random.Range(0.1f, 2f));
                Info_Metallicity.Add("O", Random.Range(0.1f, 2f));
                Info_Metallicity.Add("Ne", Random.Range(0.1f, 2f));
                Info_Metallicity.Add("Fe", Random.Range(0.1f, 2f));
                break;
            case SpectralType.K:
                Info_Metallicity.Add("H", Random.Range(56f, 64f));
                Info_Metallicity.Add("He", Random.Range(36f, 44f));
                Info_Metallicity.Add("C", Random.Range(0.1f, 2f));
                Info_Metallicity.Add("N", Random.Range(0.1f, 2f));
                Info_Metallicity.Add("O", Random.Range(0.1f, 2f));
                Info_Metallicity.Add("Ne", Random.Range(0.1f, 2f));
                Info_Metallicity.Add("Fe", Random.Range(0.1f, 2f));
                Info_Metallicity.Add("Si", Random.Range(0.1f, 2f));
                Info_Metallicity.Add("Mg", Random.Range(0.1f, 2f));
                break;
            case SpectralType.M:
                Info_Metallicity.Add("H", Random.Range(36f, 56f));
                Info_Metallicity.Add("He", Random.Range(44f, 64f));
                Info_Metallicity.Add("C", Random.Range(0.1f, 2f));
                Info_Metallicity.Add("N", Random.Range(0.1f, 2f));
                Info_Metallicity.Add("O", Random.Range(0.1f, 2f));
                Info_Metallicity.Add("Ne", Random.Range(0.1f, 2f));
                Info_Metallicity.Add("Fe", Random.Range(0.1f, 2f));
                Info_Metallicity.Add("Si", Random.Range(0.1f, 2f));
                Info_Metallicity.Add("Mg", Random.Range(0.1f, 2f));
                Info_Metallicity.Add("S", Random.Range(0.1f, 2f));
                Info_Metallicity.Add("Cl", Random.Range(0.1f, 2f));
                Info_Metallicity.Add("K", Random.Range(0.1f, 2f));
                break;
            case SpectralType.Unknown:
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(spectralType), spectralType, null);
        }
    }

    /// <summary>
    /// Stars' habitable zone inner radius for game object.
    /// </summary>
    public float GenerateGOHabitableRangeInner(SpectralType spectralType)
    {
        float zoneInner = spectralType switch
        {
            SpectralType.O => 0.0f,
            SpectralType.B => 2.2f,
            SpectralType.A => 1.6f,
            SpectralType.F => 1.1f,
            SpectralType.G => 0.95f,
            SpectralType.K => 0.37f,
            SpectralType.M => 0.08f,
            _ => 0.0f,
        };

        // Calculate the inner habitable zone boundary
        GO_HabitableRangeInner = Mathf.Sqrt(Info_Luminosity) * zoneInner;

        return GO_HabitableRangeInner;
    }

    /// <summary>
    /// Stars' habitable zone outer radius for game object.
    /// </summary>
    public float GenerateGOHabitableRangeOuter(SpectralType spectralType)
    {
        float zoneOuter = spectralType switch
        {
            SpectralType.O => 0.0f,
            SpectralType.B => 15.0f,
            SpectralType.A => 3.0f,
            SpectralType.F => 1.5f,
            SpectralType.G => 1.4f,
            SpectralType.K => 0.73f,
            SpectralType.M => 0.24f,
            _ => 0.0f,
        };

        // Calculate the outer habitable zone boundary
        GO_HabitableRangeOuter = Mathf.Sqrt(Info_Luminosity) * zoneOuter;

        return GO_HabitableRangeOuter;
    }
}
