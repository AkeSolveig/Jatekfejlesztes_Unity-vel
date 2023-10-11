using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;

public class GameSettings : MonoBehaviour
{
    public static GameSettings Instance;

    public int graphicsQuality;
    public int fullscreen;

    public AudioMixer audioMixer;
    public float volume;

    //
    private string currentScene;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
        currentScene = SceneManager.GetActiveScene().name;
        LoadSettings();
    }
    private void Update()
    {
        if(currentScene != SceneManager.GetActiveScene().name)
        {
            currentScene = SceneManager.GetActiveScene().name;
            LoadSettings();
        }
    }

    public void SaveSettings()
    {
        PlayerPrefs.SetInt("GraphicsQuality", graphicsQuality);
        PlayerPrefs.SetInt("IsFullscreen", fullscreen);
        PlayerPrefs.SetFloat("Volume", volume);
        PlayerPrefs.Save();
    }

    public void LoadSettings()
    {
        graphicsQuality = PlayerPrefs.GetInt("GraphicsQuality", 6);
        fullscreen = PlayerPrefs.GetInt("IsFullscreen", 1);
        volume = PlayerPrefs.GetFloat("Volume", 1f); 
        SetQuality(PlayerPrefs.GetInt("GraphicsQuality", 6));
        SetFullscreen(PlayerPrefs.GetInt("isFullscreen", 1) == 1); 
        SetVolume(PlayerPrefs.GetFloat("Volume", 1f)); 

    }
    public void SetVolume(float volume)
    {
        audioMixer.SetFloat("volume", volume);
        GameSettings.Instance.volume = volume;
        GameSettings.Instance.SaveSettings();
    }

    public void SetQuality(int qualityIndex)
    {
        QualitySettings.SetQualityLevel(qualityIndex);
        GameSettings.Instance.graphicsQuality = qualityIndex;
        GameSettings.Instance.SaveSettings();
    }

    public void SetFullscreen(bool isFullscreen)
    {
        Screen.fullScreen = isFullscreen;

        GameSettings.Instance.fullscreen = isFullscreen ? 1 : 0;
        GameSettings.Instance.SaveSettings();
    }
}
