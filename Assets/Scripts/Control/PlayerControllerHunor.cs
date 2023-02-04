using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class PlayerControllerHunor : MonoBehaviour
{
    [SerializeField] private float playerSpeed = 2.0f;
    [SerializeField] private float jumpHeight = 1.0f;
    [SerializeField] private float gravityValue = -9.81f;
    [SerializeField] private float minDistanceFromCamera = 7f;
    [SerializeField] private float cameraKnockbackStrength = 7f;

    private Rigidbody _rb;
    private CharacterController controller;
    private Vector3 playerVelocity;
    private bool groundedPlayer;

    private Vector2 movementInput = Vector2.zero;
    private bool jumped = false;

    private void Start()
    {
        controller = gameObject.GetComponent<CharacterController>();
        _rb = GetComponent<Rigidbody>();
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        movementInput = context.ReadValue<Vector2>();
    }

    public void onJump(InputAction.CallbackContext context)
    {
        jumped = context.action.triggered;
        Debug.Log(PlayerInput.all.Count);
    }

    void Update()
    {
        // Grounding
        groundedPlayer = controller.isGrounded;
        if (groundedPlayer && playerVelocity.y < 0)
        {
            playerVelocity.y = 0f;
        }

        // Moving
        Vector3 move = new Vector3(-movementInput.y, 0, movementInput.x);
        controller.Move(move * Time.deltaTime * playerSpeed);

        // Jumping
        if (jumped && groundedPlayer)
        {
            playerVelocity.y += Mathf.Sqrt(jumpHeight * -3.0f * gravityValue);
        }
        playerVelocity.y += gravityValue * Time.deltaTime;
        controller.Move(playerVelocity * Time.deltaTime);

        // Camera knockback
        if (transform.position.x > Camera.main.transform.position.x - minDistanceFromCamera)
        {
            controller.SimpleMove(new Vector3(-cameraKnockbackStrength, 0, 0));
            return;
        }
    }
}