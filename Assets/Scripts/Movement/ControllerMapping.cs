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

    public float playTime = 60f;
    private float collapseTime;

    public bool IsGameOver;
    [SerializeField]
    Transform indicator;
    // Start is called before the first frame update
    void Start()
    {
        collapseTime = playTime;
        JoystickAvaliable();

        // Invoke("StartGame", 1f);
        StartGame();
    }

    // Update is called once per frame
    void Update()
    {
        if (!IsGameOver)
        {
            collapseTime -= Time.deltaTime;
            indicator.transform.position = Vector3.Lerp(new Vector3(-18, 7.39f, 1.34f), new Vector3(-2, 7.39f, 1.34f), 1 - collapseTime / playTime);
            if (collapseTime <= 0)
            {


                if (lists[0].transform.position.y - lists[1].transform.position.y > 0.1f)
                    EndGame(1);
                else if (lists[1].transform.position.y - lists[0].transform.position.y > 0.1f)
                    EndGame(2);
                else
                    EndGame(3);

            }
        }
    }

    public void JoystickAvaliable()
    {
        //Get Joystick Names
        string[] temp = Input.GetJoystickNames();
        int count = 0;
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
                    if (count < MaxSupportPlayer)
                    {
                        Debug.Log("PlayerID:" + (count + 1));
                        CreatePlayer(count + 1);
                        count++;
                    }
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

        if(_id == 3)
        {
            UnityEngine.UI.Text[] texts = FindObjectsOfType<UnityEngine.UI.Text>();
            for (int i = 0; i < texts.Length; i++)
            {
                texts[i].text = "DRAW";
            }
        }
        else
        {
            PlaceLists[_id - 1].IWin();
        }
        IsGameOver = true;
    }
}
