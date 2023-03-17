using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class RockyPlanet : Planet
{
    public override float GenerateMass()
    {
        Mass = Random.Range(0.5f, 2f);  // in AU
        return Mass;
    }

    public override float GenerateRadius()
    {
        Radius = Random.Range(0.8f, 6.25f);  // in AU
        return Radius;
    }
}
