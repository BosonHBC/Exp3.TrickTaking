using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
    public enum MovementMode
    {
        Default,
        Sliding,
        Snagged
    }

    [Header("Force")]
    [SerializeField]
    private float fJumpForce;
    [SerializeField]
    private float fMovementForce;
    [Header("Restrict")]
    [SerializeField]
    private float fRestrictAngle;
    [SerializeField]
    private float fMaxSpeed;
    [SerializeField]
    private float fJoyStickSensitivity;
    public MovementMode moveMode = MovementMode.Default;
    [SerializeField]
    private float fRaycastDistance = 0.01f;

    // Parameter
    [SerializeField]
    private bool bIsGrounded;
    [SerializeField]
    private int iControlID;

    private string horiAxis = "Horizontal";
    private string vertAxis = "Vertical";
    private string rotXaxis = "Mouse X";
    private string rotYaxis = "Mouse Y";

    private string aButton = "Jump";

    // Component
    private Rigidbody rb;
    private CapsuleCollider capCollider;
    private Transform camParent;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        capCollider = GetComponent<CapsuleCollider>();
        camParent = transform.GetChild(0);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        bIsGrounded = IsGrounded();
        PerformRotation();
        PerformMovement();
        PerformJumping();
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
    }

    private void PerformJumping()
    {     
   
        if (Input.GetButtonDown(aButton) && bIsGrounded)
        {
            Debug.Log("Press a button");
            rb.AddForce(new Vector3(0, fJumpForce, 0));
        }
    }

    private void PerformMovement()
    {

        float hori = Input.GetAxis(horiAxis);
        float vert = Input.GetAxis(vertAxis);

        Vector3 veloRef = rb.velocity;
        veloRef.y = 0;
        if (veloRef.magnitude < fMaxSpeed)
            rb.AddRelativeForce(new Vector3(hori, 0, vert) * (bIsGrounded?fMovementForce:fMovementForce/3), ForceMode.Force);

    }
    private void PerformRotation()
    {
        float hori = Input.GetAxis(rotXaxis);
        float vert = Input.GetAxis(rotYaxis);

        //if (hori < 0.1f || hori > -0.1f)
        //    hori = 0;
        //if (vert < 0.1f || vert > -0.1f)
        //    vert = 0;

        if (iControlID == 0)
            fJoyStickSensitivity = 1;

        transform.Rotate(transform.up, hori * fJoyStickSensitivity);
        camParent.Rotate(new Vector3(-vert * fJoyStickSensitivity, 0, 0));

        if (camParent.localEulerAngles.x > fRestrictAngle && camParent.localEulerAngles.x < 180)
            camParent.localEulerAngles = new Vector3(fRestrictAngle, 0, 0);
        if (camParent.localEulerAngles.x < 360 - fRestrictAngle && camParent.localEulerAngles.x > 180)
            camParent.localEulerAngles = new Vector3(360 - fRestrictAngle, 0, 0);
    }
    private bool IsGrounded()
    {
        bool _groudned = false;

        Ray ray = new Ray(transform.position, Vector3.down);

        Debug.DrawRay(ray.origin, ray.direction);

        RaycastHit hit;
        if(Physics.Raycast(ray, out hit, fRaycastDistance))
        {
            if(!hit.collider.isTrigger)
                _groudned = true;
        }
        

        return _groudned;
    }


}
