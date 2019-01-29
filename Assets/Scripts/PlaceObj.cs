using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaceObj : MonoBehaviour
{
    public bool bCanPlace = true;
    public bool bPlaced;

    [SerializeField]
    private Material[] materials;
    private Renderer rRender;

    private int colliderEnter;

    

    // Start is called before the first frame update
    void Start()
    {
        rRender = GetComponent<Renderer>();
        rRender.material = materials[0];
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!bPlaced)
        {
            if (!other.isTrigger)
            {
                colliderEnter++;
                Debug.Log("Collider Enter:" + colliderEnter);
            }

            if (colliderEnter > 0)
                bCanPlace = false;
            else
                bCanPlace = true;

            if (!bCanPlace)
                rRender.material = materials[2];
            else
                rRender.material = materials[0];
        }

    }
    private void OnTriggerExit(Collider other)
    {
        if (!bPlaced)
        {
            if (!other.isTrigger)
            {
                colliderEnter--;
            }

            if (colliderEnter > 0)
                bCanPlace = false;
            else
                bCanPlace = true;

            if (!bCanPlace)
                rRender.material = materials[2];
            else
                rRender.material = materials[0];
        }
    }

    public void PlaceObject()
    {
        Debug.Log("Place");
        bPlaced = true;
        GetComponent<Collider>().isTrigger = false;
        rRender.material = materials[1];
        transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y, 0);
    }
}
