using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnPlaySelected()
    {
        SceneManager.LoadScene(2);
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
