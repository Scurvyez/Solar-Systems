using SolarSystemUI;
using UnityEngine;
using Utils;

namespace SolarSystem
{
    public class PlanetController : MonoBehaviour
    {
        public GameObject StarPrefab;
        public GameObject PlanetaryRingsPrefab;
        public GameObject AxialTiltMarkerPrefab;
        public GameObject SpinDirectionMarkerPrefab;
        public bool ReverseOrbitDirection;
        public Color PlanetTrailColor;

        private float _elapsedTime;
        private GameObject _planetaryRings;
        private GameObject _axialTiltMarker;
        private GameObject _spinDirectionMarker;
        private UI_DebugMarkers _uiDebugMarkers;
        private PlanetInfo _planetInfo;
        private Rigidbody _rigidBody;
        private TrailRenderer _trailRenderer;
        private Vector3 _startingPosition;
        private readonly float _initialTrailTime = 1f;
        private readonly float _trailTimeTransitionDuration = 1.0f;
        private float _currentTrailTime;
        private Vector3 _orbitCenter;
        private float _orbitRadius;

        private static readonly int _innerRadius = Shader.PropertyToID("_InnerRadius");
        private static readonly int _ringColor = Shader.PropertyToID("_RingColor");

        public void Start()
        {
            _planetInfo = GetComponent<PlanetInfo>();
            _rigidBody = GetComponent<Rigidbody>();
            _trailRenderer = GetComponent<TrailRenderer>();
            _uiDebugMarkers = GameObject.Find("UI_DebugMarkers").GetComponent<UI_DebugMarkers>();
            _currentTrailTime = _initialTrailTime;
            _trailRenderer.time = _currentTrailTime;

            SetGOInitialComps();
            SetGOStartingPositionRotation();
            SetGOScale();
            SetGOAxialTiltMarker();
            SetGOSpinDirectionMarker();
            //TrySetGOPlanetaryRings();
            SetGOTrailRenderer();

            _orbitCenter = StarPrefab.transform.position;
            _orbitRadius = Vector3.Distance(transform.position, _orbitCenter);

            // Calculate initial elapsed time based on the starting position (for orbit)
            float initialAngle = Mathf.Atan2(transform.position.z - _orbitCenter.z, transform.position.x - _orbitCenter.x);
            _elapsedTime = initialAngle / Mathf.Deg2Rad / (360.0f / _planetInfo.OrbitalPeriod);
        }

        public void Update()
        {
            ToggleMarkerVisibility();
            
            if (GameSpeedController.Instance is null) return;
            TryCalculateTrailRendererLength();
        }

        private void FixedUpdate()
        {
            if (GameSpeedController.Instance is null) return;
            OrbitAroundStar();
            RotateAroundOwnAxis();
        }

        private void SetGOInitialComps()
        {
            if (_rigidBody is null) return;
            _rigidBody.mass = _planetInfo.Mass;
            _rigidBody.constraints = RigidbodyConstraints.FreezePositionY;
            _rigidBody.useGravity = false;
        }

        private void SetGOScale()
        {
            transform.localScale = new Vector3(_planetInfo.Radius, _planetInfo.Radius, _planetInfo.Radius);
        }

        private void SetGOStartingPositionRotation()
        {
            float radius = _planetInfo.Index;
            float angle = Random.Range(0f, Mathf.PI * 2);
            float xComponent = Mathf.Cos(angle) * radius;
            float zComponent = Mathf.Sin(angle) * radius;

            _startingPosition = new Vector3(xComponent, 0f, zComponent);
            transform.position = _startingPosition;
            transform.rotation = Quaternion.Euler(0, 0, _planetInfo.AxialTilt);
        }
        
        private void SetGOAxialTiltMarker()
        {
            _axialTiltMarker = Instantiate(AxialTiltMarkerPrefab, transform.position, Quaternion.identity);
            _axialTiltMarker.transform.parent = transform;
            _axialTiltMarker.transform.localPosition = Vector3.zero;
            _axialTiltMarker.transform.localScale = new Vector3(1.0f, 1.0f, 0.0f);
            _axialTiltMarker.transform.localRotation = Quaternion.Euler(-_planetInfo.AxialTilt, 0f, 0f);
        }

        private void SetGOSpinDirectionMarker()
        {
            _spinDirectionMarker = Instantiate(SpinDirectionMarkerPrefab, transform.position, Quaternion.identity);
            _spinDirectionMarker.transform.parent = transform;
            _spinDirectionMarker.transform.localPosition = Vector3.zero;
            _spinDirectionMarker.transform.localScale = new Vector3(1.0f, 1.0f, 0.0f);
            _spinDirectionMarker.transform.localRotation = Quaternion.Euler(0f, 0f, 0f);
        }

        private void TrySetGOPlanetaryRings() // TODO: FIX THIS SHIT
        {
            if (!_planetInfo.HasRings) return;

            _planetaryRings = Instantiate(PlanetaryRingsPrefab, transform.position, Quaternion.identity);
            _planetaryRings.transform.parent = transform;
            _planetaryRings.transform.localPosition = Vector3.zero;

            float minInner = _planetInfo.Radius + 3f;
            float maxInner = _planetInfo.Radius + 7.5f;
            _planetInfo.InnerRingRadius = Mathf.InverseLerp(minInner, maxInner, _planetInfo.InnerRingRadius);
            _planetInfo.InnerRingRadius = Mathf.Clamp(_planetInfo.InnerRingRadius, 0.1f, 0.4f);
            _planetaryRings.transform.localScale = new Vector3(_planetInfo.OuterRingRadius, 0f, _planetInfo.OuterRingRadius);
            _planetaryRings.transform.localRotation = _planetaryRings.transform.parent.localRotation;
            _planetaryRings.transform.GetComponent<MeshRenderer>().material.SetFloat(_innerRadius, _planetInfo.InnerRingRadius);
            _planetaryRings.transform.GetComponent<MeshRenderer>().material.SetColor(_ringColor, TryGenerateRandomRingsColor());
        }

        private void SetGOTrailRenderer()
        {
            _trailRenderer.startWidth = 5.0f;
            _trailRenderer.endWidth = 0.0f;
            _trailRenderer.time = _initialTrailTime;
            _trailRenderer.startColor = PlanetTrailColor;
            _trailRenderer.endColor = Color.clear;
        }

        private static Color TryGenerateRandomRingsColor()
        {
            float r = Random.Range(0.8f, 1.0f);
            float g = Random.Range(0.7f, 0.74f);
            float b = Random.Range(0.53f, 0.57f);
            return new Color(r, g, b);
        }

        private void RotateAroundOwnAxis()
        {
            transform.Rotate(Vector3.up, Time.fixedDeltaTime * GameSpeedController.Instance.CurSpeed);
        }

        private void OrbitAroundStar()
        {
            _elapsedTime += Time.fixedDeltaTime * GameSpeedController.Instance.CurSpeed;
            float angle = (_elapsedTime * (ReverseOrbitDirection ? -1 : 1) * 360.0f / ((_planetInfo.OrbitalPeriod / ConstantsUtil.EARTH_YEAR) / 0.01f));
            float radians = angle * Mathf.Deg2Rad;

            Vector3 newPos = new(
                _orbitCenter.x + Mathf.Cos(radians) * _orbitRadius,
                _orbitCenter.y,
                _orbitCenter.z + Mathf.Sin(radians) * _orbitRadius
            );

            transform.position = newPos;
        }

        private void TryCalculateTrailRendererLength()
        {
            float targetTrailTime = TrailTimersUtil.PlanetTrailTime(GameSpeedController.Instance.CurSpeed);
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
}
