using System.Collections;
using UnityEngine;
using Utils;

public class StaticAngledCamera : MonoBehaviour
{
    [Header("Zoom Settings")]
    public float ZoomSpeed = 2000.0f;
    public float ZoomMinOffset = 10;
    public float ZoomMax = 20000.0f;

    [Header("Swivel Settings")]
    public float SwivelSpeed = 750.0f;
    public float MaxSwivelSpeed = 2000.0f;
    public float SwivelAcceleration = 100.0f;

    [Header("Rotation Settings")]
    public float RotateSpeed = 100.0f;
    public float TransitionDuration = 2.0f;

    public GameObject StarObject;
    public GameObject SelectedObject;

    private bool _isRotating = false;
    private float _zoomMin;
    private float _zoomDirection = 1.0f;
    private float _zoomMultiplier = 1.0f;
    private float _currentSwivelSpeed;
    private Camera _mainCamera;
    private Coroutine _focusCoroutine;
    private Vector3 _lastMousePosition;

    private void Start()
    {
        InitializeCamera();
    }

    private void Update()
    {
        float zoomMinFactor = SelectedObject.transform.localScale.x / ZoomMinOffset;
        _zoomMin = SelectedObject.transform.localScale.x + zoomMinFactor;
        
        HandleCursorLocking();
        HandleZooming();
        HandleSwiveling();
        HandleMouseRotation();
        HandleMouseScrollZoom();
        EnsureCameraZoomRange();
    }

    private void InitializeCamera()
    {
        if (StarObject == null) return;

        SelectedObject = StarObject;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        
        _currentSwivelSpeed = SwivelSpeed;

        _mainCamera = Camera.main;
        if (_mainCamera == null) return;

        Vector3 sphereScale = StarObject.transform.localScale;

        // Calculate the offset position for a 45-degree angle
        float distance = sphereScale.magnitude * 50.0f;
        Vector3 offset = new 
        (
            Mathf.Cos(Mathf.Deg2Rad * 45) * distance,
            Mathf.Sin(Mathf.Deg2Rad * 45) * distance,
            Mathf.Cos(Mathf.Deg2Rad * 45) * distance
        );

        Vector3 cameraPos = StarObject.transform.position + offset;

        _mainCamera.transform.position = cameraPos;
        _mainCamera.transform.LookAt(StarObject.transform.position);
    }
    
    private static void HandleCursorLocking()
    {
        if (!Input.GetKeyDown(KeyCode.Space)) return;
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

    private void HandleZooming()
    {
        _zoomDirection = Input.GetKey(KeyCode.W) ? 1.0f : (Input.GetKey(KeyCode.S) ? -1.0f : 0.0f);
        _zoomMultiplier = Input.GetKey(KeyCode.LeftShift) ? 3.0f : 1.0f;
        _mainCamera.transform.position += _zoomDirection * ZoomSpeed * _zoomMultiplier * Time.deltaTime * _mainCamera.transform.forward;
    }

    private void HandleSwiveling()
    {
        float direction = Input.GetKey(KeyCode.D) ? 1f : Input.GetKey(KeyCode.A) ? -1f : 0f;
        if (direction != 0f)
        {
            _mainCamera.transform.LookAt(SelectedObject.transform);
            _mainCamera.transform.Translate(Time.deltaTime * _currentSwivelSpeed * direction * Vector3.right);
            _currentSwivelSpeed = Mathf.Min(_currentSwivelSpeed + Time.deltaTime * SwivelAcceleration, MaxSwivelSpeed);
        }
        else
        {
            _currentSwivelSpeed = SwivelSpeed;
        }
    }

    private void HandleMouseRotation()
    {
        if (Input.GetMouseButtonDown(2))
        {
            _isRotating = true;
            Cursor.lockState = CursorLockMode.None;
            _lastMousePosition = Input.mousePosition;
        }
        else if (Input.GetMouseButtonUp(2))
        {
            _isRotating = false;
        }

        if (_isRotating)
        {
            Vector3 delta = Input.mousePosition - _lastMousePosition;
            _mainCamera.transform.RotateAround(SelectedObject.transform.position, Vector3.up, delta.x * (RotateSpeed * Time.deltaTime));
            _mainCamera.transform.RotateAround(SelectedObject.transform.position, _mainCamera.transform.right, -delta.y * (RotateSpeed * Time.deltaTime));
            _mainCamera.transform.LookAt(SelectedObject.transform.position);
            _lastMousePosition = Input.mousePosition;
        }
        else
        {
            _mainCamera.transform.LookAt(SelectedObject.transform.position);
        }
        _mainCamera.transform.rotation = Quaternion.Euler(_mainCamera.transform.rotation.eulerAngles.x, _mainCamera.transform.rotation.eulerAngles.y, _mainCamera.transform.rotation.eulerAngles.z);
    }

    private void HandleMouseScrollZoom()
    {
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        _mainCamera.transform.position += 10.0f * scroll * Time.deltaTime * ZoomSpeed * _mainCamera.transform.forward;
    }

    private void EnsureCameraZoomRange()
    {
        float distance = Vector3.Distance(_mainCamera.transform.position, SelectedObject.transform.position);
        Vector3 targetPosition = SelectedObject.transform.position - (_mainCamera.transform.forward * Mathf.Clamp(distance, _zoomMin, ZoomMax));
        if (targetPosition != _mainCamera.transform.position)
        {
            _mainCamera.transform.position = targetPosition;
        }
    }

    public void SetFocus(GameObject focusObject)
    {
        if (focusObject == SelectedObject) return;
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
            t = EasingFunctionsUtil.EaseInOutCubic(t);

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
