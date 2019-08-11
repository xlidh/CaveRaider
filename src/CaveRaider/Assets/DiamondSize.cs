using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DiamondSize : MonoBehaviour {

    private void Awake()
    {
        gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(Screen.height / 10, Screen.height / 10);
    }
}
