using UnityEngine;

public class KillerTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.CompareTag("Player"))
        {
            var playerID = collider.gameObject.GetComponent<PlayerController>().playerId;
            FindObjectOfType<GameManager>().Kill(playerID);
        }
    }
}
