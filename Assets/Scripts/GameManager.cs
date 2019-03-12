using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class GameManager : MonoBehaviour {
    public static GameManager instance { get; set; }
    public Text keyCountText;
    int keyCount;
    GameObject[] chests;
    int chestCount;
    public GameObject player;
    public Transform startPoint;
    public GameObject finalMessagePanel;
    public GameObject pausePanel;
    public Text finalMessage;
    public int delay = 3;

    void Awake()
    {
        if(instance != null && instance != this)
        {
            Destroy(gameObject);
        } else
        {
            instance = this;
        }

        chests = GameObject.FindGameObjectsWithTag("chest");
        chestCount = chests.Length;
        finalMessagePanel.SetActive(false);
        pausePanel.SetActive(false);
    }

    void Start()
    {
        player.transform.position = startPoint.transform.position;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            Paused();
    }
    
    public void increaseKeyCount()
    {
        keyCount += 1;
        keyCountText.text = keyCount.ToString();
    }

    public int CountKey()
    {
        return keyCount;
    }

    public int CountChest()
    {
        return chestCount;
    }

    public void Victory()
    {
        finalMessagePanel.SetActive(true);
        finalMessage.text = "YOU WIN";
        Invoke("Credit", delay);
    }

    public void Paused()
    {
        Time.timeScale = 0;
        pausePanel.SetActive(true);
    }

    public void Continue()
    {
        Time.timeScale = 1;
        pausePanel.SetActive(false);
    }

    public void Credit()
    {
        SceneManager.LoadScene("Credit");
    }

    public void BackToMain()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("MainMenu");
    }

    public void GameOver()
    {
        playerMovement moveScript = player.GetComponent<playerMovement>();
        Destroy(moveScript);
        finalMessagePanel.SetActive(true);
        finalMessage.text = "GameOver";
        Invoke("restart", delay);
    }

    public void restart()
    {
        SceneManager.LoadScene("Test");
    }

}
