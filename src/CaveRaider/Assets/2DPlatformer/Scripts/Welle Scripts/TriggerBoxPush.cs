using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerBoxPush : MonoBehaviour
{
    private Rigidbody2D rb;
    public float speed = 5f;
    private Vector2 size;
    private double ratioL = 5.3/18;
    private double ratioR;
    private double bodySize = 0.19;
    // Use this for initialization
    void Start()
    {
        ratioR = 1 - ratioL;
        size = GetComponent<BoxCollider2D>().size;
        size = Vector2.Scale(size, (Vector2)transform.localScale);
        rb = GetComponent<Rigidbody2D>();

    }

    void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.gameObject.CompareTag("Boy") || collision.gameObject.CompareTag("Girl"))
        {
            if (collision.transform.position.x - transform.position.x > size.x * ratioR + bodySize )
            {
                Vector2 ve;
                ve.x = -speed;
                ve.y = 0;
                rb.velocity = ve;
            }
            else if (collision.transform.position.x - transform.position.x < -(size.x * ratioL + bodySize))
            {
                Vector2 ve;
                ve.x = speed;
                ve.y = 0;
                rb.velocity = ve;
            }
        }
    }
    void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Boy") || collision.gameObject.CompareTag("Girl"))
        {
            if (collision.transform.position.x - transform.position.x > size.x * ratioR + bodySize)
            {
                Vector2 ve;
                ve.x = -speed;
                ve.y = 0;
                rb.velocity = ve;
            }
            else if (collision.transform.position.x - transform.position.x < -(size.x * ratioL + bodySize))
            {
                Vector2 ve;
                ve.x = speed;
                ve.y = 0;
                rb.velocity = ve;
            }
        }
    }


}
