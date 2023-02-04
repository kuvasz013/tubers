using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
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
