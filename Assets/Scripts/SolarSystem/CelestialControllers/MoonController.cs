
using SolarSystem;
using SolarSystemUI;
using UnityEngine;
using Utils;
using Color = UnityEngine.Color;
using Random = UnityEngine.Random;

public class MoonController : MonoBehaviour
{
    public GameObject AxialTiltMarkerPrefab;
    public GameObject SpinDirectionMarkerPrefab;
    public Color MoonTrailColor;
    
    private GameObject _axialTiltMarker;
    private GameObject _spinDirectionMarker;
    private UI_DebugMarkers _uiDebugMarkers;
    private MoonInfo _moonInfo;
    private Rigidbody _rigidBody;
    private TrailRenderer _trailRenderer;
    private Vector3 _startingPosition;
    private readonly float _initialTrailTime = 1f;
    private readonly float _trailTimeTransitionDuration = 1.0f;
    private float _currentTrailTime;
    
    private void Start()
    {
        _moonInfo = GetComponent<MoonInfo>();
        _rigidBody = GetComponent<Rigidbody>();
        _trailRenderer = GetComponent<TrailRenderer>();
        _uiDebugMarkers = GameObject.Find("UI_DebugMarkers").GetComponent<UI_DebugMarkers>();
        _currentTrailTime = _initialTrailTime;
        _trailRenderer.time = _currentTrailTime;
        
        SetGOInitialComps();
        SetRandomGOStartingPosition();
        SetGOScale();
        SetGOAxialTiltMarker();
        SetGOSpinDirectionMarker();
        SetGOTrailRenderer();
    }

    public void Update()
    {
        ToggleMarkerVisibility();
        
        if (GameSpeedController.Instance is null) return;
        TryCalculateTrailRendererLength();
    }

    private void SetGOInitialComps()
    {
        if (_rigidBody is null) return;
        _rigidBody.mass = _moonInfo.Mass;
        _rigidBody.constraints = RigidbodyConstraints.FreezePosition;
        _rigidBody.useGravity = false;
    }

    private void SetGOScale()
    {
        transform.localScale = new Vector3(_moonInfo.Radius, _moonInfo.Radius, _moonInfo.Radius);
    }

    private void SetRandomGOStartingPosition()
    {
        Transform parentPlanet = transform.parent; // Assuming parent of parent is the planet
        SphereCollider parentCollider = parentPlanet.GetComponent<SphereCollider>();
        if (parentCollider is null)
        {
            Debug.LogError("Parent planet does not have a SphereCollider.");
            return;
        }

        transform.rotation = Quaternion.Euler(0, 0, _moonInfo.AxialTilt);
        float moonSpacingOffset = _moonInfo.Radius * 2;
        float finalOffset = (_moonInfo.Index + 1) * (_moonInfo.Radius + moonSpacingOffset);

        // Generate a random point on a unit sphere
        Vector3 randomPointOnSphere = Random.onUnitSphere;

        // Scale the point based on the parent planet's scale
        Vector3 point = randomPointOnSphere * (parentPlanet.localScale.x * 75f) + parentPlanet.position;

        // Apply the random offsets
        float xComponent = Random.Range(0, 2) == 0 ? finalOffset : -finalOffset;
        float yComponent = Random.Range(0, 2) == 0 ? finalOffset : -finalOffset;
        float zComponent = Random.Range(0, 2) == 0 ? finalOffset : -finalOffset;
        point += new Vector3(xComponent, yComponent, zComponent);

        // Check if the position is valid using cached positions
        if (!MoonControllerUtil.IsPositionValid(point, _moonInfo.Radius))
        {
            // Retry once with a new random position if not valid
            point = randomPointOnSphere * (parentPlanet.localScale.x * 75f) + parentPlanet.position;
            point += new Vector3(xComponent, yComponent, zComponent);
        }

        // Set the moon's position
        transform.position = point;
        //Debug.Log($"position cached: {point}"); // :)

        // Cache the position
        MoonControllerUtil.AddCachedPosition(transform.position);
    }

    private void SetGOAxialTiltMarker()
    {
        _axialTiltMarker = Instantiate(AxialTiltMarkerPrefab, transform.position, Quaternion.identity);
        _axialTiltMarker.transform.parent = transform;
        _axialTiltMarker.transform.localPosition = Vector3.zero;
        _axialTiltMarker.transform.localScale = new Vector3(1f, 1f, 0.0f);
        _axialTiltMarker.transform.localRotation = Quaternion.Euler(_moonInfo.AxialTilt, 0f, 0f);
    }
    
    private void SetGOSpinDirectionMarker()
    {
        _spinDirectionMarker = Instantiate(SpinDirectionMarkerPrefab, transform.position, Quaternion.identity);
        _spinDirectionMarker.transform.parent = transform;
        _spinDirectionMarker.transform.localPosition = Vector3.zero;
        _spinDirectionMarker.transform.localScale = new Vector3(1f, 1f, 0.0f);
        _spinDirectionMarker.transform.localRotation = Quaternion.Euler(0f, 0f, 0f);
    }
    
    private void SetGOTrailRenderer()
    {
        _trailRenderer.startWidth = 5.0f;
        _trailRenderer.endWidth = 0.0f;
        _trailRenderer.time = _initialTrailTime;
        _trailRenderer.startColor = MoonTrailColor;
        _trailRenderer.endColor = Color.clear;
    }
    
    private void TryCalculateTrailRendererLength()
    {
        float targetTrailTime = TrailTimersUtil.MoonTrailTime(GameSpeedController.Instance.CurSpeed);
        _currentTrailTime = Mathf.Lerp(_currentTrailTime, targetTrailTime, Time.deltaTime / _trailTimeTransitionDuration);
        _trailRenderer.time = _currentTrailTime;
    }
    
    private void ToggleMarkerVisibility()
    {
        if (_uiDebugMarkers is null) return;
        _axialTiltMarker?.SetActive(_uiDebugMarkers.ShowAxialTiltMarkers);
        _spinDirectionMarker?.SetActive(_uiDebugMarkers.ShowSpinDirectionMarkers);
    }
}
