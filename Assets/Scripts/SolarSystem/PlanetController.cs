using UnityEngine;

namespace SolarSystem
{
    public class PlanetController : MonoBehaviour
    {
        public SaveManager SaveManager;
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

        public void Start()
        {
            transform.Rotate(Vector3.right, AxialTilt);
            transform.position = StartingPosition;
            transform.localScale = new Vector3(Radius, Radius, Radius);
            //InstantiateDebugVisuals();

            /*
            if (HasRings)
            {
                InstantiatePlanetaryRings();
            }
            */
        }

        public void Update()
        {
            // Rotate the planet around its own axis
            transform.Rotate(Vector3.up, Time.deltaTime * RotationPeriod * GameSpeedController.Instance.curSpeed);

            // Orbit the star
            _elapsedTime += Time.deltaTime * GameSpeedController.Instance.curSpeed;

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
            float angle = _elapsedTime * speed;
            Vector3 newPosition = center + Quaternion.Euler(0, angle * Mathf.Rad2Deg, 0) * offset;

            transform.position = newPosition;
        }

        /*
        private static Color GenerateRandomRingsColor()
        {
            float r = Random.Range(0.8f, 1.0f);
            float g = Random.Range(0.7f, 0.74f);
            float b = Random.Range(0.53f, 0.57f);
            return new Color(r, g, b);
        }
        */

        /*
        private void InstantiatePlanetaryRings()
        {
            // Create planetary rings if planet has them
            _planetaryRings = Instantiate(planetaryRingsPrefab, transform.position, Quaternion.identity);
            _planetaryRings.transform.parent = transform;
            _planetaryRings.transform.localPosition = Vector3.zero;

            // Remap innerRingsDistance to a value between 0 and 1
            float minInner = Radius + 3f;
            float maxInner = Radius + 7.5f;
            InnerRingRadius = Mathf.InverseLerp(minInner, maxInner, InnerRingRadius);
            InnerRingRadius = Mathf.Clamp(InnerRingRadius, 0.1f, 0.4f);
            _planetaryRings.transform.localScale = new Vector3(OuterRingRadius, 0f, OuterRingRadius);
            _planetaryRings.transform.localRotation = _planetaryRings.transform.parent.localRotation;
            _planetaryRings.transform.GetComponent<MeshRenderer>().material.SetFloat(_innerRadius, InnerRingRadius);
            _planetaryRings.transform.GetComponent<MeshRenderer>().material.SetColor(_ringColor, GenerateRandomRingsColor());
            // add the rest of the stuff
        }
        */
        
        /*
        private void InstantiateDebugVisuals()
        {
            // Create the axial tilt marker object
            _axialTiltMarker = Instantiate(axialTiltMarkerPrefab, transform.position, Quaternion.identity);
            _axialTiltMarker.transform.parent = transform;
            _axialTiltMarker.transform.localPosition = Vector3.zero;
            _axialTiltMarker.transform.localScale = new Vector3(1.0f, 1.0f, 0.0f);
            _axialTiltMarker.transform.localRotation = Quaternion.Euler(-AxialTilt, 0f, 0f);

            // Create the spin direction marker object
            _spinDirectionMarker = Instantiate(spinDirectionMarkerPrefab, transform.position, Quaternion.identity);
            _spinDirectionMarker.transform.parent = transform;
            _spinDirectionMarker.transform.localPosition = Vector3.zero;
            _spinDirectionMarker.transform.localScale = new Vector3(1.0f, 1.0f, 0.0f);
            _spinDirectionMarker.transform.localRotation = Quaternion.Euler(0f, 0f, 0f); // Set the initial rotation to zero
        }
        */
    }    
}
