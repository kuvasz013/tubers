using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    private GameManager gameManager;

    private void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
    }

    public void OnPlaySelected()
    {
        SceneManager.LoadScene(2 + (int)Mathf.Floor(Random.value * 3));
    }

    public void OnCreditsSelected()
    {
        SceneManager.LoadScene("Credits");
    }

    public void OnQuitSelected()
    {
        Application.Quit();
    }
}
