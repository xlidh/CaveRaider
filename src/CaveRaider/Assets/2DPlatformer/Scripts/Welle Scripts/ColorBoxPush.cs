using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorBoxPush : MonoBehaviour {

    
    private Rigidbody2D rb;
    public float speed = 3f;
    private Vector2 size;
    private double ratioL = 5.3 / 18;
    private double ratioR;
    private double bodySize = 0.19;
    private string boyTag;
    private string girlTag;
    private string boyBox;
    private string girlBox;
    // Use this for initialization
    void Start()
    {
        ratioR = 1 - ratioL;
        size = GetComponent<BoxCollider2D>().size;
        size = Vector2.Scale(size, (Vector2)transform.localScale);
        rb = GetComponent<Rigidbody2D>();
        boyTag = "Boy";
        girlTag = "Girl";
        boyBox = "BlueBox";
        girlBox = "RedBox";

    }

    void OnTriggerEnter2D(Collider2D collider)
    {

        if ((this.CompareTag(boyBox) && collider.gameObject.CompareTag(boyTag))
             || this.CompareTag(girlBox) && collider.gameObject.CompareTag(girlTag))
        {

            Debug.Log(collider.transform.position.x - transform.position.x);
            if (collider.transform.position.x - transform.position.x > size.x * ratioR + bodySize)
            {
                Vector2 ve;
                ve.x = -speed;
                ve.y = 0;
                rb.velocity = ve;
            }
            else if (collider.transform.position.x - transform.position.x < -(size.x * ratioL + bodySize))
            {
                Vector2 ve;
                ve.x = speed;
                ve.y = 0;
                rb.velocity = ve;
            }
        }
    }
    void OnTriggerStay2D(Collider2D collider)
    {
        if ((this.CompareTag(boyBox) && collider.gameObject.CompareTag(boyTag))
            || this.CompareTag(girlBox) && collider.gameObject.CompareTag(girlTag))
        {
            if (collider.transform.position.x - transform.position.x > size.x * ratioR + bodySize)
            {
                Vector2 ve;
                ve.x = -speed;
                ve.y = 0;
                rb.velocity = ve;
            }
            else if (collider.transform.position.x - transform.position.x < -(size.x * ratioL + bodySize))
            {
                Vector2 ve;
                ve.x = speed;
                ve.y = 0;
                rb.velocity = ve;
            }
        }
    }
}
