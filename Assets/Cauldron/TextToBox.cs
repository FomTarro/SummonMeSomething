using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class TextToBox : MonoBehaviour {

    [Tooltip("Color for positive text effect (praise, objective highlighting)")]
    [SerializeField]
    private Color goodColor;
    [Tooltip("Color for negative text effect (yelling, etc)")]
    [SerializeField]
    private Color badColor;

    [Tooltip("Time spent on each character")]
    [SerializeField]
    private float timePerCharacter = 0.07f;
    [SerializeField]
    [Tooltip("Time spent on each punctuation mark")]
    private float timePerPause = 0.1f;

    [Tooltip("The name of the input that, when held, should skip through dialog")]
    [SerializeField]
    private string skipButton;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public string ColorToHex(Color32 color)
    {
        string hex = "#" + color.r.ToString("X2") + color.g.ToString("X2") + color.b.ToString("X2");
        return hex;
    }

    // --- Text Display Functions ---

    // Feed this the string you want to be printed out, the text object to print it to, and if applicable, the AudioSource for the voice per letter
    public IEnumerator TypeText(string toParse, Text textBox, AudioSource voice)
    {
        bool italics = false;
        bool bold = false;
        bool bad = false;
        bool good = false;

        bool voiced = (voice != null);
        float basePitch = 1.0f;

        if (voiced)
            basePitch = voice.pitch;

        string toFill = "";
        char[] letterArray = toParse.ToCharArray();

        bool ignore = false; //for ignoring special characters that toggle styles

        foreach (char nextletter in letterArray)
        {

            switch (nextletter)
            {
                // Cases for special text effects. feel free to add your own as new cases!
                case '@': // For example, text like "Go find the @Objective@!" will wrap the word 'Objective' in the defined good color. 
                    ignore = true; //make sure this character isn't printed by ignoring it
                    good = !good; 
                    break;
                case '$':
                    ignore = true; //make sure this character isn't printed by ignoring it
                    bad = !bad;
                    break;
                case '*':
                    ignore = true; //make sure this character isn't printed by ignoring it
                    bold = !bold; //toggle bold styling
                    break;
                case '/':
                    ignore = true; //make sure this character isn't printed by ignoring it
                    italics = !italics; //toggle italic styling
                    break;
            }


            string letter = nextletter.ToString();

            if (!ignore)
            {

                if (bold)
                {

                    letter = "<b>" + letter + "</b>";

                }
                if (italics)
                {

                    letter = "<i>" + letter + "</i>";

                }
                if (good)
                {

                    letter = "<color=" + ColorToHex(goodColor) + ">" + letter + "</color>";

                }
                if (bad)
                {

                    letter = "<color=" + ColorToHex(badColor) + ">" + letter + "</color>";

                }
                toFill += letter;
                textBox.text = toFill;

            }
            //make sure the next character isn't ignored
            ignore = false;
            if (letter.Equals(".") || letter.Equals(",") || letter.Equals(";") || letter.Equals("!"))
                yield return new WaitForSeconds(timePerPause);
            else
            {
                if (voiced && !voice.isPlaying)
                {
                    voice.pitch = basePitch + UnityEngine.Random.Range(-0.2f, 0.2f);
                    voice.Play();
                }
                if (Input.GetButton(skipButton))
                {
                    yield return new WaitForSeconds(timePerCharacter*0.1f);
                }
                else
                {
                    yield return new WaitForSeconds(timePerCharacter);
                }
            }
            if (voiced)
                voice.pitch = basePitch;
        }
    }

}
