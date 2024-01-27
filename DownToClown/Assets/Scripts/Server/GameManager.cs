using NDream.AirConsole;
using Newtonsoft.Json.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum GameState { Lobby, CheckRole, WaitForPromptPicking, WaitForResponse, WaitForActing, WaitForVoting, RoundResults, GameResults};

public class GameManager : MonoBehaviour
{
    public GameState currentState;
    public AirConsole airConsole;

    public ScreenManager screenManager;

    TextAsset textAsset = Resources.Load<TextAsset>("prompts");



    public void Start()
    {
        InitializeNewGame();
    }

    public void InitializeNewGame()
    {
        currentState = GameState.Lobby;
        InitializeState(currentState);
        airConsole.onMessage += OnMessage;
    }

    public void InitializeState(GameState state)
    {
        switch(state)
        {
            case GameState.Lobby:
                InitializeLobbyState();
                break;
            case GameState.CheckRole:
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


        var active_player_ids = airConsole.GetActivePlayerDeviceIds;


        //TODO:: WE NEED TO NOT JUST SEND ARBITRARY INDEX
        for(int i = 0; i < active_player_ids.Count; i++)
        {
            airConsole.Message(i, new { msg_type = "roleAssignment", role_index = active_player_ids[i]});
        }
    }

    void InitializeWaitForPromptPicking()
    {
        screenManager.SetScreen("waitForPromptPicking");


    }

    void InitializeWaitForResponse()
    {
        screenManager.SetScreen("waitForResponse");
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

        if(msg_type == null)
        {
            Debug.Log("NO MESSAGE TYPE");
        }

        if(msg_type == "switch_scene")
        {
            InitializeState(GameState.CheckRole);
        }
        else if(msg_type == "")
        {

        }
    }
}
