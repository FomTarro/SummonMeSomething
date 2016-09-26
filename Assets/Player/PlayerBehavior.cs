using UnityEngine;
using System.Collections;

public class PlayerBehavior : MonoBehaviour {

    public int playerIndex = 0;
    private Rigidbody rb;
    [SerializeField]
    private float speed = 20.0f;
    private Vector3 movement;

    private Item heldItem;
    [SerializeField]
    PickupRadius pickupRadius;

    [SerializeField]
    AudioSource throwSound;

    [SerializeField]
    AudioSource footstepSound;

    Animator anim;

    // Use this for initialization
    void Start () {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
    }
	
	// Update is called once per frame
	void Update () {

        float x = Input.GetAxis("P"+playerIndex+"_Horizontal");
        float y = Input.GetAxis("P" + playerIndex + "_Vertical");
        float aim_angle = 0.0f;

        // USED TO CHECK OUTPUT
        //Debug.Log(" horz: " + x + "   vert: " + y);

        // CANCEL ALL INPUT BELOW THIS FLOAT
        float R_analog_threshold = 0.1f;

        if (Mathf.Abs(x) < R_analog_threshold) { x = 0.0f; }

        if (Mathf.Abs(y) < R_analog_threshold) { y = 0.0f; }

        // CALCULATE ANGLE AND ROTATE
        if (x != 0.0f || y != 0.0f)
        {
            if(!footstepSound.isPlaying)
                footstepSound.Play();
            aim_angle = (-1* Mathf.Atan2(y, x) * Mathf.Rad2Deg);

            // ANGLE GUN
            this.transform.rotation = Quaternion.AngleAxis(aim_angle, Vector3.up);

           movement = new Vector3(x, 0.0f, y);
           rb.velocity = speed*movement;

        }
        else
        {
            footstepSound.Stop();
            rb.velocity = Vector3.zero;
        }

        if (Input.GetButtonDown("P" + playerIndex + "_AButton"))
        {
            ThrowHeldItem();
        }

        anim.SetFloat("MoveSpeed", Mathf.Abs(rb.velocity.x) + Mathf.Abs(rb.velocity.z));
    }

    public IEnumerator SetHeldItem(Item i)
    {
        heldItem = i;
        anim.SetBool("HasItem", heldItem != null);
        if (heldItem == null)
        {
            yield return new WaitForSeconds(0.5f);
            pickupRadius.gameObject.SetActive(true);
        }
        else
        {
            pickupRadius.gameObject.SetActive(false);
            i.IndexOfLastHolder = playerIndex;
            i.transform.position = this.transform.position + 2.5f*transform.right + transform.up;
            i.GetComponent<Rigidbody>().isKinematic = true;
            i.transform.SetParent(this.transform);
            i.pickedUp = true;
        }
        
        
        yield return null;
    }

    public void ThrowHeldItem()
    {
        if (heldItem != null)
        {
            heldItem.pickedUp = false;
            throwSound.Play();
            Rigidbody itemRB = heldItem.GetComponent<Rigidbody>();
            heldItem.transform.SetParent(null);
            StartCoroutine(SetHeldItem(null));
            itemRB.isKinematic = false;

            Debug.Log(100 * transform.forward);
            itemRB.velocity = (15 * (transform.right + 2.5f*transform.up) + speed * movement);
        }
    }


}