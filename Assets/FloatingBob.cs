using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class FloatingBob : MonoBehaviour {
    private float y0;
    [SerializeField]
    private float speed;
    [SerializeField]
    private float amplitude;


	// Use this for initialization
	void Start () {

        y0 = transform.position.y;
	}
	
	// Update is called once per frame
	void Update () {

        transform.position =new Vector3(transform.position.x, y0 + amplitude * Mathf.Sin(speed *Time.time), transform.position.z);

    }
}
