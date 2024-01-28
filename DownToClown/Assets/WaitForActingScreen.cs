using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine.UI;
using UnityEngine;

[System.Serializable]
public class Profile
{
    public Character character;
    public string prompt;
    public string promptAnswer;
}

public class WaitForActingScreen : GameScreen
{
    public GameObject profile;
    public Image portrait;
    public TextMeshProUGUI name;
    public TextMeshProUGUI prompt;
    public TextMeshProUGUI promptAnswer;
    public List<Profile> profiles;
    public bool onCountdown;


    public Profile testProfile;

    public void OnValidate()
    {
        SetProfile(testProfile);
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
        promptAnswer.text = p.promptAnswer;
    }
}
