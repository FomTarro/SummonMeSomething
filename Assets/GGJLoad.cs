using UnityEngine;
using System.Collections;

public class GGJLoad : MonoBehaviour {

	// Use this for initialization
	void Start () {
        StartCoroutine(Load());
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public IEnumerator Load()
    {
        yield return new WaitForSeconds(2.0f);
        FindObjectOfType<IngredientsRoster>().enabled = true;
        FindObjectOfType<StartButton>().enabled = true;
        yield return StartCoroutine(FadeTo(this.GetComponent<CanvasGroup>(), 0f, 0.5f));
        this.gameObject.SetActive(false);
    }

    IEnumerator FadeTo(CanvasGroup cg, float aValue, float aTime)
    {
        float alpha = cg.alpha;
        for (float t = 0.0f; t < 1.0f; t += Time.deltaTime / aTime)
        {
            float newAlpha = Mathf.Lerp(alpha, aValue, t);
            cg.alpha = newAlpha;
            yield return null;
        }
        cg.alpha = aValue;
    }
}
