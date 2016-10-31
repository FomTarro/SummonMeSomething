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

    int rotationSpeed = 0;

	// Use this for initialization
	void Start () {
        rotationSpeed = Random.Range(85, 103);
        foreach(Transform t in transform)
        {
            t.gameObject.AddComponent<ItemBob>();
            t.GetComponent<ItemBob>().amplitude = 0.5f;
            t.GetComponent<ItemBob>().speed = Random.Range(1f, 2f);
        }
	}

    void Update()
    {
        if (!pickedUp)
        {
            float z = transform.rotation.eulerAngles.z + Time.deltaTime;
            transform.Rotate(new Vector3(0, (Time.deltaTime) * rotationSpeed, 0));

        }
        if(GetComponentInChildren<ItemBob>() != null)
            GetComponentInChildren<ItemBob>().enabled = !pickedUp;
    }
	
}
