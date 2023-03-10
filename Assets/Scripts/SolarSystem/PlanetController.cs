using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanetController : MonoBehaviour
{
    public SaveManager SaveManager;
    public GameObject starPrefab;

    public float OrbitalPeriod;
    public float RotationPeriod;
    public float AxialTilt;
    public float SurfaceTemperature;
    public float MeanDensity;
    public float SurfacePressure;
    public float SurfaceGravity;
    public float EscapeVelocity;
    public float Albedo;
    public float MagneticFieldStrength;
    public bool HasAtmosphere;
    public bool HasRings;
    public bool IsHabitable;

    public float SolarDay;
    public float SolarYear;

    public GameObject axialTiltMarkerPrefab; // Reference to the axial tilt marker prefab
    private GameObject axialTiltMarker; // Reference to the instantiated axial tilt marker game object

    public GameObject spinDirectionMarkerPrefab; // Reference to the spin direction marker prefab
    private GameObject spinDirectionMarker; // Reference to the instantiated spin direction marker game object

    public void Start()
    {
        // Calculate one solar day
        SolarDay = 1000 - RotationPeriod;

        // Calculate one solar year
        SolarYear = 11000 - OrbitalPeriod;

        transform.Rotate(Vector3.right, AxialTilt);

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
        // Offset RotationPeriod by 99.5%
        transform.Rotate(Vector3.up, Time.deltaTime * RotationPeriod);

        // Rotate the planet around the star
        // Offset OrbitalPeriod by 99.5%
        transform.RotateAround(starPrefab.transform.position, Vector3.up, Time.deltaTime * (OrbitalPeriod * 0.005f));
    }
}
