using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoonController : MonoBehaviour
{
    public SaveManager SaveManager;
    //public GameObject planetPrefab;

    public float OrbitalPeriod;
    public float RotationPeriod;
    public float AxialTilt;
    public float SurfaceTemperature;
    public float MeanDensity;
    public float SurfaceGravity;
    public float EscapeVelocity;
    public float Albedo;
    public bool HasAtmosphere;
    public bool IsHabitable;

    public GameObject axialTiltMarkerPrefab; // Reference to the axial tilt marker prefab
    private GameObject axialTiltMarker; // Reference to the instantiated axial tilt marker game object

    public GameObject spinDirectionMarkerPrefab; // Reference to the spin direction marker prefab
    private GameObject spinDirectionMarker; // Reference to the instantiated spin direction marker game object

    // Start is called before the first frame update
    void Start()
    {
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

    // Update is called once per frame
    void Update()
    {
        // Rotate the moon around its own axis
        transform.Rotate(Vector3.up, Time.deltaTime * RotationPeriod);

        // Rotate around the parent planet
        //transform.RotateAround(transform.parent.position, Vector3.up, Time.deltaTime * OrbitalPeriod);
    }
}
