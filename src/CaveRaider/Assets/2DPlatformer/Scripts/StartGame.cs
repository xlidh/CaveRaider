using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartGame : MonoBehaviour {

    public void Choice()
    {
        GameObject.Find("KeyV").GetComponent<Text>().enabled =true;
        GameObject.Find("KeyV").GetComponent<Button>().enabled = true;
        GameObject.Find("WelleV").GetComponent<Text>().enabled = true;
        GameObject.Find("WelleV").GetComponent<Button>().enabled = true;
    }
    public void KeyV()
    {
        SceneManager.LoadScene("FirstLevel");
    }
    public void WelleV()
    {
        SceneManager.LoadScene("FirstLevelWelle");
    }
}
