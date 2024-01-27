using NDream.AirConsole;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.TextCore.Text;
using static UnityEditor.Rendering.CameraUI;

public enum GameState { Lobby, CheckRole, WaitForPromptPicking, WaitForResponse, WaitForActing, WaitForVoting, RoundResults, GameResults};

public class GameManager : MonoBehaviour
{
    public GameState currentState;
    public AirConsole airConsole;
    public ScreenManager screenManager;
    public GameData gameData;

    List<int> availiablePrompts;

    public bool initedRoles = false;


    public List<T> GenerateRandomSubset<T>(List<T> list, int k)
    {
        var randomSubset = new List<T>();

        // Make a copy of the original list
        var tempList = new List<T>(list);

        while (randomSubset.Count < k && tempList.Count > 0)
        {
            int randomIndex = UnityEngine.Random.Range(0, tempList.Count);
            randomSubset.Add(tempList[randomIndex]);
            tempList.RemoveAt(randomIndex);
        }

        return randomSubset;
    }

    public List<int> GenerateRandomSubset(int n, int k)
    {
        var numbers = new List<int>(Enumerable.Range(0, n));
        return GenerateRandomSubset(numbers, k);
    }

    public void Start()
    {

        UnityEngine.TextAsset gameDataAsset = Resources.Load<UnityEngine.TextAsset>("gameData");
        gameData = JsonConvert.DeserializeObject<GameData>(gameDataAsset.text);
        availiablePrompts = Enumerable.Range(0, gameData.prompts.Count).ToList();

        InitializeNewGame();
    }

    public static readonly int NUM_PROMPT_OPTIONS = 5;
    public List<int> GetPromptOptions()
    {
        return GenerateRandomSubset<int>(availiablePrompts, NUM_PROMPT_OPTIONS);
    }

    public void ChoosePromt(int promptIndex)
    {
        availiablePrompts.Remove(promptIndex);
    }

    public void InitializeNewGame()
    {
        SetState(GameState.Lobby);
        airConsole.onMessage += OnMessage;
    }

    public void SetState(GameState state)
    {
        currentState = state;
        InitializeState(currentState);
    }


    public void InitializeState(GameState state)
    {
        switch(state)
        {
            case GameState.Lobby:
                InitializeLobbyState();
                break;
            case GameState.CheckRole:

                if (!initedRoles)
                {
                    int numClowns = 6;

                    ClownShuffler.SetNamesAndClowns(airConsole.GetControllerDeviceIds(), GenerateRandomSubset(numClowns-1, airConsole.GetControllerDeviceIds().Count));
               
                }

                InitializeCheckRole();
                break;
            case GameState.WaitForPromptPicking:
                InitializeWaitForPromptPicking();
                break;

        }
    }

    void InitializeLobbyState()
    {
        screenManager.SetScreen("lobby");
    }



    void InitializeCheckRole()
    {
        screenManager.SetScreen("checkRole");

        var deviceIDs = airConsole.GetControllerDeviceIds();

        //TODO:: WE NEED TO NOT JUST SEND ARBITRARY INDEX

        var currentRoles = ClownShuffler.rounds[ClownShuffler.currentRound].roles;

        for(int i = 0; i < deviceIDs.Count; i++)
        {
            airConsole.Message(deviceIDs[i], new { msg_type = "role_assignment", role_index = currentRoles[deviceIDs[i]]});
        }

    }

    void InitializeWaitForPromptPicking()
    {
        screenManager.SetScreen("waitForPromptPicking");

        int heraldId = ClownShuffler.rounds[ClownShuffler.currentRound].GetHerald();
        List<int> clownIds = ClownShuffler.rounds[ClownShuffler.currentRound].GetClowns();

        //send switch screen to everyone who is not heard to waiting screen just have {msg_type = "switch_screen", screen = "waiting"}

        //send switch screen to Hearld for him to go prompt picking. Send seperately the five prompt indexes in a list.


        //wait to receive
    }

    void InitializeWaitForResponse()
    {
        screenManager.SetScreen("waitForResponse");

        //set a timer for response, check if we have enough responses
    }

    void InitializeWaitForActing()
    {
        screenManager.SetScreen("waitForActing");
    }


    void InitializeWaitForVoting()
    {
        screenManager.SetScreen("waitForVoting");

        //send voting message
    }

    void InitializeRoundResults()
    {
        screenManager.SetScreen("waitForRoundResults");
    }

    void InitializeGameResults()
    {
        screenManager.SetScreen("waitForGameResults");
    }



    void OnMessage(int from, JToken data)
    {

        string msg_type = (string)data["msg_type"];

        Debug.Log(msg_type);

        if(msg_type == null)
        {
            Debug.Log("NO MESSAGE TYPE");
        }

        if(msg_type == "switch_state")
        {
            SetState(GameState.CheckRole);
        }
        else if(msg_type == "prompt_picked" && currentState == GameState.WaitForPromptPicking)
        {   

        }
        else if(msg_type == "")
        {

        }
    }

    void OnMessageLobby()
    {

    }
}
