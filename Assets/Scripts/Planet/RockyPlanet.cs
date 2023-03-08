using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockyPlanet : Planet
{
    public RockyPlanet(string name, float orbitalPeriod, float rotationPeriod, float axialTilt, float surfaceTemperature,
                        bool hasAtmosphere, bool hasRings, float surfacePressure, float surfaceGravity, float escapeVelocity,
                        float albedo, float solarDay, float magneticFieldStrength, List<Moon> moons)
                        : base(name, 0f, 0f, orbitalPeriod, rotationPeriod, axialTilt, surfaceTemperature, hasAtmosphere,
                                false, hasRings, 0f, surfacePressure, surfaceGravity, escapeVelocity, albedo,
                                solarDay, magneticFieldStrength, moons)
    {
        Mass = GenerateMass();
        Radius = GenerateRadius();
    }

    protected override float GenerateMass()
    {
        // Generate a random mass between 0.5 and 2 Earth masses for a rocky planet
        return Random.Range(0.5f, 2f);
    }

    protected override float GenerateRadius()
    {
        // Generate a random radius between 0.8 and 1.2 Earth radii for a rocky planet
        return Random.Range(0.8f, 6.25f);
    }
}
