using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerKeyGirl : PhysicObject
{

    public float maxSpeed = 5f;
    public float jumpTakeOffSpeed = 8;
    public Text WinLoseText;
    public AudioClip sound;
    public float volume;
    AudioSource audio1;

    // private Rigidbody2D rb;


    private SpriteRenderer spriteRenderer;
    private Animator animator;
    // Use this for initialization
    void Awake()
    {
        audio1 = GetComponent<AudioSource>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        WinLoseText = GameObject.Find("WinLoseText").GetComponent<Text>();
        WinLoseText.text = "";
        jumpTakeOffSpeed = 8;
        maxSpeed = 5f;
        //rb = GetComponent<Rigidbody2D>();
    }

    protected override void ComputeVelocity()
    {
        base.ComputeVelocity();
        Vector2 move = Vector2.zero;
        move.x = Input.GetAxis("Horizontal");

        if (Input.GetButtonDown("Jump") && grounded)
        {
            velocity.y = jumpTakeOffSpeed;
        }
        else if (Input.GetButtonUp("Jump"))
        {
            if (velocity.y > 0)
                velocity.y = velocity.y * 0.5f;

        }

        bool flipSprite = (spriteRenderer.flipX ? (move.x > 0.01f) : (move.x < 0.01f));

        if (flipSprite)
        {
            spriteRenderer.flipX = !spriteRenderer.flipX;
        }

        animator.SetBool("grounded", grounded);
        animator.SetFloat("velocityX", Mathf.Abs(velocity.x) / maxSpeed);

        targetVelocity = move * maxSpeed;

    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("DeadLine"))
        {
            WinLoseText.text = "YOU  LOSE";
            GameObject.Find("F").GetComponent<Image>().enabled = true;
            GameObject.Find("F").GetComponent<Image>().rectTransform.sizeDelta = new Vector2(Screen.height / 8, Screen.height / 8);
            GameObject.Find("KeyBox").GetComponent<EndpointKey>().fail = true;
        }
        if (collision.CompareTag("RedDiamond"))
        {
            audio1.Play();
        }
    }


}


