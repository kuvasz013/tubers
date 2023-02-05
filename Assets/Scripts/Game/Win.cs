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
            var routine = WinCoroutine(controller.tuberType);
            StartCoroutine(routine);
        }
    }

    private IEnumerator WinCoroutine(TuberType winner)
    {
        FindObjectOfType<KnifeManager>().StopKnives();
        manager.SetInputEnabled(false);
        manager.winner = winner;
        source.clip = lidSound;
        source.Play();

        yield return new WaitForSeconds(0.5f);
        source.clip = thudSound;
        source.Play();

        yield return new WaitForSeconds(4f);
        manager.SetInputEnabled(true);
        SceneManager.LoadScene(3);
        yield return null;
    }
}
