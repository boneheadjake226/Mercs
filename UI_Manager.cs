using UnityEngine;
using System.Collections;

public class UI_Manager : MonoBehaviour {

    public Canvas GUI;
    [Space(2, order = 1)]

    [Header("Ammo Display", order = 2)]
    public GameObject AmmoCounter;

    public GameObject ClipIconPrefab;
    public Texture FullMagIcon;
    public Texture HalfMagIcon;
    public Texture LowMagIcon;

    public GameObject ClipIconStartLocation;
    GameObject[] ClipIcons = null;
    int clipIconWidth = 100;
    

    void Start()
    {
        updateAmmo(0, 0);
    }


    public void updateAmmo(int ammoInGun, int clipSize)
    {
        AmmoCounter.GetComponent<UnityEngine.UI.Text>().text = "Ammo: " + ammoInGun + " / " + clipSize;
    }

    public void updateClips(int numClips, int[] clips, int clipSize)
    {
        if(ClipIcons != null)
        {
            //Clear Screen
            foreach (GameObject icon in ClipIcons)
            {
                Destroy(icon);
            }
        }

        ClipIcons = new GameObject[numClips];
        for (int i = 0; i < numClips; i++)
        {
            if(clips[i] != 0) { 
                GameObject clipIcon;
                clipIcon = Instantiate(ClipIconPrefab) as GameObject;
                clipIcon.transform.SetParent(GUI.transform, false);
                clipIcon.transform.position = ClipIconStartLocation.transform.position;

                //Set positon of mag on screen
                clipIcon.transform.position = new Vector3(clipIcon.transform.position.x - ((numClips - (i+1)) * clipIconWidth), clipIcon.transform.position.y, clipIcon.transform.position.z);
            
                //Set mag capacity icon
                if(clips[i] > clipSize / 2)
                {
                    clipIcon.transform.GetChild(0).GetComponent<UnityEngine.UI.RawImage>().texture = FullMagIcon;
                }
                else if(clips[i] <= clipSize / 2 && clips[i] > clipSize / 4)
                {
                    clipIcon.transform.GetChild(0).GetComponent<UnityEngine.UI.RawImage>().texture = HalfMagIcon;
                }
                else
                {
                    clipIcon.transform.GetChild(0).GetComponent<UnityEngine.UI.RawImage>().texture = LowMagIcon;
                }

                //Offset position of mag based on number of mags
                clipIcon.transform.GetChild(1).GetComponent<UnityEngine.UI.Text>().text = (i+1).ToString(); //Set mag number text 

                ClipIcons[i] = clipIcon;
            }

        }
    }
}
