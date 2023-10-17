using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class MainOptions : MonoBehaviour
{
    public Slider volumeSlider;
    public static GameSettings Instance { get; private set; }
    private void Start()
    {
        UpdateSlider();
    }
    public void UpdateSlider()
    {
        float volume = GameSettings.Instance.UpdateSlider();
        volumeSlider.value = volume;
    }
}
