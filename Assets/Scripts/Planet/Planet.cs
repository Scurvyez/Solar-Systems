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

    public Planet(string name, float mass, float radius, float orbitalPeriod, float rotationPeriod,
        float axialTilt, float surfaceTemperature, bool hasAtmosphere, bool isHabitable,
        bool hasRings, float meanDensity, float surfacePressure, float surfaceGravity, float escapeVelocity,
        float albedo, float solarDay, float magneticFieldStrength, List<Moon> moons)
    {
        Name = name; // name of the planet
        Mass = mass; // mass of the planet (measured in Earth masses)
        Radius = radius; // radius of the planet (measured in Earth radii)
        OrbitalPeriod = orbitalPeriod; // time it takes for the planet to complete one orbit around its star (measured in Earth days)
        RotationPeriod = rotationPeriod; // time it takes for the planet to complete one rotation around its axis (measured in Earth days)
        AxialTilt = axialTilt; // angle between the planet's rotational axis and the plane of its orbit (measured in degrees)
        SurfaceTemperature = surfaceTemperature; // average temperature on the planet's surface (measured in Kelvin)
        HasAtmosphere = hasAtmosphere; // boolean value indicating whether the planet has an atmosphere or not
        IsHabitable = isHabitable; // boolean value indicating whether the planet is habitable or not (i.e., can support life)
        HasRings = hasRings; // boolean value indicating whether the planet has rings or not
        MeanDensity = meanDensity; // average density of the planet (measured in grams per cubic centimeter)
        SurfacePressure = surfacePressure; // 
        SurfaceGravity = surfaceGravity; // acceleration due to gravity on the planet's surface (measured in meters per second squared)
        EscapeVelocity = escapeVelocity; // minimum velocity an object needs to escape the planet's gravitational pull (measured in kilometers per second)
        Albedo = albedo; // fraction of sunlight reflected by the planet's surface (measured as a percentage)
        SolarDay = solarDay; // time it takes for the planet to complete one rotation relative to its star (measured in Earth days)
        MagneticFieldStrength = magneticFieldStrength; // strength of the planet's magnetic field (measured in Gauss)
        Moons = moons; // list of moons orbiting the planet
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

}
