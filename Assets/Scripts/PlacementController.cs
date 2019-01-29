using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlacementController : MonoBehaviour
{
    private int controlID;
    private Transform canvas;
    private string rotXaxis = "Mouse X";
    private string rotYaxis = "Mouse Y";
    private string placeButton = "";
    [SerializeField]
    private GameObject placementPrefab;
    private Transform trPlaceObj;
    [SerializeField]
    private float placeObjMoveSpeed = 2f;

    private Transform placeMentParent;

    private bool bCanPlace;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (!ControllerMapping.instance.IsGameOver)
        {
            PerformPlaceObjMovement();

            if (Input.GetButtonDown(placeButton) && bCanPlace)
            {
                PlaceObject();
            }
        }
    }

    void PerformPlaceObjMovement()
    {
        float hori = Input.GetAxis(rotXaxis);
        float vert = Input.GetAxis(rotYaxis);

        trPlaceObj.position += placeObjMoveSpeed * (new Vector3(hori, vert, 0)) * Time.deltaTime;
    }

    public void SetUpCanvas(Transform _can, int _ID)
    {
        canvas = _can;
        controlID = _ID;
        rotXaxis = "J" + controlID + "Mouse X";
        Debug.Log(rotXaxis);
        rotYaxis = "J" + controlID + "Mouse Y";
        placeButton = "J"+ controlID + "rtButton";
        trPlaceObj = canvas.GetChild(0);


        placeMentParent = GameObject.Find("PlacementsParent").transform;
    }

    public void NextPlacementStart()
    {
        ResetCanvas();
        bCanPlace = true;
        GameObject go = Instantiate(placementPrefab);
        go.transform.parent = trPlaceObj;
        go.transform.localPosition = Vector3.zero;
        go.transform.position = placeMentParent.position;

        Color _col;
        if (controlID == 1)
            _col = new Color(255f/255f, 121f/255f, 121f/255f,1);
        else
            _col = new Color(121f/255f, 121f/255f, 255f/255f,1);

        trPlaceObj.GetChild(0).GetComponent<Text>().color = _col;
        trPlaceObj.GetChild(0).GetComponent<Text>().text = "P" + controlID;
        trPlaceObj.GetChild(0).gameObject.SetActive(true);
    }

    public void PlaceObject()
    {
        PlaceObj placeObj = trPlaceObj.GetChild(1).GetComponent<PlaceObj>();
        if (placeObj.bCanPlace)
        {
            placeObj.transform.parent = placeMentParent;
            placeObj.PlaceObject();
            bCanPlace = false;
            ControllerMapping.instance.NextPlacement();
            
        }
    }

    public void ResetCanvas()
    {
        trPlaceObj.localPosition = new Vector3(0, 0, 0);
        trPlaceObj.GetChild(0).gameObject.SetActive(false);
    }

    public void IWin()
    {
        trPlaceObj.GetChild(0).GetComponent<Text>().text = "P" + controlID + " WIN!";
        trPlaceObj.GetChild(0).gameObject.SetActive(true);
    }
}
