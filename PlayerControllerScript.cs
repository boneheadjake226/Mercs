using UnityEngine;
using System.Collections;

public class PlayerControllerScript : MonoBehaviour
{


    //Movement Variables
    Rigidbody rbody;
    public Camera playerCamera;
    public float maxWalkSpeed;


    Vector3 horizontalMovement; //horizontal plane movement (xz plane)
    public float walkStepRatio;
    public float sideStepRatio;


    public float walkAcceleration= 20;
    public float walkDeacceleration = 17;

    float walkDeccelerationX;
    float walkDeccelerationZ;

    //Jump Variables
    bool grounded = false;
    public float maxSlope = 60;
    public float jumpVelocity = 20;
    public float walkAccelAirRatio = 0.1f;


    void Start()
    {
        rbody = GetComponent<Rigidbody>();
        Cursor.lockState = CursorLockMode.Locked;
        UnityEngine.Cursor.visible = false;
        walkStepRatio = 0.30f;
        sideStepRatio = 0.15f;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (GetComponent<MouseAimScript>().isPaused)
            {
                GetComponent<MouseAimScript>().isPaused = false;
                Cursor.lockState = CursorLockMode.Locked;
                UnityEngine.Cursor.visible = false;
                GetComponent<GunScript>().canFire = false;
            }
            else
            {
                Cursor.lockState = CursorLockMode.None;
                UnityEngine.Cursor.visible = true;
                GetComponent<MouseAimScript>().isPaused = true;
                GetComponent<GunScript>().canFire = true;
            }
        }
        horizontalMovement = new Vector3(transform.position.x + (Input.GetAxis("Horizontal") * walkStepRatio), transform.position.y, transform.position.z + (Input.GetAxis("Vertical") * walkStepRatio));
        transform.Translate(Vector3.forward * Input.GetAxis("Vertical") * walkStepRatio, Space.Self);
        transform.Translate(Vector3.right * Input.GetAxis("Horizontal") * sideStepRatio, Space.Self);

        //transform.position = new Vector3(transform.position.x + (Input.GetAxis("Horizontal") * stepRatio), transform.position.y, transform.position.z + (Input.GetAxis("Vertical") * stepRatio));


        //Walking
        /*horizontalMovement = new Vector2(rbody.velocity.x, rbody.velocity.z);

        if (horizontalMovement.magnitude > maxWalkSpeed)
        {
            horizontalMovement.Normalize();
            horizontalMovement *= maxWalkSpeed;
        }

        rbody.velocity = new Vector3(horizontalMovement.x, rbody.velocity.y, horizontalMovement.y);

        if (grounded)
        {

            rbody.velocity = new Vector3(Mathf.SmoothDamp(rbody.velocity.x, 0, ref walkDeccelerationX, walkDeacceleration), rbody.velocity.y,
                Mathf.SmoothDamp(rbody.velocity.z, 0, ref walkDeccelerationZ, walkDeacceleration));
        } 

        //transform.rotation = Quaternion.Euler(0, playerCamera.GetComponent<MouseAimScript>().currentYRotation, 0);


        if (grounded)
        {
            if(Input.GetAxis("Horizontal") == 0)
            {
                rbody.AddRelativeForce(-(Input.GetAxis("horizontal") * walkDeacceleration), 0, 0);
            }

            if (Input.GetAxis("Vertical") == 0)
            {
                rbody.AddRelativeForce(0, 0, -(Input.GetAxis("Vertical") * walkDeacceleration));
            }
            rbody.AddRelativeForce(Input.GetAxis("Horizontal") * walkAcceleration, 0,
                Input.GetAxis("Vertical") * walkAcceleration);
        }
        else
        {
            //Air manuvering
            rbody.AddRelativeForce(Input.GetAxis("Horizontal") * walkAcceleration * walkAccelAirRatio, 0,
                Input.GetAxis("Vertical") * walkAcceleration * walkAccelAirRatio);
        }

        //Jumping
        if (Input.GetButtonDown("Jump") && grounded)
        {
            rbody.AddForce(0, jumpVelocity, rbody.velocity.z / 2);
        }*/
    }

    void OnCollisionStay(Collision collision)
    {
        foreach(ContactPoint contact in collision.contacts)
        {
            if(Vector3.Angle(contact.normal, Vector3.up) < maxSlope)
            {
                grounded = true;
            }
        }
    }

    void OnCollisionExit(Collision collision)
    {
        grounded = false;
    }
}
