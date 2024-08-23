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
        
        private Camera _mainCamera;
        private MeshRenderer _meshRenderer;
        private Light _lightSource;
        private PlanetInfo _planetInfo;
        private Vector3 _startingPosition;
        private Vector4 _fakeAtmosphereColor;
        private Vector4 _finalAtmosphereColor;
        private MaterialPropertyBlock _materialPropertyBlock;

        public void Start()
        {
            _mainCamera = Camera.main;
            _meshRenderer = GetComponent<MeshRenderer>();
            _planetInfo = GetComponent<PlanetInfo>();
            
            _fakeAtmosphereColor = new Vector4(0.75f, 0.75f, 0.75f, 0.75f);
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
            SetGOStartingPositionRotation(); // FOR TESTING
            UpdateShaderLightProperties();
        }
        
        private void SetGOStartingPositionRotation()
        {
            // Get the main camera
            if (_mainCamera is null)
            {
                Debug.LogError("Main camera not found");
                return;
            }

            // Calculate the target position
            Vector3 cameraPosition = _mainCamera.transform.position;
            Vector3 cameraForward = _mainCamera.transform.forward;
            Vector3 cameraRight = _mainCamera.transform.right;

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
        
        private void SetShaderAtmosphereColor()
        {
            _materialPropertyBlock.SetColor(ShaderPropertyIDs.AtmosphereColor,
                _planetInfo.HasAtmosphere ? _finalAtmosphereColor : _fakeAtmosphereColor);
            _meshRenderer.SetPropertyBlock(_materialPropertyBlock);
        }

        private void UpdateShaderLightProperties()
        {
            _materialPropertyBlock.SetVector(ShaderPropertyIDs.AmbientLightDirection, _lightSource.transform.forward);
            _meshRenderer.SetPropertyBlock(_materialPropertyBlock);
        }
        
        private static Light FindLightSource()
        {
            GameObject lightObject = GameObject.Find("MenuPlanetLight");

            if (lightObject == null) return null;
            Light lightComponent = lightObject.GetComponent<Light>();
            return lightComponent;
        }
    }
}