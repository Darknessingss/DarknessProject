using UnityEngine;
using UnityEngine.InputSystem;

public class GameInputManag : MonoBehaviour
{
    public static GameInputManag Instance { get; private set; }

    [SerializeField] private InputActionAsset heroMovementActions;

    [Header("References")]
    [SerializeField] private PlayerContMove playerController;

    private Vector2 playerMoveInput;
    private bool playerJumpPressed;
    private Vector2 carDriveInput;
    private Vector2 cameraLookInput;
    private float cameraZoomInput;

    private InputAction playerMoveAction;
    private InputAction playerJumpAction;

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
        }
        else
        {
            Debug.LogError("HeroMovement Actions not assigned!");
        }
    }

    private void Update()
    {
        UpdateInput();
        HandleInput();
    }

    private void UpdateInput()
    {
        if (playerMoveAction != null)
            playerMoveInput = playerMoveAction.ReadValue<Vector2>();

        if (playerJumpAction != null)
            playerJumpPressed = playerJumpAction.WasPressedThisFrame();
    }

    private void HandleInput()
    {
        if (playerController != null)
        {
            playerController._moveInput = playerMoveInput;
            playerController._jumpPressed = playerJumpPressed;
        }
    }

    public bool IsPlayerMode() => !isCameraMode && !isCarMode;
}