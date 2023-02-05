using UnityEngine;

public class KillerTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider collider)
    {
        Debug.Log(collider.gameObject.name + " died!");
        if (collider.gameObject.CompareTag("Player"))
        {
            Destroy(collider.gameObject);
        }
    }
}
