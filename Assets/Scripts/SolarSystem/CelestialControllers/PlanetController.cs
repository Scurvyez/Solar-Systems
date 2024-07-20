using System.Linq;
using SolarSystemUI;
using UnityEngine;
using Utils;

namespace SolarSystem
{
    public class PlanetController : MonoBehaviour
    {
        public GameObject StarPrefab;
        public GameObject AxialTiltMarkerPrefab;
        public GameObject SpinDirectionMarkerPrefab;
        public bool ReverseOrbitDirection;
        
        private float _elapsedTime;
        private GameObject _planetaryRings;
        private GameObject _axialTiltMarker;
        private GameObject _spinDirectionMarker;
        private UI_DebugMarkers _uiDebugMarkers;
        private PlanetInfo _planetInfo;
        private Rigidbody _rigidBody;
        private Light _planetLight;
        private GameObject _lightObject;
        private MaterialPropertyBlock _materialPropertyBlock;
        private Vector3 _startingPosition;
        private Vector3 _orbitCenter;
        private float _orbitRadius;
        private Vector4 _finalAtmosphereColor;
        
        private static readonly int _mainTex = Shader.PropertyToID("_MainTex");
        private static readonly int _aOTex = Shader.PropertyToID("_AOTex");
        private static readonly int _normalMap = Shader.PropertyToID("_NormalMap");
        private static readonly int _heightMap = Shader.PropertyToID("_HeightMap");
        private static readonly int _atmosphereColor = Shader.PropertyToID("_AtmosphereColor");
        private static readonly int _ambientLightDirection = Shader.PropertyToID("_AmbientLightDirection");
        private MeshRenderer _meshRenderer;

        public void Start()
        {
            _meshRenderer = GetComponent<MeshRenderer>();
            _planetInfo = GetComponent<PlanetInfo>();
            _rigidBody = GetComponent<Rigidbody>();
            _uiDebugMarkers = GameObject.Find("UI_DebugMarkers").GetComponent<UI_DebugMarkers>();
            
            _finalAtmosphereColor = ColorUtil.GetAtmosphereColor(_planetInfo.AtmosphereComposition);
            _materialPropertyBlock = new MaterialPropertyBlock();
            AssignMaterialProperties();
            SetShaderAtmosphereColor();
            
            SetGOStartingPositionRotation();
            SetGOScale();
            SetGOAxialTiltMarker();
            SetGOSpinDirectionMarker();
            
            _orbitCenter = StarPrefab.transform.position;
            _orbitRadius = Vector3.Distance(transform.position, _orbitCenter);

            // Calculate initial elapsed time based on the starting position (for orbit)
            float initialAngle = Mathf.Atan2(transform.position.z - _orbitCenter.z, transform.position.x - _orbitCenter.x);
            _elapsedTime = initialAngle / Mathf.Deg2Rad / (360.0f / _planetInfo.OrbitalPeriod);
            
            SetGOInitialComps();
        }

        private void Update()
        {
            UpdateMarkerVisibility();
            UpdateShaderLightProperties();
        }

        private void FixedUpdate()
        {
            UpdatePlanetaryLightRotation();
            UpdateOrbitAroundStar();
        }

        private void AssignPlanetTextures(Material material, string planetTypeFolder)
        {
            TexturesUtil.GetPlanetTextures(planetTypeFolder, out Texture2D albedo, out Texture2D ambientOcclusion, out Texture2D normalMap, out Texture2D heightMap);

            if (albedo != null)
            {
                material.SetTexture(_mainTex, albedo);
            }
            if (ambientOcclusion != null)
            {
                material.SetTexture(_aOTex, ambientOcclusion);
            }
            if (normalMap != null)
            {
                material.SetTexture(_normalMap, normalMap);
            }
            if (heightMap != null)
            {
                material.SetTexture(_heightMap, heightMap);
            }
        }
        
        private void AssignMaterialProperties()
        {
            switch (_planetInfo.PlanetType)
            {
                case "RockyPlanet":
                {
                    AssignPlanetTextures(_meshRenderer.material, TexturesUtil.RockyPlanetTextureFolders.ElementAt(Random.Range(1, TexturesUtil.RockyPlanetTextureFolders.Length)));
                    _meshRenderer.SetPropertyBlock(_materialPropertyBlock);
                    break;
                }
                case "GasGiant":
                {
                    AssignPlanetTextures(_meshRenderer.material, TexturesUtil.GasGiantTextureFolders.ElementAt(Random.Range(1, TexturesUtil.GasGiantTextureFolders.Length)));
                    _meshRenderer.SetPropertyBlock(_materialPropertyBlock);
                    break;
                }
            }
        }

        private void SetShaderAtmosphereColor()
        {
            if (_planetInfo.HasAtmosphere)
            {
                _materialPropertyBlock.SetColor(_atmosphereColor, _finalAtmosphereColor);
            }
            else
            {
                _materialPropertyBlock.SetColor(_atmosphereColor, Color.black);
            }
            _meshRenderer.SetPropertyBlock(_materialPropertyBlock);
        }

        private void UpdateShaderLightProperties()
        {
            if (_planetLight is null) return;
            _materialPropertyBlock.SetVector(_ambientLightDirection, _planetLight.transform.forward);
            _meshRenderer.SetPropertyBlock(_materialPropertyBlock);
        }

        private void SetGOInitialComps()
        {
            if (_rigidBody is null) return;
            _rigidBody.mass = _planetInfo.Mass;
            _rigidBody.constraints = RigidbodyConstraints.FreezePositionY;
            _rigidBody.useGravity = false;

            if (_planetInfo == null) return;

            // Create a new GameObject for the light
            _lightObject = new GameObject("PlanetLight");
            _planetLight = _lightObject.AddComponent<Light>();
            _planetLight.type = LightType.Directional;
            _planetLight.shadows = LightShadows.Hard;
            _planetLight.useColorTemperature = true;
            _planetLight.colorTemperature = 6750f;
            _planetLight.intensity = 1f;
            _planetLight.cullingMask = 1 << LayerMask.NameToLayer(_planetInfo.LayerName);

            // Set the light to not be a child of the planet
            _lightObject.transform.position = StarPrefab.transform.position;
            _lightObject.transform.SetParent(null, true);
        }

        private void SetGOScale()
        {
            if (transform.parent == null) return;
            Vector3 desiredWorldScale = new (_planetInfo.GO_Radius, _planetInfo.GO_Radius, _planetInfo.GO_Radius);
            Vector3 parentScale = transform.parent.localScale;
            transform.localScale = new Vector3(
                desiredWorldScale.x / parentScale.x,
                desiredWorldScale.y / parentScale.y,
                desiredWorldScale.z / parentScale.z
            );
        }

        private void SetGOStartingPositionRotation()
        {
            float radius = _planetInfo.Index;
            float angle = Random.Range(0f, Mathf.PI * 2);
            float xComponent = Mathf.Cos(angle) * radius;
            float zComponent = Mathf.Sin(angle) * radius;

            _startingPosition = new Vector3(xComponent, 0f, zComponent);
            transform.position = _startingPosition;
        }
        
        private void SetGOAxialTiltMarker()
        {
            _axialTiltMarker = Instantiate(AxialTiltMarkerPrefab, transform.position, Quaternion.identity);
            _axialTiltMarker.transform.parent = transform;
            _axialTiltMarker.transform.localPosition = transform.parent.position + new Vector3(0f, 2f, 0f);
            _axialTiltMarker.transform.localRotation = Quaternion.Euler(-_planetInfo.AxialTilt, 0f, 0f);
            _axialTiltMarker.transform.localScale = new Vector3(2f, 2f, 1f);
        }

        private void SetGOSpinDirectionMarker()
        {
            _spinDirectionMarker = Instantiate(SpinDirectionMarkerPrefab, transform.position, Quaternion.identity);
            _spinDirectionMarker.transform.parent = transform;
            _spinDirectionMarker.transform.localPosition = transform.parent.position + new Vector3(-2f, 0f, 0f);
            _spinDirectionMarker.transform.localRotation = Quaternion.Euler(0f, 90f, 0f);
            _spinDirectionMarker.transform.localScale = new Vector3(2f, 2f, 1f);
        }

        private void UpdateOrbitAroundStar()
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
            transform.LookAt(StarPrefab.transform);
            transform.rotation *= Quaternion.Euler(0f, -90f, 0f);
        }

        private void UpdatePlanetaryLightRotation()
        {
            _lightObject?.transform.LookAt(transform);
        }

        private void UpdateMarkerVisibility()
        {
            if (_uiDebugMarkers is null) return;
            _axialTiltMarker?.SetActive(_uiDebugMarkers.ShowAxialTiltMarkers);
            _spinDirectionMarker?.SetActive(_uiDebugMarkers.ShowSpinDirectionMarkers);
        }
    }
}
