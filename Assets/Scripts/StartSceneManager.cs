using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class StartSceneManager : MonoBehaviour
{
    public TextMeshProUGUI highScoreText;
    public TextMeshProUGUI highScoreTimeText;

    void Start()
    {
        int highScore = PlayerPrefs.GetInt("HighScore", 0); 
        float highScoreTime = PlayerPrefs.GetFloat("HighScoreTime", 0f);

        int minutes = Mathf.FloorToInt(highScoreTime / 60);
        int seconds = Mathf.FloorToInt(highScoreTime % 60);
        int milliseconds = Mathf.FloorToInt((highScoreTime * 100) % 100);

        highScoreText.text = "High Score: " + highScore.ToString();
        highScoreTimeText.text = string.Format("Time: {0:D2}:{1:D2}:{2:D2}", minutes, seconds, milliseconds);
    }
}