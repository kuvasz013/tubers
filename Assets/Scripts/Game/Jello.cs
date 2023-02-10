using UnityEngine;

[RequireComponent(typeof(MeshCollider))]
public class Jello : MonoBehaviour
{
    [SerializeField] private float bounceForce;
    private AudioSource source;

    private void Start()
    {
        source = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        source.Play();
        other.GetComponent<Rigidbody>().AddForce(Vector3.up * bounceForce, ForceMode.VelocityChange);
    }
}
