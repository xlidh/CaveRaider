using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BridgeBreak : MonoBehaviour {

    
	void Start () {
      
	}

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Boy"))
            gameObject.SetActive(false);
        if(collision.gameObject.CompareTag("Girl"))
            gameObject.SetActive(false);
    }    
    // Update is called once per frame
    void Update () {
		
	}
}
