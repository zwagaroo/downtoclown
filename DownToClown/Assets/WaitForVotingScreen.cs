using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class WaitForVotingScreen : GameScreen
{

    public bool onCountdown;
    public TextMeshProUGUI countdownTimer;
    public float timerCooldown = 60f;
    public float timer = 60f;

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
                Advance();
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
        //whatever you need to do once time runs out or everyone responded
    }
}
