using Saving;
using UnityEngine;

public class StarController : MonoBehaviour
{
    public StarController instance;
    public float StarMass;
    public float StarRadius;
    public float StarRotation;
    public float StarMagneticField;

    private Color _localChromaticity;
    private Color _localCellColor;
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
        SetPhysicalProperties();
        SetMaterialProperties();
    }

    private void Update()
    {
        RotateAroundOwnAxis();
        TryCalculateVariability();
    }

    private void SetPhysicalProperties()
    {
        StarMass = SaveManager.Instance.ActiveSave.starMass;
        StarRadius = SaveManager.Instance.ActiveSave.starActualRadius;
        StarRotation = SaveManager.Instance.ActiveSave.starRotation;
        StarMagneticField = SaveManager.Instance.ActiveSave.starMagneticField;
        transform.position = SaveManager.Instance.ActiveSave.starPosition;
        transform.localScale = new Vector3(StarRadius, StarRadius, StarRadius);
        
        _starRigidBody = transform.GetComponent<Rigidbody>();
        _starRigidBody.mass = StarMass;
    }

    private void SetMaterialProperties()
    {
        _localChromaticity = SaveManager.Instance.ActiveSave.starChromaticity;
        _localCellColor = SaveManager.Instance.ActiveSave.starCellColor;
        _variability = SaveManager.Instance.ActiveSave.starVariability;
        _startVariability = -_variability;
        _endVariability = _variability;
        
        _parentMaterial = transform.GetComponent<Renderer>();
        _parentMaterial.material.SetColor(_chromaticity, _localChromaticity);
        _parentMaterial.material.SetColor(_cellColor, _localCellColor);
    }

    private void RotateAroundOwnAxis()
    {
        transform.Rotate((StarRotation * StarMagneticField) * Vector3.up);
    }

    private void TryCalculateVariability()
    {
        _timer += Time.deltaTime;
        float t = _timer;

        if (SaveManager.Instance.ActiveSave.hasExtrinsicVariability)
        {
            _currentVariability = Mathf.Lerp(_startVariability, _endVariability, t);
            _parentMaterial.material.SetFloat(_solarFlare, _currentVariability);
        }
        else if (SaveManager.Instance.ActiveSave.hasIntrinsicVariability)
        {
            _currentVariability = Mathf.Lerp(Random.Range(_startVariability, _endVariability), Random.Range(_startVariability, _endVariability), t);
            _parentMaterial.material.SetFloat(_solarFlare, _currentVariability);
        }

        if (!(t >= 1)) return;
        _timer = 0;
        (_startVariability, _endVariability) = (_endVariability, _startVariability);
    }
}
