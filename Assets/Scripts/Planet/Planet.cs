using System.Collections.Generic;
using UnityEngine;
using Utils;

public class Planet
{
    public string Name { get; set; }
    public float Mass { get; set; }
    public float Radius { get; set; }
    public float RotationalPeriod { get; set; }
    public float OrbitalPeriod { get; set; }
    public Vector3 FocusPoint { get; set; }
    public float AxialTilt { get; set; }
    public float SurfaceTemperature { get; set; }
    public bool HasAtmosphere { get; set; }
    public bool IsHabitable { get; set; }
    public bool HasRings { get; set; }
    public float InnerRingRadius { get; set; }
    public float OuterRingRadius { get; set; }
    public float MeanDensity { get; set; }
    public float SurfacePressure { get; set; }
    public float SurfaceGravity { get; set; }
    public float EscapeVelocity { get; set; }
    public float Albedo { get; set; }
    public float MagneticFieldStrength { get; set; }
    public SerializableDictionary<string, float> Composition { get; set; }
    public SerializableDictionary<string, float> AtmosphereComposition { get; set; }
    //public List<Moon> Moons { get; set; }
    
    /// <summary>
    /// 20% chance of having an atmosphere.
    /// </summary>
    public virtual bool HasRandomAtmosphere()
    {
        HasAtmosphere = Random.value < 0.2f;
        return HasAtmosphere;
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
        // Generate a random radius between 0.25 and 5.0 Earth radii
        //Radius = Random.Range(0.25f, 5.0f);
        Radius = 2.0f;
        return Radius;
    }

    /// <summary>
    /// The length of time it takes the planet to make one full revolution around its parent star.
    /// Measured in Earth days.
    /// </summary>
    public virtual float GenerateOrbitalPeriod()
    {
        OrbitalPeriod = Random.Range(9f, 5000f);
        return OrbitalPeriod;
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
        FocusPoint = new Vector3(x, 0f, 0f);
        FocusPoint *= 1000; // world-space factor

        return FocusPoint / 2;
    }

    /// <summary>
    /// Generates a final mass value for the planet.
    /// Calculated via the planets' focal point (AU), the gravitational constant (AU^3/MO/yr^2),
    /// the planets' orbital period (years) and the host star(s) mass (solar masses).
    /// </summary>
    public virtual float GenerateMass(float starMass, Vector3 planetFocusPoint, float planetOrbitalPeriod)
    {
        // Calculate the mass using the third law of Kepler
        Mass = 4f * Mathf.Pow(Mathf.PI, 2f) * Mathf.Pow(planetFocusPoint.x, 3f) / (ConstantsUtil.GRAVITY * Mathf.Pow(planetOrbitalPeriod, 2f) * starMass);
        return Mass;
    }
    
    public virtual float GenerateRotationalPeriod()
    {
        RotationalPeriod = Random.Range(9f, 5000f); // Measured in Earth hours
        return RotationalPeriod;
    }

    public virtual float GenerateAxialTilt()
    {
        AxialTilt = Random.Range(0f, 90f); // Measured in degrees
        return AxialTilt;
    }
    
    public virtual float GenerateSurfaceTemperature()
    {
        float temperatureFinal = Random.value switch
        {
            < 0.1f => Random.Range(200f, 300f),   // 10% chance for cold planets
            < 0.4f => Random.Range(300f, 500f),   // 30% chance for temperate planets
            < 0.7f => Random.Range(500f, 700f),   // 30% chance for warm planets
            _ => Random.Range(700f, 1000f),       // 30% chance for hot planets
        };

        SurfaceTemperature = temperatureFinal; // Kelvin
        return SurfaceTemperature;
    }

    public virtual SerializableDictionary<string, float> GenerateComposition()
    {
        // initialize the Dict
        Composition = new SerializableDictionary<string, float>();
        return Composition;
    }
    
    public virtual SerializableDictionary<string, float> GenerateAtmosphereComposition(bool hasAtmosphere, float habRangeInner, 
        float habRangeOuter, float planetSurfaceTemperature)
    {
        // Initialize the dictionary
        AtmosphereComposition = new SerializableDictionary<string, float>();
        if (!hasAtmosphere)
        {
            return AtmosphereComposition;
        }

        float total = 0f;

        // Define the probabilities of each element based on the planet's surface temperature
        float temperatureFactor = Mathf.Clamp01((planetSurfaceTemperature - 273f) / 1000f);

        // Element probabilities affected by temperature
        float oxygenProb = Random.value * (1f - Mathf.Clamp01((planetSurfaceTemperature - 273f) / 100f));
        float nitrogenProb = Random.value * (1f - Mathf.Clamp01((planetSurfaceTemperature - 273f) / 200f));
        float carbonDioxideProb = Random.value * (1f - Mathf.Clamp01((planetSurfaceTemperature - 273f) / 100f));
        float methaneProb = Random.value * (1f - Mathf.Clamp01((planetSurfaceTemperature - 273f) / 150f));
        float hydrogenProb = Random.value * (1f - Mathf.Clamp01((planetSurfaceTemperature - 273f) / 200f));
        float heliumProb = Random.value * (1f - Mathf.Clamp01((planetSurfaceTemperature - 273f) / 150f));
        float neonProb = Random.value * (1f - Mathf.Clamp01((planetSurfaceTemperature - 273f) / 200f));
        float argonProb = Random.value * (1f - Mathf.Clamp01((planetSurfaceTemperature - 273f) / 100f));
        float kryptonProb = Random.value * (1f - Mathf.Clamp01((planetSurfaceTemperature - 273f) / 200f));
        float xenonProb = Random.value * (1f - Mathf.Clamp01((planetSurfaceTemperature - 273f) / 300f));
        float sulfurDioxideProb = Random.value * (1f - Mathf.Clamp01((planetSurfaceTemperature - 273f) / 100f));
        float nitrogenOxidesProb = Random.value * (1f - Mathf.Clamp01((planetSurfaceTemperature - 273f) / 150f));
        float ozoneProb = Random.value * (1f - Mathf.Clamp01((planetSurfaceTemperature - 273f) / 150f));
        float waterVaporProb = Random.value * (1f - Mathf.Clamp01((planetSurfaceTemperature - 273f) / 100f));

        total = oxygenProb + nitrogenProb + carbonDioxideProb + methaneProb + hydrogenProb + heliumProb + neonProb + 
            argonProb + kryptonProb + xenonProb + sulfurDioxideProb + nitrogenOxidesProb + ozoneProb + waterVaporProb;

        AtmosphereComposition.Add("O", oxygenProb / total);
        AtmosphereComposition.Add("N", nitrogenProb / total);
        AtmosphereComposition.Add("CO2", carbonDioxideProb / total);
        AtmosphereComposition.Add("CH4", methaneProb / total);
        AtmosphereComposition.Add("H", hydrogenProb / total);
        AtmosphereComposition.Add("He", heliumProb / total);
        AtmosphereComposition.Add("Ne", neonProb / total);
        AtmosphereComposition.Add("Ar", argonProb / total);
        AtmosphereComposition.Add("Kr", kryptonProb / total);
        AtmosphereComposition.Add("Xe", xenonProb / total);
        AtmosphereComposition.Add("SO2", sulfurDioxideProb / total);
        AtmosphereComposition.Add("NO", nitrogenOxidesProb / total);
        AtmosphereComposition.Add("O3", ozoneProb / total);
        AtmosphereComposition.Add("H2O", waterVaporProb / total);

        return AtmosphereComposition;
    }

    public virtual bool IsRandomlyHabitable(float habRangeInner, float habRangeOuter, Vector3 planetFocusPoint)
    {
        if (!HasLiquidWater(SurfaceTemperature, SurfacePressure, HasAtmosphere)) return false;
        return planetFocusPoint.x > habRangeInner - (planetFocusPoint.x * 1 / 2) && planetFocusPoint.x < habRangeOuter + (planetFocusPoint.x * 1 / 2);
    }
    
    /// <summary>
    /// 10% chance that a planet will have rings of asteroids orbiting it.
    /// </summary>
    public virtual bool HasRandomRings()
    {
        HasRings = Random.value < 0.1f;
        return HasRings;
    }

    public virtual float GenerateInnerRingRadius()
    {
        InnerRingRadius = Radius + Random.Range(3f, 7.5f);
        return InnerRingRadius;
    }

    public virtual float GenerateOuterRingRadius()
    {
        OuterRingRadius = InnerRingRadius + Random.Range(3f, 15.5f);
        return OuterRingRadius;
    }
    
    public virtual float GenerateMeanDensity(SerializableDictionary<string, float> planetComposition)
    {
        MeanDensity = 0;
        float totalProportion = 0;

        foreach (KeyValuePair<string, float> element in planetComposition)
        {
            float atomicMass = AtomicMass.GetAtomicMass(element.Key);
            MeanDensity += atomicMass * element.Value;
            totalProportion += element.Value;
        }
        return MeanDensity / totalProportion;
    }

    /// <summary>
    /// Measured in Earth atmospheres.
    /// </summary>
    public virtual float GenerateSurfacePressure()
    {
        SurfacePressure = Random.Range(0.01f, 100f);
        return SurfacePressure;
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

    public virtual float GenerateSurfaceGravity(float planetMass, float planetRadius)
    {
        SurfaceGravity = ConstantsUtil.GRAVITY * planetMass / (planetRadius * planetRadius); // Measured in meters per second squared
        return SurfaceGravity;
    }

    /// <summary>
    /// Measured in kilometers per second.
    /// </summary>
    public virtual float GenerateEscapeVelocity(float planetMass, float planetRadius)
    {
        EscapeVelocity = Mathf.Sqrt((2f * ConstantsUtil.GRAVITY * planetMass) / planetRadius) * Random.Range(1.1f, 1.3f);
        return EscapeVelocity;
    }

    public virtual float GenerateAlbedo()
    {
        Albedo = Random.Range(0.1f, 0.9f);
        return Albedo;
    }

    /// <summary>
    /// Measured in Gauss.
    /// </summary>
    public virtual float GenerateMagneticFieldStrength()
    {
        MagneticFieldStrength = Random.Range(0f, 100f);
        return MagneticFieldStrength;
    }

    /*
    public virtual List<Moon> GenerateMoons()
    {
        // Initialize the Moons list
        Moons = new ();

        // generate a random number of moons between 0 and 3
        int numMoons = Random.Range(0, 3);

        for (int i = 0; i < numMoons; i++)
        {
            Moon moon = new ();
            Moons.Add(moon);
        }
        return Moons;
    }
    */
}
