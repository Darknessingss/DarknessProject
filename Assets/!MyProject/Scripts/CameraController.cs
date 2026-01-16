using UnityEngine;
using UnityEngine.InputSystem;

public class CameraController : MonoBehaviour
{
    [Header("Camera Settings")]
    [SerializeField] private float _mouseSensitivity = 2f;
    [SerializeField] private float _zoomSpeed = 10f;
    [SerializeField] private float _minZoomDistance = 2f;
    [SerializeField] private float _maxZoomDistance = 20f;
    [SerializeField] private float _minVerticalAngle = -80f;
    [SerializeField] private float _maxVerticalAngle = 80f;

    public Vector2 _mouseDelta;
    public float _zoomInput;

    private float _currentXRotation = 0f;
    private float _currentYRotation = 0f;
    private float _currentZoomDistance = 10f;
    private Vector3 _originalPosition;

    private void Start()
    {
        _originalPosition = transform.position;

        Vector3 currentEuler = transform.eulerAngles;
        _currentXRotation = currentEuler.x;
        _currentYRotation = currentEuler.y;

        _currentZoomDistance = (_minZoomDistance + _maxZoomDistance) / 2f;
        ApplyZoom();
    }

    private void Update()
    {
        if (!GameInputManager.Instance.IsCameraMode())
            return;

        HandleCameraControl();
    }

    private void HandleCameraControl()
    {
        if (Mouse.current.rightButton.isPressed)
        {
            _currentYRotation += _mouseDelta.x * _mouseSensitivity * Time.deltaTime;
            _currentXRotation -= _mouseDelta.y * _mouseSensitivity * Time.deltaTime;

            _currentXRotation = Mathf.Clamp(_currentXRotation, _minVerticalAngle, _maxVerticalAngle);
            transform.rotation = Quaternion.Euler(_currentXRotation, _currentYRotation, 0f);
        }

        if (Mathf.Abs(_zoomInput) > 0.1f)
        {
            _currentZoomDistance -= _zoomInput * _zoomSpeed * Time.deltaTime;
            _currentZoomDistance = Mathf.Clamp(_currentZoomDistance, _minZoomDistance, _maxZoomDistance);
            ApplyZoom();
        }
    }

    private void ApplyZoom()
    {
        Vector3 zoomDirection = -transform.forward;
        transform.position = _originalPosition + (zoomDirection * _currentZoomDistance);
    }
}