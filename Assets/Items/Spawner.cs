using UnityEngine;
using System.Collections;

public class Spawner : MonoBehaviour {

    [SerializeField]
    private Item ItemToSpawn;

	// Use this for initialization
	void Start () {
        this.enabled = false;
	}
	
	// Update is called once per frame
	void Update () {
	if(GameObject.FindGameObjectsWithTag(ItemToSpawn.tag).Length <= 1)
        {
            GetComponent<ParticleSystem>().Emit(10);
            GetComponent<ParticleSystem>().startLifetime = GetComponent<ParticleSystem>().startLifetime;
            Debug.Log("SPAWNING");
            this.transform.eulerAngles = new Vector3(0, Random.Range(0f, 360f), 0);
            GameObject go = (GameObject)Instantiate(ItemToSpawn.gameObject, this.transform.position, Quaternion.identity);
            go.GetComponent<Rigidbody>().velocity = (UnityEngine.Random.Range(25, 45) * (transform.right +  0.25f* transform.up));
            
        }

	}
}
