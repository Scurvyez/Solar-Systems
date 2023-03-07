using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarMass : MonoBehaviour
{
    public StarLuminosity StarLuminosity;
    private const double constant = 3.8e28; // ergs per second
    private const float powerLawIndex = 3.5f;

    public double CalculateMass()
    {
        double m = Mathf.Pow((float)(StarLuminosity.CalculateLuminosity() / constant), (1 / powerLawIndex));

        return m;
    }
}
