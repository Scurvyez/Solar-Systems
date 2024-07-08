using UnityEngine;
using Utils;

[System.Serializable]
public class Moon
{
    //public int Index { get; set; }
    public string ParentPlanetName { get; set; }
    public string Name { get; set; }
    public float Mass { get; set; }
    public float Radius { get; set; }
    public float OrbitalDistanceFrom { get; set; }
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

    /*
    public virtual int GetIndex(int index)
    {
        Index = index;
        return Index;
    }
    */
    
    public virtual float GenerateMass()
    {
        // Generate a random mass between 0.1 and 10 Earth masses
        Mass = Random.Range(0.1f, 5f);
        return Mass;
    }

    public virtual float GenerateGORadius()
    {
        Radius = 0.25f;
        return Radius;
    }

    public virtual float GenerateOrbitalDistanceFrom()
    {
        OrbitalDistanceFrom = 1f;
        return OrbitalDistanceFrom;
    }

    //private const double SolRadiusM = 6.96342E8; // in m
    public virtual float GenerateOrbitalPeriod()
    {
        //float minDist = (float)(SaveManager.instance.activeSave.starRadius / SolRadiusM);
        OrbitalPeriod = Random.Range(3.5f, 200f); // Measured in Earth days
        return OrbitalPeriod;
    }

    public virtual float GenerateRotationPeriod()
    {
        RotationPeriod = Random.Range(5.75f, 75f); // Measured in Earth days
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
        SurfaceGravity = ConstantsUtil.GRAVITY * Mass / (Radius * Radius); // Measured in meters per second squared
        return SurfaceGravity;
    }

    public virtual float GenerateEscapeVelocity()
    {
        EscapeVelocity = Mathf.Sqrt((2f * ConstantsUtil.GRAVITY * Mass) / Radius) * Random.Range(1.1f, 1.3f); // Measured in kilometers per second
        return EscapeVelocity;
    }

    public virtual float GenerateAlbedo()
    {
        Albedo = UnityEngine.Random.Range(0.1f, 0.9f); // a random value between 0.1 and 0.9
        return Albedo;
    }
}
