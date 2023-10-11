using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Rendering;

public class SettingsMenu : MonoBehaviour
{
    public AudioMixer audioMixer;
    private int fullscreen;
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
