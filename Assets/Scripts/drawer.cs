using UnityEngine;
using System.Collections;

public class drawer : MonoBehaviour, IObject {

    public ParticleSystem particles;
    public TextMesh message;
    public GameObject map;
    public float delay = 3;
    Animator animator;
    ProGameManger gameManager;

    // Use this for initialization
    void Start () {
        animator = GetComponent<Animator>();
        map.SetActive(false);
        particles.Stop(true);
        message.text = "";
        gameManager = ProGameManger.instance;
	}

    public void Finish()
    {
        if(gameManager.CountKey() == gameManager.CountChest())
        {
            // Complete Game;
            lamp.instance.oilDepletion = 0f;
            animator.SetTrigger("actedOn");
        } else
        {
            int pending = gameManager.CountChest() - gameManager.CountKey();
            if (pending == 1)
                message.text = pending.ToString() + " key remaining.";
            else
                message.text = pending.ToString() + " keys remaining.";
        }
        Invoke("ClearText", delay);
    }


    public void Action()
    {
        Finish();
    }

    public void Complete()
    {
        map.SetActive(true);
        particles.Play(true);
        Destroy(map, delay);
        Invoke("Victory", 3);
    }

    void Victory()
    {
        ProGameManger.instance.Victory();
    }

    public void ClearText()
    {
        message.text = "";
    }


    // Update is called once per frame
    void Update () {
	
	}
}
