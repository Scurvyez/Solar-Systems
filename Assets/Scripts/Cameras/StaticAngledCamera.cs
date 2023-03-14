using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.EventSystems;

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

    [SerializeField]
    private GameObject selectedObject;
    private float zoomDirection = 1.0f;
    private float zoomMultiplier = 1.0f;

    private void Start()
    {
        selectedObject = StarObject;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        ZoomSpeed = 1000.0f;
        ZoomMin = selectedObject.transform.localScale.x + 100.0f;
        ZoomMax = 20000.0f;

        SwivelSpeed = 100.0f;
        MaxSwivelSpeed = 2000.0f;
        SwivelAcceleration = 100.0f;
        currentSpeed = SwivelSpeed;

        RotateSpeed = 100.0f;

        // Get the main camera component
        MainCamera = Camera.main;

        // Grab the star objects' size
        Vector3 sphereScale = StarObject.transform.localScale;

        // Set the main camera's position based on the sphere object's scale
        Vector3 cameraPos = new(StarObject.transform.position.x, StarObject.transform.position.y + (sphereScale.y * 100.0f), StarObject.transform.position.z - (sphereScale.z * 100.0f));
        MainCamera.transform.position = cameraPos;

        // Look at the center of the sphere object
        MainCamera.transform.LookAt(StarObject.transform.position);
    }

    private void Update()
    {
        // camera locking & cursor visibility
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

        MainCamera.transform.LookAt(selectedObject.transform.position);

        // Zoom in/out with the W/S keys
        zoomDirection = Input.GetKey(KeyCode.W) ? 1.0f : (Input.GetKey(KeyCode.S) ? -1.0f : 0.0f);
        zoomMultiplier = Input.GetKey(KeyCode.LeftShift) ? 3.0f : 1.0f;
        MainCamera.transform.position += zoomDirection * ZoomSpeed * zoomMultiplier * Time.deltaTime * MainCamera.transform.forward;

        // Swivel with the A and D keys
        float direction = Input.GetKey(KeyCode.D) ? 1f : Input.GetKey(KeyCode.A) ? -1f : 0f;
        if (direction != 0f)
        {
            MainCamera.transform.LookAt(selectedObject.transform);
            MainCamera.transform.Translate(Time.deltaTime * currentSpeed * direction * Vector3.right);
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
            Cursor.lockState = CursorLockMode.None;
            lastMousePosition = Input.mousePosition;
        }
        else if (Input.GetMouseButtonUp(2))
        {
            isRotating = false;
        }

        if (isRotating)
        {
            Vector3 delta = Input.mousePosition - lastMousePosition;
            MainCamera.transform.RotateAround(selectedObject.transform.position, Vector3.up, delta.x * (RotateSpeed * Time.deltaTime));
            MainCamera.transform.RotateAround(selectedObject.transform.position, MainCamera.transform.right, -delta.y * (RotateSpeed * Time.deltaTime));
            MainCamera.transform.LookAt(selectedObject.transform.position);
            lastMousePosition = Input.mousePosition;
        }
        else
        {
            MainCamera.transform.LookAt(selectedObject.transform.position);
        }

        // Allow free rotation on all axes
        MainCamera.transform.rotation = Quaternion.Euler(MainCamera.transform.rotation.eulerAngles.x, MainCamera.transform.rotation.eulerAngles.y, MainCamera.transform.rotation.eulerAngles.z);

        // Zoom in and out with the mouse wheel
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        MainCamera.transform.position += 100.0f * scroll * Time.deltaTime * ZoomSpeed * MainCamera.transform.forward;

        // Ensure the camera is within the desired zoom range
        float distance = Vector3.Distance(MainCamera.transform.position, selectedObject.transform.position);
        Vector3 targetPosition = selectedObject.transform.position - (MainCamera.transform.forward * Mathf.Clamp(distance, ZoomMin, ZoomMax));
        if (targetPosition != MainCamera.transform.position)
        {
            MainCamera.transform.position = targetPosition;
        }
    }

    public void SetFocus(GameObject focusObject)
    {
        selectedObject = focusObject;
    }
}
