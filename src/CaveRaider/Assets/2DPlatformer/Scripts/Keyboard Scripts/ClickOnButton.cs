using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class ClickOnButton : MonoBehaviour {

   public void ExitGame()
    {
        GameObject.Find("KeyBox").GetComponent<EndpointKey>().quit = true;
        GameObject.Find("KeyBox").GetComponent<Endpoint>().quit = true;
    }
}
