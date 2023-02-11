using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaticAngledCamera : MonoBehaviour
{
    public GameObject StarObject;
    private Camera MainCamera;

    public float ZoomSpeed;
    public float ZoomMin;
    public float ZoomMax;

    private const float SolRadii = 695700000.0f;
    private double StarRadius;
    private float RadiusAsSolarRadii;
    private const float OClassBaseCameraSpeed = 400f;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        ZoomSpeed = 100.0f;
        ZoomMin = RadiusAsSolarRadii;
        ZoomMax = 480000.0f;
    }

    private void Awake()
    {
        // Get the main camera component
        MainCamera = Camera.main;

        // grab the generated radius for the star
        StarRadius = SaveManager.instance.activeSave.starRadius;
        RadiusAsSolarRadii = (float)StarRadius / SolRadii;
        // apply that radius value to the x, y, and z planes
        Vector3 sphereScale = new (RadiusAsSolarRadii, RadiusAsSolarRadii, RadiusAsSolarRadii);

        print(sphereScale);

        // Set the main camera's position based on the sphere object's scale
        Vector3 cameraPos = new (StarObject.transform.position.x, StarObject.transform.position.y + (sphereScale.y * 20f), StarObject.transform.position.z - (sphereScale.z * 20f));

        MainCamera.transform.position = cameraPos;

        // Look at the center of the sphere object
        MainCamera.transform.LookAt(StarObject.transform.position);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (Cursor.lockState == CursorLockMode.Locked)
            {
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }
            else
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }
        }

        // Zoom in with the W key
        if (Input.GetKey(KeyCode.W))
        {
            MainCamera.transform.position = MainCamera.transform.position + ScrollSpeedMultiplier() * Time.deltaTime * MainCamera.transform.forward;
        }

        // Zoom out with the S key
        if (Input.GetKey(KeyCode.S))
        {
            MainCamera.transform.position = MainCamera.transform.position - ScrollSpeedMultiplier() * Time.deltaTime * MainCamera.transform.forward;
        }

        // Zoom in and out with the mouse wheel
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        MainCamera.transform.position = MainCamera.transform.position + 100.0f * scroll * ScrollSpeedMultiplier() * Time.deltaTime * MainCamera.transform.forward;

        // Ensure the camera is within the desired zoom range
        float distance = Vector3.Distance(MainCamera.transform.position, StarObject.transform.position);
        if (distance < ZoomMin)
        {
            MainCamera.transform.position = StarObject.transform.position - (MainCamera.transform.forward * ZoomMin);
        }
        else if (distance > ZoomMax)
        {
            MainCamera.transform.position = StarObject.transform.position - (MainCamera.transform.forward * ZoomMax);
        }
    }

    private float ScrollSpeedMultiplier()
    {
        string starClass = SaveManager.instance.activeSave.starClassAsString;
        float cameraSpeedFactor = (OClassBaseCameraSpeed * RadiusAsSolarRadii) / 3;

        ZoomSpeed = starClass switch
        {
            "O" when RadiusAsSolarRadii >= 1250f && RadiusAsSolarRadii <= 1500f => cameraSpeedFactor,
            "O" when RadiusAsSolarRadii >= 1000f && RadiusAsSolarRadii <= 1250f => cameraSpeedFactor,
            "O" when RadiusAsSolarRadii >= 800f && RadiusAsSolarRadii <= 1000f => cameraSpeedFactor,
            "O" when RadiusAsSolarRadii >= 500f && RadiusAsSolarRadii <= 800f => cameraSpeedFactor,
            "O" when RadiusAsSolarRadii >= 100f && RadiusAsSolarRadii <= 500f => cameraSpeedFactor,
            "O" when RadiusAsSolarRadii >= 30f && RadiusAsSolarRadii <= 100f => cameraSpeedFactor,
            "O" when RadiusAsSolarRadii >= 10f && RadiusAsSolarRadii <= 30f => cameraSpeedFactor,
            "O" when RadiusAsSolarRadii >= 6.6f && RadiusAsSolarRadii <= 10f => cameraSpeedFactor,
            "B" => 300f,
            "A" => 100f,
            "F" => 50f,
            "G" => 40f,
            "K" => 30f,
            "M" => 20f,
            _ => 0f,
        };
        return ZoomSpeed;
    }
}
