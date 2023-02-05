using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(AudioSource))]
public class MusicManager : MonoBehaviour
{
    [SerializeField] AudioClip menu;
    [SerializeField] AudioClip level;
    [SerializeField] private AudioSource source;

    private void Awake()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
        var objs = FindObjectsOfType<GameManager>();
        if (objs.Length > 1)
        {
            Destroy(gameObject);
        }
        else
        {
            DontDestroyOnLoad(gameObject);
        }
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name.StartsWith("Level"))
        {
            source.Stop();
            source.clip = level;
            source.Play();
        } else if (!source.isPlaying)
        {
            source.clip= menu;
            source.Play();
        }
    }
}
