using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GasGiant : Planet
{
    public GasGiant(string name, float mass, float radius, float orbitalPeriod, float rotationPeriod, float axialTilt,
        float surfaceTemperature, bool hasAtmosphere, bool isHabitable, bool hasRings, float meanDensity, float surfacePressure, float surfaceGravity,
        float escapeVelocity, float albedo, float solarDay, float magneticFieldStrength)
        : base(name, mass, radius, orbitalPeriod, rotationPeriod, axialTilt, surfaceTemperature, hasAtmosphere,
            isHabitable, hasRings, meanDensity, surfacePressure, surfaceGravity, escapeVelocity, albedo, solarDay, magneticFieldStrength, new List<Moon>())
    {
        
    }
}
