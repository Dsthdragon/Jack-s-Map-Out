using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour {

    public GameObject mainMenuPanel;
    public GameObject helpMenuPanel;

    public Text Level;
    public Text Time;

    public void Start()
    {
        helpMenuPanel.SetActive(false);
        Level.text = "LEVEL: " + PlayerPrefs.GetInt("LEVEL");
        Time.text = "TIME: " + PlayerPrefs.GetFloat("TIME");
    }

    public void StartGame()
    {
        GameInformation.scene = "proceduralTest";
        GameInformation.level = 1;
        GameInformation.start = System.DateTime.Now;
        SceneManager.LoadScene("LoadingScreen");
    }

    public void Credit()
    {
        SceneManager.LoadScene("Credit");
    }

    public void Help()
    {
        mainMenuPanel.SetActive(false);
        helpMenuPanel.SetActive(true);
    }

    public void BackToMain()
    {
        helpMenuPanel.SetActive(false);
        mainMenuPanel.SetActive(true);
    }

    public void Exit()
    {
        Application.Quit();
    }
}
