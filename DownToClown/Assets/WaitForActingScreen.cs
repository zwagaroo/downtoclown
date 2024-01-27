using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine.UI;
using UnityEngine;

public class Profile
{
    public string name;
    public Sprite portrait;
    public string prompt;
    public string promptAnswer;

    public Profile(string newname, Sprite newportrait, string newprompt, string newpromptAnswer)
    {
        name = newname;
        portrait = newportrait;
        prompt = newprompt;
        promptAnswer = newpromptAnswer;
    }
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

    public void HideProfile()
    {
        profile.SetActive(false);
    }
    public void DisplayProfile()
    {
        profile.SetActive(true);
    }

    public void AddProfileTolist()
    {

    }
}
