using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Planet
{
    public string Name { get; set; }
    public float Mass { get; set; }
    public float Radius { get; set; }
    public float OrbitalPeriod { get; set; }
    public float RotationPeriod { get; set; }
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
    //public List<Moon> Moons { get; set; }

    public const float GravConstant = 6.674e-11f;

    public virtual float AtmosphereHeight()
    {
        if (!HasAtmosphere) return 0f; // if no atmosphere, no value
        return Radius * 2.5f; // set height of atmosphere, above planet's surface
    }

    public virtual Color GetColor()
    {
        return Color.white; // default base color of all planets
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

    public virtual float GenerateMass()
    {
        // Generate a random mass between 0.1 and 10 Earth masses
        Mass = Random.Range(0.1f, 10f);
        return Mass;
    }

    public virtual float GenerateRadius()
    {
        // Generate a random radius between 0.5 and 2.5 Earth radii
        Radius = Random.Range(0.5f, 2.5f);
        return Radius;
    }

    //private const double SolRadiusM = 6.96342E8; // in m
    public virtual float GenerateOrbitalPeriod()
    {
        //float minDist = (float)(SaveManager.instance.activeSave.starRadius / SolRadiusM);
        OrbitalPeriod = Random.Range(25.0f, 10000f); // Measured in Earth days
        return OrbitalPeriod;
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
        SurfaceTemperature = Random.Range(50f, 500f); // Measured in Kelvin
        return SurfaceTemperature;
    }

    public virtual bool HasRandomAtmosphere()
    {
        HasAtmosphere = Random.value < 0.5f; // 50% chance of having an atmosphere
        return HasAtmosphere;
    }

    public virtual bool IsRandomlyHabitable()
    {
        IsHabitable = Random.value < 0.2f; // 20% chance of being habitable
        return IsHabitable;
    }

    public virtual bool HasRandomRings()
    {
        HasRings = Random.value < 0.1f; // 10% chance of having rings
        return HasRings;
    }

    public virtual float GenerateMeanDensity()
    {
        MeanDensity = Random.Range(0.5f, 10f); // Measured in grams per cubic centimeter
        return MeanDensity;
    }

    public virtual float GenerateSurfacePressure()
    {
        SurfacePressure = Random.Range(0.01f, 100f); // Measured in atmospheres
        return SurfacePressure;
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
        Albedo = UnityEngine.Random.Range(0.1f, 0.9f); // a random value between 0.1 and 0.9
        return Albedo;
    }

    public virtual float GenerateMagneticFieldStrength()
    {
        MagneticFieldStrength = UnityEngine.Random.Range(0f, 100f); // a random value between 0 and 100 Gauss
        return MagneticFieldStrength;
    }

    /*
    protected virtual List<Moon> GenerateMoons()
    {
        // generate a random number of moons between 0 and 10
        int numMoons = Random.Range(0, 11);

        for (int i = 0; i < numMoons; i++)
        {
            // generate random properties for the moon within some reasonable ranges
            string name = "Moon " + i;
            float mass = Random.Range(1e20f, 1e23f);
            float radius = Random.Range(100f, 1000f);
            float orbitalPeriod = Random.Range(1f, 100f);
            float rotationPeriod = Random.Range(1f, 100f);
            float axialTilt = Random.Range(0f, 180f);
            float surfaceTemperature = Random.Range(50f, 300f);
            bool hasAtmosphere = Random.value < 0.5f;
            bool isHabitable = Random.value < 0.1f;
            float meanDensity = Random.Range(1f, 5f);
            float surfaceGravity = Random.Range(0.1f, 2f);
            float escapeVelocity = Random.Range(1f, 10f);
            float albedo = Random.Range(0.1f, 0.9f);

            Moon moon = new Moon(name, mass, radius, orbitalPeriod, rotationPeriod, axialTilt, surfaceTemperature,
                                 hasAtmosphere, isHabitable, meanDensity, surfaceGravity, escapeVelocity, albedo);
            Moons.Add(moon);
        }

        return Moons;
    }
    */
}
