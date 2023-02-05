using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void OnPlaySelected()
    {
        SceneManager.LoadScene(1);
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
