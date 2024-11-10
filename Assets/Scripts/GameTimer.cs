using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameTimer : MonoBehaviour
{
    public TextMeshProUGUI timerText;

    private float elapsedTime = 0f;
    private bool timerRunning = false;

    void Start()
    {
        timerText.text = "Game Timer: 00:00:00";
        StartTimer();
    }

    void Update()
    {
        if (timerRunning)
        {
            elapsedTime += Time.deltaTime;

            int minutes = Mathf.FloorToInt(elapsedTime / 60);
            int seconds = Mathf.FloorToInt(elapsedTime % 60);
            int milliseconds = Mathf.FloorToInt((elapsedTime * 100) % 100);

            timerText.text = string.Format("Game Timer: {0:D2}:{1:D2}:{2:D2}", minutes, seconds, milliseconds);
        }
    }

    private void StartTimer()
    {
        timerRunning = true;
    }

    public void StopTimer()
    {
        timerRunning = false;
    }

    public void ResetTimer()
    {
        elapsedTime = 0f;
        timerText.text = "Game Timer: 00:00:00";
    }
}
