using UnityEngine;
using System;
using System.Collections;

public class GameInformation : MonoBehaviour {

    void Awake()
    {
        DontDestroyOnLoad(transform.gameObject);
    }

    public static int level { get; set; }

    public static DateTime start { get; set; }
    public static DateTime end { get; set; }

    public static string scene { get; set; }
}
