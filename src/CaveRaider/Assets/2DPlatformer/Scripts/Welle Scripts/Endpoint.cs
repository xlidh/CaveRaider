using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Endpoint : MonoBehaviour
{

    public Text WinLoseText;
    public Text TimeText;
    public Image Red;
    public Image Blue;
    public Text RedAmount;
    public Text BlueAmount;
    public int girlMark;
    public int boyMark;
    private bool girlWin;
    private bool boyWin;
    private float time;
    private float delay = 3f;
    public bool fail;
    public bool quit;
    private int level;
    private int current;
    private string scene;
    private float quitdelay;

    // Use this for initialization
    void Start()
    {
        quit = false;
        quitdelay = 2f;
        time = 0f;
        boyMark = girlMark = 0;
        level = 0;
        Red = GameObject.Find("Red").GetComponent<Image>();
        Blue = GameObject.Find("Blue").GetComponent<Image>();
        WinLoseText = GameObject.Find("WinLoseText").GetComponent<Text>();
        TimeText = GameObject.Find("TimeText").GetComponent<Text>();
        RedAmount = GameObject.Find("RedAmount").GetComponent<Text>();
        BlueAmount = GameObject.Find("BlueAmount").GetComponent<Text>();
        WinLoseText.text = "";
        TimeText.text = "";
        RedAmount.text = "";
        BlueAmount.text = "";
        scene = SceneManager.GetActiveScene().name;
        if (scene == "FirstLevelWelle")
        {
            current = 1;
        }
        else if (scene == "SecondLevelWelle")
        {
            current = 2;
        }
        else if (scene == "ThirdLevelWelle")
        {
            current = 3;
        }
    }
    private void Update()
    {
        if (!quit)
        {
            if (Input.GetKeyDown(KeyCode.F1))
            {
                GameObject.Find("Boy").GetComponent<PlayerBoy>().end = true;
                GameObject.Find("Girl").GetComponent<PlayerGirl>().end = true;
                level = 1;
            }
            else if (Input.GetKeyDown(KeyCode.F2))
            {
                GameObject.Find("Boy").GetComponent<PlayerBoy>().end = true;
                GameObject.Find("Girl").GetComponent<PlayerGirl>().end = true;
                level = 2;
            }
            else if (Input.GetKeyDown(KeyCode.F3))
            {
                GameObject.Find("Boy").GetComponent<PlayerBoy>().end = true;
                GameObject.Find("Girl").GetComponent<PlayerGirl>().end = true;
                level = 3;
            }

            if (level == 1)
            {
                if (delay > 0)
                {
                    delay -= Time.deltaTime;
                }
                else if (GameObject.Find("Boy").GetComponent<PlayerBoy>().next && GameObject.Find("Girl").GetComponent<PlayerGirl>().next)
                {
                    SceneManager.LoadScene("FirstLevelWelle");
                }
            }
            else if (level == 2)
            {
                if (delay > 0)
                {
                    delay -= Time.deltaTime;
                }
                else if(GameObject.Find("Boy").GetComponent<PlayerBoy>().next && GameObject.Find("Girl").GetComponent<PlayerGirl>().next)
                {
                    SceneManager.LoadScene("SecondLevelWelle");
                }
            }
            else if (level == 3)
            {
                if (delay > 0)
                {
                    delay -= Time.deltaTime;
                }
                else if(GameObject.Find("Boy").GetComponent<PlayerBoy>().next && GameObject.Find("Girl").GetComponent<PlayerGirl>().next)
                {
                    SceneManager.LoadScene("ThirdLevelWelle");
                }
            }

            if (fail)
            {
                GameObject.Find("Boy").GetComponent<PlayerBoy>().end = true;
                GameObject.Find("Girl").GetComponent<PlayerGirl>().end = true;
                if (delay > 0)
                {
                    delay -= Time.deltaTime;
                }
                else if(GameObject.Find("Boy").GetComponent<PlayerBoy>().next && GameObject.Find("Girl").GetComponent<PlayerGirl>().next)
                {
                    SceneManager.LoadScene(scene);
                }
            }
            else if(GameObject.Find("Boy").GetComponent<PlayerBoy>().next && GameObject.Find("Girl").GetComponent<PlayerGirl>().next)
            {
                time += Time.deltaTime;
            }
        }
        else
        {

            if(quitdelay > 0)
            {
                quitdelay -= Time.deltaTime;
                GameObject.Find("Boy").GetComponent<PlayerBoy>().end = true;
                GameObject.Find("Girl").GetComponent<PlayerGirl>().end = true;
                if (GameObject.Find("Boy").GetComponent<PlayerBoy>().next && GameObject.Find("Girl").GetComponent<PlayerGirl>().next)
                {
                    Application.Quit();
                }
            }
            else
            {
                Application.Quit();
            }
        }
    }
    private void OnTriggerEnter2D(Collider2D trigger)
    {
        if (trigger.gameObject.CompareTag("Boy"))
        {
            if (girlWin)
            {
                winnning();
            }
            else
            {
                boyWin = true;
            }
        }
        if (trigger.gameObject.CompareTag("Girl"))
        {
            if (boyWin)
            {
                winnning();
            }
            else
            {
                girlWin = true;
            }
        }

    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Boy"))
        {
            boyWin = false;
        }
        if (collision.CompareTag("Girl"))
        {
            girlWin = false;
        }
    }
    private void winnning()
    {
        WinLoseText.text = "YOU WIN!!"; string temp = "Time Used:" + Mathf.RoundToInt(time).ToString() + "s";
        TimeText.text = temp;
        temp = "    :     " + girlMark.ToString();
        RedAmount.text = temp;
        temp = "    :     " + boyMark.ToString();
        BlueAmount.text = temp;
        Blue.enabled = true;
        Red.enabled = true;
        GameObject.Find("Boy").GetComponent<PlayerBoy>().end = true;
        GameObject.Find("Girl").GetComponent<PlayerGirl>().end = true;
        int score = girlMark + boyMark;
        GameObject grade;
        if (score > 5)
        {
            grade = GameObject.Find("A");
        }
        else if (score < 3)
        {
            grade = GameObject.Find("C");
        }
        else
        {
            grade = GameObject.Find("B");
        }
        grade.GetComponent<Image>().enabled = true;
        grade.GetComponent<Image>().rectTransform.sizeDelta = new Vector2(Screen.height / 8, Screen.height / 8);
        if (current == 3)
        {
            GameObject.Find("RedBox").GetComponent<AudioSource>().enabled = false;
            GameObject.Find("BlueBox").GetComponent<AudioSource>().enabled = true;
        }
        else
        {
            level = current + 1;
        }
    }

}
