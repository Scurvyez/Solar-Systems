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
    public float SolarDay { get; set; }
    public float MagneticFieldStrength { get; set; }
    public List<Moon> Moons { get; set; }

    public const float GravConstant = 6.674e-11f;

    public Planet(string name, float mass, float radius, float orbitalPeriod, float rotationPeriod,
        float axialTilt, float surfaceTemperature, bool hasAtmosphere, bool isHabitable,
        bool hasRings, float meanDensity, float surfacePressure, float surfaceGravity, float escapeVelocity,
        float albedo, float solarDay, float magneticFieldStrength, List<Moon> moons)
    {
        Name = name; // name of the planet
        Mass = GenerateMass(); // mass of the planet (measured in Earth masses)
        Radius = GenerateRadius(); // radius of the planet (measured in Earth radii)
        OrbitalPeriod = GenerateOrbitalPeriod(); // time it takes for the planet to complete one orbit around its star (measured in Earth days)
        RotationPeriod = GenerateRotationPeriod(); // time it takes for the planet to complete one rotation around its axis (measured in Earth days)
        AxialTilt = GenerateAxialTilt(); // angle between the planet's rotational axis and the plane of its orbit (measured in degrees)
        SurfaceTemperature = GenerateSurfaceTemperature(); // average temperature on the planet's surface (measured in Kelvin)
        HasAtmosphere = HasRandomAtmosphere(); // boolean value indicating whether the planet has an atmosphere or not
        IsHabitable = IsRandomlyHabitable(); // boolean value indicating whether the planet is habitable or not (i.e., can support life)
        HasRings = HasRandomRings(); // boolean value indicating whether the planet has rings or not
        MeanDensity = GenerateMeanDensity(); // average density of the planet (measured in grams per cubic centimeter)
        SurfacePressure = GenerateSurfacePressure(); // 
        SurfaceGravity = GenerateSurfaceGravity(); // acceleration due to gravity on the planet's surface (measured in meters per second squared)
        EscapeVelocity = GenerateEscapeVelocity(); // minimum velocity an object needs to escape the planet's gravitational pull (measured in kilometers per second)
        Albedo = GenerateAlbedo(); // fraction of sunlight reflected by the planet's surface (measured as a percentage)
        SolarDay = GenerateSolarDay(); // time it takes for the planet to complete one rotation relative to its star (measured in Earth days)
        MagneticFieldStrength = GenerateMagneticFieldStrength(); // strength of the planet's magnetic field (measured in Gauss)
        Moons = GenerateMoons(); // list of moons orbiting the planet
    }

    protected virtual float AtmosphereHeight()
    {
        if (!HasAtmosphere) return 0f; // if no atmosphere, no value
        return Radius * 2.5f; // set height of atmosphere, above planet's surface
    }

    protected virtual Color GetColor()
    {
        return Color.white; // default base color of all planets
    }

    protected virtual bool HasLiquidWater()
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

    protected virtual float GenerateMass()
    {
        // Generate a random mass between 0.1 and 10 Earth masses
        return Random.Range(0.1f, 10f);
    }

    protected virtual float GenerateRadius()
    {
        // Generate a random radius between 0.5 and 2.5 Earth radii
        return Random.Range(0.5f, 2.5f);
    }

    protected virtual float GenerateOrbitalPeriod()
    {
        return Random.Range(0.1f, 1000f); // Measured in Earth days
    }

    protected virtual float GenerateRotationPeriod()
    {
        return Random.Range(0.1f, 1000f); // Measured in Earth days
    }

    protected virtual float GenerateAxialTilt()
    {
        return Random.Range(0f, 90f); // Measured in degrees
    }

    protected virtual float GenerateSurfaceTemperature()
    {
        return Random.Range(50f, 500f); // Measured in Kelvin
    }

    protected virtual bool HasRandomAtmosphere()
    {
        return Random.value < 0.5f; // 50% chance of having an atmosphere
    }

    protected virtual bool IsRandomlyHabitable()
    {
        return Random.value < 0.2f; // 20% chance of being habitable
    }
    
    protected virtual bool HasRandomRings()
    {
        return Random.value < 0.1f; // 10% chance of having rings
    }

    protected virtual float GenerateMeanDensity()
    {
        return Random.Range(0.5f, 10f); // Measured in grams per cubic centimeter
    }

    protected virtual float GenerateSurfacePressure()
    {
        return Random.Range(0.01f, 100f); // Measured in atmospheres
    }

    protected virtual float GenerateSurfaceGravity()
    {
        return GravConstant * Mass / (Radius * Radius); // Measured in meters per second squared
    }

    protected virtual float GenerateEscapeVelocity()
    {
        return Mathf.Sqrt((2f * GravConstant * Mass) / Radius) * Random.Range(1.1f, 1.3f); // Measured in kilometers per second
    }

    protected virtual float GenerateAlbedo()
    {
        return UnityEngine.Random.Range(0.1f, 0.9f); // a random value between 0.1 and 0.9
    }

    protected virtual float GenerateSolarDay()
    {
        return UnityEngine.Random.Range(1f, 365f); // a random value between 1 and 365 Earth days
    }

    protected virtual float GenerateMagneticFieldStrength()
    {
        return UnityEngine.Random.Range(0f, 100f); // a random value between 0 and 100 Gauss
    }

    protected virtual List<Moon> GenerateMoons()
    {
        List<Moon> moons = new List<Moon>();

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
            moons.Add(moon);
        }

        return moons;
    }
}
