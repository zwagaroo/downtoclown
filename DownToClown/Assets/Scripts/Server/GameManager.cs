using JetBrains.Annotations;
using NDream.AirConsole;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Unity.VisualScripting;
using UnityEditor.U2D.Animation;
using UnityEngine;
using UnityEngine.TextCore.Text;
using static Unity.Collections.Unicode;
using static UnityEditor.Rendering.CameraUI;

[System.Serializable]
public enum GameState { Lobby, Cutscene, CheckRole, WaitForPromptPicking, WaitForResponse, WaitForActing, WaitForVoting, RoundResults, GameResults};

[System.Serializable]
public class GameManager : MonoBehaviour
{
    public GameState currentState;
    public AirConsole airConsole;
    public ScreenManager screenManager;
    public GameData gameData;
    public LobbyScreen lobbyScreen;


    //score boards


    List<int> availiablePrompts;

    public int currentRound;

    public string currentPrompt;

    public bool initedRoles = false;

    public List<int> deviceIds = new List<int>();

    public int responseCount;

    public List<Dictionary<int, int>> voteResult = new();

    public List<Dictionary<int, string>> prompt_answers = new List<Dictionary<int, string>>();

    public WaitForPromptPickingScreen WaitForPromptPickingScreen;

    public WaitForActingScreen waitForActingScreen;

    public RoundResultsScreen roundResultsScreen;

    public PhoneController phoneController;




    public void StartGame()
    {
        SetState(GameState.Cutscene);
    }

    public List<T> GenerateRandomSubset<T>(List<T> list, int k)
    {
        var randomSubset = new List<T>();

        // Make a copy of the original list
        var tempList = new List<T>(list);

        while (randomSubset.Count < k && tempList.Count > 0)
        {
            int randomIndex = Random.Range(0, tempList.Count);
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

    public void ChoosePrompt(int promptIndex)
    {
        availiablePrompts.Remove(promptIndex);
    }

    public void InitializeNewGame()
    {
        SetState(GameState.Lobby);
        airConsole.onMessage += OnMessage;
        airConsole.onConnect += OnConnect;
        currentRound = 0;
    }

    public void SetState(GameState state)
    {
        currentState = state;
        InitializeState(currentState);
    }



    //TODO FOR WHEN CHANI IS READY TO HOOK UP THE THINGY
    public void OnConnect(int i)
    {
        lobbyScreen.AddPlayer();
    }




    public void InitializeState(GameState state)
    {
        switch (state)
        {
            case GameState.Lobby:
                InitializeLobbyState();
                break;
            case GameState.WaitForPromptPicking:
                InitializeCheckRole();
                InitializeWaitForPromptPicking();
                break;
            case GameState.WaitForResponse:
                InitializeWaitForResponse();
                break;
            case GameState.WaitForActing:
                InitializeWaitForActing();
                break;
            case GameState.WaitForVoting:
                InitializeWaitForVoting();
                break;
            case GameState.RoundResults:
                InitializeRoundResults();
                break;
            case GameState.GameResults:
                InitializeGameResults();
                break;
            case GameState.Cutscene:
                InitializeCutscene();
                break;
        }
    }

    void InitializeLobbyState()
    {
        screenManager.SetScreen("lobby");
    }



    void InitializeCheckRole()
    {

        if (!initedRoles)
        {
            int numCharacters = gameData.characters.Count;
            int numPlayers = airConsole.GetControllerDeviceIds().Count;

            List<int> characters = GenerateRandomSubset(Enumerable.Range(1, numCharacters - 1).ToList(), numPlayers - 1);
            characters.Add(0);
            ClownShuffler.SetNamesAndClowns(airConsole.GetControllerDeviceIds(), characters);
            initedRoles = true;
        }

    }

    void InitializeWaitForPromptPicking()
    {
        screenManager.SetScreen("waitForPromptPicking");

        Round round = ClownShuffler.rounds[currentRound];
        int heraldId = round.GetHerald();

        Debug.Log(heraldId);
        List<int> clownIds = round.GetClowns();
        //send switch screen to everyone who is not heard to waiting screen just have {msg_type = "switch_screen", screen = "waiting"}


        foreach (int id in clownIds)
        {
            Debug.Log("role assignment to id " + id);

            airConsole.Message(id,
                new
                {
                    msg_type = "role_assignment",
                    role = new Character(gameData.characters[round.roles[id]], currentRound)
                });
        }

        List<string> prompts = new List<string>();
        List<int> prompt_indicies = GetPromptOptions();
        for (int i = 0; i < prompt_indicies.Count; i++)
        {
            prompts.Add(gameData.prompts[prompt_indicies[i]]);
        }

        Debug.Log("prompt picking " + heraldId);
        airConsole.Message(heraldId,
                new
                {
                    msg_type = "prompt_picking",
                    role = gameData.characters[round.roles[heraldId]],
                    prompts = prompts
                });
    }
/*
    IEnumerator Test(List<int> clownIds, Round round, int heraldId) {

    }*/

    void InitializeWaitForResponse()
    {
        prompt_answers.Add(new Dictionary<int, string>());
        screenManager.SetScreen("waitForResponse");

        responseCount = 0;
    }

    void InitializeWaitForActing()
    {
        waitForActingScreen.CreateProfileList();
        waitForActingScreen.SetProfile(waitForActingScreen.profiles[0]);
        screenManager.SetScreen("waitForActing");
    }


    void InitializeWaitForVoting()
    {
        screenManager.SetScreen("waitForVoting");
        responseCount = 0;

        voteResult.Add(new Dictionary<int, int>());

        //send voting message
    }

    void InitializeRoundResults()
    {

        //Tally logic,

        int max = 0;
        int firstClown = -1;

        foreach (var (clownID, numVotes) in voteResult[currentRound])
        {
            if(numVotes >= max)
            {
                firstClown = clownID;
            }
        }

        int secondMax = 0;

        int secondClown = -1;
        foreach (var (clownID, numVotes) in voteResult[currentRound])
        {
            if (numVotes >= secondMax && clownID != firstClown)
            {
                secondClown = clownID;
            }
        }

        /*        int thirdMax = 0;

                int thirdClown = -1;
                foreach (var (clownID, numVotes) in voteResult[currentRound])
                {
                    if (numVotes > thirdMax && clownID != firstClown && clownID != secondClown)
                    {
                        thirdClown = clownID;
                    }
                }*/

        roundResultsScreen.SetWinners(gameData.characters[firstClown], gameData.characters[secondClown]);

        screenManager.SetScreen("roundResults");
    }

    void InitializeGameResults()
    {
        screenManager.SetScreen("gameResults");
    }


    void InitializeCutscene()
    {
        screenManager.SetScreen("cutscene");
    }


    void OnMessage(int from, JToken data)
    {

        string msg_type = (string)data["msg_type"];

        if (msg_type == null)
        {
            Debug.Log("NO MESSAGE TYPE");
        }


        if (msg_type == "switch_state")
        {
            SetState(GameState.CheckRole);
        }

        switch (currentState)
        {
            case GameState.Lobby:
                OnMessageLobby(from, data, msg_type);
                break;
            case GameState.CheckRole:
                OnMessageCheckRole(from, data, msg_type);
                break;
            case GameState.WaitForPromptPicking:
                OnMessageWaitForPromptPicking(from, data, msg_type);
                break;
            case GameState.WaitForResponse:
                OnMessageWaitForResponse(from, data, msg_type);
                break;
            case GameState.WaitForActing:
                OnMessageWaitForActing(from, data, msg_type);
                break;
            case GameState.WaitForVoting:
                OnMessageWaitForVoting(from, data, msg_type);
                break;
            case GameState.RoundResults:
                OnMessageRoundResults(from, data, msg_type);
                break;
            case GameState.GameResults:
                OnMessageGameResults(from, data, msg_type);
                break;

        }

    }

    void OnMessageLobby(int from, JToken data, string msg_type)
    {

    }

    void OnMessageCheckRole(int from, JToken data, string msg_type)
    {

    }

    void OnMessageWaitForPromptPicking(int from, JToken data, string msg_type)
    {
        if (msg_type == "prompt_picked")
        {
            currentPrompt = (string)data["prompt"];
            //display the prompt

            //send everyone 

            int heraldId = ClownShuffler.rounds[currentRound].GetHerald();
            List<int> clownIds = ClownShuffler.rounds[currentRound].GetClowns();

            foreach (int id in clownIds)
            {
                airConsole.Message(id,
                    new
                    {
                        msg_type = "start_response"
                    });
            }

            airConsole.Message(heraldId,
                    new
                    {
                        msg_type = "wait"
                    });

            SetState(GameState.WaitForResponse);

            WaitForPromptPickingScreen.onPromptChosen.Invoke();
        }
    }
    public void StopWaitingForResponse()
    {
        if (currentState == GameState.WaitForResponse)
        {
            int heraldId = ClownShuffler.rounds[currentRound].GetHerald();
            List<int> clownIds = ClownShuffler.rounds[currentRound].GetClowns();

            airConsole.Message(heraldId, new { msg_type = "start_acting", prompts = GetPromptOptions() });

            //send switch screen to Hearld for him to go prompt picking. Send seperately the five prompt indexes in a list
            for (int i = 0; i < clownIds.Count; i++)
            {
                airConsole.Message(clownIds[i], new { msg_type = "wait" });
            }
            SetState(GameState.WaitForActing);
        }
    }

    void OnMessageWaitForResponse(int from, JToken data, string msg_type)
    {
        if (msg_type == "prompt_answer")
        {
            prompt_answers[currentRound][from] = (string)data["answer"];
            //add counter to get responses

            responseCount += 1;


            if (responseCount >= airConsole.GetControllerDeviceIds().Count - 1)
            {


                int heraldId = ClownShuffler.rounds[currentRound].GetHerald();


                List<int> clownIds = ClownShuffler.rounds[currentRound].GetClowns();

                airConsole.Message(heraldId, new { msg_type = "start_acting" });

                //send switch screen to Hearld for him to go prompt picking. Send seperately the five prompt indexes in a list
                for (int i = 0; i < clownIds.Count; i++)
                {
                    airConsole.Message(clownIds[i], new { msg_type = "wait" });
                }

                SetState(GameState.WaitForActing);
            }
            //I will 


        }
    }

    [System.Serializable]
    public struct CharacterResponse{
        public Character character;
        public string response;

        public CharacterResponse(Character character, string response)
        {
            this.character = character;
            this.response = response;
        }
    }

    void OnMessageWaitForActing(int from, JToken data, string msg_type)
    {
        if(msg_type == "done_acting")
        {
            var deviceIDs = airConsole.GetControllerDeviceIds();

            List< CharacterResponse> characterResponses = new();

            foreach (var deviceID in deviceIDs)
            {
                if (ClownShuffler.rounds[currentRound].roles[deviceID] != 0)
                {
                    characterResponses.Add(new CharacterResponse(gameData.characters[ClownShuffler.rounds[currentRound].roles[deviceID]], prompt_answers[currentRound][deviceID]));
                }
            }

            foreach (var deviceID in deviceIDs)
            {
                Debug.Log(JsonConvert.SerializeObject(characterResponses));
                airConsole.Message(deviceID, new { msg_type = "start_voting", characterDataList = characterResponses });
            }

            SetState(GameState.WaitForVoting);
        }
    }

    void OnMessageWaitForVoting(int from, JToken data, string msg_type)
    {
        ///TODO!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
        if (msg_type == "vote_result")
        {
            var deviceIDs = airConsole.GetControllerDeviceIds();
            int[] voteData = data["vote_data"].ToObject<int[]>();

            for(int i = 0; i < deviceIDs.Count; i++)
            {
                if (ClownShuffler.rounds[currentRound].roles[deviceIDs[i]] != 0)
                {
                    if (!voteResult[currentRound].ContainsKey(ClownShuffler.rounds[currentRound].roles[deviceIDs[i]]))
                    {
                        voteResult[currentRound][ClownShuffler.rounds[currentRound].roles[deviceIDs[i]]] = 0;
                    }
                    voteResult[currentRound][ClownShuffler.rounds[currentRound].roles[deviceIDs[i]]] += voteData[i];
                }
            }

            responseCount++;

            if (responseCount >= deviceIDs.Count)
            {

                //phone down, wait for a second and then set next state
                phoneController.PhoneDown();


                StartCoroutine(SetNextStateAfterXSeconds(GameState.RoundResults, .7f));
            }
        }
    }

    IEnumerator SetNextStateAfterXSeconds(GameState state, float time)
    {
        yield return new WaitForSeconds(time);
        SetState(state);
    }


    void OnMessageRoundResults(int from, JToken data, string msg_type)
    {

    }

    void OnMessageGameResults(int from, JToken data, string msg_type)
    {

    }
}