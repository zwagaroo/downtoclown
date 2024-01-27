using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;

public class Player
{
    public string name;
}


public class Character
{
    public string name;
}



[Serializable]
public class Round
{
    public Dictionary<Player, Character> roles = new();

    public List<Character> GetUsedCharacters()
    {
        return roles.Values.ToList();
    }

    public static List<Character> GetPreviousCharacters(List<Round> rounds, Player player, int currentRound)
    {
        List<Character> previousCharacters = new List<Character>();
        for (int round = 0; round < currentRound; round++)
        {
            previousCharacters.Add(rounds[round].roles[player]);
        }
        return previousCharacters;
    }

    public static List<Character> GetInvalidCharacters(List<Round> rounds, Player player, int currentRound)
    {
        List<Character> previousCharacters = GetPreviousCharacters(rounds, player, currentRound);
        return rounds[currentRound].GetUsedCharacters().Union(previousCharacters).ToList();
    }

    public static List<Character> GetValidCharacters(List<Round> rounds, Player player, int currentRound, List<Character> characters)
    {
        return characters.Except(GetInvalidCharacters(rounds, player, currentRound)).ToList();
    }

    public static Character PickRandomCharacter(List<Round> rounds, Player player, int currentRound, List<Character> characters)
    {
        List<Character> validCharacters = GetValidCharacters(rounds, player, currentRound, characters);
        if (validCharacters.Count == 0)
        {
            Debug.LogError("NO VALID CHARACTER AVAILIABLE");
        }
        int randomIndex = UnityEngine.Random.Range(0, validCharacters.Count);
        return validCharacters[randomIndex];
    }
}

public class ClownShuffler : MonoBehaviour
{
    List<Round> rounds;

    public void SetNamesAndClowns(List<Player> players, List<Character> characters)
    {
        rounds = new List<Round>();

        for (int i = 0; i < players.Count; i++)
        {
            rounds.Add(new Round());
        }

        foreach (var player in players)
        {
            for (int round = 0; round < players.Count; round++)
            {
                rounds[round].roles[player] = Round.PickRandomCharacter(rounds, player, round, characters);
            }
        }
    }

    public void Start()
    {
        // Create some sample players and characters
        List<Player> players = new List<Player>
        {
            new Player { name = "Player1" },
            new Player { name = "Player2" },
            new Player { name = "Player3" },
            new Player { name = "Player4" }
        };

        List<Character> characters = new List<Character>
        {
            new Character { name = "Character1" },
            new Character { name = "Character2" },
            new Character { name = "Character3" },
            new Character { name = "Character4" }
        };

        // Set names and clowns
        SetNamesAndClowns(players, characters);

        // Print out the mappings for each round
        for (int round = 0; round < rounds.Count; round++)
        {
            Debug.Log("Round " + (round + 1) + " mappings:");
            foreach (var kvp in rounds[round].roles)
            {
                Debug.Log(kvp.Key.name + " - " + kvp.Value.name);
            }
        }
    }
}
