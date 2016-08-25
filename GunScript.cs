using UnityEngine;
using System.Collections;

public class GunScript : MonoBehaviour {

    [Header("Camera Objects")]
    public GameObject playerCamera;
    public GameObject cameraReturnPosition;
    public GameObject aimCamPosition;
    private float camVelocity = 0;
    private float aimRatio;
    public float aimSpeed;
    private bool isAimming;
    [Space(2)]

    [Header("User Interface")]
    public UI_Manager GUI;
    [Space(2)]

    //Bullet Objects
    [Header("Bullet Objects")]
    public GameObject bullet;
    public GameObject bulletSpawn;
    public GameObject bulletSound;
    public GameObject muzzleFlash;
    [Space(2)]

    [Header("Gun Model and Attach Points")]
    public Transform GunRootBone;
    public Transform HandBone;

    [HideInInspector]
    public bool canFire;
    bool boltLocked;
    bool chambered;
    float waitForBoltCycle = 0.0f;

    /*public GameObject rightHandIK;
    private Vector3 rhIKReturn;
    private float rightHandVelocity = 0.0F;

    public GameObject leftHandIK;
    private Vector3 lhIKReturn;
    private float leftHandVelocity = 0.0F;*/

    public float recoilRatio;

    public float fireRate = 12;
    float waitTilNextFire = 0;
    int clipSize = 30;
    int clipCount = 6;
    public int ammoInGun = 30;
    int[] clips = new int[10];
    int currentClip = 0;


    


    void Start () {
        //rhIKReturn = rightHandIK.transform.position;
        //leftHandIK.transform.position;

        //GunModel.transform.SetParent(HandBone);

        recoilRatio = 0.4f;
        isAimming = false;
        aimSpeed = 1.2f;
        canFire = false;
        boltLocked = false;
        chambered = false;

        for (int i = 0; i < clipCount; i++)
        {
            clips[i] = clipSize;
            Debug.Log("Clip" + i + ": " + clips[i]);
        }
        Debug.Log("Current Clip:" + currentClip);

        GUI.GetComponent<UI_Manager>().updateClips(clipCount, clips, clipSize);
    }
	
	void Update () {

        //Update Ammo Counter
        GUI.GetComponent<UI_Manager>().updateAmmo(ammoInGun, clipSize);

        //Fire
        if (Input.GetMouseButton(0))
        {
            fireGun();
        }

        //ADS
        if (Input.GetMouseButtonDown(1))
        {
            aimDownSights();
        }

        //cylcleBolt
        if(Input.GetKey(KeyCode.LeftShift) && Input.GetKeyDown(KeyCode.T))
        {
            cycleBolt();
        }

        //Bolt Release
        if (Input.GetKeyDown(KeyCode.T))
        {
            releaseBolt();
        }

        //Reload
        if (Input.GetKeyDown(KeyCode.R))
        {
            if (GetComponent<Animator>().GetBool("magOut"))
            {
                reload(-1);
            }
            GUI.GetComponent<UI_Manager>().updateClips(clipCount, clips, clipSize);
        }

        //////////////////////////////// Mag Selection Stand-in
        if (Input.GetKeyDown(KeyCode.Alpha1) && clips[0] != 0 && GetComponent<Animator>().GetBool("magOut"))
        {
            reload(0);

        }
        else if (Input.GetKeyDown(KeyCode.Alpha2) && clips[1] != 0 && GetComponent<Animator>().GetBool("magOut"))
        {
            reload(1);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3) && clips[2] != 0 && GetComponent<Animator>().GetBool("magOut"))
        {
            reload(2);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha4) && clips[3] != 0 && GetComponent<Animator>().GetBool("magOut"))
        {
            reload(3);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha5) && clips[4] != 0 && GetComponent<Animator>().GetBool("magOut"))
        {
            reload(4);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha6) && clips[5] != 0 && GetComponent<Animator>().GetBool("magOut"))
        {
            reload(5);
        }

        waitForBoltCycle -= Time.deltaTime;
    }
    
    void aimDownSights()
    {
        if (!isAimming)
        {
            playerCamera.transform.position = aimCamPosition.transform.position;
            isAimming = true;
        }else
        {
            playerCamera.transform.position = cameraReturnPosition.transform.position;
            isAimming = false;
        }
    }

    void fireGun()
    {
        //Variables needed in order to maintain pre-fabs
        GameObject holdSound;
        GameObject holdMuzzleFlash;

        if(ammoInGun == 0)
        {
            canFire = false;
            boltLocked = true;
            chambered = false;
        }
        if (canFire && waitTilNextFire <= 0 && chambered && GetComponent<Animator>().GetBool("magIn"))
        {
            if (bullet != null)
            {
                Instantiate(bullet, bulletSpawn.transform.position, bulletSpawn.transform.rotation);
            }

            if (bulletSound)
            {
                holdSound = (GameObject) Instantiate(bulletSound, bulletSpawn.transform.position, bulletSpawn.transform.rotation);
                holdSound.transform.parent = transform;
            }
            if (muzzleFlash)
            {
                holdMuzzleFlash = (GameObject)Instantiate(muzzleFlash, bulletSpawn.transform.position, bulletSpawn.transform.rotation);
                holdMuzzleFlash.transform.parent = transform;
            }

            //Muzzle Rise
            GetComponent<MouseAimScript>().xRotation -= recoilRatio * 10;

            waitTilNextFire = 1;
            ammoInGun--;
            if(clips[currentClip] > 0)
            {
                clips[currentClip]--;
            }
        }
        waitTilNextFire -= Time.deltaTime * fireRate;

        //currentRecoilPosZ = Mathf.SmoothDamp(currentRecoilPosZ, 0, ref currentRecoilPosZVel, recoilRecoverTime);
    }
    
    void reload(int clipSelection)
    {
        if(clipCount > 0 && GetComponent<Animator>().GetBool("magOut"))
        {

            if (clipSelection == -1)
            {
                if(clips[currentClip] == 0)
                {
                    for (int i = currentClip; i < clipCount; i++)
                    {
                        clips[i] = clips[i + 1];
                        Debug.Log("Clip " + i + ": " + clips[i]);
                    }
                    clipCount--;
                    ammoInGun += clips[currentClip];
                    Debug.Log("Mag Empty, Dropped It");
                }
                Debug.Log("Clip Inserted, ClipAmmo: " + clips[currentClip] + "\nAmmo in Gun: " + ammoInGun);
            }
            else
            {
                currentClip = clipSelection;

                if (chambered)
                {
                    ammoInGun = 1;
                    ammoInGun += clips[currentClip];
                }
                else
                {
                    ammoInGun = clips[currentClip];
                }
                
                Debug.Log("Clip Selected: " + clipSelection);
            }
            /*if (Input.GetKeyDown(KeyCode.Alpha1) && clips[1] != 0)
            {
                currentClip = 0;

            }
            else if (Input.GetKeyDown(KeyCode.Alpha2) && clips[2] != 0)
            {
                currentClip = 1;

            }
            else if (Input.GetKeyDown(KeyCode.Alpha3) && clips[3] != 0)
            {
                currentClip = 2;

            }
            else if (Input.GetKeyDown(KeyCode.Alpha4) && clips[4] != 0)
            {
                currentClip = 3;

            }
            else if (Input.GetKeyDown(KeyCode.Alpha5) && clips[5] != 0)
            {
                currentClip = 4;

            }
            else if (Input.GetKeyDown(KeyCode.Alpha6) && clips[6] != 0)
            {
                currentClip = 5;

            }*/
        }
    }

    void cycleBolt()
    {
        if(waitForBoltCycle <= 0)
        {
            if (ammoInGun > 0 && chambered)
            {
                ammoInGun--;
                clips[currentClip]--;
                Debug.Log("Cycled Bolt, Ammo: " + ammoInGun);
            }
            else if (!chambered)
            {
                if(ammoInGun == 0)
                {
                    boltLocked = false;
                }
                else
                {
                    canFire = true;
                    chambered = true;
                    boltLocked = false;
                    Debug.Log("bolt unlocked, can fire");
                }
            }
            
            waitForBoltCycle = 2.2f;
        }
    }

    void releaseBolt()
    {
        if(boltLocked && clips[currentClip] > 0)
        {
            canFire = true;
            boltLocked = false;
            chambered = true;
            clips[currentClip]--;
            Debug.Log("bolt unlocked, can fire");
        }
    }
}
