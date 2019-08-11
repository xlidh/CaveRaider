using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EndpointKey : MonoBehaviour
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
    private float delay = 2.5f;
    public bool fail;
    private int level;
    private int current;
    private string scene;
    public bool quit;
    // Use this for initialization
    void Start()
    {
        quit = false;
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
        if(scene == "FirstLevel")
        {
            current = 1;
        }
        else if(scene == "SecondLevel")
        {
            current = 2;
        }
        else if (scene == "ThirdLevel")
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
                level = 1;
            }
            else if (Input.GetKeyDown(KeyCode.F2))
            {
                level = 2;
            }
            else if (Input.GetKeyDown(KeyCode.F3))
            {
                level = 3;
            }

            if (level == 1)
            {
                if (delay > 0)
                {
                    delay -= Time.deltaTime;
                }
                else
                {
                    SceneManager.LoadScene("FirstLevel");
                }
            }
            else if (level == 2)
            {
                if (delay > 0)
                {
                    delay -= Time.deltaTime;
                }
                else
                {
                    SceneManager.LoadScene("SecondLevel");
                }
            }
            else if (level == 3)
            {
                if (delay > 0)
                {
                    delay -= Time.deltaTime;
                }
                else
                {
                    SceneManager.LoadScene("ThirdLevel");
                }
            }

            if (fail)
            {
                if (delay > 0)
                {
                    delay -= Time.deltaTime;
                }
                else
                {
                    SceneManager.LoadScene(scene);
                }
            }
            else
            {
                time += Time.deltaTime;
            }
        }
        else
        {
            Application.Quit();
        }
    }
    private void OnTriggerEnter2D(Collider2D trigger)
    {
        if (trigger.gameObject.CompareTag("Boy"))
        {
            if (girlWin)
            {
                winning();
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
                winning();
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
    private void winning()
    {

        WinLoseText.text = "YOU  WIN!!";
        string temp = "Time Used:" + Mathf.RoundToInt(time).ToString() + "s";
        TimeText.text = temp;
        temp = "    :     " + girlMark.ToString();
        RedAmount.text = temp;
        temp = "    :     " + boyMark.ToString();
        BlueAmount.text = temp;
        Blue.enabled = true;
        Red.enabled = true;
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
