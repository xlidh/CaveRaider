using System.Collections;
using System.Collections.Generic;
using System.IO.Ports;
using System.Threading;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TwoWelles : PhysicsObject
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
    const string PUSH = "!#-3333";
    const string PULL = "!#-4444";
    const string TAP = "!#-2222";
    const string DOUBLETAP = "!#-1111";
    const string TRIPLETAP = "!#-5555";
    const string UNDEFINED = "!#-1234";
    private static SerialPort sp;
    private static SerialPort sp_3d;
    protected static bool start;
    static pos data = new pos() { x = 0, y = 0 };

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
    static void spSet()
    {
        string[] ports = SerialPort.GetPortNames();
        sp = new SerialPort(ports[0]);
        sp.Open();
        Debug.Log("2 open");
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
        sp.Close();
    }
    //Synchronous receive dataflow and return the pos value to the Update Threaing
    static pos Receive()
    {
        pos rec = new pos() { x = 0, y = 0 };
        for (int i = 0; i < 20; i++)
        {
            int recvv = sp.ReadByte();
            if (recvv != 2 && recvv != 14 && recvv != 16 && recvv != 18 && recvv != 35 && recvv != 48)
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


        return rec;
    }
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
        	// 2D
            data = Receive();

        }
        start = false;
        spStop();
        next = true;

    }
    protected void ThirdThread()
    {
        string[] ports = SerialPort.GetPortNames();
        sp_3d = new SerialPort(ports[1]);
        sp_3d.Open();
        Debug.Log("3 open");
        while (true)
        {
            string recv = "";
            for (int i = 0; i < 7; i++)
            {
                recv += (char)sp_3d.ReadByte();
            }
            switch (recv)
            {
                case PUSH:
                    Debug.Log("Push");
                    break;
                case PULL:
                    Debug.Log("Pull");
                    break;
                case TAP:
                    Debug.Log("Tap");
                    break;
                case DOUBLETAP:
                    Debug.Log("Double Tap");
                    break;
                case TRIPLETAP:
                    Debug.Log("Triple Tap");
                    break;
                case UNDEFINED:
                    Debug.Log("Undefined");
                    break;
                default:
                    break;
            }
        }
        
    }
    //DONE*******************************

    // Use this for initialization
    void Awake()
    {
        // Plug in 2D Welle fisrt!
        audio1 = GetComponent<AudioSource>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        start = false;
        end = false;
        next = false;
        //Wake up another Threading
        ThreadStart sd = new ThreadStart(SecondThread);
        Thread sdd = new Thread(sd);
        ThreadStart td = new ThreadStart(ThirdThread);
        Thread tdd = new Thread(td);
        sdd.Start();
        tdd.Start();
        maxSpeed = 8;
        speed = 5;


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
            if (data.x != 0 && data.y != 0)
            {
                Debug.Log(data.x + "    " + data.y);

            }
            Thread.Sleep(15);
            if (data.y < 152)
            {
                if (data.y > 70)                                // JUMP
                {
                    if (grounded) { velocity.y = jumpTakeOffSpeed; }
                }
                else if (!grounded && velocity.y > 0) { velocity.y = velocity.y * 0.5f; }

            }

            if (data.x > 50) { velocity.x = speed; }      //RIGHT       

            if (data.x < -20) { velocity.x = -speed; }    //LEFT
        }

        bool flipSprite = (spriteRenderer.flipX ? (velocity.x > 0.01f) : (velocity.x < -0.01f));
        if (flipSprite) { spriteRenderer.flipX = !spriteRenderer.flipX; }

        animator.SetBool("grounded", grounded);
        animator.SetFloat("velocityX", Mathf.Abs(velocity.x) / speed);
    }
} 
