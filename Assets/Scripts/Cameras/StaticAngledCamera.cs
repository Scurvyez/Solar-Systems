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

    public float SwivelSpeed;
    public float MaxSwivelSpeed;
    public float SwivelAcceleration;

    public float currentSpeed;

    public float RotateSpeed;
    public bool isRotating = false;
    public Vector3 lastMousePosition;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        ZoomSpeed = 1000.0f;
        ZoomMin = StarObject.transform.localScale.x + 100.0f;
        ZoomMax = 480000.0f;

        SwivelSpeed = 100.0f;
        MaxSwivelSpeed = 2000.0f;
        SwivelAcceleration = 100.0f;
        currentSpeed = SwivelSpeed;

        RotateSpeed = 2.0f;
    }

    private void Awake()
    {
        // Get the main camera component
        MainCamera = Camera.main;

        // Grab the star objects' size
        Vector3 sphereScale = StarObject.transform.localScale;

        // Set the main camera's position based on the sphere object's scale
        Vector3 cameraPos = new (StarObject.transform.position.x, StarObject.transform.position.y + (sphereScale.y * 100.0f), StarObject.transform.position.z - (sphereScale.z * 100.0f));
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
        float zoomDirection = Input.GetKey(KeyCode.W) ? 1.0f : (Input.GetKey(KeyCode.S) ? -1.0f : 0.0f);
        float zoomMultiplier = Input.GetKey(KeyCode.LeftShift) ? 3.0f : 1.0f;
        MainCamera.transform.position += zoomDirection * ZoomSpeed * zoomMultiplier * Time.deltaTime * MainCamera.transform.forward;

        // Swivel with the A and D keys
        if (Input.GetKey(KeyCode.D))
        {
            MainCamera.transform.LookAt(StarObject.transform);
            MainCamera.transform.Translate(Time.deltaTime * currentSpeed * Vector3.right);
            currentSpeed = Mathf.Min(currentSpeed + Time.deltaTime * SwivelAcceleration, MaxSwivelSpeed);
        }
        else if (Input.GetKey(KeyCode.A))
        {
            MainCamera.transform.LookAt(StarObject.transform);
            MainCamera.transform.Translate(Time.deltaTime * currentSpeed * Vector3.left);
            currentSpeed = Mathf.Min(currentSpeed + Time.deltaTime * SwivelAcceleration, MaxSwivelSpeed);
        }
        else
        {
            currentSpeed = SwivelSpeed;
        }

        // Rotate freely with the mouse wheel button
        if (Input.GetMouseButtonDown(2))
        {
            isRotating = true;
            lastMousePosition = Input.mousePosition;
        }
        else if (Input.GetMouseButtonUp(2))
        {
            isRotating = false;
        }

        if (isRotating)
        {
            Vector3 delta = Input.mousePosition - lastMousePosition;
            MainCamera.transform.RotateAround(StarObject.transform.position, Vector3.up, delta.x * RotateSpeed);
            MainCamera.transform.RotateAround(StarObject.transform.position, MainCamera.transform.right, -delta.y * RotateSpeed);
            MainCamera.transform.LookAt(StarObject.transform.position);
            lastMousePosition = Input.mousePosition;
        }
        else
        {
            MainCamera.transform.LookAt(StarObject.transform.position);
        }

        // Limit rotation to y-axis only
        MainCamera.transform.rotation = Quaternion.Euler(MainCamera.transform.rotation.eulerAngles.x, MainCamera.transform.rotation.eulerAngles.y, MainCamera.transform.rotation.eulerAngles.z);

        // Zoom in and out with the mouse wheel
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        MainCamera.transform.position += 100.0f * scroll * Time.deltaTime * ZoomSpeed * MainCamera.transform.forward;

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
