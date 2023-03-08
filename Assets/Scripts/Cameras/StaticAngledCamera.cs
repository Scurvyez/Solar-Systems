using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaticAngledCamera : MonoBehaviour
{
    public GameObject StarObject;
    private Camera MainCamera;

    public float SwivelSpeed;
    public float ZoomSpeed;
    public float ZoomMin;
    public float ZoomMax;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        SwivelSpeed = 400f;
        ZoomSpeed = 400.0f;
        ZoomMin = StarObject.transform.localScale.x + 20f;
        ZoomMax = 480000.0f;
    }

    private void Awake()
    {
        // Get the main camera component
        MainCamera = Camera.main;

        // Grab the star objects' size
        Vector3 sphereScale = StarObject.transform.localScale;

        // Set the main camera's position based on the sphere object's scale
        Vector3 cameraPos = new (StarObject.transform.position.x, StarObject.transform.position.y + (sphereScale.y * 20.5f), StarObject.transform.position.z - (sphereScale.z * 20.5f));
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

        // Zoom in/out with the W/S keys
        if (Input.GetKey(KeyCode.W))
        {
            MainCamera.transform.position += ZoomSpeed * Time.deltaTime * MainCamera.transform.forward;
        }
        else if (Input.GetKey(KeyCode.S))
        {
            MainCamera.transform.position -= ZoomSpeed * Time.deltaTime * MainCamera.transform.forward;
        }

        // Swivel with the A and D keys
        if (Input.GetKey(KeyCode.D))
        {
            MainCamera.transform.LookAt(StarObject.transform);
            MainCamera.transform.Translate(Time.deltaTime * SwivelSpeed * Vector3.right);
        }
        else if (Input.GetKey(KeyCode.A))
        {
            MainCamera.transform.LookAt(StarObject.transform);
            MainCamera.transform.Translate(Time.deltaTime * SwivelSpeed * Vector3.left);
        }

        // Limit rotation to y-axis only
        MainCamera.transform.rotation = Quaternion.Euler(45f, MainCamera.transform.rotation.eulerAngles.y, MainCamera.transform.rotation.eulerAngles.z);

        // Zoom in and out with the mouse wheel
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        MainCamera.transform.position = MainCamera.transform.position + 100.0f * scroll * ZoomSpeed * Time.deltaTime * MainCamera.transform.forward;

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
}
