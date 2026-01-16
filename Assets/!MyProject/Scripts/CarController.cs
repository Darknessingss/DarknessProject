using UnityEngine;
using UnityEngine.InputSystem;

public class CarController : MonoBehaviour
{
    [SerializeField] private Transform _driverSeat;
    [SerializeField] private GameObject _pressEIcon;
    [SerializeField] private float _speed = 10f;
    [SerializeField] private float _turnSpeed = 50f;
    [SerializeField] private float _interactionRadius = 5f;

    public Vector2 _driveInput;
    public bool _isDriving;

    private GameObject _player;
    private Vector3 _playerExitPosition;

    private void Start()
    {
        if (_pressEIcon != null)
            _pressEIcon.SetActive(false);
    }

    private void Update()
    {
        if (_isDriving)
        {
            if (!GameInputManager.Instance.IsCarMode())
                return;

            transform.Translate(Vector3.forward * _driveInput.y * _speed * Time.deltaTime);
            transform.Rotate(Vector3.up * _driveInput.x * _turnSpeed * Time.deltaTime);
        }
        else
        {
            CheckForPlayer();
        }
    }

    private void CheckForPlayer()
    {
        if (!GameInputManager.Instance.IsPlayerMode())
            return;

        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player == null) return;

        float distance = Vector3.Distance(transform.position, player.transform.position);
        bool showIcon = distance <= _interactionRadius;

        if (_pressEIcon != null)
            _pressEIcon.SetActive(showIcon);

        if (showIcon && Keyboard.current.eKey.wasPressedThisFrame)
        {
            _player = player;
            EnterCar();
        }
    }

    private void EnterCar()
    {
        if (_player == null) return;

        _playerExitPosition = _player.transform.position;
        _player.SetActive(false);
        _isDriving = true;

        GameInputManager.Instance.SwitchToCarMode();

        if (_pressEIcon != null)
            _pressEIcon.SetActive(false);
    }

    public void ExitCar()
    {
        if (_player == null) return;

        Vector3 exitPos = transform.position - transform.forward * 3f;
        exitPos.y = Mathf.Max(_playerExitPosition.y, transform.position.y + 1f);

        _player.transform.position = exitPos;
        _player.transform.rotation = Quaternion.identity;
        _player.SetActive(true);
        _isDriving = false;
        _player = null;

        GameInputManager.Instance.SwitchToPlayerMode();
    }
}