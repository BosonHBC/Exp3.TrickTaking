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
    [SerializeField] GameObject playerPrefab;
    [SerializeField]
    private Transform[] spawns;
    [SerializeField]
    private Camera[] cams;
    [SerializeField]
    GameObject canvasPrefab;
    public int MaxSupportPlayer = 2;
    // Start is called before the first frame update
    void Start()
    {
        JoystickAvaliable();
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
    }

    public void StartGame()
    {

    }
}
