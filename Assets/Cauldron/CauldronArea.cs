using UnityEngine;
using System.Collections;

public class CauldronArea : MonoBehaviour {

    [SerializeField]
    private IngredientsRoster cauldronComputer;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.GetComponent<Item>() != null)
        {
            Item itemInThePot = other.gameObject.GetComponent<Item>();
            Debug.Log(itemInThePot.IngredientType.ToString());
            cauldronComputer.TossInCauldron(itemInThePot);
        }
    }

}
