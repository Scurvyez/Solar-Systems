using Saving;
using SolarSystem;
using UnityEngine;
using Utils;
using Random = UnityEngine.Random;

public class StarController : MonoBehaviour
{
    public float StarMass;
    public float StarRadius;

    private StarInfo _starInfo;
    private Rigidbody _starRigidBody;
    private Renderer _parentMaterial;
    private UI_TooltipTrigger _uiTooltipTrigger;

    private float _curGameSpeed;
    private Color _localChromaticity;
    private Color _localCellColor;
    private float _currentVariability;
    private float _timer = 0;
    private float _variability;
    private float _startVariability;
    private float _endVariability;

    private void Start()
    {
        SetCachedComponents();
        SetPhysicalProperties();
        SetMaterialProperties();
        
        _uiTooltipTrigger = GetComponent<UI_TooltipTrigger>();
        _uiTooltipTrigger.ContentKey = _starInfo.Name;
    }

    private void Update()
    {
        _curGameSpeed = GameSpeedController.Instance.CurSpeed;
    }
    
    private void FixedUpdate()
    {
        RotateAroundOwnAxis();
        TryCalculateVariability();
    }
    
    private void SetCachedComponents()
    {
        _starInfo = transform.GetComponent<StarInfo>();
        _starRigidBody = transform.GetComponent<Rigidbody>();
        _parentMaterial = transform.GetComponent<Renderer>();
    }

    private void SetPhysicalProperties()
    {
        StarMass = SaveManager.Instance.ActiveSave.starMass;
        StarRadius = SaveManager.Instance.ActiveSave.starActualRadius;
        transform.position = SaveManager.Instance.ActiveSave.starPosition;
        transform.localScale = new Vector3(StarRadius, StarRadius, StarRadius);
        _starRigidBody.mass = StarMass;
    }

    private void SetMaterialProperties()
    {
        _localChromaticity = SaveManager.Instance.ActiveSave.starChromaticity;
        _localCellColor = SaveManager.Instance.ActiveSave.starCellColor;
        _variability = SaveManager.Instance.ActiveSave.starVariability;
        _startVariability = -_variability;
        _endVariability = _variability;
        _parentMaterial.material.SetColor(ShaderPropertyIDs.Chromaticity, _localChromaticity);
        _parentMaterial.material.SetColor(ShaderPropertyIDs.CellColor, _localCellColor);
    }

    private void RotateAroundOwnAxis()
    {
        transform.Rotate(Vector3.up, Time.fixedDeltaTime * _curGameSpeed);
    }
    
    private void TryCalculateVariability()
    {
        _timer += Time.deltaTime;
        float t = _timer;

        if (SaveManager.Instance.ActiveSave.hasExtrinsicVariability)
        {
            _currentVariability = Mathf.Lerp(_startVariability, _endVariability, t);
            _parentMaterial.material.SetFloat(ShaderPropertyIDs.SolarFlare, _currentVariability);
        }
        else if (SaveManager.Instance.ActiveSave.hasIntrinsicVariability)
        {
            _currentVariability = Mathf.Lerp(Random.Range(_startVariability, _endVariability), Random.Range(_startVariability, _endVariability), t);
            _parentMaterial.material.SetFloat(ShaderPropertyIDs.SolarFlare, _currentVariability);
        }

        if (!(t >= 1)) return;
        _timer = 0;
        (_startVariability, _endVariability) = (_endVariability, _startVariability);
    }
}
