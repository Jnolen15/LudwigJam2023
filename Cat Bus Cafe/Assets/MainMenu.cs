using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    private void Start()
    {
        Time.timeScale = 1;
    }

    public void Play()
    {
        SceneManager.LoadScene("Level");
    }

    public void Main()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void Tutorial()
    {
        Debug.Log("TODO");
        //SceneManager.LoadScene("Tutorial");
    }

    public void Quit()
    {
        Application.Quit();
    }
}
