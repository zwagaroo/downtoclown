using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class RoundResultsScreen : GameScreen
{
    public GameObject playerPrefab;
    public bool onCountdown;
    public TextMeshProUGUI countdownTimer;
    public float timerCooldown = 5f;
    public float timer = 5f;
    public UnityEvent OnTimerEnd;
    public override void Update()
    {
        UpdateCountdown();
    }

    public void DisplayWinner()
    {
        
    }
    public void UpdateCountdown()
    {
        if (onCountdown)
        {
            timer -= Time.deltaTime;

            if (Mathf.CeilToInt(timer) <= 0)
            {
                OnTimerEnd.Invoke();
            }
        }
    }

    public void BeginCountdown()
    {
        onCountdown = true;
    }

    public void ResetCountdown()
    {
        timer = timerCooldown;
        onCountdown = false;

    }

}
