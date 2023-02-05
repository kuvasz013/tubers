using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void OnPlaySelected()
    {
        SceneManager.LoadScene(2 + (int)Mathf.Floor(Random.value * 3));
    }

    public void OnCreditsSelected()
    {
        Debug.Log("Credits");
    }

    public void OnQuitSelected()
    {
        Application.Quit();
    }
}
