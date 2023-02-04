using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerControlller : MonoBehaviour, Controls.IPlayerActions
{
    [SerializeField] private InputActionAsset asset;
    [SerializeField] private float maxSpeed;
    [SerializeField] private float minDistanceFromCamera;

    private Rigidbody _rb;
    private InputAction _moveAction;

    private void Awake()
    {
        _moveAction = asset.FindAction("Move");
    }

    private void Start()
    {
        _rb = GetComponent<Rigidbody>();
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (!context.performed) return;
        _rb.AddForce(new Vector3(0f, 600f, 0f));
    }

    private void FixedUpdate()
    {
        if (_moveAction.IsPressed() && _rb.velocity.magnitude <= maxSpeed && transform.position.x <= Camera.main.transform.position.x - minDistanceFromCamera)
        {
            var vec = _moveAction.ReadValue<Vector2>();
            _rb.AddForce(new Vector3(-vec.y, 0, vec.x) * 60f);
        }

        if (transform.position.x > Camera.main.transform.position.x - minDistanceFromCamera)
        {
            _rb.AddForce(new Vector3(-60f, 0, 0));
            return;
        }

    }

    public void OnMove(InputAction.CallbackContext context)
    {
    }
}
