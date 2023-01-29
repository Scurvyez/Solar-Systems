using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarFlux : MonoBehaviour
{
    public StarMagnitude StarMagnitude;
    public StarDistance StarDistance;

    /// <summary>
    /// Calculates the flux of a star using the apparent magnitude and absolute magnitude.
    /// flux = 10^(-0.4 * (apparent magnitude - absolute magnitude))
    /// </summary>
    public double CalculateFlux()
    {
        //double apparentMagnitude = StarMagnitude.GenerateRandomApparentMagnitudePDF();
        //double distance = (float)StarDistance.GenerateDistanceToStar();
        //double absoluteMagnitude = StarMagnitude.CalculateAbsoluteMagnitude(apparentMagnitude, distance);

        double d = Mathf.Pow(10, -0.4f * ((float)StarMagnitude.GenerateRandomApparentMagnitudePDF() - (float)StarMagnitude.CalculateAbsoluteMagnitude(StarMagnitude.GenerateRandomApparentMagnitudePDF(), StarDistance.GenerateDistanceToStar())));

        return d;
    }
}
