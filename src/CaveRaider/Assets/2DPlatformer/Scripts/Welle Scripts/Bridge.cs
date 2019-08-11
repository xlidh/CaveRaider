using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bridge : MonoBehaviour {
    private GameObject BridgeButton;
    private BridgeButton button;
    private Vector3 max;
    private Vector3 min;

    void Start ()
    {
        BridgeButton = GameObject.Find("ButtonPush");
        button = BridgeButton.GetComponent<BridgeButton>();
        max = new Vector3(35, transform.position.y, transform.position.z);
        min = new Vector3(transform.position.x, transform.position.y, transform.position.z);
    }
	
	void Update () {
        if (button.outward == true)
        {
            Vector3 delta = new Vector3(Time.deltaTime, 0, 0);
            if(transform.position.x < max.x)
            {
                transform.position += delta;
                if (transform.position.x > max.x)
                {
                    transform.position = max;
                }
            }
        }
        else
        {
            Vector3 delta = new Vector3(Time.deltaTime, 0, 0);
            if (transform.position.x > min.x)
            {
                transform.position -= delta;
                if (transform.position.x < min.x)
                {
                    transform.position = min;
                }
            }

        }
	}
}
