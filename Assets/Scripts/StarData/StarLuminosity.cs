using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarLuminosity : MonoBehaviour
{
    public StarFlux starFlux;
    public StarDistance starDistance;

    /// <summary>
    /// Calculates a stars' total energy output using its' flux value.
    /// Measured in watts per square meter.
    /// </summary>
    /// <returns>bolometric flux, watts per square meter (double)</returns>
    public double CalculateEnergyOutput()
    {
        double finalStarFlux = starFlux.CalculateFlux();

        // b = total ernergy output in L = b × 4 * PI * d^2.
        double b = finalStarFlux / (4 * Mathf.PI * Mathf.Pow((float)starDistance.GenerateDistanceToStar(), 2));
        b *= (4 * Mathf.PI * Mathf.Pow((float)starDistance.GenerateDistanceToStar(), 2));

        return b;
    }

    /// <summary>
    /// Uses a stars' total energy output (watts per square meter) 
    /// and its' distance from Earth (meters)
    /// to calculate the stars' luminosity. Total energy output 
    /// needs to be converted into bolometric flux first.
    /// </summary>
    /// <returns>luminosity, watts, (double)</returns>
    public double CalculateLuminosity()
    {
        // luminosity (l) = totalEnergyOutput (b) * (4 * PI * distance (d)^2)
        double l = CalculateEnergyOutput() * (4 * Mathf.PI * Mathf.Pow((float)starDistance.GenerateDistanceToStar(), 2));

        return l;
    }
}
