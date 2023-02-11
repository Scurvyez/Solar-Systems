using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarMagnitude : MonoBehaviour
{
    public StarDistance starDistance;

    /// <summary>
    /// Generates a random apparent magnitude for a star.
    /// This must be random and arbitrary since this is not real data.
    /// Generated using a simplified probability density function.
    /// Returned as a float.
    /// </summary>
    public double GenerateRandomApparentMagnitudePDF()
    {
        // Define the probability density function (PDF)
        // for the distribution of apparent magnitudes
        double pdf(double x)
        {
            // This is an example PDF, you can use your own function
            // that fits the distribution of apparent magnitudes
            return x * Mathf.Exp((float)-x);
        }

        // Define the range of possible magnitudes
        double minMagnitude = -30.0f;
        double maxMagnitude = 30.0f;

        double magnitudePDF = GenerateRandomApparentMagnitude(pdf, minMagnitude, maxMagnitude);

        return magnitudePDF;
    }

    /// <summary>
    /// Secondary function used in GenerateRandomApparentMagnitudePDF()
    /// to calculate a final apparent magnitude.
    /// </summary>
    public double GenerateRandomApparentMagnitude(System.Func<double, double> pdf, double minMagnitude, double maxMagnitude)
    {
        double magnitude = 0;
        double rnd = Random.value;
        double integral = 0;

        for (double x = minMagnitude; x < maxMagnitude; x += 0.01f)
        {
            integral += pdf(x) * 0.01f;
            if (integral >= rnd)
            {
                magnitude = x;
                break;
            }
        }

        return magnitude;
    }

    /// <summary>
    /// Calculates the stars' final absolute magnitude.
    /// </summary>
    public double CalculateAbsoluteMagnitude(double apparentMagnitude, double distance)
    {
        apparentMagnitude = GenerateRandomApparentMagnitudePDF();
        distance = starDistance.GenerateDistanceToStar();
        // Use the formula M = m + 5 - 5 * log10(d/10)
        // where m is the apparent magnitude and d is the distance to the star in parsecs

        return apparentMagnitude + 5 - 5 * Mathf.Log10((float)distance / 10);
    }
}
