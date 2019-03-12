using UnityEngine;
using System.Collections;
using System;

public class Chest : MonoBehaviour, IObject {

    public GameObject item;
    public float delay = 3f;
    public bool itemVisible;
    public ParticleSystem particle;
    public bool isOpen;

    Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
        item.SetActive(false);
        particle.Stop(true);
        isOpen = false;
    }

    public void Open()
    {
        item.SetActive(true);
        particle.Play(true);
        Destroy(item, delay);
        //GameManager.instance.increaseKeyCount();
        ProGameManger.instance.increaseKeyCount();
    }

    public void Action()
    {
        animator.SetTrigger("actedOn");
    }
}
