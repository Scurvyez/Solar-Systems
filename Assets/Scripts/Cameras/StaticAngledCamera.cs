using System.Collections;
using UnityEngine;
using UnityEngine.Serialization;
using Utils;

public class StaticAngledCamera : MonoBehaviour
{
    public GameObject StarObject;
    public float ZoomSpeed = 2000.0f;
    public float ZoomMin;
    public float ZoomMax = 20000.0f;
    public float SwivelSpeed = 100.0f;
    public float MaxSwivelSpeed = 2000.0f;
    public float SwivelAcceleration = 100.0f;
    public float currentSpeed;
    public float RotateSpeed = 100.0f;
    public bool IsRotating = false;
    public Vector3 LastMousePosition;
    public float TransitionDuration = 2.0f;
    public GameObject SelectedObject;
    
    private float _zoomDirection = 1.0f;
    private float _zoomMultiplier = 1.0f;
    private Camera _mainCamera;
    private Coroutine _focusCoroutine;

    private void Start()
    {
        if (StarObject == null) return;
        SelectedObject = StarObject;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        ZoomMin = SelectedObject.transform.localScale.x + 100.0f;
        currentSpeed = SwivelSpeed;

        // Get the main camera component
        _mainCamera = Camera.main;

        // Grab the star objects' size
        Vector3 sphereScale = StarObject.transform.localScale;

        // Set the main camera's position based on the sphere object's scale
        Vector3 cameraPos = new(StarObject.transform.position.x, StarObject.transform.position.y + (sphereScale.y * 100.0f), StarObject.transform.position.z - (sphereScale.z * 100.0f));
        
        if (_mainCamera == null) return;
        _mainCamera.transform.position = cameraPos;

        // Look at the center of the sphere object
        _mainCamera.transform.LookAt(StarObject.transform.position);
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

        _mainCamera.transform.LookAt(SelectedObject.transform.position);

        // Zoom in/out with the W/S keys
        _zoomDirection = Input.GetKey(KeyCode.W) ? 1.0f : (Input.GetKey(KeyCode.S) ? -1.0f : 0.0f);
        _zoomMultiplier = Input.GetKey(KeyCode.LeftShift) ? 3.0f : 1.0f;
        _mainCamera.transform.position += _zoomDirection * ZoomSpeed * _zoomMultiplier * Time.deltaTime * _mainCamera.transform.forward;

        // Swivel with the A and D keys
        float direction = Input.GetKey(KeyCode.D) ? 1f : Input.GetKey(KeyCode.A) ? -1f : 0f;
        if (direction != 0f)
        {
            _mainCamera.transform.LookAt(SelectedObject.transform);
            _mainCamera.transform.Translate(Time.deltaTime * currentSpeed * direction * Vector3.right);
            currentSpeed = Mathf.Min(currentSpeed + Time.deltaTime * SwivelAcceleration, MaxSwivelSpeed);
        }
        else
        {
            currentSpeed = SwivelSpeed;
        }

        // Rotate freely with the mouse wheel button
        if (Input.GetMouseButtonDown(2))
        {
            IsRotating = true;
            Cursor.lockState = CursorLockMode.None;
            LastMousePosition = Input.mousePosition;
        }
        else if (Input.GetMouseButtonUp(2))
        {
            IsRotating = false;
        }

        if (IsRotating)
        {
            Vector3 delta = Input.mousePosition - LastMousePosition;
            _mainCamera.transform.RotateAround(SelectedObject.transform.position, Vector3.up, delta.x * (RotateSpeed * Time.deltaTime));
            _mainCamera.transform.RotateAround(SelectedObject.transform.position, _mainCamera.transform.right, -delta.y * (RotateSpeed * Time.deltaTime));
            _mainCamera.transform.LookAt(SelectedObject.transform.position);
            LastMousePosition = Input.mousePosition;
        }
        else
        {
            _mainCamera.transform.LookAt(SelectedObject.transform.position);
        }

        // Allow free rotation on all axes
        _mainCamera.transform.rotation = Quaternion.Euler(_mainCamera.transform.rotation.eulerAngles.x, _mainCamera.transform.rotation.eulerAngles.y, _mainCamera.transform.rotation.eulerAngles.z);

        // Zoom in and out with the mouse wheel
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        _mainCamera.transform.position += 100.0f * scroll * Time.deltaTime * ZoomSpeed * _mainCamera.transform.forward;

        // Ensure the camera is within the desired zoom range
        float distance = Vector3.Distance(_mainCamera.transform.position, SelectedObject.transform.position);
        Vector3 targetPosition = SelectedObject.transform.position - (_mainCamera.transform.forward * Mathf.Clamp(distance, ZoomMin, ZoomMax));
        if (targetPosition != _mainCamera.transform.position)
        {
            _mainCamera.transform.position = targetPosition;
        }
    }

    public void SetFocus(GameObject focusObject)
    {
        if (_focusCoroutine != null)
        {
            StopCoroutine(_focusCoroutine);
        }
        _focusCoroutine = StartCoroutine(SmoothFocusTransition(focusObject));
    }
    
    private IEnumerator SmoothFocusTransition(GameObject newFocusObject)
    {
        Vector3 startPosition = _mainCamera.transform.position;
        Quaternion startRotation = _mainCamera.transform.rotation;
        Vector3 offset = _mainCamera.transform.position - SelectedObject.transform.position; // Calculate initial offset

        float elapsedTime = 0f;

        while (elapsedTime < TransitionDuration)
        {
            Vector3 currentTargetPosition = newFocusObject.transform.position + offset;
            Quaternion currentTargetRotation = Quaternion.LookRotation(newFocusObject.transform.position - _mainCamera.transform.position);

            float t = elapsedTime / TransitionDuration;
            t = EasingFunctionsUtil.EaseInOutQuad(t);

            _mainCamera.transform.position = Vector3.Lerp(startPosition, currentTargetPosition, t);
            _mainCamera.transform.rotation = Quaternion.Lerp(startRotation, currentTargetRotation, t);

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Ensure the camera is exactly at the final target position and rotation
        _mainCamera.transform.position = newFocusObject.transform.position + offset;
        _mainCamera.transform.rotation = Quaternion.LookRotation(newFocusObject.transform.position - _mainCamera.transform.position);
        SelectedObject = newFocusObject;
    }
}
