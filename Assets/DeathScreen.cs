using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DeathScreen : MonoBehaviour
{
    public static bool gameHasEnded;

    public GameObject gameOverUI;
    public GameObject playersHand;
    public GameObject pauseMenuCanvas;

    public void GameHasEnded()
    {
        pauseMenuCanvas.GetComponent<PauseMenu>().enabled = false;
        playersHand.SetActive(false);
        gameOverUI.SetActive(true);
        Time.timeScale = 0f;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        gameHasEnded = true;
    }
    public void LoadMenu()
    {
        pauseMenuCanvas.GetComponent<PauseMenu>().enabled = true;
        playersHand.SetActive(true);
        gameOverUI.SetActive(false);
        gameHasEnded = false;
        Time.timeScale = 1f;
        SceneManager.LoadScene(0);
    }
}
