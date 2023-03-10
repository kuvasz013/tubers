using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class PlayerAudio : MonoBehaviour
{
    [SerializeField] private AudioClip jump;
    [SerializeField] private AudioClip death;
    [SerializeField] private AudioClip fall;


    private AudioSource source;

    private void Start()
    {
        source = GetComponent<AudioSource>();
    }

    public void PlayJump()
    {
        source.clip = jump;
        source.Play();
    }

    public void PlayDeath()
    {
        source.clip = death;
        source.Play();
    }
    public void PlayFall()
    {
        source.clip = fall;
        source.Play();
    }
}
