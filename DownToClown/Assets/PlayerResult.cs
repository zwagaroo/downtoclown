using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerResult : MonoBehaviour
{
    public TMP_Text name;
    public TMP_Text score;

    public void SetResult(Result result)
    {
        name.text = result.name;
        score.text = "Score - " + result.score;
    }
}
