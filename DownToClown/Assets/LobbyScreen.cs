using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class LobbyScreen : GameScreen
{
    public GameObject playerPrefab;
    public GameObject playerList;
    public int numPlayers = 0;
    public bool canStart;
    public Button startButton;
    public bool onCountdown;
    public TextMeshProUGUI countdownTimer;
    public float timer = 10f;
    public UnityEvent onTimerEnd;
    public override void Update()
    {
        //for use for debugging
        if (Input.GetKeyDown(KeyCode.Z))
                {
                    AddPlayer();
                }

        if (Input.GetKeyDown(KeyCode.X))
        {
            RemovePlayer();
        }

        if (Input.GetKeyDown(KeyCode.C))
        {
            if (canStart)
            {
                BeginCountdown();
            }
        }

        UpdateCountdown();
    }

    public void UpdateCountdown()
    {
        if (onCountdown)
        {
            if (numPlayers < 3)
            {
                InterruptCountdown();
                return;
            }

            timer -= Time.deltaTime;
            countdownTimer.text = Mathf.CeilToInt(timer).ToString();

            if (Mathf.CeilToInt(timer) <= 0)
            {
                onTimerEnd.Invoke();
                StartGame();
            }
        }
    }

    public void InterruptCountdown()
    {
        onCountdown = false;
        timer = 10f;
        countdownTimer.gameObject.SetActive(false);
    }

    public void StartGame()
    {
        //TODO: start game lol
        this.gameObject.SetActive(false);
    }

    public void AddPlayer()
    {
        if (numPlayers >= 6)
        {
            return;
        }
        numPlayers += 1;
        GameObject temp = Instantiate(playerPrefab, playerList.transform);
        temp.GetComponentInChildren<TextMeshProUGUI>().text = "Player " + numPlayers;
        if(numPlayers >= 3)
        {
            EnableStart();
        }
    }

    public void RemovePlayer()
    {
        //Right now it removes the last player in the list but should be changed so
        //it removes specific player
        if(numPlayers <= 0)
        {
            return;
        }
        numPlayers--;
        Destroy(playerList.transform.GetChild(playerList.transform.childCount-1).gameObject);
        if(numPlayers < 3)
        {
            DisableStart();
        }
    }

    public void EnableStart()
    {
        startButton.interactable = true;
        canStart = true;
    }

    public void DisableStart()
    {
        startButton.interactable = false;
        canStart = false;
    }

    public void BeginCountdown()
    {
        onCountdown = true;
        countdownTimer.gameObject.SetActive(true);
    }
}
