
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BridgeButton : MonoBehaviour
{
    private Vector3 max;
    private Vector3 min;
    private bool onPress;
    public bool outward;
    private double threshold;
    public double time = 0.5;
    public float speed = 2f;
    private string boyTag = "Boy";
    private string girlTag = "Girl";

    private void Start()
    {
        max = transform.position;
        onPress = false;
        outward = false;
        min.x = transform.position.x;
        min.z = transform.position.z;
        min.y = transform.position.y - GetComponent<BoxCollider2D>().size.y;
        threshold = GetComponent<BoxCollider2D>().size.y/2;
    }
    private void Update()
    {
        Vector3 delta = new Vector3(0, Time.deltaTime * speed, 0);
        if (onPress == false && transform.position.y < max.y)
        {
            if (time > 0)
            {
                time -= Time.deltaTime;
            }
            else
            {
                outward = false;
                transform.position += delta;
            }
        }

    }
    private void OnTriggerEnter2D(Collider2D collider)
    {
        if ((collider.gameObject.CompareTag(boyTag) || collider.gameObject.CompareTag(girlTag))
            && collider.transform.position.y - transform.position.y > threshold)
        {
            time = 0.5;
            onPress = true;
            outward = true;
            Vector3 delta = new Vector3(0, Time.deltaTime * speed, 0);
            transform.position -= delta;
            if (transform.position.y < min.y)
            {
                transform.position = min;
            }
        }
    }
    private void OnTriggerStay2D(Collider2D collider)
    {
        if ((collider.gameObject.CompareTag(boyTag) || collider.gameObject.CompareTag(girlTag))
            && collider.transform.position.y - transform.position.y > threshold)
        {
            onPress = true;
            Vector3 delta = new Vector3(0, Time.deltaTime * 2, 0);
            transform.position -= delta;
            if (transform.position.y < min.y)
            {
                transform.position = min;
            }
        }
    }
    private void OnTriggerExit2D(Collider2D collider)
    {
        onPress = false;
    }


}
