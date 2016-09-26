using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class StartButton : MonoBehaviour {

    [SerializeField]
    IngredientsRoster ir;

	// Use this for initialization
	void Start () {
        StartCoroutine(Flicker());
	}
	
	// Update is called once per frame
	void Update () {

        if (Input.GetButtonDown("StartButton"))
        {
            ir.startGame();
            Destroy(this.gameObject);
        }

	}

    public IEnumerator Flicker()
    {
        this.GetComponent<Text>().enabled = !this.GetComponent<Text>().enabled;
        yield return new WaitForSeconds(1.0f);
        StartCoroutine(Flicker());
        yield return null;
    }
}
