using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarTemperature : MonoBehaviour
{
    public StarLuminosity StarLuminosity;

    public static StarTemperature instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    public double CalculateRoughTemperature()
    {
        double rT = Mathf.Pow((float)StarLuminosity.CalculateLuminosity(), 0.14880952f);

        // in Kelvins
        return rT;
    }



}
