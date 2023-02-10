using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void OnPlaySelected()
    {
        SceneManager.LoadScene("CharacterSelector");
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
