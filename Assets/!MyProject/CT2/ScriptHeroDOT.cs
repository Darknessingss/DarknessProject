using DG.Tweening;
using UnityEngine;

public class ScriptHeroDOT : MonoBehaviour
{
    [Header("Параметры движения")]
    [SerializeField] private float moveDistance = 3f;
    [SerializeField] private float moveDuration = 0.5f;
    [SerializeField] private float jumpHeight = 1f;
    [SerializeField] private float jumpDuration = 0.7f;
    [SerializeField] private float rotateDuration = 0.3f;
    [SerializeField] private AnimationCurve moveCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);
    [SerializeField] private AnimationCurve jumpCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);

    [Header("Настройки ввода")]
    [SerializeField] private bool useWASD = true;
    [SerializeField] private bool useArrows = true;

    private bool isMoving = false;
    private bool isJumping = false;
    private Vector3 forwardDirection = Vector3.forward;
    private Vector3 rightDirection = Vector3.right;

    void Update()
    {
        HandleMovementInput();
        HandleRotationInput();
        HandleJumpInput();
    }

    private void HandleMovementInput()
    {
        if (isMoving) return;

        Vector3 moveDirection = Vector3.zero;
        bool hasInput = false;

        if ((useWASD && Input.GetKeyDown(KeyCode.W)) || (useArrows && Input.GetKeyDown(KeyCode.UpArrow)))
        {
            moveDirection = forwardDirection;
            hasInput = true;
        }
        else if ((useWASD && Input.GetKeyDown(KeyCode.S)) || (useArrows && Input.GetKeyDown(KeyCode.DownArrow)))
        {
            moveDirection = -forwardDirection;
            hasInput = true;
        }
        else if ((useWASD && Input.GetKeyDown(KeyCode.D)) || (useArrows && Input.GetKeyDown(KeyCode.RightArrow)))
        {
            moveDirection = rightDirection;
            hasInput = true;
        }
        else if ((useWASD && Input.GetKeyDown(KeyCode.A)) || (useArrows && Input.GetKeyDown(KeyCode.LeftArrow)))
        {
            moveDirection = -rightDirection;
            hasInput = true;
        }

        if (hasInput)
        {
            MoveCharacter(moveDirection);
        }
    }

    private void HandleRotationInput()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            RotateCharacter(-90f);
        }
        else if (Input.GetKeyDown(KeyCode.E))
        {
            RotateCharacter(90f);
        }
    }

    private void HandleJumpInput()
    {
        if (Input.GetKeyDown(KeyCode.Space) && !isJumping && !isMoving)
        {
            Jump();
        }
    }

    private void MoveCharacter(Vector3 direction)
    {
        isMoving = true;

        Vector3 endPosition = transform.position + direction * moveDistance;

        transform.DOMove(endPosition, moveDuration)
            .SetEase(moveCurve)
            .OnComplete(() => isMoving = false);

        Sequence moveSequence = DOTween.Sequence();
        moveSequence.Append(transform.DOScaleY(0.8f, moveDuration / 3));
        moveSequence.Append(transform.DOScaleY(1f, moveDuration / 3));
        moveSequence.SetEase(Ease.OutQuad);
    }

    private void Jump()
    {
        if (isJumping) return;

        isJumping = true;

        Vector3 endPosition = transform.position + forwardDirection * moveDistance;

        transform.DOJump(endPosition, jumpHeight, 1, jumpDuration)
            .SetEase(jumpCurve)
            .OnComplete(() => isJumping = false);

        transform.DOScale(new Vector3(1.1f, 0.9f, 1.1f), jumpDuration / 4)
            .SetLoops(4, LoopType.Yoyo)
            .SetEase(Ease.OutQuad);
    }

    private void RotateCharacter(float angle)
    {
        if (isMoving || isJumping) return;

        forwardDirection = Quaternion.Euler(0, angle, 0) * forwardDirection;
        rightDirection = Quaternion.Euler(0, angle, 0) * rightDirection;

        transform.DORotate(new Vector3(0, transform.eulerAngles.y + angle, 0),
                          rotateDuration,
                          RotateMode.FastBeyond360)
                .SetEase(Ease.OutBack);
    }
}