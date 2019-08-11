using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StickController : MonoBehaviour {

    private Rigidbody2D rb2d;
    public float torque = 2f;

	// Use this for initialization
	void Start () {
        rb2d = GetComponent<Rigidbody2D>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            rb2d.AddTorque(torque); 
        }
    }
}
