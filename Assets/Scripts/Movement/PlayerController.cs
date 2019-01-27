using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Force")]
    [SerializeField]
    private float fJumpForce;
    [SerializeField]
    private float fMovementSpeed;
    // Parameter
    [SerializeField]
    private bool bIsGrounded;
    [SerializeField]
    private float fRaycastDistance;
    [SerializeField]
    private int iControlID;

    private string horiAxis = "Horizontal";
    private string vertAxis = "Vertical";
    private string rotXaxis = "Mouse X";
    private string rotYaxis = "Mouse Y";

    private string aButton = "Jump";
    private string rtButton = "Palce";

    // Component
    private Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();   
    }

    // Update is called once per frame
    void Update()
    {
        bIsGrounded = IsGrounded();
        PerformJumping();
        PerformMovement();
    }

    public void SetControllerID(int _ID)
    {
        iControlID = _ID;

        // Movement
        horiAxis = "J" + iControlID + "Horizontal";
        vertAxis = "J" + iControlID + "Vertical";

        // Rotation
        rotXaxis = "J" + iControlID + "Mouse X";
        rotYaxis = "J" + iControlID + "Mouse Y";

        // Jump
        aButton = "J" + iControlID + "aButton";

        // Place
        rtButton = "J" + iControlID + "rtButton";
        SetMaterial();
    }

    private void PerformJumping()
    {

        if (Input.GetButtonDown(aButton) && bIsGrounded)
        {

            rb.AddForce(new Vector3(0, fJumpForce, 0), ForceMode.Force);
        }
    }

    private void PerformMovement()
    {
        float hori = Input.GetAxis(horiAxis);

        transform.position += new Vector3(hori * fMovementSpeed * Time.deltaTime, 0, 0);
    }

    private bool IsGrounded()
    {
        bool _groudned = false;

        Ray ray = new Ray(transform.position, Vector3.down);

        Debug.DrawRay(ray.origin, ray.direction);

        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, fRaycastDistance))
        {
            if (!hit.collider.isTrigger)
                _groudned = true;
        }


        return _groudned;
    }

    public void SetMaterial()
    {
        Material _mat = Resources.Load<Material>("Material/m_P" + iControlID);
        transform.Find("Model").GetComponent<Renderer>().material = _mat;
    }
}
