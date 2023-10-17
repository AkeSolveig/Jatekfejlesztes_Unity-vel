using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Points : MonoBehaviour
{
    //points ui
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI gameScoreText;
    public TextMeshProUGUI highScoreText;

    public int score;
    private int gameScore;
    private int highScore;

    private void Start()
    {
        score = 500;
        UpdateScore();
    }
    public void AddScore(int scoreToAdd)
    {
        score += scoreToAdd;
        gameScore += scoreToAdd;
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

    public void UpdateHighScore()
    {
        Debug.Log("inside highscore update");
        highScore = PlayerPrefs.GetInt("HighScore", 0);
        if(gameScore > highScore)
        {
            highScore = gameScore;
            PlayerPrefs.SetInt("HighScore", highScore);
        }
        UpdateGamesScore();
    }
    public void UpdateGamesScore()
    {
        gameScoreText.text = "" + gameScore;
        highScoreText.text = "" + highScore;
        Debug.Log(gameScore + " / " + highScore);
    }
    public void GetHighScore()
    {
        highScore = PlayerPrefs.GetInt("HighScore", 0);
    }

}
