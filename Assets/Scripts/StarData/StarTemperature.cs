using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarTemperature : MonoBehaviour
{
    public StarLuminosity starLuminosity;

    public double CalculateRoughTemperature()
    {
        double rT = Mathf.Pow((float)starLuminosity.CalculateLuminosity(), 0.14880952f);

        // in Kelvins
        return rT;
    }
}
