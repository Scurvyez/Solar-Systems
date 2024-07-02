using UnityEngine;

public class StarController : MonoBehaviour
{
    public SaveManager SaveManager;
    public StarController instance;
    public float StarMass;
    public float StarRadius;
    public float StarRotation;
    public float StarMagneticField;

    private static readonly int _chromaticity = Shader.PropertyToID("_Chromaticity");
    private static readonly int _cellColor = Shader.PropertyToID("_CellColor");
    private static readonly int _solarFlare = Shader.PropertyToID("_SolarFlare");
    private float _currentVariability;
    private float _timer = 0;
    private float _variability;
    private float _startVariability;
    private float _endVariability;
    private Rigidbody _starRigidBody;
    private Renderer _parentMaterial;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        StarMass = SaveManager.instance.activeSave.starMass;
        StarRadius = SaveManager.instance.activeSave.starActualRadius;
        StarRotation = SaveManager.instance.activeSave.starRotation;
        transform.position = SaveManager.instance.activeSave.starPosition;
        StarMagneticField = SaveManager.instance.activeSave.starMagneticField;
        Color localChromaticity = SaveManager.instance.activeSave.starChromaticity;
        Color localCellColor = SaveManager.instance.activeSave.starCellColor;
        _variability = SaveManager.instance.activeSave.starVariability;
        
        _starRigidBody = transform.GetComponent<Rigidbody>();
        _parentMaterial = transform.GetComponent<Renderer>();
        
        transform.localScale = new Vector3(StarRadius, StarRadius, StarRadius);
        
        _starRigidBody.mass = StarMass;
        _startVariability = -_variability;
        _endVariability = _variability;
        _parentMaterial.material.SetColor(_chromaticity, localChromaticity);
        _parentMaterial.material.SetColor(_cellColor, localCellColor);
    }

    private void Update()
    {
        transform.Rotate((StarRotation * StarMagneticField) * Vector3.up);

        _timer += Time.deltaTime;
        float t = _timer;

        if (SaveManager.instance.activeSave.hasExtrinsicVariability)
        {
            _currentVariability = Mathf.Lerp(_startVariability, _endVariability, t);
            _parentMaterial.material.SetFloat(_solarFlare, _currentVariability);
        }
        else if (SaveManager.instance.activeSave.hasIntrinsicVariability)
        {
            _currentVariability = Mathf.Lerp(Random.Range(_startVariability, _endVariability), Random.Range(_startVariability, _endVariability), t);
            _parentMaterial.material.SetFloat(_solarFlare, _currentVariability);
        }

        if (!(t >= 1)) return;
        _timer = 0;
        (_startVariability, _endVariability) = (_endVariability, _startVariability);
    }
}
