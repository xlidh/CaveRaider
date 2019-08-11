using System.Collections;
using System.Collections.Generic;
using System.IO.Ports;
using System.Threading;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class demo1 : PhysicsObject
{

    public bool end;
    public bool next;
    public float maxSpeed = 7;
    public float jumpTakeOffSpeed = 10;
    public AudioClip sound;
    public float volume;
    AudioSource audio1;

    private SpriteRenderer spriteRenderer;
    private Animator animator;
    public float speed = 2f;

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
    //private static SerialPort sp_3d;
    protected static bool start;
    static pos data = new pos() { x = 0, y = 0 };
    static pos origin = new pos() { x = 0, y = 0 };

    static int threshold = 10;
    private static bool drag;

    static byte[] bytes_Conf = { 0x23, 0x23, 0x23, 0x23, 0x23, 0x23, 0x00, 0x06, 0x30, 0x01, 0x42, 0x10 };
    static byte[] bytes_Recal = { 0x23, 0x23, 0x23, 0x23, 0x23, 0x23, 0x00, 0x06, 0x40, 0x01, 0x00, 0x08 };
    static byte[] bytes_Start = { 0x23, 0x23, 0x23, 0x23, 0x23, 0x23, 0x00, 0x06, 0x30, 0x01, 0x10, 0x00 };
    static byte[] bytes_Stop = { 0x23, 0x23, 0x23, 0x23, 0x23, 0x23, 0x00, 0x06, 0x30, 0x01, 0x20, 0x00 };
    // private int jump = 0;

    //Define a new DataType to store the position
    protected struct pos
    {
        public int x;
        public int y;

    };
    protected struct vector
    {
        public int x;
        public int y;
    }
    vector vect = new vector() { x = 0, y = 0 };
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

        if (rec.x == 0 && rec.y == 0)
        {
            drag = false;
            origin = rec;
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
    /*protected void ThirdThread()
    {

        string[] ports = SerialPort.GetPortNames();
        sp_3d = new SerialPort(ports[1]);
        sp_3d.Open();
        // Debug.Log("3 open");
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
                    jump = 0;
                    break;
                case PULL:
                    Debug.Log("Pull");
                    jump = 0;
                    break;
                case TAP:
                    Debug.Log("Tap");
                    jump = 1;
                    break;
                case DOUBLETAP:
                    Debug.Log("Double Tap");
                    jump = 0;
                    break;
                case TRIPLETAP:
                    Debug.Log("Triple Tap");
                    jump = 0;
                    break;
                case UNDEFINED:
                    Debug.Log("Undefined");
                    jump = 0;
                    break;
                default:
                    break;
            }
        }

    }*/
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
        Thread sdd = new Thread(sd);
        //ThreadStart td = new ThreadStart(ThirdThread);
        //Thread tdd = new Thread(td);
        sdd.Start();
        //tdd.Start();
        maxSpeed = 8;
        speed = 5;


    }
    void Update()
    {
        velocity.x = 0;
        ComputeVelocity();
        gameObject.transform.position = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y, -18);
    }
    protected override void ComputeVelocity()
    {

        if (start)
        {
            //Debug.Log("X" + (data.x - origin.x));
            //Debug.Log("Y" + (data.y - origin.y));
            if (!drag)
            {
                if (data.x != 0 || data.y != 0)
                {
                    origin = data;
                    drag = true;
                }
            }
            /* if (jump == 1)
             {

                 if (grounded) { velocity.y = jumpTakeOffSpeed; jump = 0; }

                 else if (!grounded && velocity.y > 0) { velocity.y = velocity.y * 0.5f; jump = 0; }

             }*/
            if (drag)
            {
                vect.x = data.x - origin.x;
                vect.y = data.y - origin.y;

                // if (data.y < 152)
                // {
                if (vect.y > threshold)
                {
                    transform.Translate(Vector3.forward * speed * (vect.y / threshold) * Time.deltaTime);
                }
                if (vect.y < -threshold)
                {
                    transform.Translate(Vector3.back * speed * (vect.y / threshold) * Time.deltaTime);
                }
                // }
                if (vect.x < -threshold)
                {
                    transform.Translate(Vector3.left * speed * (vect.x / threshold) * Time.deltaTime);
                }
                if (vect.x > threshold)
                {
                    transform.Translate(Vector3.right * speed * (vect.x / threshold) * Time.deltaTime);
                }
            }

            bool flipSprite = (spriteRenderer.flipX ? (velocity.x > 0.01f) : (velocity.x < -0.01f));
            if (flipSprite) { spriteRenderer.flipX = !spriteRenderer.flipX; }

            animator.SetBool("grounded", grounded);
            animator.SetFloat("velocityX", Mathf.Abs(velocity.x) / speed);
        }
    }
}

