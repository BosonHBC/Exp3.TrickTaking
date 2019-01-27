using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnagArea : MonoBehaviour
{

    private SphereCollider sphCollider;
    private Vector3 snagPoint;
    // Start is called before the first frame update
    void Start()
    {
        sphCollider = GetComponent<SphereCollider>();
        snagPoint = sphCollider.center;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        
    }
}
