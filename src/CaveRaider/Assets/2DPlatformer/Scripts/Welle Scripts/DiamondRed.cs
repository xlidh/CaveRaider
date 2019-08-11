using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiamondRed : MonoBehaviour
{
    private Endpoint endpoint;
    void Start()
    {
        endpoint = GameObject.Find("KeyBox").GetComponent<Endpoint>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Girl"))
        {
            endpoint.girlMark++;
            gameObject.SetActive(false);
        }
    }
}
