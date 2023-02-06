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
        var objs = FindObjectsOfType<MusicManager>();
        if (objs.Length > 1)
        {
            Destroy(gameObject);
        }
        else
        {
            DontDestroyOnLoad(gameObject);
        }
    }

    private void Start()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
        OnSceneLoaded(SceneManager.GetActiveScene(), LoadSceneMode.Single);
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name.StartsWith("Level"))
        {
            if (source.clip == level) return;
            Debug.Log("Start level music");
            source.clip = level;
            source.Play();
        } else
        {
            if (source.clip == menu) return;
            Debug.Log("Start menu music");
            source.clip= menu;
            source.Play();
        }
    }
}
