using UnityEngine;
using System.Collections;

public class Item : MonoBehaviour {

    [SerializeField]
    private Ingredient ingredientType;
    public Ingredient IngredientType
    {
        get { return ingredientType; }
    }

    [SerializeField]
    private int indexOfLastHolder;
    public int IndexOfLastHolder
    {
        get { return indexOfLastHolder; }
        set { indexOfLastHolder = value; }
    }

    public bool pickedUp = false;


	// Use this for initialization
	void Start () {
	
	}

    void Update()
    {
        if (!pickedUp)
        {
            float z = transform.rotation.eulerAngles.z + Time.deltaTime;
            transform.Rotate(new Vector3(0, (Time.deltaTime) * 100, 0));
        }
    }
	
}
