using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllerMapping : MonoBehaviour
{
    public static ControllerMapping instance;
    void Awake()
    {
        if (instance != this || instance == null)
            instance = this;
    }
    List<PlayerController> lists = new List<PlayerController>();
    List<PlacementController> PlaceLists = new List<PlacementController>();

    [SerializeField] GameObject playerPrefab;
    [SerializeField]
    private Transform[] spawns;
    [SerializeField]
    private Camera[] cams;
    [SerializeField]
    GameObject canvasPrefab;
    public int MaxSupportPlayer = 2;

    private int iCurrentPlaceId;

    public bool IsGameOver;
    // Start is called before the first frame update
    void Start()
    {
        JoystickAvaliable();

        // Invoke("StartGame", 1f);
        StartGame();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void JoystickAvaliable()
    {
        //Get Joystick Names
        string[] temp = Input.GetJoystickNames();

        //Check whether array contains anything
        if (temp.Length > 0)
        {
            //Iterate over every element
            for (int i = 0; i < temp.Length; ++i)
            {
                //Check if the string is empty or not
                if (!string.IsNullOrEmpty(temp[i]))
                {
                    //Not empty, controller temp[i] is connected
                    Debug.Log("Controller " + i + " is connected using: " + temp[i]);
                    if (i < MaxSupportPlayer)
                        CreatePlayer(i + 1);
                    else
                        Debug.Log("Over support players, not going to spawn");
                }
                else
                {
                    //If it is empty, controller i is disconnected
                    //where i indicates the controller number
                    Debug.Log("Controller: " + i + " is disconnected.");

                }
            }
        }
    }

    public void CreatePlayer(int _ID)
    {
        GameObject go = Instantiate(playerPrefab);
        go.GetComponent<PlayerController>().SetControllerID(_ID);
        go.name = "Player_" + _ID;
        go.transform.position = spawns[_ID - 1].position;
        lists.Add(go.GetComponent<PlayerController>());

        GameObject go2 = Instantiate(canvasPrefab);
        go2.name = "Canvas_" + _ID;
        Canvas _canvas = go2.GetComponent<Canvas>();
        _canvas.worldCamera = cams[_ID - 1];
        go.GetComponent<PlacementController>().SetUpCanvas(_canvas.transform, _ID);
        PlaceLists.Add(go.GetComponent<PlacementController>());


    }

    public void StartGame()
    {
        iCurrentPlaceId = 0;
        PlaceLists[iCurrentPlaceId].NextPlacementStart();
    }

    public void NextPlacement()
    {
        if (!IsGameOver)
        {
            for (int i = 0; i < PlaceLists.Count; i++)
            {
                PlaceLists[i].ResetCanvas();
            }
            iCurrentPlaceId++;
            iCurrentPlaceId %= (MaxSupportPlayer);

            PlaceLists[iCurrentPlaceId].NextPlacementStart();
        }
      
    }

    public void EndGame(int _id)
    {
        PlaceLists[_id-1].IWin();
        IsGameOver = true;
    }
}
