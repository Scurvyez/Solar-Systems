using System.Linq;
using SolarSystem;
using UnityEngine;
using Utils;
using Random = UnityEngine.Random;

public class MoonController : MonoBehaviour
{
    public GameObject Star;
    public GameObject Planet;
    
    private MoonInfo _moonInfo;
    private Rigidbody _rigidBody;
    private MeshFilter _meshFilter;
    private MeshRenderer _meshRenderer;
    private MeshCollider _meshCollider;
    private Light _planetLight;
    private GameObject _lightObject;
    private UI_TooltipTrigger _uiTooltipTrigger;
    private Vector3 _startingPosition;
    private Vector3 _initialOffset;
    private Vector4 _fakeAtmosphereColor;
    private Vector4 _finalAtmosphereColor;
    private MaterialPropertyBlock _materialPropertyBlock;
    
    private void Start()
    {
        Star = GameObject.Find("Star");
        _moonInfo = GetComponent<MoonInfo>();
        Planet = GameObject.Find(_moonInfo.ParentPlanetName);
        _rigidBody = GetComponent<Rigidbody>();
        _meshRenderer = GetComponent<MeshRenderer>();
        _meshFilter = GetComponent<MeshFilter>();
        _meshCollider = GetComponent<MeshCollider>();
        _uiTooltipTrigger = GetComponent<UI_TooltipTrigger>();
        _uiTooltipTrigger.ContentKey = gameObject.name;
        _lightObject = GameObject.Find(_moonInfo.ParentPlanetName + "_Light");
        _planetLight = _lightObject.GetComponent<Light>();
        _fakeAtmosphereColor = new Vector4(0.75f, 0.75f, 0.75f, 0.75f);
        _finalAtmosphereColor = ColorUtil.GetAtmosphereColor(_moonInfo.AtmosphereComposition);
        _materialPropertyBlock = new MaterialPropertyBlock();
        
        SetGOInitialComps();
        AssignMaterialProperties();
        SetShaderAtmosphereColor();
        SetRandomGOStartingPosition();
        SetGOScale();
    }

    private void Update()
    {
        UpdateShaderLightProperties();
    }
    
    private void FixedUpdate()
    {
        UpdateRotationRelativeToStar();
        UpdatePositionRelativeToPlanet();
    }
    
    private static void AssignMoonTextures(Material material, string moonTypeFolder)
    {
        TexturesUtil.GetPlanetTextures(moonTypeFolder, out Texture2D albedo, out Texture2D ambientOcclusion, out Texture2D normalMap, out Texture2D heightMap);
        TexturesUtil.GetCloudTexture(out Texture2D cloudTexture);
        
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
        if (cloudTexture != null)
        {
            material.SetTexture(ShaderPropertyIDs.CloudTex, cloudTexture);
        }
    }

    private void AssignMaterialProperties()
    {
        AssignMoonTextures(_meshRenderer.material, TexturesUtil.MoonTextureFolders.ElementAt(Random.Range(1, TexturesUtil.MoonTextureFolders.Length)));
        _meshRenderer.SetPropertyBlock(_materialPropertyBlock);
    }
    
    private void SetShaderAtmosphereColor()
    {
        _materialPropertyBlock.SetColor(ShaderPropertyIDs.AtmosphereColor,
            _moonInfo.HasAtmosphere ? _finalAtmosphereColor : _fakeAtmosphereColor);
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
            _rigidBody.mass = _moonInfo.Mass;
            _rigidBody.constraints = RigidbodyConstraints.FreezePosition;
            _rigidBody.useGravity = false;
        }
        
        if (_meshFilter != null && _meshCollider != null)
        {
            _meshCollider.sharedMesh = _meshFilter.mesh;
        }
        
        if (Planet.GetComponent<PlanetInfo>() == null) return;
        PlanetInfo parentPlanetInfo = Planet.GetComponent<PlanetInfo>();
        gameObject.layer = LayerMask.NameToLayer(parentPlanetInfo.LayerName);
    }

    private void SetGOScale()
    {
        Vector3 desiredWorldScale = new (_moonInfo.Radius, _moonInfo.Radius, _moonInfo.Radius);
        transform.localScale = desiredWorldScale;
    }
    
    private void SetRandomGOStartingPosition()
    {
        Transform parentPlanet = Planet.transform;
        PlanetInfo parentPlanetInfo = parentPlanet.GetComponent<PlanetInfo>();
        float parentPlanetRadius = parentPlanetInfo.GO_Radius;
        transform.localRotation = Quaternion.Euler(0, 0, _moonInfo.AxialTilt);

        // in world coordinates
        float minDistance = parentPlanetRadius + _moonInfo.Radius + 0.5f;
        float maxDistance = parentPlanetRadius + _moonInfo.Radius + 1.5f;

        Vector3 point;
        int maxAttempts = 1000; // Limit the number of attempts to prevent infinite loops
        int attempt = 0;

        do
        {
            Vector3 randomPointOnSphere = Random.onUnitSphere;
            float distanceFromParent = Random.Range(minDistance, maxDistance);
            point = randomPointOnSphere * distanceFromParent + parentPlanet.position;

            attempt++;
            if (attempt <= maxAttempts) continue;
            Debug.LogError($"Could not find a valid position for the moon after {maxAttempts} attempts.");
            break;

        } while (!MoonControllerUtil.IsPositionValid(point, _moonInfo.Radius));

        transform.position = point;
        MoonControllerUtil.AddCachedPosition(transform.position);
        _initialOffset = transform.position - parentPlanet.position;
    }
    
    private void UpdateRotationRelativeToStar()
    {
        transform.LookAt(Star.transform);
        transform.rotation *= Quaternion.Euler(0f, -90f, 0f);
    }
    
    private void UpdatePositionRelativeToPlanet()
    {
        if (Planet is null) return;
        transform.position = Planet.transform.position + _initialOffset;
    }
}
