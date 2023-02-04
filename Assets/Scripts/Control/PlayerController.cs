using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody), typeof(PlayerInput))]
public class PlayerController : MonoBehaviour
{
    [SerializeField] private float acceleration;
    [SerializeField] private float accelerationAir;
    [SerializeField] private float maxVelocity;
    [SerializeField] private float jumpHeight;
    [SerializeField] private float turnSpeed;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private float groundCheckDistance;
    [SerializeField] private float damping;

    private Rigidbody _rb;
    private Vector2 _movementVector = Vector2.zero;
    private bool _isJump = false;
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
        _isJump = context.action.triggered;
    }

    void FixedUpdate()
    {
        var velocityXZ = new Vector2(_rb.velocity.x, _rb.velocity.z);

        _rb.AddForce(-new Vector3(velocityXZ.x, 0, velocityXZ.y) * damping);

        if (velocityXZ.magnitude >= maxVelocity)
        {
            var dragVector = -velocityXZ * 10f;
            _rb.AddForce(new Vector3(dragVector.x, 0, dragVector.y));
        }

        if (_moveAction.IsPressed() && velocityXZ.magnitude <= maxVelocity)
        {
            _rb.AddForce(new Vector3(-_movementVector.y, 0, _movementVector.x) * (GetGrounded() ? acceleration : accelerationAir));
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(_rb.velocity.normalized), Time.deltaTime * turnSpeed);
        }

        if (_isJump && GetGrounded())
        {
            _rb.AddForce(new Vector3(0, jumpHeight, 0));
        }
    }

    bool GetGrounded()
    {
        var hits = Physics.RaycastAll(groundCheck.position, -Vector3.up, groundCheckDistance);
        return hits.Any(c => c.collider.CompareTag("Ground"));
    }
}