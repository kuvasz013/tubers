using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Collider))]
public class Win : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private GameManager manager;

    [SerializeField] private AudioClip lidSound;
    [SerializeField] private AudioClip thudSound;

    private AudioSource source;
    private bool win;

    private void Start()
    {
        source = GetComponent<AudioSource>();
        manager = FindObjectOfType<GameManager>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !win)
        {
            win = true;
            Debug.Log(other.name + " WINS!");
            animator.SetTrigger("CloseLid");
            var player = other.gameObject;
            var controller = player.GetComponent<PlayerController>();
            GameManager.Winner = controller.tuberType;
            Debug.Log($"Set winner to {controller.tuberType}");
            StartCoroutine(nameof(WinCoroutine));
        }
    }

    private IEnumerator WinCoroutine()
    {
        FindObjectOfType<KnifeManager>().StopKnives();
        manager.SetInputEnabled(false);

        source.clip = lidSound;
        source.Play();
        yield return new WaitForSeconds(1f);

        source.clip = thudSound;
        source.Play();

        yield return new WaitForSeconds(3f);
        manager.SetInputEnabled(true);
        SceneManager.LoadScene("EndGame");
        yield return null;
    }
}
