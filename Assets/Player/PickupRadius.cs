using UnityEngine;
using System.Collections;

public class PickupRadius : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<Item>() != null)
        {
            Item itemInThePot = other.gameObject.GetComponent<Item>();
            Debug.Log(itemInThePot.IngredientType.ToString());
            SendMessageUpwards("SetHeldItem", itemInThePot);
        }
    }
}
