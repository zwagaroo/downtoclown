using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;
using Random = System.Random;

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

    private Sprite LoadSprite(string imagePath)
    {
        return Resources.Load<Sprite>(imagePath);
    }

    // Get icon sprite
    public Sprite GetIconSprite()
    {
        return LoadSprite(iconImage);
    }

    // Get description sprite
    public Sprite GetDescriptionSprite()
    {
        return LoadSprite(descriptionImage);
    }

    // Get profile sprite
    public Sprite GetProfileSprite()
    {
        return LoadSprite(profileImage);
    }

    public Character(Character character, int round)
    {
        this.id = character.id;
        this.name = character.name;
        this.description = character.description;
        this.rule = ReplaceSounds(character.rule, round);

        this.iconImage = character.iconImage;
        this.descriptionImage = character.descriptionImage;
        this.profileImage = character.profileImage;
    }


    public Character()
    {

    }

    public static string[] clownSoundEffects = {
        "Honk",
        "Squeak",
        "Boing",
        "Crash",
        "Bang",
        "Zap",
        "Pop",
        "Bzzt",
        "Ding",
        "Toot",
        "Clang",
        "Whir",
        "Thunk",
        "Twang",
        "Splash",
        "Whoop",
        "Hoot",
        "Chirp",
        "Clink",
        "Splat",
        "Thud",
        "Wham",
        "Zoom",
        "Bam",
        "Jingle",
        "Giggle",
        "Hiccup",
        "Chuckle",
        "Sizzle",
        "Mwah"
    };

    static string ReplaceSounds(string text, int round)
    {
        int seed = (int)DateTime.Now.Ticks & 0x0000FFFF + round;
        Random random = new Random(seed);
        string pattern = @"\{sound\}";

        // Shuffle the array of replacements
        string[] shuffledReplacements = clownSoundEffects.OrderBy(x => random.Next()).ToArray();

        int currentIndex = 0;
        return Regex.Replace(text, pattern, m => {
            // If we reached the end of the replacements, reset the index and reshuffle the array
            if (currentIndex >= shuffledReplacements.Length)
            {
                currentIndex = 0;
                shuffledReplacements = clownSoundEffects.OrderBy(x => random.Next()).ToArray();
            }
            return shuffledReplacements[currentIndex++];
        });
    }
}
