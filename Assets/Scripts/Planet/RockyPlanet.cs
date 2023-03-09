using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class RockyPlanet : Planet
{
    public override float GenerateMass()
    {
        // Generate a random mass between 0.5 and 2 Earth masses for a rocky planet
        Mass = Random.Range(0.5f, 2f);
        return Mass;
    }

    public override float GenerateRadius()
    {
        // Generate a random radius between 0.8 and 1.2 Earth radii for a rocky planet
        Radius = Random.Range(0.8f, 6.25f);
        return Radius;
    }
}
