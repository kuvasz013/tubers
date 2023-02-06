using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private GameObject loader;
    [SerializeField] private Spinner spinner;
    private bool clicked = false;

    public void OnPlaySelected()
    {
        if (clicked) return;
        clicked = true;
        loader.SetActive(true);
        spinner.StartSpinner();
        SceneManager.LoadSceneAsync(2 + (int)Mathf.Floor(Random.value * 3));
        //SceneManager.LoadScene("CharacterSelector");
    }

    public void OnCreditsSelected()
    {
        if (clicked) return;
        clicked = true;
        SceneManager.LoadScene("Credits");
    }

    public void OnQuitSelected()
    {
        if (clicked) return;
        clicked = true;
        Application.Quit();
    }
}
