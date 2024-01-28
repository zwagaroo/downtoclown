using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class RoundResultsScreen : GameScreen
{
    public GameObject playerPrefab;
    public bool onCountdown;
    public TextMeshProUGUI countdownTimer;
    public float timerCooldown = 5f;
    public float timer = 5f;
    public UnityEvent OnTimerEnd;

    public TextMeshProUGUI name1;
    public TextMeshProUGUI name2;
    public TextMeshProUGUI name3;
    public Image image1;
    public Image image2;
    public Image image3;

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

    public void SetWinners(Character character1, Character character2, Character character3=null)
    {
        name1.text = character1.name;
        name2.text = character2.name;
        if(character3 != null)
        {
            name3.text = character3.name;
            image3.sprite = character3.GetIconSprite();
        }
        image1.sprite = character1.GetIconSprite();
        image2.sprite = character2.GetIconSprite();
        

    }

}
