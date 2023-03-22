using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanetController : MonoBehaviour
{
    public SaveManager SaveManager;
    public GameObject starPrefab;

    public float RotationPeriod;
    public float OrbitalPeriod;
    public float SemiMajorAxis;
    public Vector3 FocusPoint;
    public float Eccentricity;
    public float SemiMinorAxis;
    public float AxialTilt;
    public bool IsHabitable;

    private float semiMinorAxis;
    private float timeElapsed;

    public GameObject axialTiltMarkerPrefab; // Reference to the axial tilt marker prefab
    private GameObject axialTiltMarker; // Reference to the instantiated axial tilt marker game object

    public GameObject spinDirectionMarkerPrefab; // Reference to the spin direction marker prefab
    private GameObject spinDirectionMarker; // Reference to the instantiated spin direction marker game object

    public bool reverseOrbitDirection = false; // Flag to reverse the direction of the planet's orbit

    public void Start()
    {
        transform.Rotate(Vector3.right, AxialTilt);

        transform.position = CalculatePosition(0);

        // Create the axial tilt marker object
        axialTiltMarker = Instantiate(axialTiltMarkerPrefab, transform.position, Quaternion.identity);
        axialTiltMarker.transform.parent = transform;
        axialTiltMarker.transform.localPosition = Vector3.zero;
        axialTiltMarker.transform.localScale = new Vector3(5.5f, 5.5f, 0.0f);
        axialTiltMarker.transform.localRotation = Quaternion.Euler(-AxialTilt, 0f, 0f);

        // Create the spin direction marker object
        spinDirectionMarker = Instantiate(spinDirectionMarkerPrefab, transform.position, Quaternion.identity);
        spinDirectionMarker.transform.parent = transform;
        spinDirectionMarker.transform.localPosition = Vector3.zero;
        spinDirectionMarker.transform.localScale = new Vector3(5.5f, 5.5f, 0.0f);
        spinDirectionMarker.transform.localRotation = Quaternion.Euler(0f, 0f, 0f); // Set the initial rotation to zero
    }

    public void Update()
    {
        // Rotate the planet around its own axis
        transform.Rotate(Vector3.up, Time.deltaTime * RotationPeriod);

        timeElapsed += Time.deltaTime;
        if (!reverseOrbitDirection)
        {
            transform.position = CalculatePosition(timeElapsed);
        }
        else
        {
            transform.position = CalculatePosition(OrbitalPeriod - timeElapsed);
        }

        if (timeElapsed >= OrbitalPeriod)
        {
            timeElapsed = 0f;
        }
    }

    /// <summary>
    /// Calculates the position of the planet based on its orbital parameters and the current time elapsed.
    /// </summary>
    private Vector3 CalculatePosition(float time)
    {
        // angle between the planet and the pericenter
        // (the point in the orbit closest to the star), as seen from the focus of the ellipse
        float meanAnomaly = 2 * Mathf.PI * (time / OrbitalPeriod);

        // the angle between the pericenter and a hypothetical circle with the same radius as the semi - major axis
        float eccentricAnomaly = CalculateEccentricAnomaly(meanAnomaly);

        // angle between the pericenter and the planet as seen from the focus of the ellipse
        float trueAnomaly = CalculateTrueAnomaly(eccentricAnomaly);

        // calculates the radius of the planet's position vector
        float radius = SemiMajorAxis * (1 - Eccentricity * Eccentricity) / (1 + Eccentricity * Mathf.Cos(trueAnomaly));
        Vector3 position = new (radius * Mathf.Cos(trueAnomaly), 0f, radius * Mathf.Sin(trueAnomaly));

        return position;
    }

    /// <summary>
    /// Calculates the eccentric anomaly of the planet's orbit based on the mean anomaly
    /// </summary>
    private float CalculateEccentricAnomaly(float meanAnomaly)
    {
        float eccentricAnomaly = meanAnomaly;

        // set to 10 iterations
        // adjust to achieve a higher or lower degree of accuracy
        for (int i = 0; i < 10; i++)
        {
            // applying Kepler's equation
            eccentricAnomaly = meanAnomaly + Eccentricity * Mathf.Sin(eccentricAnomaly);
        }

        return eccentricAnomaly;
    }

    /// <summary>
    /// Calculates the true anomaly of an orbiting body given its eccentric anomaly
    /// Angle between the periapsis (the point of closest approach to the focus) of the orbit and the position of the orbiting body, 
    /// as measured from the focus of the ellipse.
    /// </summary>
    private float CalculateTrueAnomaly(float eccentricAnomaly)
    {
        // derived from the equation of the ellipse in polar coordinates
        // calculate the cosine of the true anomaly using the cosine of the eccentric anomaly and the eccentricity of the orbit
        // calculate the true anomaly itself using the inverse cosine function
        float trueAnomaly = Mathf.Acos((Mathf.Cos(eccentricAnomaly) - Eccentricity) / (1 - Eccentricity * Mathf.Cos(eccentricAnomaly)));

        // if eccentric anomaly is greater than pi (180 degrees)...
        // adjust the true anomaly by subtracting it from 2pi to ensure it is within the correct range of 0 to 2pi
        if (eccentricAnomaly > Mathf.PI)
        {
            trueAnomaly = 2 * Mathf.PI - trueAnomaly;
        }

        return trueAnomaly;
    }
}
