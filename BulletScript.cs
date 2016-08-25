using UnityEngine;
using System.Collections;

public class BulletScript : MonoBehaviour {

    public float maxDist = 1000000;
    public GameObject bulletHole;
    float floatInFrontOfWall = .1f;

    void Update()
    {
        RaycastHit hitInfo;
        if(Physics.Raycast(transform.position, transform.forward, out hitInfo, maxDist))
        {
            if(bulletHole && hitInfo.collider.gameObject.tag == "Level Parts")
            {
                Instantiate(bulletHole, hitInfo.point +
                    (hitInfo.normal * floatInFrontOfWall), Quaternion.LookRotation(hitInfo.normal));
            }
        }
        Destroy(gameObject);
    }
}
