using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class DeadLine : MonoBehaviour {


    // Use this for initialization
    void Start() {

    }

    // Update is called once per frame
    void Update() {

    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Boy") || other.gameObject.CompareTag("Girl"))
        {
            other.gameObject.GetComponent<Rigidbody2D>().transform.position = new Vector3(0, 0, 10);
            GameObject.Find("Camera").GetComponent<MidpointCamera>().fix = true;
        }
    } 
}
