using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxTrap : MonoBehaviour {

    public float minGroundNormalY = 0.65f;
    public float gravityModifier = 1f;

    protected bool grounded;
    protected Vector2 groundNormal;
    protected Vector2 velocity;
    protected Rigidbody2D rb2d;
    protected ContactFilter2D contactFilter;
    protected RaycastHit2D[] hitBuffer = new RaycastHit2D[16];
    protected const float shellRadius = 0.01f;
    protected List<RaycastHit2D> hitBufferList = new List<RaycastHit2D>(16);
    protected Vector2 targetVelocity;

    protected const float minMoveDistance = 0.001f;
    public static bool go;

    
    // Use this for initialization

    private void OnEnable()
    {
        rb2d = GetComponent<Rigidbody2D>();
    }

    void Start()
    {

        contactFilter.useTriggers = false;
        contactFilter.SetLayerMask(Physics2D.GetLayerCollisionMask(gameObject.layer));
        contactFilter.useLayerMask = true;
        go = false;
    }
    
    // Update is called once per frame
    
    void Update()
    {
        
        
            targetVelocity = Vector2.zero;

            ComputeVelocity();
        
    }

    protected virtual void ComputeVelocity()
    {

    }

    public void changeGo()
    {
        go = true;
        Debug.Log(1);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (go)
        {
            if (other.gameObject.CompareTag("Player"))
            {
                other.gameObject.SetActive(false);
            }
        }
    }

    private void FixedUpdate()
    {
        
        
            velocity += gravityModifier * Physics2D.gravity * Time.deltaTime;
            velocity.x = targetVelocity.x;

            grounded = false;
            Vector2 moveAlongGround = new Vector2(groundNormal.y, -groundNormal.x);
            Vector2 deltaPosition = velocity * Time.deltaTime;
            Vector2 move = moveAlongGround * deltaPosition.x;
            Movement(move, false);
            move = Vector2.up * deltaPosition.y;
            Movement(move, true);
        
    }

    void Movement(Vector2 move, bool ymovement)
    {
        if (go) { 
        float distance = move.magnitude;
        if (distance > minMoveDistance)
        {
            int count = rb2d.Cast(move, contactFilter, hitBuffer, distance + shellRadius);
            hitBufferList.Clear();
            for (int i = 0; i < count; i++)
            {
                hitBufferList.Add(hitBuffer[i]);
            }

            for (int i = 0; i < hitBufferList.Count; i++)
            {
                Vector2 currentNormal = hitBufferList[i].normal;
                if (currentNormal.y > minGroundNormalY)
                {
                    grounded = true;
                    if (ymovement)
                    {
                        groundNormal = currentNormal;
                        currentNormal.x = 0;
                    }
                }

                float projection = Vector2.Dot(velocity, currentNormal);
                if (projection < 0)
                {
                    velocity = velocity - projection * currentNormal;
                }

                float modifiedDisatance = hitBufferList[i].distance - shellRadius;
                distance = modifiedDisatance < distance ? modifiedDisatance : distance;
            }
        }

        rb2d.position = rb2d.position + move.normalized * distance;
    }
        }

}
