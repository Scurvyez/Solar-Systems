using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarDistance : MonoBehaviour
{
    //private const double LYToMeters = 9.461e15;

    /// <summary>
    /// Calls our random number generation method.
    /// </summary>
    public void Start()
    {
        RandomDoubleValue();
    }

    /// <summary>
    /// Generates a random number.
    /// </summary>
    public System.Random RandomDoubleValue()
    {
        // Create a new random number generator
        System.Random random = new();
        return random;
    }

    /// <summary>
    /// Calculates the chance that the value given via RandomDoubleValue()
    /// is within realistic bounds according to a very simplified
    /// probability density function.
    /// Returns the value (probability) as a double.
    /// Later converted to a % of 100 for readability on-screen.
    /// </summary>
    public double DistanceToStarProbability()
    {

        // Define the range of possible distances
        double minDistance = 0;
        double maxDistance = 50;

        // Generate a random number within the range
        double randomDistance = RandomDoubleValue().NextDouble() * (maxDistance - minDistance) + minDistance;
        randomDistance *= 3.26;

        // Define the probability density function (PDF)
        // for the distribution of star distances
        double pDF_Distances(double x)
        {
            // Change if necessary
            return x * Mathf.Exp((float)-x);
        }

        double pdfDistance = pDF_Distances(randomDistance);

        return pdfDistance;
    }

    /// <summary>
    /// Calculates a random yet realistic (within known bounds) distance, in light years
    /// using the generated stars' parallax angle.
    /// d = 1 / p
    /// Where d = distance (in parsecs), and p = parallax angle (in arcsecs).
    /// Returns distance as a double.
    /// </summary>
    public double GenerateDistanceToStar()
    {
        // Generate a random number for the star's parallax angle
        double parallaxAngle = RandomDoubleValue().NextDouble() * 0.0001;

        double distance = 1 / (parallaxAngle / 1000);

        // convert to light years
        //distance *= 3.26;

        // convert to meters
        //distance *= LYToMeters;

        return distance;
    }
}
