using UnityEngine;

public class PlayerContMove : MonoBehaviour
{
    [SerializeField] private float _speed = 5f;
    [SerializeField] private float _jumpForce = 5f;
    [SerializeField] private float _gravity = -10f;
    [SerializeField] private float _groundCheckDistance = 0.2f;
    [SerializeField] private float _minHeight = 0.3f;

    public Vector2 _moveInput;
    public bool _jumpPressed;

    private float _verticalVelocity;
    private bool _isGrounded;

    private void Update()
    {
        if (!GameInputManag.Instance.IsPlayerMode())
            return;

        CheckGround();
        HandleGravity();

        if (transform.position.y < _minHeight)
        {
            transform.position = new Vector3(transform.position.x, _minHeight, transform.position.z);
            _verticalVelocity = 0;
            _isGrounded = true;
        }

        Move();
        Jump();
    }

    private void CheckGround()
    {
        Ray ray = new Ray(transform.position, Vector3.down);
        _isGrounded = Physics.Raycast(ray, _groundCheckDistance);
    }

    private void HandleGravity()
    {
        if (!_isGrounded)
        {
            _verticalVelocity += _gravity * Time.deltaTime;
        }
        else if (_verticalVelocity < 0)
        {
            _verticalVelocity = 0;
        }

        transform.position += Vector3.up * _verticalVelocity * Time.deltaTime;
    }

    private void Move()
    {
        Vector3 direction = new Vector3(_moveInput.x, 0, _moveInput.y);
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