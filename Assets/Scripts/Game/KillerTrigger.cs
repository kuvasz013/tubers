using UnityEngine;

public class KillerTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider collider)
    {
        Debug.Log("TRIGGER" + collider.gameObject.name);
        if (collider.gameObject.CompareTag("Player"))
        {
            Destroy(collider.gameObject);
        }
    }
}
