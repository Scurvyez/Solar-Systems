using UnityEngine;

public class PlanetController : MonoBehaviour
{
    public SaveManager SaveManager;
    public GameObject starPrefab;

    public float Radius;
    public Vector3 StartingPosition;
    public float RotationPeriod;
    public float OrbitalPeriod;
    public float SemiMajorAxis;
    public Vector3 FocusPoint;
    public float Eccentricity;
    public float SemiMinorAxis;
    public float AxialTilt;
    public bool IsHabitable;
    public bool HasRings;
    public float InnerRingRadius;
    public float OuterRingRadius;

    private float elapsedTime;

    public GameObject planetaryRingsPrefab;
    private GameObject planetaryRings;
    public float innerRingsDistance;
    public float outerRingsDistance;

    public GameObject axialTiltMarkerPrefab; // Reference to the axial tilt marker prefab
    private GameObject axialTiltMarker; // Reference to the instantiated axial tilt marker game object

    public GameObject spinDirectionMarkerPrefab; // Reference to the spin direction marker prefab
    private GameObject spinDirectionMarker; // Reference to the instantiated spin direction marker game object

    public bool reverseOrbitDirection = false; // Flag to reverse the direction of the planet's orbit

    public void Start()
    {
        transform.Rotate(Vector3.right, AxialTilt);
        transform.position = StartingPosition;
        InstantiateDebugVisuals();

        if (HasRings)
        {
            InstantiatePlanetaryRings();
        }
    }

    public void Update()
    {
        // Rotate the planet around its own axis
        transform.Rotate(Vector3.up, Time.deltaTime * RotationPeriod * GameSpeedController.Instance.curSpeed);

        // Orbit the star
        elapsedTime += Time.deltaTime * GameSpeedController.Instance.curSpeed;

        float orbitSpeed = 2 * Mathf.PI / OrbitalPeriod;

        if (!reverseOrbitDirection)
        {
            Orbit(starPrefab.transform.position, -orbitSpeed);
        }
        else
        {
            Orbit(starPrefab.transform.position, orbitSpeed);
        }
    }

    private void Orbit(Vector3 center, float speed)
    {
        Vector3 offset = StartingPosition - center;
        float angle = elapsedTime * speed;
        Vector3 newPosition = center + Quaternion.Euler(0, angle * Mathf.Rad2Deg, 0) * offset;

        transform.position = newPosition;
    }

    public Color GenerateRandomRingsColor()
    {
        float r = Random.Range(0.8f, 1.0f);
        float g = Random.Range(0.7f, 0.74f);
        float b = Random.Range(0.53f, 0.57f);
        return new Color(r, g, b);
    }

    public void InstantiatePlanetaryRings()
    {
        // Create planetary rings if planet has them
        planetaryRings = Instantiate(planetaryRingsPrefab, transform.position, Quaternion.identity);
        planetaryRings.transform.parent = transform;
        planetaryRings.transform.localPosition = Vector3.zero;

        // Remap innerRingsDistance to a value between 0 and 1
        float minInner = Radius + 3f;
        float maxInner = Radius + 7.5f;
        InnerRingRadius = Mathf.InverseLerp(minInner, maxInner, InnerRingRadius);
        InnerRingRadius = Mathf.Clamp(InnerRingRadius, 0.1f, 0.4f);
        planetaryRings.transform.localScale = new Vector3(OuterRingRadius, 0f, OuterRingRadius);
        planetaryRings.transform.localRotation = Quaternion.Euler(0f, 0f, 0f);
        planetaryRings.transform.GetComponent<MeshRenderer>().material.SetFloat("_InnerRadius", InnerRingRadius);
        planetaryRings.transform.GetComponent<MeshRenderer>().material.SetColor("_RingColor", GenerateRandomRingsColor());
        // add the rest of the stuff
    }

    public void InstantiateDebugVisuals()
    {
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
}
