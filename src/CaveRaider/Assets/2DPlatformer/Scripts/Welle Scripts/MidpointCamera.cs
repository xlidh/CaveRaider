using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MidpointCamera : MonoBehaviour {

    private Camera m_camera;
    private Rigidbody2D Boy;
    private Rigidbody2D Girl;
    private float min = 5;
    private float max = 1000;
    private float xx = 10;
    private float yy = 5;
    public bool fix;
    // Use this for initialization
    void Start()
    {
        fix = false;
        m_camera = GetComponent<Camera>();
        Girl = GameObject.Find("Girl").GetComponent<Rigidbody2D>();
        Boy = GameObject.Find("Boy").GetComponent<Rigidbody2D>();

    }
    private void Update()
    {
        if( !fix)
        {
            float width = 0;
            float height = 0;
            Vector2 g = Girl.position;
            Vector2 b = Boy.position;
            if (Mathf.Abs((g.x - b.x)) > xx)
            {
                width = (Mathf.Abs((g.x - b.x)) - xx) / 2.5f;
            }
            if (Mathf.Abs((g.y - b.y)) > yy)
            {
                height = (Mathf.Abs((g.y - b.y)) - yy) / 2.5f;
            }

            m_camera.orthographicSize = Mathf.Min(min + (width + height), max);
            transform.position = new Vector3((g.x + b.x) / 2, (g.y + b.y) / 2, transform.position.z);
        }

    }
}
