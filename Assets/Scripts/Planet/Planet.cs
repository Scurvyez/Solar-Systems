using System.Collections.Generic;
using UnityEngine;
using Utils;
using Random = UnityEngine.Random;

public class Planet
{
    public string PlanetType { get; set; }
    public int GO_OrbitPosition { get; set; }
    public string Info_Name { get; set; }
    public float Info_Mass { get; set; }
    public float GO_Radius { get; set; }
    public float Info_Radius { get; set; }
    public float Info_RotationalPeriod { get; set; }
    public float Info_OrbitalPeriod { get; set; }
    public Vector3 Info_FocusPoint { get; set; }
    public float Info_AxialTilt { get; set; }
    public float Info_SurfaceTemperature { get; set; }
    public bool Info_HasAtmosphere { get; set; }
    public bool Info_IsHabitable { get; set; }
    public bool Info_HasRings { get; set; }
    public float GO_InnerRingRadius { get; set; }
    public float GO_OuterRingRadius { get; set; }
    public float Info_MeanDensity { get; set; }
    public float Info_SurfacePressure { get; set; }
    public float Info_SurfaceGravity { get; set; }
    public float Info_EscapeVelocity { get; set; }
    public float Info_Albedo { get; set; }
    public float Info_MagneticFieldStrength { get; set; }
    public int Info_Moons { get; set; }
    public SerializableDictionary<string, float> Info_Composition { get; set; }
    public SerializableDictionary<string, float> Info_AtmosphereComposition { get; set; }
    
    /// <summary>
    /// 20% chance of having an atmosphere.
    /// </summary>
    public virtual bool HasRandomAtmosphere()
    {
        Info_HasAtmosphere = true;
        return Info_HasAtmosphere;
    }

    /// <summary>
    /// Returns a height if the planet has an atmosphere.
    /// </summary>
    public virtual float AtmosphereHeight(bool hasAtmosphere, float planetRadius)
    {
        return (!hasAtmosphere) ? 0f : planetRadius * 2.5f;
    }

    public virtual Color GetColor()
    {
        return Color.white; // default base color of all planets
    }

    /// <summary>
    /// The radius of the planet GameObject in our scene.
    /// </summary>
    public virtual float GenerateGORadius()
    {
        GO_Radius = 1f;
        return GO_Radius;
    }
    
    /// <summary>
    /// Returns a value in kilometers.
    /// </summary>
    public virtual float GenerateInfoRadius()
    {
        Info_Radius = Random.Range(1000f, 15000f);
        return Info_Radius;
    }

    /// <summary>
    /// The length of time it takes the planet to make one full revolution around its parent star.
    /// Measured in Earth days.
    /// </summary>
    public virtual float GenerateOrbitalPeriod()
    {
        Info_OrbitalPeriod = Random.Range(9f, 5000f);
        return Info_OrbitalPeriod;
    }

    public virtual Vector3 GenerateFocusPoint()
    {
        // Define a range of x-coordinates for the second focus point
        float minDistanceKm = 800000f; // 800 K km
        float maxDistanceKm = 500000000f; // 500 million km
        float minX = minDistanceKm / 149597870.7f; // convert to AU
        float maxX = maxDistanceKm / 149597870.7f; // convert to AU

        // Generate a random x-coordinate within the range
        float x = Random.Range(minX, maxX);

        // Construct the second focus point vector
        Info_FocusPoint = new Vector3(x, 0f, 0f);
        Info_FocusPoint *= 1000; // world-space factor

        return Info_FocusPoint / 2;
    }

    /// <summary>
    /// Generates a final mass value for the planet.
    /// Returned in kilograms.
    /// </summary> 
    public virtual float GenerateMass(float planetRadius, float planetMeanDensity)
    {
        float radiusInMeters = planetRadius * 1000f;
        float volInCubicM = (4f / 3f) * Mathf.PI * Mathf.Pow(radiusInMeters, 3f);
        float planetMassInKG = planetMeanDensity * volInCubicM;

        Info_Mass = planetMassInKG;
        return Info_Mass;
    }
    
    public virtual float GenerateRotationalPeriod()
    {
        Info_RotationalPeriod = Random.Range(9f, 5000f); // Measured in Earth hours
        return Info_RotationalPeriod;
    }

    public virtual float GenerateAxialTilt()
    {
        Info_AxialTilt = Random.Range(0f, 35f); // Measured in degrees
        return Info_AxialTilt;
    }
    
    public virtual float GenerateSurfaceTemperature()
    {
        float temperatureFinal = Random.value switch
        {
            < 0.3f => Random.Range(200f, 300f),
            < 0.5f => Random.Range(300f, 500f),
            < 0.7f => Random.Range(500f, 700f),
            _ => Random.Range(700f, 1000f),
        };

        Info_SurfaceTemperature = temperatureFinal; // Kelvin
        return Info_SurfaceTemperature;
    }

    public virtual SerializableDictionary<string, float> GenerateComposition()
    {
        // initialize the Dict
        Info_Composition = new SerializableDictionary<string, float>();
        return Info_Composition;
    }
    
    public virtual SerializableDictionary<string, float> GenerateAtmosphereComposition(bool hasAtmosphere)
    {
        // Initialize the dictionary
        Info_AtmosphereComposition = new SerializableDictionary<string, float>();
        if (!hasAtmosphere)
        {
            return Info_AtmosphereComposition;
        }

        float total = 100f;

            float oxygen = Random.Range(0f, total);
            Info_AtmosphereComposition.Add("O", oxygen);

            total -= oxygen;
            float nitrogen = Random.Range(0f, total);
            Info_AtmosphereComposition.Add("N", nitrogen);

            total -= nitrogen;
            float carbonDioxide = Random.Range(0f, total);
            Info_AtmosphereComposition.Add("CO2", carbonDioxide);

            total -= carbonDioxide;
            float methane = Random.Range(0f, total);
            Info_AtmosphereComposition.Add("CH4", methane);

            total -= methane;
            float hydrogen = Random.Range(0f, total);
            Info_AtmosphereComposition.Add("H", hydrogen);

            total -= hydrogen;
            float helium = Random.Range(0f, total);
            Info_AtmosphereComposition.Add("He", helium);

            total -= helium;
            float neon = Random.Range(0f, total);
            Info_AtmosphereComposition.Add("Ne", neon);

            total -= neon;
            float argon = Random.Range(0f, total);
            Info_AtmosphereComposition.Add("Ar", argon);

            total -= argon;
            float krypton = Random.Range(0f, total);
            Info_AtmosphereComposition.Add("Kr", krypton);

            total -= krypton;
            float xenon = Random.Range(0f, total);
            Info_AtmosphereComposition.Add("Xe", xenon);

            total -= xenon;
            float sulfurDioxide = Random.Range(0f, total);
            Info_AtmosphereComposition.Add("SO2", sulfurDioxide);

            total -= sulfurDioxide;
            float nitrogenOxides = Random.Range(0f, total);
            Info_AtmosphereComposition.Add("NO", nitrogenOxides);

            total -= nitrogenOxides;
            float ozone = Random.Range(0f, total);
            Info_AtmosphereComposition.Add("O3", ozone);

            total -= ozone;
            float waterVapor = total;
            Info_AtmosphereComposition.Add("H2O", waterVapor);

            return Info_AtmosphereComposition;
    }

    public virtual bool IsRandomlyHabitable(float habRangeInner, float habRangeOuter, Vector3 planetFocusPoint)
    {
        if (!HasLiquidWater(Info_SurfaceTemperature, Info_SurfacePressure, Info_HasAtmosphere)) return false;
        return planetFocusPoint.x > habRangeInner - (planetFocusPoint.x * 1 / 2) && planetFocusPoint.x < habRangeOuter + (planetFocusPoint.x * 1 / 2);
    }
    
    /// <summary>
    /// 10% chance that a planet will have rings of asteroids orbiting it.
    /// </summary>
    public virtual bool HasRandomRings()
    {
        Info_HasRings = Random.value < 0.1f;
        return Info_HasRings;
    }

    public virtual float GenerateInnerRingRadius()
    {
        GO_InnerRingRadius = GO_Radius + Random.Range(3f, 7.5f);
        return GO_InnerRingRadius;
    }

    public virtual float GenerateOuterRingRadius()
    {
        GO_OuterRingRadius = GO_InnerRingRadius + Random.Range(3f, 15.5f);
        return GO_OuterRingRadius;
    }
    
    /// <summary>
    /// 
    /// </summary>
    public virtual float GenerateMeanDensity(SerializableDictionary<string, float> planetComposition)
    {
        Info_MeanDensity = 0;
        float totalProportion = 0;

        foreach (KeyValuePair<string, float> element in planetComposition)
        {
            float atomicMass = AtomicMass.GetAtomicMass(element.Key);
            Info_MeanDensity += atomicMass * element.Value;
            totalProportion += element.Value;
        }
        return Info_MeanDensity / totalProportion;
    }

    /// <summary>
    /// Measured in Earth atmospheres.
    /// </summary>
    public virtual float GenerateSurfacePressure(SerializableDictionary<string, float> planetAtmosphereComposition, float planetSurfaceGravity, float planetTemperature)
    {
        // Dictionary to store molar masses of common atmospheric gases (in kg/mol)
        Dictionary<string, float> molarMasses = new Dictionary<string, float>
        {
            { "O", 32f },
            { "N", 28f },
            { "CO2", 44f },
            { "CH4", 16f },
            { "H", 2f },
            { "He", 4f },
            { "Ne", 20f },
            { "Ar", 40f },
            { "Kr", 84f },
            { "Xe", 131f },
            { "SO2", 64f },
            { "NO", 30f },
            { "O3", 48f },
            { "H2O", 18f }
        };
        
        float totalPressure = 0f;

        foreach (var gas in planetAtmosphereComposition)
        {
            string gasName = gas.Key;
            float gasProportion = gas.Value;

            if (!molarMasses.TryGetValue(gasName, out float molarMass)) continue;
            // Convert molar mass from g/mol to kg/mol (1 g/mol = 0.001 kg/mol)
            molarMass *= 10f;
            
            // Calculate partial pressure using the ideal gas law: P = (n/V)RT, where n is the number of moles, V is volume,
            // R is the gas constant, and T is temperature. Here, we'll use a proportional approach.
            float partialPressure = (gasProportion * planetSurfaceGravity * molarMass * planetTemperature) / ConstantsUtil.GAS_CONSTANT;
            totalPressure += partialPressure;
        }

        float surfacePressureInAtm = totalPressure / ConstantsUtil.EARTH_ATMOSPHERIC_PRESSURE;
        Info_SurfacePressure = surfacePressureInAtm; // Convert to Earth atmospheres
        return Info_SurfacePressure;
    }
    
    /// <summary>
    /// Checks if the planet's surface temperature is within the habitable range.
    /// Checks if the planet has an atmosphere.
    /// Checks if the atmospheric pressure is within the range required for liquid water.
    /// </summary>
    public virtual bool HasLiquidWater(float planetSurfaceTemp, float planetSurfacePressure, bool hasAtmosphere)
    {
        if (planetSurfaceTemp is < 273 or > 373) return false;
        if (!hasAtmosphere) return false;
        return planetSurfacePressure is >= 0.1f and <= 100f;
    }

    /// <summary>
    /// Measured/returned in meters per second squared.
    /// </summary>
    public virtual float GenerateSurfaceGravity(float planetMass, float planetRadius)
    {
        float planetRadInM = planetRadius * 1000f;
        float surfaceGravity = ConstantsUtil.GRAVITY * planetMass / (planetRadInM * planetRadInM);

        Info_SurfaceGravity = surfaceGravity;
        return Info_SurfaceGravity;
    }
    
    /// <summary>
    /// Measured/returned in kilometers per second.
    /// </summary>
    public virtual float GenerateEscapeVelocity(float planetMass, float planetRadius)
    {
        float planetRadInM = planetRadius * 1000f;
        float escapeVelocityInMetersPerSecond = Mathf.Sqrt((2 * ConstantsUtil.GRAVITY * planetMass) / planetRadInM); // m/s

        Info_EscapeVelocity = escapeVelocityInMetersPerSecond / 1000f; // km/s
        return Info_EscapeVelocity;
    }

    public virtual float GenerateAlbedo()
    {
        Info_Albedo = Random.Range(0.1f, 0.9f);
        return Info_Albedo;
    }

    /// <summary>
    /// Measured in Gauss.
    /// </summary>
    public virtual float GenerateMagneticFieldStrength()
    {
        Info_MagneticFieldStrength = Random.Range(0f, 100f);
        return Info_MagneticFieldStrength;
    }

    public virtual int GenerateNumMoons()
    {
        int numMoons = 0;
        Info_Moons = numMoons;
        return Info_Moons;
    }
}
