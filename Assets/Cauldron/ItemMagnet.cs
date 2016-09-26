using UnityEngine;
using System.Collections;

public class ItemMagnet : MonoBehaviour {

    [SerializeField]
    private LayerMask magnetLayer;
    [SerializeField]
    private float fieldRadius;
    [SerializeField]
    private float fieldForce;

    public int modifier = -1;
 
    void Update()
    {

        Collider[] colliders;
        Rigidbody rigidbody;

        colliders = Physics.OverlapSphere(transform.position, fieldRadius, magnetLayer);
 
     foreach (Collider collider in colliders)
        {

            rigidbody = collider.GetComponent<Rigidbody>();

            if (rigidbody == null)
            {
                continue;
            }
            rigidbody.AddExplosionForce(fieldForce * modifier, transform.position, fieldRadius);
        }
    }
}
