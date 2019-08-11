using System.Collections;
using System.Collections.Generic;
using System.IO.Ports;
using System.Threading;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerGirl : PhysicsObject
{
    public bool end;
    public bool next;
    public float maxSpeed = 7;
    public float jumpTakeOffSpeed = 7;
    public AudioClip sound;
    public float volume;
    AudioSource audio1;

    private SpriteRenderer spriteRenderer;
    private Animator animator;
    public float speed = 4f;


    //ADDED******************************
    static string Conf_res = "2323232323230800023010420000";
    static string Recal_res = "2323232323230800024008000000";
    private static SerialPort sp;
    public Text winLoseText;
    protected static bool start;
    static pos data = new pos() { x = 0, y = 0 };
    static pos origin = new pos() { x = 0, y = 0 };
    private static bool drag;
    static int x_thres = 20;
    static int y_thres = 10;
    static byte[] bytes_Conf = { 0x23, 0x23, 0x23, 0x23, 0x23, 0x23, 0x00, 0x06, 0x30, 0x01, 0x42, 0x10 };
    static byte[] bytes_Recal = { 0x23, 0x23, 0x23, 0x23, 0x23, 0x23, 0x00, 0x06, 0x40, 0x01, 0x00, 0x08 };
    static byte[] bytes_Start = { 0x23, 0x23, 0x23, 0x23, 0x23, 0x23, 0x00, 0x06, 0x30, 0x01, 0x10, 0x00 };
    static byte[] bytes_Stop = { 0x23, 0x23, 0x23, 0x23, 0x23, 0x23, 0x00, 0x06, 0x30, 0x01, 0x20, 0x00 };
    //Define a new DataType to store the position
    protected struct pos
    {
        public int x;
        public int y;

    };
    static pos Receive()
    {
        pos rec = new pos() { x = 0, y = 0 };
        for (int i = 0; i < 20; i++)
        {
            int recvv = sp.ReadByte(); //recvv != 2 && recvv != 14 && recvv != 16 && recvv != 18 && recvv != 35 && recvv != 48
            if (true)
            {
                if (i == 14)
                {
                    rec.x = recvv;
                }
                if (i == 15 && recvv == 255)
                {
                    rec.x = rec.x - 255;

                }
                if (i == 16)
                {
                    rec.y = recvv;
                }
                if (i == 17 && recvv == 254)
                {
                    rec.y = rec.y - 255;
                }
            }
        }
        if (rec.x == 0 && rec.y == 0)
        {
            drag = false;
            origin = rec;
        }
        return rec;
    }
    static void spSet()
    {
        string[] ports = SerialPort.GetPortNames();
        Debug.Log(ports[1]);
        sp = new SerialPort(ports[1]);
        Debug.Log("Serial port  :SET\n");
        Debug.Log("Serial port name :" + sp.PortName + "\n");
        sp.Open();
        Debug.Log("Serial port open  :" + sp.IsOpen + "\n");
        sp.ReadTimeout = 1000;
        sp.WriteTimeout = 1000;
    }
    static void spConf()
    {

        sp.Write(bytes_Conf, 0, bytes_Conf.Length);
        Debug.Log("Configure Response:");
        string res = "";
        for (int i = 0; i < 14; i++)
        {
            string recvv = sp.ReadByte().ToString("X2");
            res += recvv;
        }
        if (res == Conf_res)
        {
            Debug.Log("Configure succeed!");
        }

    }
    static void spRecal()
    {
        sp.Write(bytes_Recal, 0, bytes_Recal.Length);
        Debug.Log("Recalibration Response:");
        string res = "";
        for (int i = 0; i < 14; i++)
        {
            string recvv = sp.ReadByte().ToString("X2");
            res += recvv;
        }
        if (res == Recal_res)
        {
            Debug.Log("Recalibration succeed!");
        }

    }
    static void spStart()
    {
        sp.Write(bytes_Start, 0, bytes_Start.Length);
        string res = "";
        for (int i = 0; i < 14; i++)
        {
            string recvv = sp.ReadByte().ToString("X2");
            res += recvv;
        }
    }
    static void spStop()
    {
        sp.Write(bytes_Stop, 0, bytes_Stop.Length);
        sp.Write(bytes_Stop, 0, bytes_Stop.Length);
        Debug.Log("Send stop");
    }
    //Synchronous receive dataflow and return the pos value to the Update Threaing
    protected void SecondThread()
    {
        start = false;
        spSet();
        spConf();
        spRecal();
        spStart();
        start = true;
        while (!end)
        {
            data = Receive();
        }
        start = false;
        spStop();
        sp.DiscardInBuffer();
        sp.Close();
        next = true;
    }
    //DONE*******************************

    // Use this for initialization
    void Awake()
    {
        audio1 = GetComponent<AudioSource>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        start = false;
        end = false;
        next = false;
        //Wake up another Threading
        ThreadStart sd = new ThreadStart(SecondThread);
        Thread td = new Thread(sd);
        td.Start();
        maxSpeed = 8;
        speed = 5;
        winLoseText = GameObject.Find("WinLoseText").GetComponent<Text>();
        winLoseText.text = "";

    }
    void Update()
    {
        velocity.x = 0;
        ComputeVelocity();
    }
    protected override void ComputeVelocity()
    {
        if (start)
        {
            if (!drag)
            {
                if (data.x != 0 || data.y != 0)
                {
                    origin = data;
                    drag = true;
                }
            }
            else
            {
                if (data.y < 200)
                {
                    if (data.y - origin.y > y_thres)
                    {
                        if (grounded) { velocity.y = jumpTakeOffSpeed; }

                    }
                    else if (!grounded && velocity.y > 0) { velocity.y = velocity.y * 0.5f; } //Jump and hold
                }
                
                if (data.x - origin.x < -x_thres)
                {
                    velocity.x = speed;     //RIGHT    

                }
                if (data.x - origin.x > x_thres)
                {
                    velocity.x = -speed;    //LEFT
                }
            }
        }

        bool flipSprite = (spriteRenderer.flipX ? (velocity.x > 0.01f) : (velocity.x < -0.01f));
        if (flipSprite) { spriteRenderer.flipX = !spriteRenderer.flipX; }

        animator.SetBool("grounded", grounded);
        animator.SetFloat("velocityX", Mathf.Abs(velocity.x) / speed);
    }
    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("DeadLine"))
        {
            winLoseText.text = "YOU  LOSE";
            GameObject.Find("F").GetComponent<Image>().enabled = true;
            GameObject.Find("F").GetComponent<Image>().rectTransform.sizeDelta = new Vector2(Screen.height / 8, Screen.height / 8);
            GameObject.Find("KeyBox").GetComponent<Endpoint>().fail = true;
        }
        if (collision.CompareTag("RedDiamond"))
        {
            audio1.Play();
        }
    }
}