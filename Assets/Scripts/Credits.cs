using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class Credits : MonoBehaviour {

    public float creditSpeed = 5f;
    Transform credit;
	// Use this for initialization
	void Start () {
        credit = GetComponent<Transform>();
	}
	
	// Update is called once per frame
	void Update () {
        
        credit.position += new Vector3(0,creditSpeed * Time.deltaTime,0);
	}

    public void BackToMain()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
