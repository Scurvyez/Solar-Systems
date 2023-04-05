using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Planet
{
    public string Name { get; set; }
    public float Mass { get; set; }
    public float Radius { get; set; }
    public float RotationPeriod { get; set; }
    public float OrbitalPeriod { get; set; }
    public float SemiMajorAxis { get; set; }
    public Vector3 FocusPoint { get; set; }
    public float Eccentricity { get; set; }
    public float SemiMinorAxis { get; set; }
    public float AxialTilt { get; set; }
    public float SurfaceTemperature { get; set; }
    public bool HasAtmosphere { get; set; }
    public bool IsHabitable { get; set; }
    public bool HasRings { get; set; }
    public float MeanDensity { get; set; }
    public float SurfacePressure { get; set; }
    public float SurfaceGravity { get; set; }
    public float EscapeVelocity { get; set; }
    public float Albedo { get; set; }
    public float MagneticFieldStrength { get; set; }
    public SerializableDictionary<string, float> Composition { get; set; }
    public SerializableDictionary<string, float> AtmosphereComposition { get; set; }
    public List<Moon> Moons { get; set; }
    
    public const float GravConstant = 6.674e-11f;

    public virtual bool HasRandomAtmosphere()
    {
        HasAtmosphere = Random.value < 0.2f; // 20% chance of having an atmosphere
        return HasAtmosphere;
    }

    public virtual float AtmosphereHeight()
    {
        if (!HasAtmosphere) return 0f; // if no atmosphere, no value
        return Radius * 2.5f; // set height of atmosphere, above planet's surface
    }

    public virtual Color GetColor()
    {
        return Color.white; // default base color of all planets
    }

    public virtual float GenerateRadius()
    {
        // Generate a random radius between 0.25 and 5.0 Earth radii
        Radius = Random.Range(0.25f, 5.0f);
        return Radius;
    }

    public virtual float GenerateOrbitalPeriod()
    {
        OrbitalPeriod = Random.Range(50.0f, 1000f); // Measured in Earth days
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
    public virtual float GenerateMass()
    {
        float starMass = (float)SaveManager.instance.activeSave.starMass; // solar masses

        // Calculate the mass using the third law of Kepler
        Mass = 4f * Mathf.Pow(Mathf.PI, 2f) * Mathf.Pow(FocusPoint.x, 3f) / (GravConstant * Mathf.Pow(OrbitalPeriod, 2f) * starMass);

        return Mass;
    }

    public virtual float GenerateSemiMajorAxis()
    {
        string starClass = SaveManager.instance.activeSave.starClassAsString;
        float starMass = (float)SaveManager.instance.activeSave.starMass * (1.98847f * Mathf.Pow(10f, 30f)); // kg
        float P = OrbitalPeriod * 86400f; // seconds
        float planetMass = Mass; // kg

        float M = starMass + planetMass; // kg
        M /= 1.9885e+30f;
        float x = (Mathf.Pow(P, 2) * GravConstant * M) / 4 * Mathf.Pow(Mathf.PI, 2);
        float cubeRoot = Mathf.Pow(x, 1f / 3f);
        SemiMajorAxis = cubeRoot;

        float scalingFactor = 0;
        scalingFactor = starClass switch
        {
            "O" => 2,
            "B" => 25,
            "A" => 50,
            "F" => 100,
            "G" => 100,
            "K" => 100,
            "M" => 100,
            _ => scalingFactor,
        };

        SemiMajorAxis *= scalingFactor; // world-space factor

        return SemiMajorAxis;
    }

    public virtual float GenerateEccentricity()
    {
        // Get the position of the two foci
        Vector3 f1 = new Vector3(0f, 0f, 0f);
        Vector3 f2 = FocusPoint;

        float fociDistance = Vector3.Distance(f1, f2);
        Eccentricity = fociDistance / (2f * SemiMajorAxis);
        if (Eccentricity > 0.99f)
        {
            Eccentricity = 0.99f;
        }

        return Eccentricity;
    }

    public virtual float GenerateSemiMinorAxis()
    {
        SemiMinorAxis = SemiMajorAxis * Mathf.Sqrt(1 - Mathf.Pow(Eccentricity, 2));

        return SemiMinorAxis;
    }

    public virtual float GenerateRotationPeriod()
    {
        RotationPeriod = Random.Range(0.1f, 1000f); // Measured in Earth days
        return RotationPeriod;
    }

    public virtual float GenerateAxialTilt()
    {
        AxialTilt = Random.Range(0f, 90f); // Measured in degrees
        return AxialTilt;
    }

    public virtual float GenerateSurfaceTemperature()
    {
        // Calculate the greenhouse effect of the atmosphere
        float greenhouseEffect = 1f;
        if (HasAtmosphere)
        {
            float pressureFactor = Mathf.Clamp01((SurfacePressure - 0.1f) / 99.9f);
            greenhouseEffect = Mathf.Lerp(1f, 1.5f, pressureFactor);
        }

        // Calculate the temperature based on the distance from the star
        float temperature = (float)SaveManager.instance.activeSave.starTemperature * Mathf.Sqrt(1f / SemiMajorAxis);

        // Calculate the temperature based on the luminosity of the star
        temperature *= Mathf.Sqrt((float)SaveManager.instance.activeSave.starLuminosity);

        // Calculate the temperature based on the albedo of the planet
        temperature *= Mathf.Pow(1f - Albedo, 0.25f);

        // Calculate the temperature based on the greenhouse effect of the atmosphere
        temperature *= greenhouseEffect;

        // Account for the planet's rotational period
        float dayLengthFactor = Mathf.Lerp(1.2f, 0.8f, Mathf.Clamp01((RotationPeriod - 1f) / 23f));
        temperature *= dayLengthFactor;

        // Account for the eccentricity of the orbit
        float eccentricityFactor = Mathf.Lerp(1f, 1.4f, Eccentricity);
        temperature *= eccentricityFactor;

        SurfaceTemperature = temperature; // Kelvin

        return SurfaceTemperature;
    }

    public virtual SerializableDictionary<string, float> GenerateComposition()
    {
        // initialize the Dict
        Composition = new ();
        return Composition;
    }

    public virtual SerializableDictionary<string, float> GenerateAtmosphereComposition()
    {
        // initialize the Dict
        AtmosphereComposition = new ();
        if (!HasAtmosphere)
        {
            return AtmosphereComposition;
        }

        float total = 0f;
        float minDistance = SaveManager.instance.activeSave.habitableRangeInner;
        float maxDistance = SaveManager.instance.activeSave.habitableRangeOuter;
        float distanceFactor = Mathf.Clamp01(((SemiMajorAxis / 1000f) - minDistance) / (maxDistance - minDistance));

        // Define the probabilities of each element
        // based on planets' surface temperature, distance from host star
        float oxygenProb = Random.value * (1f - Mathf.Clamp01((SurfaceTemperature - 273f) / (100f * distanceFactor)));
        float nitrogenProb = Random.value * (1f - Mathf.Clamp01((SurfaceTemperature - 273f) / (200f * distanceFactor)));
        float carbonDioxideProb = Random.value * (1f - Mathf.Clamp01((SurfaceTemperature - 273f) / (100f * distanceFactor)));
        float methaneProb = Random.value * (1f - Mathf.Clamp01((SurfaceTemperature - 273f) / (150f * distanceFactor)));
        float hydrogenProb = Random.value * (1f - Mathf.Clamp01((SurfaceTemperature - 273f) / (200f * distanceFactor)));
        float heliumProb = Random.value * (1f - Mathf.Clamp01((SurfaceTemperature - 273f) / (150f * distanceFactor)));
        float neonProb = Random.value * (1f - Mathf.Clamp01((SurfaceTemperature - 273f) / (200f * distanceFactor)));
        float argonProb = Random.value * (1f - Mathf.Clamp01((SurfaceTemperature - 273f) / (100f * distanceFactor)));
        float kryptonProb = Random.value * (1f - Mathf.Clamp01((SurfaceTemperature - 273f) / (200f * distanceFactor)));
        float xenonProb = Random.value * (1f - Mathf.Clamp01((SurfaceTemperature - 273f) / (300f * distanceFactor)));
        float sulfurDioxideProb = Random.value * (1f - Mathf.Clamp01((SurfaceTemperature - 273f) / (100f * distanceFactor)));
        float nitrogenOxidesProb = Random.value * (1f - Mathf.Clamp01((SurfaceTemperature - 273f) / (150f * distanceFactor)));
        float ozoneProb = Random.value * (1f - Mathf.Clamp01((SurfaceTemperature - 273f) / (150f * distanceFactor)));
        float waterVaporProb = Random.value * (1f - Mathf.Clamp01((SurfaceTemperature - 273f) / (100f * distanceFactor)));

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

    public virtual bool IsRandomlyHabitable()
    {
        if (HasLiquidWater() == true)
        {
            float innerHabZone = SaveManager.instance.activeSave.habitableRangeInner;
            float outerHabZone = SaveManager.instance.activeSave.habitableRangeOuter;

            if (FocusPoint.x > innerHabZone - (FocusPoint.x * 1 / 2) && FocusPoint.x < outerHabZone + (FocusPoint.x * 1 / 2))
            {
                return true;
            }
        }
        return false;
    }

    public virtual bool HasRandomRings()
    {
        HasRings = Random.value < 0.1f; // 10% chance of having rings
        return HasRings;
    }

    public virtual float GenerateMeanDensity()
    {
        float volume = (4 / 3) * Mathf.PI * Mathf.Pow(Radius, 3);
        MeanDensity = Mass / volume; // (kg/m³)

        return MeanDensity;
    }

    public virtual float GenerateSurfacePressure()
    {
        SurfacePressure = Random.Range(0.01f, 100f); // Measured in atmospheres
        return SurfacePressure;
    }

    public virtual bool HasLiquidWater()
    {
        // check if the planet's surface temperature is within the habitable range
        if (SurfaceTemperature >= 273 && SurfaceTemperature <= 373)
        {
            // check if the planet has an atmosphere
            if (HasAtmosphere)
            {
                // check if the atmospheric pressure is within the range required for liquid water
                if (SurfacePressure >= 0.1f && SurfacePressure <= 100f)
                {
                    return true; // planet has liquid water
                }
            }
        }
        return false; // planet does not have liquid water
    }

    public virtual float GenerateSurfaceGravity()
    {
        SurfaceGravity = GravConstant * Mass / (Radius * Radius); // Measured in meters per second squared
        return SurfaceGravity;
    }

    public virtual float GenerateEscapeVelocity()
    {
        EscapeVelocity = Mathf.Sqrt((2f * GravConstant * Mass) / Radius) * Random.Range(1.1f, 1.3f); // Measured in kilometers per second
        return EscapeVelocity;
    }

    public virtual float GenerateAlbedo()
    {
        Albedo = Random.Range(0.1f, 0.9f); // a random value between 0.1 and 0.9
        return Albedo;
    }

    public virtual float GenerateMagneticFieldStrength()
    {
        MagneticFieldStrength = Random.Range(0f, 100f); // a random value between 0 and 100 Gauss
        return MagneticFieldStrength;
    }

    public virtual List<Moon> GenerateMoons()
    {
        // Initialize the Moons list
        Moons = new ();

        // generate a random number of moons between 0 and 5
        int numMoons = Random.Range(0, 5);

        for (int i = 0; i < numMoons; i++)
        {
            Moon moon = new ();
            moon.Name = Name;
            moon.GenerateMass();
            moon.GenerateRadius();
            moon.GenerateOrbitalPeriod();
            moon.GenerateRotationPeriod();
            moon.GenerateAxialTilt();
            moon.GenerateSurfaceTemperature();
            moon.HasRandomAtmosphere();
            moon.IsRandomlyHabitable();
            moon.GenerateMeanDensity();
            moon.GenerateSurfaceGravity();
            moon.GenerateEscapeVelocity();
            moon.GenerateAlbedo();
            Moons.Add(moon);
        }

        return Moons;
    }
}
