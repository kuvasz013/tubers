using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    [SerializeField] private float speed = 1;
    public bool isMoving = false;

    private void Start()
    {
        // Start camera here
        isMoving = true;
    }

    void FixedUpdate()
    {
        if (!isMoving) return;
        var vec = transform.InverseTransformVector(speed * Time.deltaTime * new Vector3(1, 0, 0));
        transform.Translate(vec);
    }
}
