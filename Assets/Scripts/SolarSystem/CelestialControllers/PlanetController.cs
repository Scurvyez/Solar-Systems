using System.Linq;
using UnityEngine;
using Utils;

namespace SolarSystem
{
    public class PlanetController : MonoBehaviour
    {
        public GameObject Star;
        public bool ReverseOrbitDirection;
        
        private float _curGameSpeed;
        private float _elapsedTime;
        private PlanetInfo _planetInfo;
        private Rigidbody _rigidBody;
        private MeshFilter _meshFilter;
        private MeshRenderer _meshRenderer;
        private MeshCollider _meshCollider;
        private Light _planetLight;
        private GameObject _lightObject;
        private UI_TooltipTrigger _uiTooltipTrigger;
        private Vector3 _startingPosition;
        private Vector3 _orbitCenter;
        private float _orbitRadius;
        private Vector4 _fakeAtmosphereColor;
        private Vector4 _finalAtmosphereColor;
        private MaterialPropertyBlock _materialPropertyBlock;
        
        public void Start()
        {
            Star = GameObject.Find("Star");
            
            _planetInfo = GetComponent<PlanetInfo>();
            _rigidBody = GetComponent<Rigidbody>();
            _meshRenderer = GetComponent<MeshRenderer>();
            _meshFilter = GetComponent<MeshFilter>();
            _meshCollider = GetComponent<MeshCollider>();
            _uiTooltipTrigger = GetComponent<UI_TooltipTrigger>();
            _uiTooltipTrigger.ContentKey = gameObject.name;

            _fakeAtmosphereColor = new Vector4(0.75f, 0.75f, 0.75f, 0.75f);
            _finalAtmosphereColor = ColorUtil.GetAtmosphereColor(_planetInfo.AtmosphereComposition);
            _materialPropertyBlock = new MaterialPropertyBlock();
            
            SetGOInitialComps();
            AssignMaterialProperties();
            SetShaderAtmosphereColor();
            SetGOStartingPosition();
            SetGOScale();
            
            _orbitCenter = Star.transform.position;
            _orbitRadius = Vector3.Distance(transform.position, _orbitCenter);

            // Calculate initial elapsed time based on the starting position (for orbit)
            float initialAngle = Mathf.Atan2(transform.position.z - _orbitCenter.z, transform.position.x - _orbitCenter.x);
            _elapsedTime = initialAngle / Mathf.Deg2Rad / (360.0f / _planetInfo.OrbitalPeriod);
        }

        private void Update()
        {
            _curGameSpeed = GameSpeedController.Instance.CurSpeed;
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
                material.SetTexture(ShaderPropertyIDs.MainTex, albedo);
            }
            if (ambientOcclusion != null)
            {
                material.SetTexture(ShaderPropertyIDs.AOTex, ambientOcclusion);
            }
            if (normalMap != null)
            {
                material.SetTexture(ShaderPropertyIDs.NormalMap, normalMap);
            }
            if (heightMap != null)
            {
                material.SetTexture(ShaderPropertyIDs.HeightMap, heightMap);
            }

            if (!_planetInfo.HasAtmosphere) return;
            TexturesUtil.GetCloudTexture(out Texture2D cloudTexture);
            
            if (cloudTexture != null)
            {
                material.SetTexture(ShaderPropertyIDs.CloudTex, cloudTexture);
            }
        }
        
        private void AssignMaterialProperties()
        {
            switch (_planetInfo.PlanetType)
            {
                case "RockyPlanet":
                {
                    AssignPlanetTextures(_meshRenderer.material, 
                        TexturesUtil.RockyPlanetTextureFolders.ElementAt(Random.Range(1, TexturesUtil.RockyPlanetTextureFolders.Length)));
                    _meshRenderer.SetPropertyBlock(_materialPropertyBlock);
                    break;
                }
                case "GasGiant":
                {
                    AssignPlanetTextures(_meshRenderer.material, 
                        TexturesUtil.GasGiantTextureFolders.ElementAt(Random.Range(1, TexturesUtil.GasGiantTextureFolders.Length)));
                    _meshRenderer.SetPropertyBlock(_materialPropertyBlock);
                    break;
                }
            }
        }

        private void SetShaderAtmosphereColor()
        {
            _materialPropertyBlock.SetColor(ShaderPropertyIDs.AtmosphereColor,
                _planetInfo.HasAtmosphere ? _finalAtmosphereColor : _fakeAtmosphereColor);
            _meshRenderer.SetPropertyBlock(_materialPropertyBlock);
        }

        private void UpdateShaderLightProperties()
        {
            if (_planetLight is null) return;
            _materialPropertyBlock.SetVector(ShaderPropertyIDs.AmbientLightDirection, 
                _planetLight.transform.forward);
            _meshRenderer.SetPropertyBlock(_materialPropertyBlock);
        }

        private void SetGOInitialComps()
        {
            if (_rigidBody != null)
            {
                _rigidBody.mass = _planetInfo.Mass;
                _rigidBody.constraints = RigidbodyConstraints.FreezePositionY;
                _rigidBody.useGravity = false;
            }

            if (_meshFilter != null && _meshCollider != null)
            {
                _meshCollider.sharedMesh = _meshFilter.mesh;
            }

            if (_planetInfo == null) return;
            _lightObject = new GameObject(_planetInfo.Name + "_Light");
            _planetLight = _lightObject.AddComponent<Light>();
            _planetLight.type = LightType.Directional;
            _planetLight.shadows = LightShadows.Hard;
            _planetLight.useColorTemperature = true;
            _planetLight.colorTemperature = 6750f;
            _planetLight.intensity = 1f;
            _planetLight.cullingMask = 1 << LayerMask.NameToLayer(_planetInfo.LayerName);

            _lightObject.transform.position = Star.transform.position;
            _lightObject.transform.SetParent(null, true);
        }

        private void SetGOScale()
        {
            transform.localScale = new Vector3(
                _planetInfo.GO_Radius, 
                _planetInfo.GO_Radius, 
                _planetInfo.GO_Radius);
        }

        private void SetGOStartingPosition()
        {
            float radius = _planetInfo.Index;
            float angle = Random.Range(0f, Mathf.PI * 2);
            float xComponent = Mathf.Cos(angle) * radius;
            float zComponent = Mathf.Sin(angle) * radius;

            _startingPosition = new Vector3(xComponent, 0f, zComponent);
            transform.position = _startingPosition;
        }
        
        private void UpdateOrbitAroundStar()
        {
            _elapsedTime += Time.fixedDeltaTime * _curGameSpeed;
            float angle = (_elapsedTime * (ReverseOrbitDirection ? -1 : 1) * 360.0f / ((_planetInfo.OrbitalPeriod / ConstantsUtil.EARTH_YEAR) / 0.01f));
            float radians = angle * Mathf.Deg2Rad;

            Vector3 newPos = new(
                _orbitCenter.x + Mathf.Cos(radians) * _orbitRadius,
                _orbitCenter.y,
                _orbitCenter.z + Mathf.Sin(radians) * _orbitRadius
            );

            transform.position = newPos;
            transform.LookAt(Star.transform);
            transform.rotation *= Quaternion.Euler(0f, -90f, 0f);
        }

        private void UpdatePlanetaryLightRotation()
        {
            _lightObject?.transform.LookAt(transform);
        }
    }
}
