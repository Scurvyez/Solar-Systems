using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class StarProperties : MonoBehaviour
{
    public enum SpectralType { O, B, A, F, G, K, M, Unknown }
    public SpectralType SpectralClass { get; set; }
    public string SystemName { get; set; }
    public double Age { get; set; }
    public double Mass { get; set; }
    public double Radius { get; set; }
    public double Luminosity { get; set; }
    public double Temperature { get; set; }
    public double Rotation { get; set; }
    public double MagneticField { get; set; }
    public SerializableDictionary<string, float> Metallicity { get; set; }
    public Color Chromaticity { get; set; }
    public Color CellColor { get; set; }
    public double Variability { get; set; }
    public Vector3 Size { get; set; }

    private const double SolLuminosity = 3.846e26; // in watts
    private const double SolMassKG = 1.9881e30; // in kg
    private const double SolRadiusM = 6.96342E8; // in m
    private const double StefanBoltzmannConstant = 5.670373E-8;

    public bool ExtrinsicVariability;
    public bool IntrinsicVariability;

    private string Letters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
    private string Numbers = "0123456789";

    // Arrays to store the consonants and vowels
    private char[] Consonants = { 'b', 'c', 'd', 'f', 'g', 'h', 'j', 'k', 'l', 'm', 'n', 'p', 'q', 'r', 's', 't', 'v', 'w', 'x', 'y', 'z' };
    private char[] Vowels = { 'a', 'e', 'i', 'o', 'u' };

    // Roman numerals for hyphenated names
    private string[] RomanNumerals = { "I", "II", "III", "IV", "V", "VI", "VII", "VIII", "IX", "X" };

    // Random number generator
    private System.Random nameGenRandom = new ();

    /*
    public StarProperties instance;

    private void Awake()
    {
        instance = this;
    }
    */

    void Start()
    {
        
    }

    /// <summary>
    /// Chooses between 2 available naming methods.
    /// Randomly picks one and returns the generated star system name.
    /// </summary>
    public string PickNamingMethodAndGenerate()
    {
        if (nameGenRandom.Next(100) < 8)
        {
            SystemName = GenerateStarSystemNameSemiUnique();
        }
        else
        {
            SystemName = GenerateStarSystemNameGeneric();
        }

        return SystemName;
    }
    
    /// <summary>
    /// Generates a semi-unique star system name using the English alphabet.
    /// Alternating patterns of consonants and vowels.
    /// (Lazy naming method.)
    /// </summary>
    private string GenerateStarSystemNameSemiUnique()
    {
        // Set the name length to a random number between 3 and 9
        int nameLength = nameGenRandom.Next(3, 10);

        // Start with an empty string
        string tempSystemName = "";

        // Loop for the specified number of characters in the name
        for (int i = 0; i < nameLength; i++)
        {
            // Choose a random character from either the consonants or vowels
            char letter = i % 2 == 0 ? Consonants[nameGenRandom.Next(Consonants.Length)] : Vowels[nameGenRandom.Next(Vowels.Length)];

            // Add the letter to the name
            tempSystemName += letter;
        }

        // Capitalize the first letter of the name
        tempSystemName = char.ToUpper(tempSystemName[0]) + tempSystemName.Substring(1);

        // Check if the name should be hyphenated
        if (nameGenRandom.Next(100) < 50)
        {
            // If so, choose a random Roman numeral and add it to the name
            string numeral = RomanNumerals[nameGenRandom.Next(RomanNumerals.Length)];
            tempSystemName = tempSystemName + "-" + numeral;
        }

        // Return the generated name
        return tempSystemName;
    }

    /// <summary>
    /// Generates a standard star system name.
    /// Based on how the International Astronomical Union (IAU) names stars.
    /// A simple mix of "x" letters, a "-", followed by a mix of "y" numbers.
    /// </summary>
    private string GenerateStarSystemNameGeneric()
    {
        string tempSystemName = "";
        int nameLength = Random.Range(5, 14);
        bool addingLetters = true;
        for (int i = 0; i < nameLength; i++)
        {
            if (addingLetters)
            {
                tempSystemName += Letters[Random.Range(0, Letters.Length)];
            }
            else
            {
                tempSystemName += Numbers[Random.Range(0, Numbers.Length)];
            }

            if (i == (nameLength / 2) - 1)
            {
                tempSystemName += "-";
                addingLetters = false;
            }
        }
        return tempSystemName;
    }
    
    /// <summary>
    /// Generates a random yet realistic radius for the star.
    /// Used later to rescale star GameObject by applying final value to a Vector3.
    /// Used to calculate luminosity.
    /// Measured in solar radii.
    /// </summary>
    public double GenerateRadius(SpectralType spectralType)
    {
        float randomNumber = Random.Range(0, 250);

        Radius = spectralType switch
        {
            SpectralType.O when randomNumber == 250 => Random.Range(1250f, 1500f),
            SpectralType.O when randomNumber <= 249 && randomNumber >= 240 => Random.Range(1000f, 1250f),
            SpectralType.O when randomNumber <= 239 && randomNumber >= 230 => Random.Range(800f, 1000f),
            SpectralType.O when randomNumber <= 229 && randomNumber >= 220 => Random.Range(500f, 800f),
            SpectralType.O when randomNumber <= 219 && randomNumber >= 200 => Random.Range(100f, 500f),
            SpectralType.O when randomNumber <= 199 && randomNumber >= 175 => Random.Range(30f, 100f),
            SpectralType.O when randomNumber <= 174 && randomNumber >= 125 => Random.Range(10f, 30f),
            SpectralType.O => Random.Range(6.6f, 10f),
            SpectralType.B => Random.Range(1.8f, 6.6f),
            SpectralType.A => Random.Range(1.4f, 1.8f),
            SpectralType.F => Random.Range(1.15f, 1.4f),
            SpectralType.G => Random.Range(0.96f, 1.15f),
            SpectralType.K => Random.Range(0.7f, 0.96f),
            SpectralType.M => Random.Range(0.08f, 0.7f),
            _ => Radius,
        } * SolRadiusM; // convert to solar radii
        return Radius;
    }

    /// <summary>
    /// Generates a random yet realistic surface temperature value for the star.
    /// Used to calculate luminosity.
    /// Measured in kelvins.
    /// </summary>
    public double GenerateTemperature(SpectralType spectralType)
    {
        Temperature = spectralType switch
        {
            SpectralType.O => Random.Range(30000f, 60000f),
            SpectralType.B => Random.Range(10000f, 30000f),
            SpectralType.A => Random.Range(7500f, 10000f),
            SpectralType.F => Random.Range(6000f, 7500f),
            SpectralType.G => Random.Range(5200f, 6000f),
            SpectralType.K => Random.Range(3700f, 5200f),
            SpectralType.M => Random.Range(2400f, 3700f),
            _ => Temperature,
        };
        return Temperature;
    }

    /// <summary>
    /// Generates a random yet realistic luminosity for the star.
    /// Calculated via the stars radius and temperature.
    /// Used to calculate a stars mass.
    /// Measured in solar luminosities.
    /// </summary>
    public double GenerateLuminosity(SpectralType spectralType)
    {
        double starRadius = GenerateRadius(spectralType);
        double starTemperature = GenerateTemperature(spectralType);
        Luminosity = ((float)StefanBoltzmannConstant * (4 * Mathf.PI * Mathf.Pow((float)starRadius, 2)) * Mathf.Pow((float)starTemperature, 4));
        Luminosity /= SolLuminosity; // convert to solar luminosity
        return Luminosity;
    }

    /// <summary>
    /// Generates a random yet realistic mass value for the star.
    /// Calculated via the mass - luminosity relation.
    /// Measured in solar masses.
    /// </summary>
    public double GenerateMass(SpectralType spectralType)
    {
        double starLuminosity = GenerateLuminosity(spectralType);
        Mass = Mathf.Pow((float)starLuminosity / 1f, 3f / 4f);
        return Mass;
    }

    /// <summary>
    /// Generates a random yet realistic age for the star.
    /// Bigger stars (O class) burn hotter and faster, so shorter lives.
    /// Smaller stars (M class) burn much slower and lower, so they have longer lives.
    /// Measured in years.
    /// </summary>
    public double GenerateAge(SpectralType spectralType)
    {
        Age = spectralType switch
        {
            SpectralType.O => Random.Range(5000000, 10000000),
            SpectralType.B => Random.Range(50000000, 100000000),
            SpectralType.A => Random.Range(500000000, 1000000000),
            SpectralType.F => Random.Range(2500000000, 5000000000),
            SpectralType.G => Random.Range(5000000000, 10000000000),
            SpectralType.K => Random.Range(25000000000, 50000000000),
            SpectralType.M => Random.Range(50000000000, 100000000000),
            _ => Age,
        };
        return Age;
    }

    /// <summary>
    /// Generates a random yet realistic axial rotation value for the star.
    /// Measured in kilometers / second.
    /// </summary>
    public double GenerateRotation(SpectralType spectralType)
    {
        Rotation = spectralType switch
        {
            SpectralType.O => Random.Range(20f, 30f),
            SpectralType.B => Random.Range(5f, 15f),
            SpectralType.A => Random.Range(1f, 10f),
            SpectralType.F => Random.Range(0.5f, 5f),
            SpectralType.G => Random.Range(0.1f, 1f),
            SpectralType.K => Random.Range(0.05f, 0.5f),
            SpectralType.M => Random.Range(0.01f, 0.05f),
            _ => Rotation,
        };
        return Rotation;
    }

    /// <summary>
    /// Generates a random yet realistic magnetic field value for the star.
    /// Used as a factor in calculating final axial rotation.
    /// Measured in teslas.
    /// </summary>
    public double GenerateMagneticField(SpectralType spectralType)
    {
        MagneticField = spectralType switch
        {
            SpectralType.O => Random.Range(0.05f, 1.0f),
            SpectralType.B => Random.Range(0.03f, 0.05f),
            SpectralType.A => Random.Range(0.01f, 0.03f),
            SpectralType.F => Random.Range(0.003f, 0.01f),
            SpectralType.G => Random.Range(0.001f, 0.003f),
            SpectralType.K => Random.Range(0.0005f, 0.001f),
            SpectralType.M => Random.Range(0.0001f, 0.0005f),
            _ => MagneticField,
        };
        return MagneticField;
    }

    /// <summary>
    /// Generates a realistic color for the star object.
    /// Used as the base color in the star's shader graph.
    /// (update to include hydrogen lines later as a factor?)
    /// </summary>
    public Color GenerateChromaticity(SpectralType spectralType)
    {
        Chromaticity = spectralType switch
        {
            SpectralType.O => new Color(146, 181, 255, 255),
            SpectralType.B => new Color(162, 192, 255, 255),
            SpectralType.A => new Color(213, 224, 255, 255),
            SpectralType.F => new Color(249, 245, 255, 255),
            SpectralType.G => new Color(255, 237, 227, 255),
            SpectralType.K => new Color(255, 218, 181, 255),
            SpectralType.M => new Color(255, 181, 108, 255),
            _ => Chromaticity,
        };
        return Chromaticity;
    }

    /// <summary>
    /// Generates a new random color to be used in the star objects _CellColor shader property.
    /// Said number is generated within a range of values based on the stars' chromaticity.
    /// </summary>
    public Color RandomCellColorGenerator()
    {
        // the range, -/+ the chromaticity,
        // to be filtered through for r, g, and b values of our output Color
        float rGBRange = 255;

        CellColor = new(
            Chromaticity.r + Random.Range(-rGBRange, rGBRange),
            Chromaticity.g + Random.Range(-rGBRange, rGBRange),
            Chromaticity.b + Random.Range(-rGBRange, rGBRange),
            Chromaticity.a);
        return CellColor;
    }
    
    /// <summary>
    /// Generates variability values for the star.
    /// Rolls to see if star will have variability at all.
    /// Can be intrinsic or extrinsic.
    /// Or none, in which case output = 0f.
    /// </summary>
    public double GenerateIntrinsicVariability(SpectralType spectralType)
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
            Variability = 0f;
        }
        else
        {
            Variability = spectralType switch
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
        return Variability;
    }

    /// <summary>
    /// Generates random yet realistic chemical compositions for the star.
    /// Each star class contains set compounds. (not random)
    /// Each stars' compounds have a random % though.
    /// </summary>
    public void GenerateComposition(SpectralType spectralType)
    {
        switch (spectralType)
        {
            case SpectralType.O:
                Metallicity.Add("Hydrogen", Random.Range(74f, 76f));
                Metallicity.Add("Helium", Random.Range(24f, 26f));
                break;
            case SpectralType.B:
                Metallicity.Add("Hydrogen", Random.Range(58f, 70f));
                Metallicity.Add("Helium", Random.Range(28f, 42f));
                Metallicity.Add("Carbon", Random.Range(0.1f, 2f));
                Metallicity.Add("Nitrogen", Random.Range(0.1f, 2f));
                Metallicity.Add("Oxygen", Random.Range(0.1f, 2f));
                break;
            case SpectralType.A:
                Metallicity.Add("Hydrogen", Random.Range(71f, 74f));
                Metallicity.Add("Helium", Random.Range(25f, 28f));
                Metallicity.Add("Carbon", Random.Range(0.1f, 2f));
                Metallicity.Add("Nitrogen", Random.Range(0.1f, 2f));
                Metallicity.Add("Oxygen", Random.Range(0.1f, 2f));
                Metallicity.Add("Neon", Random.Range(0.1f, 2f));
                break;
            case SpectralType.F:
                Metallicity.Add("Hydrogen", Random.Range(54f, 64f));
                Metallicity.Add("Helium", Random.Range(35f, 45f));
                Metallicity.Add("Carbon", Random.Range(0.1f, 2f));
                Metallicity.Add("Nitrogen", Random.Range(0.1f, 2f));
                Metallicity.Add("Oxygen", Random.Range(0.1f, 2f));
                Metallicity.Add("Neon", Random.Range(0.1f, 2f));
                Metallicity.Add("Iron", Random.Range(0.1f, 2f));
                break;
            case SpectralType.G:
                Metallicity.Add("Hydrogen", Random.Range(74f, 84f));
                Metallicity.Add("Helium", Random.Range(14f, 24f));
                Metallicity.Add("Carbon", Random.Range(0.1f, 2f));
                Metallicity.Add("Nitrogen", Random.Range(0.1f, 2f));
                Metallicity.Add("Oxygen", Random.Range(0.1f, 2f));
                Metallicity.Add("Neon", Random.Range(0.1f, 2f));
                Metallicity.Add("Iron", Random.Range(0.1f, 2f));
                break;
            case SpectralType.K:
                Metallicity.Add("Hydrogen", Random.Range(56f, 64f));
                Metallicity.Add("Helium", Random.Range(36f, 44f));
                Metallicity.Add("Carbon", Random.Range(0.1f, 2f));
                Metallicity.Add("Nitrogen", Random.Range(0.1f, 2f));
                Metallicity.Add("Oxygen", Random.Range(0.1f, 2f));
                Metallicity.Add("Neon", Random.Range(0.1f, 2f));
                Metallicity.Add("Iron", Random.Range(0.1f, 2f));
                Metallicity.Add("Silicon", Random.Range(0.1f, 2f));
                Metallicity.Add("Magnesium", Random.Range(0.1f, 2f));
                break;
            case SpectralType.M:
                Metallicity.Add("Hydrogen", Random.Range(36f, 56f));
                Metallicity.Add("Helium", Random.Range(44f, 64f));
                Metallicity.Add("Carbon", Random.Range(0.1f, 2f));
                Metallicity.Add("Nitrogen", Random.Range(0.1f, 2f));
                Metallicity.Add("Oxygen", Random.Range(0.1f, 2f));
                Metallicity.Add("Neon", Random.Range(0.1f, 2f));
                Metallicity.Add("Iron", Random.Range(0.1f, 2f));
                Metallicity.Add("Silicon", Random.Range(0.1f, 2f));
                Metallicity.Add("Magnesium", Random.Range(0.1f, 2f));
                Metallicity.Add("Sulfur", Random.Range(0.1f, 2f));
                Metallicity.Add("Chlorine", Random.Range(0.1f, 2f));
                Metallicity.Add("Potassium", Random.Range(0.1f, 2f));
                break;
            default:
                break;
        }
    }
}
