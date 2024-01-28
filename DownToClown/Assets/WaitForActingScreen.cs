using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.Events;
using NDream.AirConsole;
using UnityEngine.Profiling;

[System.Serializable]
public class Profile
{
    public Character character;
    public string prompt;
    public string promptAnswer;

    public Profile(Character c, string prompt, string promptAnswer)
    {
        character = c;
        this.prompt = prompt;
        this.promptAnswer = promptAnswer;
    }
}

public class WaitForActingScreen : GameScreen
{
    public GameObject profile;
    public Image portrait;
    public TextMeshProUGUI restriction;
    public TextMeshProUGUI name;
    public TextMeshProUGUI prompt;
    public TextMeshProUGUI promptAnswer;
    public List<Profile> profiles;
    public int currentProfileIndex;
    public bool onCountdown;
    public GameManager gameManager;


    public Profile testProfile;
    public UnityEvent onAdvance;

    public void Start()
    {
        currentProfileIndex = 0;
    }
    public void Update()
    {
        if (Input.GetKey(KeyCode.LeftArrow)) {
            if(currentProfileIndex == 0)
            {
                return;
            }

            SetProfile(profiles[currentProfileIndex - 1]);
            currentProfileIndex--;

        }

        if (Input.GetKey(KeyCode.RightArrow)){

            if (currentProfileIndex >= (profiles.Count - 1))
            {
                onAdvance.Invoke();
                return;
            }

            SetProfile(profiles[currentProfileIndex + 1]);
            currentProfileIndex++;
        }
    }

    public void OnValidate()
    {
        SetProfile(testProfile);
    }

    public void CreateProfileList()
    {
        var deviceIDs = gameManager.airConsole.GetControllerDeviceIds();

        foreach(var deviceID in deviceIDs)
        {

            var roleIndex = ClownShuffler.rounds[gameManager.currentRound].roles[deviceID];

            if (roleIndex != 0)
            {
                profiles.Add(new Profile(gameManager.gameData.characters[roleIndex], gameManager.currentPrompt, gameManager.prompt_answers[gameManager.currentRound][deviceID]));
            }
        }
    }

    public void HideProfile()
    {
        profile.SetActive(false);
    }
    public void DisplayProfile()
    {
        profile.SetActive(true);
    }

    public void SetProfile(Profile p)
    {
        name.text = p.character.name;
        portrait.sprite = p.character.GetProfileSprite();
        prompt.text = p.prompt;
        if(promptAnswer.text != null)
        {
            promptAnswer.text = p.promptAnswer;
        }
        else
        {
            promptAnswer.text = "";
        }
        restriction.text = p.character.rule;
    }
}
