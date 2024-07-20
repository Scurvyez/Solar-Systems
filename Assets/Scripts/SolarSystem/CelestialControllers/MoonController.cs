using SolarSystem;
using SolarSystemUI;
using UnityEngine;
using Random = UnityEngine.Random;

public class MoonController : MonoBehaviour
{
    public GameObject StarPrefab;
    public GameObject AxialTiltMarkerPrefab;
    public GameObject SpinDirectionMarkerPrefab;
    
    private GameObject _axialTiltMarker;
    private GameObject _spinDirectionMarker;
    private UI_DebugMarkers _uiDebugMarkers;
    private MoonInfo _moonInfo;
    private Rigidbody _rigidBody;
    private Vector3 _startingPosition;
    private MaterialPropertyBlock _materialPropertyBlock;
    
    private static readonly int _mainTex = Shader.PropertyToID("_MainTex");
    
    private void Start()
    {
        _moonInfo = GetComponent<MoonInfo>();
        _rigidBody = GetComponent<Rigidbody>();
        _uiDebugMarkers = GameObject.Find("UI_DebugMarkers").GetComponent<UI_DebugMarkers>();
        
        _materialPropertyBlock = new MaterialPropertyBlock();
        AssignMaterialProperties();
        
        SetGOInitialComps();
        SetRandomGOStartingPosition();
        SetGOScale();
        SetGOAxialTiltMarker();
        SetGOSpinDirectionMarker();
    }

    private void Update()
    {
        ToggleMarkerVisibility();
        transform.LookAt(StarPrefab.transform);
    }

    private void AssignMaterialProperties()
    {
        Texture2D[] textures = Resources.LoadAll<Texture2D>("Textures/Moon");
        
        Texture2D randomTexture = textures[Random.Range(0, textures.Length)];
        _materialPropertyBlock.SetTexture(_mainTex, randomTexture);
        GetComponent<MeshRenderer>().SetPropertyBlock(_materialPropertyBlock);
    }
    
    private void SetGOInitialComps()
    {
        if (_rigidBody is null) return;
        _rigidBody.mass = _moonInfo.Mass;
        _rigidBody.constraints = RigidbodyConstraints.FreezePosition;
        _rigidBody.useGravity = false;
        
        if (transform.parent.GetComponent<PlanetInfo>() == null) return;
        PlanetInfo parentPlanetInfo = transform.parent.GetComponent<PlanetInfo>();
        gameObject.layer = LayerMask.NameToLayer(parentPlanetInfo.LayerName);
    }

    private void SetGOScale()
    {
        Vector3 desiredWorldScale = new (_moonInfo.Radius, _moonInfo.Radius, _moonInfo.Radius);
        Vector3 parentScale = transform.parent.localScale;
        transform.localScale = new Vector3(
            desiredWorldScale.x / parentScale.x,
            desiredWorldScale.y / parentScale.y,
            desiredWorldScale.z / parentScale.z
        );
    }
    
    private void SetRandomGOStartingPosition()
    {
        Transform parentPlanet = transform.parent;
        SphereCollider parentCollider = parentPlanet.GetComponent<SphereCollider>();
        if (parentCollider is null)
        {
            Debug.LogError("[MoonController.SetRandomGOStartingPosition] Parent planet does not have a SphereCollider.");
            return;
        }
        
        PlanetInfo parentPlanetInfo = parentPlanet.GetComponent<PlanetInfo>();
        if (parentPlanetInfo is null)
        {
            Debug.LogError("[MoonController.SetRandomGOStartingPosition] Parent planet does not have a PlanetInfo component.");
            return;
        }

        MoonControllerUtil.cachedMoonPositions.Clear();
        float parentPlanetRadius = parentPlanetInfo.GO_Radius;
        transform.localRotation = Quaternion.Euler(0, 0, _moonInfo.AxialTilt);

        // Define the minimum and maximum distance for the moons
        float minDistance = parentPlanetRadius + _moonInfo.Radius + 2f; // Minimum distance from the parent planet
        float maxDistance = parentPlanetRadius + _moonInfo.Radius + 3f; // Maximum distance from the parent planet

        // Attempt to find a valid position
        Vector3 point;
        int maxAttempts = 1000; // Limit the number of attempts to prevent infinite loops
        int attempt = 0;

        do
        {
            // Generate a random point on a unit sphere
            Vector3 randomPointOnSphere = Random.onUnitSphere;

            // Scale the point based on the parent planet's scale
            float distanceFromParent = Random.Range(minDistance, maxDistance);
            point = randomPointOnSphere * distanceFromParent + parentPlanet.position;

            attempt++;
            if (attempt <= maxAttempts) continue;
            Debug.LogError($"[MoonController.SetRandomGOStartingPosition] Could not find a valid position for the moon after {maxAttempts} attempts.");
            break;

        } while (!MoonControllerUtil.IsPositionValid(point, _moonInfo.Radius));

        // Set the moon's position
        transform.position = point;

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
    
    private void ToggleMarkerVisibility()
    {
        if (_uiDebugMarkers is null) return;
        _axialTiltMarker?.SetActive(_uiDebugMarkers.ShowAxialTiltMarkers);
        _spinDirectionMarker?.SetActive(_uiDebugMarkers.ShowSpinDirectionMarkers);
    }
}
