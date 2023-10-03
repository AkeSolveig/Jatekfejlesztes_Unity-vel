using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Points : MonoBehaviour
{
    public TextMeshProUGUI scoreText;
    public int score;

    private void Start()
    {
        score = 4000;
        UpdateScore();
    }
    public void AddScore(int scoreToAdd)
    {
        score += scoreToAdd;
        UpdateScore();
    }
    public void SubstractScore(int scoreToTake)
    {
        score -= scoreToTake;
        UpdateScore();
    }
    public void UpdateScore()
    {
        scoreText.text = "" + score;
    }
    
    private void Update()
    {
        
    }

}
