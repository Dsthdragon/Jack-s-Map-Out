using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelLoader : MonoBehaviour {

    public GameObject LoadingScreen;
    public Slider slider;


    void Start()
    {
        //StartCoroutine(LoadAsynchronusly(GameInformation.scene));
        StartCoroutine(LoadAsynchronusly("proceduralTest"));
    }

    IEnumerator LoadAsynchronusly(string sceneTitle)
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneTitle);
        while(!operation.isDone)
        {
            yield return new WaitForSeconds(3);
            float progress = Mathf.Clamp01(operation.progress / .9f);
            Debug.Log(progress);
            slider.value = progress;
        }
    }
}
