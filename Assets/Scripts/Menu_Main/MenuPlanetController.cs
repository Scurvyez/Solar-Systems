using System.Linq;
using SolarSystem;
using UnityEngine;
using Utils;

namespace Menu_Main
{
    public class MenuPlanetController : MonoBehaviour
    {
        public float DistanceFromCamera = 10f;
        public float OffsetFromCenter = 2f;
        
        private PlanetInfo _planetInfo;
        private Vector3 _startingPosition;
        private MaterialPropertyBlock _materialPropertyBlock;
        private Vector4 _finalAtmosphereColor;
        private MeshRenderer _meshRenderer;
        private Light _lightSource;
        
        private static readonly int _mainTex = Shader.PropertyToID("_MainTex");
        private static readonly int _aOTex = Shader.PropertyToID("_AOTex");
        private static readonly int _normalMap = Shader.PropertyToID("_NormalMap");
        private static readonly int _heightMap = Shader.PropertyToID("_HeightMap");
        private static readonly int _atmosphereColor = Shader.PropertyToID("_AtmosphereColor");
        private static readonly int _atmosphereSize = Shader.PropertyToID("_AtmosphereSize");
        private static readonly int _ambientLightDirection = Shader.PropertyToID("_AmbientLightDirection");
        
        public void Start()
        {
            _meshRenderer = GetComponent<MeshRenderer>();
            _planetInfo = GetComponent<PlanetInfo>();
            
            _finalAtmosphereColor = ColorUtil.GetAtmosphereColor(_planetInfo.AtmosphereComposition);
            _materialPropertyBlock = new MaterialPropertyBlock();
            AssignMaterialProperties();
            SetShaderAtmosphereColor();
            SetGOStartingPositionRotation();
            
            _lightSource = FindLightSource();
            if (_lightSource == null)
            {
                Debug.LogError("Light source not found in the scene.");
            }
        }
        
        private void Update()
        {
            SetGOStartingPositionRotation();
            UpdateShaderLightProperties();
        }
        
        private void SetGOStartingPositionRotation()
        {
            // Get the main camera
            Camera mainCamera = Camera.main;
            if (mainCamera is null)
            {
                Debug.LogError("Main camera not found");
                return;
            }

            // Calculate the target position
            Vector3 cameraPosition = mainCamera.transform.position;
            Vector3 cameraForward = mainCamera.transform.forward;
            Vector3 cameraRight = mainCamera.transform.right;

            // Offset the position to the left
            Vector3 offset = -cameraRight * OffsetFromCenter;
            _startingPosition = cameraPosition + cameraForward * DistanceFromCamera + offset;
            transform.position = _startingPosition;
            
            transform.rotation = Quaternion.Euler(0f, 0f, 0f);
        }
        
        private void AssignMaterialProperties()
        {
            AssignPlanetTextures(_meshRenderer.material, TexturesUtil.RockyPlanetTextureFolders.ElementAt(Random.Range(1, TexturesUtil.RockyPlanetTextureFolders.Length)));
            _meshRenderer.SetPropertyBlock(_materialPropertyBlock);
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
        
        private void SetShaderAtmosphereColor()
        {
            if (_planetInfo.HasAtmosphere)
            {
                _materialPropertyBlock.SetColor(_atmosphereColor, _finalAtmosphereColor);
            }
            else
            {
                _materialPropertyBlock.SetColor(_atmosphereColor, Color.black);
                _materialPropertyBlock.SetFloat(_atmosphereSize, 0f);
            }
            _meshRenderer.SetPropertyBlock(_materialPropertyBlock);
        }

        private void UpdateShaderLightProperties()
        {
            _materialPropertyBlock.SetVector(_ambientLightDirection, _lightSource.transform.forward);
            _meshRenderer.SetPropertyBlock(_materialPropertyBlock);
        }
        
        private Light FindLightSource()
        {
            // Example: Finding a light by tag
            GameObject lightObject = GameObject.Find("MenuPlanetLight");

            if (lightObject != null)
            {
                Light lightComponent = lightObject.GetComponent<Light>();
                return lightComponent;
            }
            else
            {
                return null;
            }
        }
    }
}