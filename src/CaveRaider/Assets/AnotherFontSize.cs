using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AnotherFontSize : MonoBehaviour {

    private void Awake()
    {
        gameObject.GetComponent<Text>().fontSize = Screen.height / 19;
    }
}
