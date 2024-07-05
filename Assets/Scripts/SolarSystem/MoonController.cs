using SolarSystem;
using SolarSystemUI;
using UnityEngine;

public class MoonController : MonoBehaviour
{
    public GameObject MoonPrefab;
    public GameObject AxialTiltMarkerPrefab;
    public GameObject SpinDirectionMarkerPrefab;
    public int Index;
    public float Radius;
    public float OrbitalDistanceFrom;
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
    public bool ReverseOrbitDirection = false;
    
    private GameObject _axialTiltMarker;
    private GameObject _spinDirectionMarker;
    private DebugMarkers_UI _debugMarkersUI;
    private TrailRenderer _trailRenderer;

    private void Start()
    {
        _trailRenderer = GetComponent<TrailRenderer>();
        _debugMarkersUI = GameObject.Find("UI_DebugMarkers").GetComponent<DebugMarkers_UI>();
        SetRandomGOStartingPosition();
        SetGOScale();
        SetGOAxialTiltMarker();
        SetGOSpinDirectionMarker();
    }

    public void Update()
    {
        ToggleMarkerVisibility();
        
        if (GameSpeedController.Instance is null) return;
        _trailRenderer.time = 0.1f * GameSpeedController.Instance.CurSpeed;
        //RotateAroundOwnAxis();
        //OrbitAroundPlanet();
    }

    private void SetGOScale()
    {
        transform.localScale = new Vector3(Radius, Radius, Radius);
    }

    private void SetRandomGOStartingPosition()
    {
        const float moonSpacingOffset = 10f;
        float finalOffset = 40f + (Index * (Radius + moonSpacingOffset));
        
        // get 0 or finalOffset for use in new Vector3 just below...
        float xComponent = Random.Range(0, 2) == 0 ? finalOffset : -finalOffset;
        float yComponent = Random.Range(0, 2) == 0 ? finalOffset : -finalOffset;
        float zComponent = Random.Range(0, 2) == 0 ? finalOffset : -finalOffset;
        
        transform.position = transform.parent.position + new Vector3(xComponent, yComponent, zComponent);
    }

    private void SetGOAxialTiltMarker()
    {
        _axialTiltMarker = Instantiate(AxialTiltMarkerPrefab, transform.position, Quaternion.identity);
        _axialTiltMarker.transform.parent = transform;
        _axialTiltMarker.transform.localPosition = Vector3.zero;
        _axialTiltMarker.transform.localScale = new Vector3(1f, 1f, 0.0f);
        _axialTiltMarker.transform.localRotation = Quaternion.Euler(-AxialTilt, 0f, 0f);
    }
    
    private void SetGOSpinDirectionMarker()
    {
        _spinDirectionMarker = Instantiate(SpinDirectionMarkerPrefab, transform.position, Quaternion.identity);
        _spinDirectionMarker.transform.parent = transform;
        _spinDirectionMarker.transform.localPosition = Vector3.zero;
        _spinDirectionMarker.transform.localScale = new Vector3(1f, 1f, 0.0f);
        _spinDirectionMarker.transform.localRotation = Quaternion.Euler(0f, 0f, 0f);
    }
    
    private void RotateAroundOwnAxis()
    {
        transform.Rotate(Vector3.up, Time.deltaTime * RotationPeriod * GameSpeedController.Instance.CurSpeed);
    }

    private void OrbitAroundPlanet()
    {
        Vector3 orbitalAxis = transform.parent.up; // Use parent's up direction for orbital axis
        float orbitSpeed = 360.0f / OrbitalPeriod * Time.deltaTime * GameSpeedController.Instance.CurSpeed;
        transform.RotateAround(transform.parent.position, orbitalAxis, ReverseOrbitDirection ? -orbitSpeed : orbitSpeed);
    }
    
    private void ToggleMarkerVisibility()
    {
        if (_debugMarkersUI is null) return;
        _axialTiltMarker?.SetActive(_debugMarkersUI.ShowAxialTiltMarkers);
        _spinDirectionMarker?.SetActive(_debugMarkersUI.ShowSpinDirectionMarkers);
    }
}
