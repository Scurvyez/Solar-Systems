using SolarSystemUI;
using UnityEngine;

namespace SolarSystem
{
    public class PlanetController : MonoBehaviour
    {
        public GameObject starPrefab;
        public GameObject planetaryRingsPrefab;
        public GameObject axialTiltMarkerPrefab;
        public GameObject spinDirectionMarkerPrefab;
        
        public bool reverseOrbitDirection = false; // Flag to reverse planet orbit direction
        public Vector3 StartingPosition;
        public float Radius;
        public float RotationPeriod;
        public float OrbitalPeriod;
        public float AxialTilt;
        public bool IsHabitable;
        public bool HasRings;
        public float InnerRingRadius;
        public float OuterRingRadius;

        private float _elapsedTime;
        private GameObject _planetaryRings;
        private GameObject _axialTiltMarker;
        private GameObject _spinDirectionMarker;
        private static readonly int _innerRadius = Shader.PropertyToID("_InnerRadius");
        private static readonly int _ringColor = Shader.PropertyToID("_RingColor");
        private DebugMarkers_UI _debugMarkersUI;
        private TrailRenderer _trailRenderer;

        public void Start()
        {
            _trailRenderer = GetComponent<TrailRenderer>();
            _debugMarkersUI = GameObject.Find("UI_DebugMarkers").GetComponent<DebugMarkers_UI>();
            SetGOStartingPosition();
            SetGOScale();
            SetGOAxialTiltMarker();
            SetGOSpinDirectionMarker();
            //TrySetGOPlanetaryRings();
        }

        public void Update()
        {
            ToggleMarkerVisibility();
            
            if (GameSpeedController.Instance is null) return;
            _trailRenderer.time = 0.5f * GameSpeedController.Instance.CurSpeed;
            RotateAroundOwnAxis();
            OrbitAroundStar();
        }

        private void SetGOScale()
        {
            transform.localScale = new Vector3(Radius, Radius, Radius);
        }
        
        private void SetGOStartingPosition()
        {
            transform.position = StartingPosition;
        }

        private void SetGOAxialTiltMarker()
        {
            _axialTiltMarker = Instantiate(axialTiltMarkerPrefab, transform.position, Quaternion.identity);
            _axialTiltMarker.transform.parent = transform;
            _axialTiltMarker.transform.localPosition = Vector3.zero;
            _axialTiltMarker.transform.localScale = new Vector3(1.0f, 1.0f, 0.0f);
            _axialTiltMarker.transform.localRotation = Quaternion.Euler(-AxialTilt, 0f, 0f);
        }

        private void SetGOSpinDirectionMarker()
        {
            _spinDirectionMarker = Instantiate(spinDirectionMarkerPrefab, transform.position, Quaternion.identity);
            _spinDirectionMarker.transform.parent = transform;
            _spinDirectionMarker.transform.localPosition = Vector3.zero;
            _spinDirectionMarker.transform.localScale = new Vector3(1.0f, 1.0f, 0.0f);
            _spinDirectionMarker.transform.localRotation = Quaternion.Euler(0f, 0f, 0f);
        }

        private void TrySetGOPlanetaryRings() // TODO: FIX THIS SHIT
        {
            if (!HasRings) return;
            
            _planetaryRings = Instantiate(planetaryRingsPrefab, transform.position, Quaternion.identity);
            _planetaryRings.transform.parent = transform;
            _planetaryRings.transform.localPosition = Vector3.zero;

            float minInner = Radius + 3f;
            float maxInner = Radius + 7.5f;
            InnerRingRadius = Mathf.InverseLerp(minInner, maxInner, InnerRingRadius);
            InnerRingRadius = Mathf.Clamp(InnerRingRadius, 0.1f, 0.4f);
            _planetaryRings.transform.localScale = new Vector3(OuterRingRadius, 0f, OuterRingRadius);
            _planetaryRings.transform.localRotation = _planetaryRings.transform.parent.localRotation;
            _planetaryRings.transform.GetComponent<MeshRenderer>().material.SetFloat(_innerRadius, InnerRingRadius);
            _planetaryRings.transform.GetComponent<MeshRenderer>().material.SetColor(_ringColor, TryGenerateRandomRingsColor());
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
            transform.Rotate(Vector3.up, Time.deltaTime * GameSpeedController.Instance.CurSpeed);
        }

        private void OrbitAroundStar()
        {
            _elapsedTime += Time.deltaTime * GameSpeedController.Instance.CurSpeed;
            float orbitSpeed = 2 * Mathf.PI / OrbitalPeriod;

            if (!reverseOrbitDirection)
            {
                TryCalculateOrbit(starPrefab.transform.position, -orbitSpeed);
            }
            else
            {
                TryCalculateOrbit(starPrefab.transform.position, orbitSpeed);
            }
        }
        
        private void TryCalculateOrbit(Vector3 center, float speed)
        {
            Vector3 offset = StartingPosition - center;
            float angle = _elapsedTime * speed;
            Vector3 newPosition = center + Quaternion.Euler(0, angle * Mathf.Rad2Deg, 0) * offset;
            transform.position = newPosition;
        }

        private void ToggleMarkerVisibility()
        {
            if (_debugMarkersUI is null) return;
            _axialTiltMarker?.SetActive(_debugMarkersUI.ShowAxialTiltMarkers);
            _spinDirectionMarker?.SetActive(_debugMarkersUI.ShowSpinDirectionMarkers);
        }
    }
}
