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

    // C#
    void OnMessage(int fromID, JToken data) {
        string action = (string) data["action"];
        int amount = (int) data["info"]["amount"];
        float torque = (float) data["info"]["torque"];
        Debug.Log(fromID + "ALSKDJALKSJDLASKJDAS! " + action);
        AirConsole.instance.Message(fromID, "BEEEEEEEEEP");
    }

    // Update is called once per frame
    void Update()
    {

    }
}
