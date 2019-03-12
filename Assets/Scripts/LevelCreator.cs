using UnityEngine;
using System.Collections;

public class LevelCreator {

    public int barrels;
    public int chests;
    public int rooms;
    

    public LevelCreator(int level)
    {
        if(level >= 1 && level < 5)
        {
            barrels = 3;
            chests = Random.Range(3, 5);
            rooms = Random.Range(5,8);
        } else if(level >= 5 && level < 10)
        {

            barrels = 4;
            chests = Random.Range(4, 6);
            rooms = Random.Range(7, 9);
        }
        else if (level >= 10 && level < 15)
        {

            barrels = 5;
            chests = Random.Range(5, 6);
            rooms = Random.Range(9, 11);
        }
        else if (level >= 15 && level < 20)
        {

            barrels = 8;
            chests = Random.Range(6, 7);
            rooms = Random.Range(11,13);
        } else
        {
            barrels = 10;
            chests = Random.Range(8, 9);
            rooms = Random.Range(13, 15);
        }
    }

    
}
