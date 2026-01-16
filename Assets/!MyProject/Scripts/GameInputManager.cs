using UnityEngine;
using UnityEngine.InputSystem;

public class GameInputManager : MonoBehaviour
{
    public static GameInputManager Instance { get; private set; }

    [SerializeField] private InputActionAsset heroMovementActions;

    [Header("References")]
    [SerializeField] private CameraController cameraController;
    [SerializeField] private CarController carController;
    [SerializeField] private PlayerControlMovement playerController;

    private Vector2 playerMoveInput;
    private bool playerJumpPressed;
    private Vector2 carDriveInput;
    private Vector2 cameraLookInput;
    private float cameraZoomInput;

    private InputAction playerMoveAction;
    private InputAction playerJumpAction;
    private InputAction carDriveAction;
    private InputAction carExitAction;
    private InputAction cameraLookAction;
    private InputAction cameraZoomAction;

    private bool isCameraMode = false;
    private bool isCarMode = false;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            InitializeInput();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void InitializeInput()
    {
        if (heroMovementActions != null)
        {
            heroMovementActions.Enable();

            playerMoveAction = heroMovementActions.FindAction("HeroesController/MovementPlayerBase");
            playerJumpAction = heroMovementActions.FindAction("HeroesController/JumpPlayerBase");

            carDriveAction = heroMovementActions.FindAction("CarController/CarMovementBase");
            carExitAction = heroMovementActions.FindAction("CarController/Exit");

            cameraLookAction = heroMovementActions.FindAction("CameraController/CameraLook");
            cameraZoomAction = heroMovementActions.FindAction("CameraController/Zoom");

            if (playerMoveAction == null) Debug.LogError("MovementPlayerBase not found!");
            if (playerJumpAction == null) Debug.LogError("JumpPlayerBase not found!");
            if (carDriveAction == null) Debug.LogError("CarMovementBase not found!");
            if (carExitAction == null) Debug.LogError("Exit not found!");
            if (cameraLookAction == null) Debug.LogError("CameraLook not found!");
            if (cameraZoomAction == null) Debug.LogError("Zoom not found!");
        }
        else
        {
            Debug.LogError("HeroMovement Actions not assigned!");
        }

        SwitchToPlayerMode();
    }

    private void Update()
    {
        CheckModeSwitching();
        UpdateInput();
        HandleInput();
    }

    private void CheckModeSwitching()
    {
        if (Keyboard.current.xKey.wasPressedThisFrame)
        {
            if (!isCameraMode && !isCarMode)
            {
                SwitchToCameraMode();
            }
            else if (isCameraMode)
            {
                SwitchToPlayerMode();
            }
        }
    }

    private void UpdateInput()
    {
        if (playerMoveAction != null)
            playerMoveInput = playerMoveAction.ReadValue<Vector2>();
        if (playerJumpAction != null)
            playerJumpPressed = playerJumpAction.ReadValue<float>() > 0.5f;

        if (carDriveAction != null)
            carDriveInput = carDriveAction.ReadValue<Vector2>();

        if (cameraLookAction != null)
            cameraLookInput = cameraLookAction.ReadValue<Vector2>();
        if (cameraZoomAction != null)
            cameraZoomInput = cameraZoomAction.ReadValue<Vector2>().y;
    }

    private void HandleInput()
    {

        if (isCameraMode && cameraController != null)
        {
            cameraController._mouseDelta = cameraLookInput;
            cameraController._zoomInput = cameraZoomInput;
        }
        else if (isCarMode && carController != null)
        {
            carController._driveInput = carDriveInput;

            if (carExitAction != null && carExitAction.ReadValue<float>() > 0.5f)
            {
                if (carController._isDriving)
                    carController.ExitCar();
            }
        }
        else if (playerController != null)
        {
            playerController._moveInput = playerMoveInput;
            playerController._jumpPressed = playerJumpPressed;
        }
    }

    public void SwitchToPlayerMode()
    {
        isCameraMode = false;
        isCarMode = false;

        if (playerController != null && !playerController.gameObject.activeSelf)
            playerController.gameObject.SetActive(true);

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        Debug.Log("Switched to Player Mode");
    }

    public void SwitchToCameraMode()
    {
        if (cameraController == null)
        {
            Debug.LogError("CameraController reference is not set!");
            return;
        }

        isCameraMode = true;
        isCarMode = false;

        if (playerController != null)
            playerController.gameObject.SetActive(false);

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        Debug.Log("Switched to Camera Mode");
    }

    public void SwitchToCarMode()
    {
        isCameraMode = false;
        isCarMode = true;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        Debug.Log("Switched to Car Mode");
    }

    public bool IsCameraMode() => isCameraMode;
    public bool IsCarMode() => isCarMode;
    public bool IsPlayerMode() => !isCameraMode && !isCarMode;
}