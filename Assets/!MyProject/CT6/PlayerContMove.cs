using UnityEngine;

public class PlayerContMove : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float _speed = 5f;
    [SerializeField] private float _jumpForce = 5f;
    [SerializeField] private float _gravity = -10f;
    [SerializeField] private float _groundCheckDistance = 0.3f;
    [SerializeField] private float _playerHeight = 1.0f; // Высота игрока

    public Vector2 _moveInput;
    public bool _jumpPressed;

    private float _verticalVelocity;
    private bool _isGrounded;
    private Collider _collider;

    private void Start()
    {
        _collider = GetComponent<Collider>();

        if (_collider != null)
        {
            _playerHeight = _collider.bounds.size.y;
        }
    }

    private void Update()
    {
        if (!GameInputManag.Instance.IsPlayerMode())
            return;

        CheckGround();
        HandleGravity();

        Move();
        Jump();
    }

    private void CheckGround()
    {
        Vector3 rayOrigin = transform.position - Vector3.up * (_playerHeight * 0.45f);

        Ray ray = new Ray(rayOrigin, Vector3.down);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, _groundCheckDistance))
        {
            if (hit.collider.CompareTag("Ground"))
            {
                _isGrounded = true;
                Debug.DrawRay(rayOrigin, Vector3.down * _groundCheckDistance, Color.green);

                float groundY = hit.point.y + (_playerHeight * 0.5f);
                transform.position = new Vector3(transform.position.x, groundY, transform.position.z);
            }
            else
            {
                _isGrounded = false;
                Debug.DrawRay(rayOrigin, Vector3.down * _groundCheckDistance, Color.yellow);
            }
        }
        else
        {
            _isGrounded = false;
            Debug.DrawRay(rayOrigin, Vector3.down * _groundCheckDistance, Color.red);
        }
    }

    private void HandleGravity()
    {
        _verticalVelocity += _gravity * Time.deltaTime;

        _verticalVelocity = Mathf.Max(_verticalVelocity, -20f);
        transform.position += Vector3.up * _verticalVelocity * Time.deltaTime;

        if (_isGrounded && _verticalVelocity < 0)
        {
            _verticalVelocity = 0;
        }
    }

    private void Move()
    {
        Vector3 direction = new Vector3(_moveInput.x, 0, _moveInput.y);
        direction = direction.normalized;

        transform.position += direction * _speed * Time.deltaTime;

        if (direction.magnitude > 0.1f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 10f * Time.deltaTime);
        }
    }

    private void Jump()
    {
        if (_jumpPressed && _isGrounded)
        {
            _verticalVelocity = _jumpForce;
            _isGrounded = false;
            _jumpPressed = false;
        }
    }
}