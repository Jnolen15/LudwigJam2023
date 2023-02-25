using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] private GameObject pauseMenu;
    [SerializeField] bool paused;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            PauseGame();
        }
    }

    public void PauseGame()
    {
        paused = !paused;

        if (paused)
        {
            Time.timeScale = 0;
            pauseMenu.SetActive(true);
        } 
        else
        {
            Time.timeScale = 1;
            pauseMenu.SetActive(false);
        }
    }
}
