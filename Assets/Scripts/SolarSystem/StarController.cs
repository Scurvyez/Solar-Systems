using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarController : MonoBehaviour
{
    public SaveManager SaveManager;

    public double StarMass;
    public double StarRadius;
    public double StarRotation;
    public double starMagneticField;
    public double variability;
    private float currentVariability;
    private float _timer = 0;
    private float _startVariability;
    private float _endVariability;
    private const float SolRadii = 695700000.0f;

    public StarController instance;

    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        // grab the saved mass value of our generated star
        StarMass = SaveManager.instance.activeSave.starMass;
        // grab the objects' RigidBody component
        Rigidbody parentRigidBody = transform.GetComponent<Rigidbody>();
        // set the mass value of the RigidBody to our saved value
        parentRigidBody.mass = (float)StarMass;

        // grab the generated radius for the star
        StarRadius = SaveManager.instance.activeSave.starRadius / SolRadii;

        // grab the saved rotation (on own axis) value for our generated star
        StarRotation = SaveManager.instance.activeSave.starRotation;
        // grab the saves magnetic field value
        starMagneticField = SaveManager.instance.activeSave.starMagneticField;

        // grab the objects' material
        var parentMaterial = transform.GetComponent<Renderer>();
        // define the color in our saved settings
        Color localChromaticity = SaveManager.instance.activeSave.starChromaticity;
        Color localCellColor = SaveManager.instance.activeSave.starCellColor;
        // change the shaders "Chromaticity" color to the one above
        parentMaterial.material.SetColor("_Chromaticity", localChromaticity);
        parentMaterial.material.SetColor("_CellColor", localCellColor);

        // Set the starting and ending values for variability
        variability = SaveManager.instance.activeSave.starVariability;
        _startVariability = -(float)variability;
        _endVariability = (float)variability;

        // set the stars' volume fallOff distance
        //var parentAudioSource = transform.GetComponent<AudioSource>();
        //parentAudioSource.minDistance = 1f;
        //parentAudioSource.maxDistance = transform.localScale.x + (transform.localScale.x * 0.75f);
    }

    private void Update()
    {
        // grab the objects' material
        var parentMaterial = transform.GetComponent<Renderer>();
        // apply rotation every frame
        transform.Rotate(((float)StarRotation * (float)starMagneticField) * Vector3.up);

        _timer += Time.deltaTime;
        float t = _timer;

        if (SaveManager.instance.activeSave.hasExtrinsicVariability)
        {
            currentVariability = Mathf.Lerp(_startVariability, _endVariability, t);
            parentMaterial.material.SetFloat("_SolarFlare", currentVariability);
        }
        else if (SaveManager.instance.activeSave.hasIntrinsicVariability)
        {
            currentVariability = Mathf.Lerp(Random.Range(_startVariability, _endVariability), Random.Range(_startVariability, _endVariability), t);
            parentMaterial.material.SetFloat("_SolarFlare", currentVariability);
        }

        if (t >= 1)
        {
            _timer = 0;
            float temp = _startVariability;
            _startVariability = _endVariability;
            _endVariability = temp;
        }
    }
}
