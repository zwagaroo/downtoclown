using NDream.AirConsole;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;


public class Result
{
    public int score;
    public string name;
}

public class ResultsController : MonoBehaviour
{
    public GameManager gameManager;

    public PlayerResult playerResultPrefab;

    public GameObject parent;

    List<Result> results = new List<Result>();

    // Start is called before the first frame update

    public void Populate()
    {
        results.Clear();

        var deviceIDs = gameManager.airConsole.GetControllerDeviceIds();

        foreach(var deviceID in deviceIDs)
        {
            results.Add(new Result { score = gameManager.scores[deviceID] , name = gameManager.airConsole.GetNickname(deviceID) });
        }

        //sort

        results.Sort((x,y) => x.score.CompareTo(y.score));

        foreach(var result in results)
        {
            PlayerResult newResult = Instantiate(playerResultPrefab, parent.transform);
            newResult.SetResult(result);
        }
    }
}

/*
 
 using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Linq;

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
    public GameManager gameManager;

    public List<Character> clownsUsed;

    public void Start()
    {
        onTimerEnd.AddListener(StartGame);
    }
    public override void Update()
    {
        ////for use for debugging
        //if (Input.GetKeyDown(KeyCode.Z))
        //        {
        //            AddPlayer();
        //        }

        //if (Input.GetKeyDown(KeyCode.X))
        //{
        //    RemovePlayer();
        //}

        //if (Input.GetKeyDown(KeyCode.C))
        //{
        //    if (canStart)
        //    {
        //        BeginCountdown();
        //    }
        //}

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
        gameManager.SetState(GameState.WaitForPromptPicking);
    }

    public void AddPlayer()
    {
        if (numPlayers >= 6)
        {
            return;
        }
        numPlayers += 1;

        if (numPlayers == 1)
        {
            clownsUsed.Add(gameManager.gameData.characters[0]);
        }
        else
        {
            List<Character> allCharacters = gameManager.gameData.characters;
            List<Character> validCharacters = allCharacters.Except(clownsUsed).ToList();
            Character nextCharacter = validCharacters[Random.Range(0, validCharacters.Count)];
            clownsUsed.Add(nextCharacter);
        }
        GameObject temp = Instantiate(playerPrefab, playerList.transform);
        temp.GetComponentInChildren<TextMeshProUGUI>().text = "Player " + numPlayers;
        temp.GetComponent<Image>().sprite = clownsUsed[clownsUsed.Count - 1].GetIconSprite();
        Debug.Log(clownsUsed[clownsUsed.Count - 1].GetIconSprite().name);

        if (numPlayers >= 3)
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

 */
