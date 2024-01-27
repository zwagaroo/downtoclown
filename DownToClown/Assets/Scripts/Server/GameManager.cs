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
    public List<string> prompts;

    public bool initedRoles = false;

    public void Start()
    {

        UnityEngine.TextAsset promptsAsset = Resources.Load<UnityEngine.TextAsset>("prompts");
        UnityEngine.TextAsset roleDescriptionAsset = Resources.Load<UnityEngine.TextAsset>("roleDescriptions");

        prompts = JsonConvert.DeserializeObject<List<string>>(promptsAsset.text);

        InitializeNewGame();
    }

    public void InitializeNewGame()
    {
        currentState = GameState.Lobby;
        InitializeState(currentState);
        airConsole.onMessage += OnMessage;
    }

    public List<int> GenerateRandomSubset(int n, int k)
    {
        var numbers = Enumerable.Range(0, n + 1).ToList();
        var randomSubset = new List<int>();

        while (randomSubset.Count < k)
        {
            int randomIndex = UnityEngine.Random.Range(0, numbers.Count);
            randomSubset.Add(numbers[randomIndex]);
            numbers.RemoveAt(randomIndex);
        }

        return randomSubset;

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

        for(int i = 0; i < deviceIDs.Count; i++)
        {
            airConsole.Message(deviceIDs[i], new { msg_type = "roleAssignment", role_index = deviceIDs[i]});
        }
    }

    void InitializeWaitForPromptPicking()
    {
        screenManager.SetScreen("waitForPromptPicking");

        //wait to receive
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

        Debug.Log(msg_type);

        if(msg_type == null)
        {
            Debug.Log("NO MESSAGE TYPE");
        }

        if(msg_type == "ready")
        {
            InitializeState(GameState.CheckRole);
        }
        else if(msg_type == "prompt_picked" && currentState == GameState.WaitForPromptPicking)
        {   

        }
        else if(msg_type == "")
        {

        }
    }
}
