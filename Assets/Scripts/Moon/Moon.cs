using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
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

    public const float GravConstant = 6.674e-11f;

    public virtual string GenerateRandomName()
    {
        Name = "Moon " + UnityEngine.Random.Range(1, 1000);
        return Name;
    }

    public virtual float GenerateMass()
    {
        // Generate a random mass between 0.1 and 10 Earth masses
        Mass = Random.Range(0.1f, 5f);
        return Mass;
    }

    public virtual float GenerateRadius()
    {
        // Generate a random radius between 0.5 and 2.5 Earth radii
        Radius = Random.Range(0.5f, 2.0f);
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
        SurfaceTemperature = Random.Range(-500f, 500f); // Measured in Kelvin
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

    public virtual float GenerateMeanDensity()
    {
        MeanDensity = Random.Range(0.5f, 10f); // Measured in grams per cubic centimeter
        return MeanDensity;
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
}
