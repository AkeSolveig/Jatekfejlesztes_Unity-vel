using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DeathScreen : MonoBehaviour
{
    public static bool gameHasEnded;

    public GameObject gameWonUI;
    public GameObject gameLostUI;

    public GameObject playersHand;
    public GameObject pauseMenuCanvas;
    public GameObject canvasHUD;

    public CharacterStats playerStats;

    public void GameHasEnded()
    {
        if(playerStats.isDead == true)
        {
            gameLostUI.SetActive(true);
        }
        else
        {
            gameWonUI.SetActive(true);
        }
        pauseMenuCanvas.GetComponent<PauseMenu>().enabled = false;
        canvasHUD.SetActive(false);
        playersHand.SetActive(false);
        Time.timeScale = 0f;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        gameHasEnded = true;
    }
    public void LoadMenu()
    {
        pauseMenuCanvas.GetComponent<PauseMenu>().enabled = true;
        playersHand.SetActive(true);
        gameLostUI.SetActive(false);
        gameWonUI.SetActive(false);
        gameHasEnded = false;
        Time.timeScale = 1f;
        SceneManager.LoadScene(0);
    }
}
