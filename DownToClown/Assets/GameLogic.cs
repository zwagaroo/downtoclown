using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NDream.AirConsole;
using Newtonsoft.Json.Linq;

public class GameLogic : MonoBehaviour
{
    // Start is called before the first frame update
    void Awake()
    {
        AirConsole.instance.onMessage += OnMessage;
    }


    void OnMessage(int from, JToken data) {
        AirConsole.instance.Message(from, "Full of pixels!");
    }

    // Update is called once per frame
    void Update()
    {

    }
}
