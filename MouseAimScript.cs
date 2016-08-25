using UnityEngine;
using System.Collections;

public class MouseAimScript : MonoBehaviour {

    //Mouse Look variables
    [HideInInspector]
    public bool isPaused = false;
    
    public float lookSensitivity = 22;
    public float lookSmoothDamp = 0.1f;
    [HideInInspector]
    public float yRotation;
    [HideInInspector]
    public float xRotation;
    [HideInInspector]
    public float currentYRotation;
    float currentXRotation;
    float yRotVelocity;
    float xRotVelocity;
    [HideInInspector]
    public float currentAimRatio = 1;

    public GameObject playerCamera;
    public Transform torsoBone;


    //Zoom Variables
    public float defaultCameraAngle = 60;
    [HideInInspector]
    public float currentTargetCameraAngle = 60;
    float ratioZoom = 1;
    float ratioZoomVel;
    public float ratioZoomSpeed = 0.2f;
	
	// Update is called once per frame
	void Update () {
        if (!isPaused)
        {
            yRotation += Input.GetAxis("Mouse X") * currentAimRatio * lookSensitivity;
            xRotation -= Input.GetAxis("Mouse Y") * currentAimRatio * lookSensitivity;

            xRotation = Mathf.Clamp(xRotation, -80, 80);

            currentXRotation = Mathf.SmoothDamp(currentXRotation, xRotation, ref xRotVelocity, lookSmoothDamp);
            currentYRotation = Mathf.SmoothDamp(currentYRotation, yRotation, ref yRotVelocity, lookSmoothDamp);

            transform.rotation = Quaternion.Euler(0, currentYRotation, 0);
            torsoBone.rotation = Quaternion.Euler(currentXRotation, currentYRotation, -90);
        }
        /*ADS Zoom
        if(currentAimRatio == 1)
        {
            ratioZoom = Mathf.SmoothDamp(ratioZoom, 1, ref ratioZoomVel, ratioZoomSpeed);
        }
        else
        {
            ratioZoom = Mathf.SmoothDamp(ratioZoom, 0, ref ratioZoomVel, ratioZoomSpeed);
        }
        Camera.main.fieldOfView = Mathf.Lerp(currentTargetCameraAngle, defaultCameraAngle, ratioZoom); */
    }
}
