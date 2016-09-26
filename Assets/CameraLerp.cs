using UnityEngine;
using System.Collections;

public class CameraLerp : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {
        //StartCoroutine(posLerp(new Vector3(0, 30, -40), 30f, 5f));
    }

    // Update is called once per frame
    void Update()
    {

    }

    public IEnumerator posLerp(Vector3 posValue, float rotValue, float aTime)
    {
        Vector3 initialPos = transform.position;
        float initialRot = transform.rotation.eulerAngles.y;

        for (float t = 0.0f; t < 1.0f; t += Time.deltaTime / aTime)
        {
            transform.position = Vector3.Lerp(initialPos, posValue, t);

            transform.rotation = Quaternion.AngleAxis(Mathf.Lerp(initialRot, rotValue, t), Vector3.right);
            yield return null;
        }
        transform.position = posValue;
        transform.rotation = Quaternion.AngleAxis(rotValue, Vector3.right);
    }
}
