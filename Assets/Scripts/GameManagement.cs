using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManagement : MonoBehaviour
{
    public static GameManagement instance;                  //So we can grab this from any script

    public static bool GameIsPaused = false;

    public GameObject pauseMenuUI;



    private void Awake()
    {
        instance = this;
    }


    public void Pause()
    {
        Time.timeScale = 0;                //Stops game time

    }

    public void Resume()
    {
        Time.timeScale = 1f;

    }

}
