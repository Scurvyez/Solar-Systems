using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Moon
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
    public float MeanDensity { get; set; }
    public float SurfaceGravity { get; set; }
    public float EscapeVelocity { get; set; }
    public float Albedo { get; set; }

    public Moon(string name, float mass, float radius, float orbitalPeriod, float rotationPeriod,
        float axialTilt, float surfaceTemperature, bool hasAtmosphere, bool isHabitable,
        float meanDensity, float surfaceGravity, float escapeVelocity, float albedo)
    {
        Name = name; // name of the moon
        Mass = mass; // mass of the moon (measured in kilograms)
        Radius = radius; // radius of the moon (measured in kilometers)
        OrbitalPeriod = orbitalPeriod; // time it takes for the moon to complete one orbit around its parent planet (measured in Earth days)
        RotationPeriod = rotationPeriod; // time it takes for the moon to complete one rotation on its own axis (measured in Earth days)
        AxialTilt = axialTilt; // angle between the moon's rotational axis and the plane of its orbit around its parent planet (measured in degrees)
        SurfaceTemperature = surfaceTemperature; // average temperature on the moon's surface (measured in Kelvin)
        HasAtmosphere = hasAtmosphere; // boolean value that indicates whether the moon has an atmosphere
        IsHabitable = isHabitable; // boolean value that indicates whether the moon is habitable or not (i.e., can support life)
        MeanDensity = meanDensity; // average density of the moon (measured in kilograms per cubic centimeter)
        SurfaceGravity = surfaceGravity; // acceleration due to gravity on the moon's surface (measured in meters per second squared)
        EscapeVelocity = escapeVelocity; // minimum velocity an object needs to escape the moon's gravitational pull (measured in kilometers per second)
        Albedo = albedo; // represents the fraction of solar energy reflected by the moon's surface (measured as a percentage)
    }
}
