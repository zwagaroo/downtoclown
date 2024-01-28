using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.IMGUI.Controls;
using UnityEngine;
using UnityEngine.Events;

public class WaitForResponseScreen : GameScreen
{
    public bool onCountdown;
    public TextMeshProUGUI countdownTimer;
    public float timerCooldown = 90f;
    public float timer = 90f;
    public UnityEvent OnTimerEnd;
    public GameManager gameManager;
    public override void Update()
    {
        UpdateCountdown();
    }

    public void UpdateCountdown()
    {
        if (onCountdown)
        {
            timer -= Time.deltaTime;
            countdownTimer.text = Mathf.CeilToInt(timer).ToString();

            if (Mathf.CeilToInt(timer) <= 0)
            {
                OnTimerEnd.Invoke();
            }
        }
    }

    public void BeginCountdown()
    {
        onCountdown = true;
        countdownTimer.gameObject.SetActive(true);
    }

    public void ResetCountdown()
    {
        timer = timerCooldown;
        onCountdown = false;
        countdownTimer.gameObject.SetActive(false);

    }



    public void Advance()
    {
        gameManager.SetState(GameState.WaitForActing);
    }
}
