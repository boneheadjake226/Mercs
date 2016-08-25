using UnityEngine;
using System.Collections;

public class animationController : MonoBehaviour {

    private Animator anim;
    private bool inIdle;
    /*public AnimationClip Reload_MagOut;
    public AnimationClip Reload_MagIdle;
    public AnimationClip Reload_MagIn;
    public AnimationClip Idle;
    public AnimationClip ChargeHandle;
    public AnimationClip Bolt_Release;

    public Rigidbody rb;*/

    void Start()
    {
        anim = GetComponent<Animator>();
        anim.SetBool("magOut", false);
        anim.SetBool("magIn", true);
        anim.SetBool("chargeHandle", false);
        anim.SetBool("boltRelease", false);
    }
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.R) && anim.GetBool("magIn"))
        {
            Debug.Log("Pulling Mag Out");
            anim.SetBool("magOut", true);
            anim.SetBool("magIn", false);
            Debug.Log("magOut after transition" + anim.GetBool("magOut")
                        + "\nmagIn after transition" + anim.GetBool("magIn"));
        }
        else if(Input.GetKeyDown(KeyCode.R) && anim.GetBool("magOut"))
        {
            Debug.Log("Putting Mag In");
            anim.SetBool("magIn", true);
            anim.SetBool("magOut", false);
        }

        if(Input.GetKey(KeyCode.LeftShift) && Input.GetKeyDown(KeyCode.T))
        {
            Debug.Log("Charging Handle");
            anim.SetBool("chargeHandle", true);
        }
        else
        {
            anim.SetBool("chargeHandle", false);
        }

        if (!Input.GetKey(KeyCode.LeftShift) && Input.GetKeyDown(KeyCode.T) && !anim.GetBool("chargeHandle"))
        {
            Debug.Log("Releasing Bolt");
            anim.SetBool("boltRelease", true);
        }
        else
        {
            anim.SetBool("boltRelease", false);

        }
    }
}
