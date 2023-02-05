using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody), typeof(PlayerInput))]
public class PlayerController : MonoBehaviour
{
    [SerializeField] private float acceleration;
    [SerializeField] private float accelerationAir;
    [SerializeField] private float maxVelocity;
    [SerializeField] private float jumpForce;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private float groundCheckDistance;
    [SerializeField] private float damping;
    [SerializeField] public TuberType tuberType;

    private Rigidbody _rb;
    private Vector2 _movementVector = Vector2.zero;
    private InputAction _moveAction;

    private void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _moveAction = GetComponent<PlayerInput>().actions.FindAction("Move");
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        _movementVector = context.ReadValue<Vector2>();
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.action.triggered && GetGrounded())
        {
            _rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
    }

    void FixedUpdate()
    {
        var velocityXZ = new Vector2(_rb.velocity.x, _rb.velocity.z);

        _rb.AddForce(-new Vector3(velocityXZ.x, 0, velocityXZ.y) * damping);

        if (velocityXZ.magnitude >= maxVelocity)
        {
            var dragVector = -velocityXZ * 10f;
            _rb.AddForce(new Vector3(dragVector.x, 0, dragVector.y * 0.5f));
        }

        if (_moveAction.IsPressed() && velocityXZ.magnitude <= maxVelocity)
        {
            var force = new Vector3(-_movementVector.y, 0, _movementVector.x) * (GetGrounded() ? acceleration : accelerationAir);
            _rb.AddForce(force);
        }
    }

    bool GetGrounded()
    {
        var hits = Physics.RaycastAll(groundCheck.position, -Vector3.up, groundCheckDistance, LayerMask.GetMask("Ground"));
        return hits.Length > 0;
    }
}