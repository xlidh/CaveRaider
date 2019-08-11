using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FontSize : MonoBehaviour {

	// Use this for initialization
	void Awake () {
        gameObject.GetComponent<Text>().fontSize = Screen.height / 6;
	}
}
