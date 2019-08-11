using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimeFontSize : MonoBehaviour {
    private void Awake()
    {
        gameObject.GetComponent<Text>().fontSize = Screen.height/ 16;
    }
}
