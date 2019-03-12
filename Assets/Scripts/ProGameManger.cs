using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;

public class ProGameManger : MonoBehaviour
{
    public static ProGameManger instance { get; set; }
    
    // GameObjects
    public GameObject[] rooms;
    public GameObject chest;
    public GameObject drawer;
    public GameObject Barrel;
    public GameObject wall;
    public GameObject player;


    public GameObject finalMessagePanel;
    public GameObject pausePanel;
    public GameObject lightPanel;
    public GameObject KeyMainPanel;
    public GameObject GameOverPanel;

    public GameObject ControlContainer;

    public Text finalMessage;
    public Text keyCountText;

    // Start Panel
    public GameObject StartPanel;
    public Text LevelText;
    public Text KeyText;
    public Text BarrelText;

    // digits 
    //public int numberOfChests;
    //public int numberOfBarrels;
    //public int minNumberOfRoom;
    public float maxDoorDistance;
    public float seconds;
    public int delay = 3;


    // transform
    List<Transform> doors;
    List<Transform> objectLocations;
    List<GameObject> roomInGame;
    List<GameObject> barrels;
    List<GameObject> chests;

    // digits
    int numberOfChests;
    int numberOfBarrels;
    int minNumberOfRoom;
    float averageMagnitude;
    int keyCount;
    int chestCount;

    void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
        }
        if(GameInformation.level == 0)
        {
            GameInformation.level = 1;
        }
    }

    // Use this for initialization
    void Start()
    {
        Begin();
    }

    void Begin()
    {
        LevelCreator createLevel = new LevelCreator(GameInformation.level);
        numberOfBarrels = createLevel.barrels;
        numberOfChests = createLevel.chests;
        minNumberOfRoom = createLevel.rooms;
        player.SetActive(false);
        lightPanel.SetActive(false);
        KeyMainPanel.SetActive(false);
        GameOverPanel.SetActive(false);
        finalMessagePanel.SetActive(false);
        ControlContainer.SetActive(false);
        doors = new List<Transform>();
        objectLocations = new List<Transform>();
        roomInGame = new List<GameObject>();
        barrels = new List<GameObject>();
        chests = new List<GameObject>();

        FirstRoom();
    }


    public void increaseKeyCount()
    {
        keyCount += 1;
        keyCountText.text = keyCount.ToString();
    }

    void FirstRoom()
    {
        GameObject firstRoom = Instantiate(rooms[Random.Range(0, rooms.Length-1)]);
        Vector3 center = new Vector3(0, 0, 0);
        firstRoom.transform.position = center;
        Rooms room = firstRoom.GetComponent<Rooms>();
        roomInGame.Add(firstRoom);
        foreach (Transform door in room.doors)
        {
            doors.Add(door.transform);
        }
        foreach (Transform objectHolder in room.objectsLocation)
        {
            objectLocations.Add(objectHolder);
        }
        //StartCoroutine(Generate());
        Generate();
    }

    public void Generate()
    {
        //WaitForSeconds delay = new WaitForSeconds(seconds);

        while (doors.Count > 0)
        {
            //yield return delay;
            OtherRoom();
        }
        MeanVector();
        //StartCoroutine(GenerateBarrel());
        GenerateBarrel();
    }
    public void GenerateBarrel()
    {
        //WaitForSeconds delay = new WaitForSeconds(seconds);
        while (barrels.Count < numberOfBarrels)
        {
            //yield return delay;
            AddBarrel();
        }
        //StartCoroutine(GenerateChest());
        GenerateChest();
    }

    public void GenerateChest()
    {

        //WaitForSeconds delay = new WaitForSeconds(seconds);
        while (chests.Count < numberOfChests)
        {
            //yield return delay;
            AddChest();
        }
        AddDrawer();
        AddPlayer();
        LevelText.text = "LEVEL: " + GameInformation.level;
        KeyText.text = numberOfChests.ToString();
        BarrelText.text = numberOfBarrels.ToString();
        StartPanel.SetActive(true);
        Time.timeScale = 0;
    }

    void OtherRoom()
    {
        if (roomInGame.Count < minNumberOfRoom)
        {
            GameObject nextRoom = Instantiate(rooms[Random.Range(0, rooms.Length - 1)]);
            Transform door = doors[0];
            doors.RemoveAt(0);
            Rooms nextRoomScript = nextRoom.GetComponent<Rooms>();
            Transform[] roomDoors = nextRoomScript.doors;
            int startDoorIndex = Random.Range(0, roomDoors.Length - 1);
            Transform startDoors = roomDoors[startDoorIndex];
            nextRoomScript.room.transform.parent = startDoors;
            Quaternion rot = new Quaternion();


            for (int i = 0; i < roomDoors.Length; i++)
            {
                if (i != startDoorIndex)
                {
                    roomDoors[i].parent = startDoors;
                }
            }

            rot.eulerAngles = new Vector3(door.rotation.eulerAngles.x, door.rotation.eulerAngles.y + 180f, door.rotation.eulerAngles.z);

            startDoors.position = door.position;
            startDoors.rotation = rot;
            Renderer col = nextRoomScript.room.transform.GetComponent<Renderer>();
            Collider[] cols = Physics.OverlapBox(nextRoomScript.room.transform.position, new Vector3((col.bounds.size.x / 2) - .1f, col.bounds.size.y, (col.bounds.size.z / 2) - .1f));
            bool addWall = false;
            foreach (Collider col1 in cols)
            {
                if (col1.transform.root != startDoors.root)
                {
                    addWall = true;
                    break;
                }
            }

            if (addWall == false)
            {
                for (int i = 0; i < roomDoors.Length; i++)
                {
                    if (i != startDoorIndex)
                    {
                        doors.Add(roomDoors[i]);
                    }
                }
                roomInGame.Add(nextRoom);
                foreach (Transform objectHolder in nextRoomScript.objectsLocation)
                {
                    objectLocations.Add(objectHolder);
                }
            }
            else
            {
                Destroy(nextRoom);
                int x = 0;
                bool isCloseToAnotherDoor = false;
                foreach (Transform sideDoor in doors)
                {
                    if (Vector3.Distance(door.position, sideDoor.position) < maxDoorDistance)
                    {
                        doors.RemoveAt(x);
                        isCloseToAnotherDoor = true;
                        break;
                    }
                    x++;
                }
                if (isCloseToAnotherDoor == false)
                {
                    GameObject _wall = Instantiate(wall);
                    objectLocations.Add(_wall.GetComponent<wall>().objectsLocation);
                    _wall.transform.parent = door;
                    _wall.transform.localPosition = new Vector3(0, 0, 0);
                    _wall.transform.rotation = door.rotation;
                }
            }
        }
        else
        {
            Transform door = doors[0];
            doors.RemoveAt(0);
            int x = 0;
            bool isCloseToAnotherDoor = false;
            foreach (Transform sideDoor in doors)
            {
                if (Vector3.Distance(door.position, sideDoor.position) < maxDoorDistance)
                {
                    doors.RemoveAt(x);
                    isCloseToAnotherDoor = true;
                    break;
                }
                x++;
            }
            if (isCloseToAnotherDoor == false)
            {
                GameObject _wall = Instantiate(wall);
                _wall.transform.parent = door;
                objectLocations.Add(_wall.GetComponent<wall>().objectsLocation);
                _wall.transform.localPosition = new Vector3(0, 0, 0);
                _wall.transform.rotation = door.rotation;
            }
        }
    }

    public void MeanVector()
    {
        foreach (Transform objectLocation in objectLocations)
        {
            averageMagnitude += objectLocation.position.magnitude;
        }
        averageMagnitude /= objectLocations.Count;
    }

    public void AddBarrel()
    {
        int startIndex = Random.Range(0, objectLocations.Count - 1);
        Transform barrelLocation = objectLocations[startIndex];
        if (barrels.Count == 0)
        {
            objectLocations.RemoveAt(startIndex);
            GameObject _barrel = Instantiate(Barrel);
            _barrel.transform.position = barrelLocation.transform.position;
            _barrel.transform.position += new Vector3(0, 1, 0);
            _barrel.transform.rotation = _barrel.transform.rotation;
            barrels.Add(_barrel);
        }
        else
        {
            bool withinRange = false;
            foreach (GameObject _barrel in barrels)
            {
                if (averageMagnitude/1.5 > Vector3.Distance(_barrel.transform.position, barrelLocation.position))
                {
                    withinRange = true;
                }
            }
            if (withinRange == false)
            {
                objectLocations.RemoveAt(startIndex);
                GameObject _barrel = Instantiate(Barrel);
                _barrel.transform.position = barrelLocation.transform.position;
                _barrel.transform.position += new Vector3(0, 1, 0);
                _barrel.transform.rotation = _barrel.transform.rotation;
                barrels.Add(_barrel);
            }
        }
    }

    public void AddChest()
    {
        int startIndex = Random.Range(0, objectLocations.Count - 1);
        Transform chestLocation = objectLocations[startIndex];
        if (barrels.Count == 0)
        {
            objectLocations.RemoveAt(startIndex);
            GameObject _chest = Instantiate(chest);
            _chest.transform.position = new Vector3(chestLocation.transform.position.x, chestLocation.transform.position.y + 1, chestLocation.transform.position.z);
            _chest.transform.rotation = chestLocation.transform.rotation;
            chests.Add(_chest);
        }
        else
        {
            bool withinRange = false;
            foreach (GameObject _chest in chests)
            {
                if (averageMagnitude/1.5 > Vector3.Distance(_chest.transform.position, chestLocation.position))
                {
                    withinRange = true;
                }
            }
            if (withinRange == false)
            {
                objectLocations.RemoveAt(startIndex);
                GameObject _chest = Instantiate(chest);
                _chest.transform.position = new Vector3(chestLocation.transform.position.x, chestLocation.transform.position.y + 1, chestLocation.transform.position.z);
                _chest.transform.rotation = chestLocation.transform.rotation;
                chests.Add(_chest);
            }
        }
    }

    public void AddDrawer()
    {
        int startIndex = Random.Range(0, objectLocations.Count - 1);
        Transform drawerLocation = objectLocations[startIndex];
        objectLocations.RemoveAt(startIndex);
        GameObject _drawer = Instantiate(drawer);
        _drawer.transform.position = new Vector3(drawerLocation.transform.position.x, drawerLocation.transform.position.y + 1, drawerLocation.transform.position.z);
        _drawer.transform.rotation = drawerLocation.transform.rotation;
    }

    public void AddPlayer()
    {
        int startIndex = Random.Range(0, objectLocations.Count - 1);
        Transform playerLocation = objectLocations[startIndex];
        objectLocations.RemoveAt(startIndex);
        player.transform.position = new Vector3(playerLocation.transform.position.x, playerLocation.transform.position.y + .3f, playerLocation.transform.position.z);
        player.transform.rotation = playerLocation.transform.rotation;
        
        lightPanel.SetActive(true);
        KeyMainPanel.SetActive(true);
        player.SetActive(true);
    }

    void Update()
    {
        if(StartPanel.activeSelf == true)
        {
            if (Input.anyKey)
            {
                StartPanel.SetActive(false);
                Continue();
            }
                
        }
        if (Input.GetKeyDown(KeyCode.Escape))
            Paused();
    }
    public int CountKey()
    {
        return keyCount;
    }

    public int CountChest()
    {
        return numberOfChests;
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
        ControlContainer.SetActive(false);
    }

    public void Continue()
    {
        Time.timeScale = 1;
        pausePanel.SetActive(false);
        ControlContainer.SetActive(true);
    }

    public void Credit()
    {
        GameInformation.level = GameInformation.level + 1;
        SceneManager.LoadScene("proceduralTest");
    }

    public void BackToMain()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("MainMenu");
    }

    public void GameOver()
    {
        Time.timeScale = 0;
        playerMovement moveScript = player.GetComponent<playerMovement>();
        Destroy(moveScript);
        finalMessagePanel.SetActive(true);
        finalMessage.text = "GameOver";
        GameOverPanel.SetActive(true);
        ControlContainer.SetActive(false);
        GameInformation.end = System.DateTime.Now;
        SaveScore.UpdateScore();
    }

    

    public void restart()
    {
        GameInformation.level = 1;
        SceneManager.LoadScene("proceduralTest");
    }
}
