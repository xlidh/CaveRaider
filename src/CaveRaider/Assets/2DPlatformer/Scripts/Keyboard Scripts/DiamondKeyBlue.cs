using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiamondKeyBlue : MonoBehaviour
{
    private EndpointKey endpoint;
    void Start()
    {
        endpoint = GameObject.Find("KeyBox").GetComponent<EndpointKey>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Boy"))
        {
            endpoint.boyMark++;
            gameObject.SetActive(false);
        }
    }
}
