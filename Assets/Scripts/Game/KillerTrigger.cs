using UnityEngine;

public class KillerTrigger : MonoBehaviour
{
    public bool isFallSound = false;

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.CompareTag("Player"))
        {
            var playerID = collider.gameObject.GetComponent<PlayerController>().playerId;
            var audio = collider.gameObject.GetComponent<PlayerAudio>();

            if (isFallSound)
            {
                audio.PlayFall();
            } else
            {
                audio.PlayDeath();
            }

            FindObjectOfType<GameManager>().Kill(playerID);
        }
    }
}
