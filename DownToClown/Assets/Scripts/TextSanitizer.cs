using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextSanitizer : MonoBehaviour
{
    public static Dictionary<string, string> wordDictionary = new Dictionary<string, string>()
    {
        {"fuck", "honk"},
        {"dick", "balloon"},
        {"shit", "pie"},
        {"asshole", "clown nose"},
        {"bitch", "silly goose"},
        {"bastard", "bozo"},
        {"damn", "circus like"},
        {"hell", "a funhouse"},
        {"crap", "clown car"},
        {"piss", "pie-in-the-face"},
        {"stripper", "juggler"}
    };

    public static string Sanitize(string input)
    {
        // Split the input string into words
        var words = input.Split(' ');

        // Replace any bad words
        for (var i = 0; i < words.Length; i++)
        {
            string word = words[i].ToLower(); // case insensitive

            // If the word is in the dictionary,
            // replace it with the corresponding alternative word
            if (wordDictionary.ContainsKey(word))
            {
                words[i] = wordDictionary[word];
            }
        }

        // Recombine the words and return the sanitized string
        return string.Join(" ", words);
    }
}