using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GasGiant : Planet
{
    public override float GenerateMass()
    {
        Mass = Random.Range(10.0f, 350.0f); // in AU
        return Mass;
    }

    public override float GenerateRadius()
    {
        Radius = Random.Range(3.5f, 12.0f); // in AU
        return Radius;
    }
}
