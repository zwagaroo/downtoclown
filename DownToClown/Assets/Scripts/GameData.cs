using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class GameData
{
    public List<string> prompts;
    public List<Character> characters;
}

[Serializable]
public class Character
{
    public int id;
    public string name;
    public string description;
    public string rule;
    public string iconImage;
    public string descriptionImage;
    public string profileImage;
}
