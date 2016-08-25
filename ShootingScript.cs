using UnityEngine;
using System.Collections;

public class ShootingScript : MonoBehaviour {

    public float cooldown = 0.15f;
    float cooldownRemaining = 0;

    public GameObject debrisPrefab;

	// Use this for initialization
	void Start () {
	    
	}
	
	// Update is called once per frame
	void Update () {
        cooldownRemaining -= Time.deltaTime;

        if (Input.GetMouseButton(0) && cooldownRemaining <= 0)
        {
            cooldownRemaining = cooldown;
            Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);
            RaycastHit hitInfo;

            if( Physics.Raycast(ray, out hitInfo))
            {
                Vector3 hitPoint = hitInfo.point;
                GameObject hitObject = hitInfo.collider.gameObject;

                Debug.Log("Hit Object: " + hitObject.name);
                Debug.Log("Hit Point: " + hitPoint);                

                if(debrisPrefab != null)
                {
                    Instantiate( debrisPrefab, hitPoint, Quaternion.FromToRotation(Vector3.up, hitInfo.normal));
                }
            }
        }
	}
}
