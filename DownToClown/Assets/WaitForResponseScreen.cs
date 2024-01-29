using System.Collections;
using System.Collections.Generic;
using TMPro;
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

    public TextMeshProUGUI questionText;
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
                timer = 0;
            }
        }
    }

    public void BeginCountdown()
    {
        onCountdown = true;
        countdownTimer.gameObject.SetActive(true);

        questionText.text = gameManager.currentPrompt;
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
