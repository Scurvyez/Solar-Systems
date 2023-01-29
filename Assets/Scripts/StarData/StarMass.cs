using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarMass : MonoBehaviour
{
    public StarLuminosity StarLuminosity;
    private const double Constant = 3.8e28; // ergs per second
    private const float PowerLawIndex = 3.5f;

    public double CalculateMass()
    {
        double m = Mathf.Pow((float)(StarLuminosity.CalculateLuminosity() / Constant), (1 / PowerLawIndex));

        return m;
    }
}
