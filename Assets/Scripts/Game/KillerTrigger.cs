using UnityEngine;

public class KillerTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.CompareTag("Player"))
        {
            Debug.Log(collider.gameObject.name + " died!");
            Destroy(collider.gameObject);
        }
    }
}
