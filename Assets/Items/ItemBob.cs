using UnityEngine;
using System.Collections;

public class ItemBob : MonoBehaviour {

    public float y0;
    [SerializeField]
    public float speed;
    [SerializeField]
    public float amplitude;


    // Use this for initialization
    void Start()
    {

       y0 = transform.localPosition.y;
    }

    // Update is called once per frame
    void Update()
    {
        transform.localPosition = new Vector3(transform.localPosition.x, y0 + amplitude * Mathf.Sin(speed * Time.time), transform.localPosition.z);
    }
}
