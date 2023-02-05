using UnityEngine;

public class KillPlane : MonoBehaviour
{
    [SerializeField] private float speed;

    public bool isMoving = false;
    public bool isFallSound = false;

    void FixedUpdate()
    {
        if (!isMoving) return;
        var vec = transform.InverseTransformVector(speed * Time.deltaTime * new Vector3(1, 0, 0));
        transform.Translate(vec);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.gameObject.CompareTag("Player"))
        {
            var audio = collision.collider.gameObject.GetComponent<PlayerAudio>();

            if (isFallSound)
            {
                audio.PlayFall();
            }
            else
            {
                audio.PlayDeath();
            }

            var playerID = collision.collider.gameObject.GetComponent<PlayerController>().playerId;
            FindObjectOfType<GameManager>().Kill(playerID);
        }
    }
}
