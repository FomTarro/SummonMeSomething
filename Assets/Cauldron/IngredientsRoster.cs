using UnityEngine;
using System.Collections.Generic;
using System;
using UnityEngine.UI;
using System.Collections;

public class IngredientsRoster : MonoBehaviour {

    [System.Serializable]
    public class IngredientAdjectiveEntry
    {
        public Ingredient ingredient;
        public Adjective adjective;

        public IngredientAdjectiveEntry(Ingredient i, Adjective a)
        {
            ingredient = i;
            adjective = a;
        }
    }


    [Header("Item Roster")]
    [SerializeField]
    private List<IngredientAdjectiveEntry> rosterToShuffle;

    // dictionary to look up based on ingredients 
    private Dictionary<Ingredient, Adjective> shuffledRoster = new Dictionary<Ingredient, Adjective>();

    private List<Adjective> possibleDescriptors = new List<Adjective>();
    private List<Adjective> chosenDescriptors = new List<Adjective>();
    private List<Adjective> chosenDetractors = new List<Adjective>();

    [SerializeField]
    private List<List<Ingredient>> inThePot = new List<List<Ingredient>>();
    [Header("UI Hookups")]
    [SerializeField]
    private List<GameObject> P1_Locations = new List<GameObject>();
    [SerializeField]
    private List<GameObject> P2_Locations = new List<GameObject>();

    [SerializeField]
    private List<Text> scoreLists = new List<Text>();
    private int round = 0;

    public int winScore;

    [SerializeField]
    private Color32 goodColor;
    [SerializeField]
    private Color32 badColor;

    [SerializeField]
    private Text textField;
    [SerializeField]
    private CanvasGroup backPanel;
    [SerializeField]
    private CanvasGroup gameplayUI;
    [SerializeField]
    private AudioSource textVoice;
    [SerializeField]
    private AudioSource textLaugh;
    [SerializeField]
    private AudioSource splash;

    [SerializeField]
    private GameObject cauldronShader;

    [SerializeField]
    private GameObject startGate;

    [SerializeField]
    private Sprite thinking, laughing, angry;
    [SerializeField]
    private Image teacher;

    [SerializeField]
    private CanvasGroup mainMenuPanel;

    private string demand = "";


    private bool inIntro = false;

    private int skipPresses = 0;

    private float skipTime = 0f;

    // Use this for initialization
    void Start () {
        backPanel.alpha = 0.0f;
    }
	
	// Update is called once per frame
	void Update () {
        if (inIntro)
        {
            if(Input.GetButtonDown("P0_AButton") || Input.GetButtonDown("P0_AButton"))
            {
                if(skipTime < 0.5f)
                {
                    skipPresses++;
                }
                skipTime = 0;
            }
            if (skipPresses > 6)
            {
                inIntro = false;
                CheckSkipIntro();
            }

            skipTime += Time.deltaTime;
        }
	}

    public void startGame()
    {
        StartCoroutine(FadeTo(mainMenuPanel, 0.0f, 0.5f));
        inThePot.Add(new List<Ingredient>());
        inThePot.Add(new List<Ingredient>());
        Shuffle2();
        StartCoroutine(Tutorial());

    }


    public void Shuffle2()
    {
        rosterToShuffle.Clear();
        shuffledRoster.Clear();

        List<Adjective> possibleDescriptors = new List<Adjective>();

        foreach (Adjective a in Enum.GetValues(typeof(Adjective)))
            possibleDescriptors.Add(a);

        foreach (Ingredient i in Enum.GetValues(typeof(Ingredient)))
        {
            Adjective randomAdjective = possibleDescriptors[UnityEngine.Random.Range(0, possibleDescriptors.Count)];
            rosterToShuffle.Add(new IngredientAdjectiveEntry(i, randomAdjective));
            possibleDescriptors.Remove(randomAdjective);
        }

        foreach (IngredientAdjectiveEntry iae in rosterToShuffle)
        {
            shuffledRoster.Add(iae.ingredient, iae.adjective);
        }

    }

    public void TossInCauldron(Item i)
    {
        Ingredient ingredient = i.IngredientType;
        splash.Play();
        int playerIndex = i.IndexOfLastHolder;
        if (!inThePot[playerIndex].Contains(ingredient) && inThePot[playerIndex].Count < 3)
        {
            if(playerIndex == 0)
            {
                i.transform.position = P1_Locations[inThePot[playerIndex].Count].transform.position;
                i.transform.SetParent(P1_Locations[inThePot[playerIndex].Count].transform);
            }
            else
            {
                i.transform.position = P2_Locations[inThePot[playerIndex].Count].transform.position;
                i.transform.SetParent(P2_Locations[inThePot[playerIndex].Count].transform);

            }
           
            i.GetComponent<Rigidbody>().isKinematic = true;
            inThePot[playerIndex].Add(ingredient);
            Debug.Log("Player " + playerIndex + " put " + ingredient.ToString() + " in the cauldron!");
        }
        else
        {
            Destroy(i.gameObject);
        }

        if(inThePot[playerIndex].Count == 3)
        {
            StartCoroutine(Outcome(inThePot[playerIndex], playerIndex));
            inThePot[playerIndex].Clear();
        }
    }

    public IEnumerator Tutorial()
    {
        inIntro = true;
        textField.color = Color.black;
        StartCoroutine(FadeTo(backPanel, 1f, 0.5f));
        string tutorialText = "Good evening, witches-in-training!\nTonight's exam is that of the Summoner's Ritual!";
        yield return StartCoroutine(TypeText(tutorialText, textField, textVoice));
        yield return new WaitForSeconds(0.25f);
        tutorialText = "Here's how it works.\nI'm going to ask you to summon me something!";
        yield return StartCoroutine(TypeText(tutorialText, textField, textVoice));
        yield return new WaitForSeconds(0.25f);
        tutorialText = "That something will need to have the traits I @WANT@,\nbut not the ones I $DON'T WANT$.";
        yield return StartCoroutine(TypeText(tutorialText, textField, textVoice));
        yield return new WaitForSeconds(0.25f);
        tutorialText = "Each type of reagent laying around the exam room\ncorresponds to a different trait!";
        yield return StartCoroutine(TypeText(tutorialText, textField, textVoice));
        yield return new WaitForSeconds(0.25f);
        tutorialText = "Just toss them in the cauldron with the 'CTRL Key'!\nEvery three is a new summon.\nFirst witch to "+winScore+" worthy summons passes the exam.";
        yield return StartCoroutine(TypeText(tutorialText, textField, textVoice));
        yield return new WaitForSeconds(0.25f);
        tutorialText = "But here's the thing... Every exam session,\nI magically shuffle the bond\nbetween the reagent and its trait!+";
        yield return StartCoroutine(TypeText(tutorialText, textField, textVoice));
        yield return new WaitForSeconds(0.25f);
        tutorialText = "I'll tell you how your summon stacks up,\nbut I'll tell you nothing about which\nreagents caused which trait.";
        yield return StartCoroutine(TypeText(tutorialText, textField, textVoice));
        yield return new WaitForSeconds(0.25f);

        // Here's what to delegate out to a seperate coroutine
        yield return StartCoroutine(EndOfIntro());
    }

    private IEnumerator EndOfIntro()
    {
        teacher.sprite = thinking;
        string tutorialText = "\nLet's get started then, shall we?";
        yield return StartCoroutine(TypeText(tutorialText, textField, textVoice));
        yield return new WaitForSeconds(0.25f);
        StartCoroutine(Camera.main.GetComponent<CameraLerp>().posLerp(new Vector3(0, 35, -40), 45f, 5f));
        startGate.SetActive(false);
        StartCoroutine(FadeTo(gameplayUI, 1.0f, 0.5f));
        foreach (Spawner s in FindObjectsOfType<Spawner>())
            s.enabled = true;
        yield return StartCoroutine(GenerateDemand(1, 0));
    }

    private IEnumerator SkipIntro()
    {
        textLaugh.Stop();
        textVoice.Stop();
        yield return new WaitForSeconds(0.25f);
        teacher.sprite = angry;
        string tutorialText = "\n...Oh? Think you already know how this works?";
        yield return StartCoroutine(TypeText(tutorialText, textField, textVoice));
        yield return new WaitForSeconds(0.25f);
        yield return StartCoroutine(EndOfIntro());
    }

    private void CheckSkipIntro()
    {
        StopAllCoroutines();
        StartCoroutine(SkipIntro());
    }

    public void testOutcome()
    {
        List<Ingredient> ingredients = new List<Ingredient>();
        StartCoroutine(Outcome(ingredients, 0));
    }

    public IEnumerator Winner()
    {
        yield return StartCoroutine(TypeText("And that's the exam!+", textField, textVoice));
        yield return StartCoroutine(TypeText("Well summoned by all, but there can be only one winner!", textField, textVoice));
        yield return StartCoroutine(TypeText("Thanks for playing this Global Game Jam 2016 entry!\nArt by Dillon DeSimone and John Guerra\nCode by Tom Farro", textField, textVoice));
        yield return new WaitForSeconds(10.0f);
        Reset();
    }

    public void Reset()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(0);
    }

    public IEnumerator Outcome(List<Ingredient> ingredients, int playerIndex)
    {
        //List<Ingredient> shuffledIngredients = new List<Ingredient>();
        List<Ingredient> givenIngredients = new List<Ingredient>();
        List<Ingredient> goodIngredients = new List<Ingredient>();
        List<Ingredient> badIngredients = new List<Ingredient>();
        List<Ingredient> neutralIngredients = new List<Ingredient>();

        int positives = 0;
        int negatives = 0;

        textField.color = Color.black;
        StartCoroutine(FadeTo(backPanel, 1f, 0.5f));
        cauldronShader.GetComponent<Renderer>().material.SetFloat("_BulgeScale", 1.4f);

        FindObjectOfType<ItemMagnet>().modifier = 1;
        foreach (Ingredient i in ingredients)
            givenIngredients.Add(i);

        string response = "You summoned me something...\n";

        foreach(Ingredient i in givenIngredients)
        {
            if (chosenDescriptors.Contains(shuffledRoster[i]))
            {
                goodIngredients.Add(i);
            }
            else if (chosenDetractors.Contains(shuffledRoster[i]))
            {
                badIngredients.Add(i);
            }
            else
            {
                neutralIngredients.Add(i);
            }
        }


        List<Ingredient> goodClone = new List<Ingredient>();
        List<Ingredient> shuffledGood = new List<Ingredient>();

        foreach (Ingredient i in goodIngredients)
            goodClone.Add(i);

        foreach (Ingredient i in goodIngredients)
        {
            Ingredient randomIngredient = goodClone[UnityEngine.Random.Range(0, goodClone.Count)];
            shuffledGood.Add(randomIngredient);
            goodClone.Remove(randomIngredient);
        }

        List<Ingredient> neutralClone = new List<Ingredient>();
        List<Ingredient> shuffledNeutral= new List<Ingredient>();

        foreach (Ingredient i in neutralIngredients)
            neutralClone.Add(i);


        foreach (Ingredient i in neutralIngredients)
        {
            Ingredient randomIngredient = neutralClone[UnityEngine.Random.Range(0, neutralClone.Count)];
            shuffledNeutral.Add(randomIngredient);
            neutralClone.Remove(randomIngredient);
        }

        List<Ingredient> badClone = new List<Ingredient>();
        List<Ingredient> shuffledBad = new List<Ingredient>();

        foreach (Ingredient i in badIngredients)
            badClone.Add(i);


        foreach (Ingredient i in badIngredients)
        {
            Ingredient randomIngredient = badClone[UnityEngine.Random.Range(0, badClone.Count)];
            shuffledBad.Add(randomIngredient);
            badClone.Remove(randomIngredient);
        }


        Adjective currentIngredientDescriptor;

        // The Good
        if (shuffledGood.Count > 1)
        {
            currentIngredientDescriptor = shuffledRoster[shuffledGood[0]];
            positives++;
            response += "@" + currentIngredientDescriptor.ToString().ToUpper() + "@";
            for (int j = 1; j < shuffledGood.Count - 1; j++)
            {
                currentIngredientDescriptor = shuffledRoster[shuffledGood[j]];
                positives++;
                response += ", " + "@" + currentIngredientDescriptor.ToString().ToUpper() + "@";
            }
            currentIngredientDescriptor = shuffledRoster[shuffledGood[shuffledGood.Count - 1]];
            positives++;
            if (shuffledNeutral.Count == 0 && shuffledBad. Count == 0)
                response += " and " + "@" + currentIngredientDescriptor.ToString().ToUpper() + "@";
            else
                response += ", " + "@" + currentIngredientDescriptor.ToString().ToUpper() + "@";
        }
        else if (shuffledGood.Count == 1)
        {
            currentIngredientDescriptor = shuffledRoster[shuffledGood[0]];
            positives++;
            response += "@" + currentIngredientDescriptor.ToString().ToUpper() + "@";
        }

        if (shuffledGood.Count > 0 && shuffledNeutral.Count > 1 || shuffledGood.Count > 0 && shuffledNeutral.Count > 0 && shuffledBad.Count > 0)
            response += ", ";
        else if ((shuffledNeutral.Count == 1 && shuffledBad.Count == 0) || (shuffledNeutral.Count == 0 && shuffledBad.Count == 1))
            response += " and ";

        // The Neutral
        if (shuffledNeutral.Count > 1)
        {
            currentIngredientDescriptor = shuffledRoster[shuffledNeutral[0]];
            response += currentIngredientDescriptor.ToString().ToUpper();
            for (int j = 1; j < shuffledNeutral.Count - 1; j++)
            {
                currentIngredientDescriptor = shuffledRoster[shuffledNeutral[j]];
                response += ", " + currentIngredientDescriptor.ToString().ToUpper();
            }
            currentIngredientDescriptor = shuffledRoster[shuffledNeutral[shuffledNeutral.Count - 1]];
            if(shuffledBad.Count  == 0)
                response += " and " + currentIngredientDescriptor.ToString().ToUpper();
            else
                response += ", " + currentIngredientDescriptor.ToString().ToUpper();
        }
        else if (shuffledNeutral.Count == 1)
        {
            currentIngredientDescriptor = shuffledRoster[shuffledNeutral[0]];
            response +=  currentIngredientDescriptor.ToString().ToUpper();
        }

        if(shuffledNeutral.Count > 0 && shuffledBad.Count > 1)
        response += ", ";
        else if (shuffledNeutral.Count > 0 && shuffledBad.Count == 1)
        response += " and ";

        // The Bad
        if (shuffledBad.Count > 1)
        {
            currentIngredientDescriptor = shuffledRoster[shuffledBad[0]];
            negatives++;
            response += "$" + currentIngredientDescriptor.ToString().ToUpper() + "$";
            for (int j = 1; j < shuffledBad.Count - 1; j++)
            {
                currentIngredientDescriptor = shuffledRoster[shuffledBad[j]];
                negatives++;
                response += ", " + "$" + currentIngredientDescriptor.ToString().ToUpper() + "$";
            }
            currentIngredientDescriptor = shuffledRoster[shuffledBad[shuffledBad.Count - 1]];
            negatives++;
            response += " and " + "$" + currentIngredientDescriptor.ToString().ToUpper() + "$";
        }
        else if (shuffledBad.Count == 1)
        {
            currentIngredientDescriptor = shuffledRoster[shuffledBad[0]];
            negatives++;
            response += "$" + currentIngredientDescriptor.ToString().ToUpper() + "$";
        }

        response += "!";
        Debug.Log(response);
        yield return(StartCoroutine(TypeText(response, textField, textVoice)));
        yield return new WaitForSeconds(0.5f);
        if(playerIndex == 0)
        {
            foreach (GameObject go in P1_Locations)
            {
                Destroy(go.GetComponentInChildren<Item>().gameObject);
            }
        }
        else
        {
            foreach (GameObject go in P2_Locations)
            {
                Destroy(go.GetComponentInChildren<Item>().gameObject);
            }
        }
        if (positives == chosenDescriptors.Count && negatives == 0)
        {
            scoreLists[playerIndex].text = (int.Parse(scoreLists[playerIndex].text) + 1).ToString();
            yield return StartCoroutine(TypeText("Excellent! Marvelous!\nThat is exactly what I had in mind!", textField, textVoice));
            round++;
            yield return new WaitForSeconds(0.5f);
            int likes = Mathf.Clamp((round), 1, 2);
            int dislikes = Mathf.Clamp((round / 3), 0, 1);
            if (scoreLists[0].text.Equals(winScore.ToString()) || scoreLists[1].text.Equals(winScore.ToString()))
            {
                StartCoroutine(Winner());
            }
            else
            {
                StartCoroutine(GenerateDemand(likes, dislikes));
            }
        }
        else if (positives > 0 && negatives == 0)
        {
            yield return StartCoroutine(TypeText("It's not a bad start, but...\nTry again!", textField, textVoice));
            yield return new WaitForSeconds(0.5f);
            yield return (StartCoroutine(TypeText(demand, textField, textVoice)));
            textField.color = Color.white;
            StartCoroutine(FadeTo(backPanel, 0f, 0.5f));

        }
        else if (negatives > 0)
        {
            yield return StartCoroutine(TypeText("That isn't what I asked for at all!+\nPathetic! Try again!", textField, textVoice));
            yield return new WaitForSeconds(0.5f);
            yield return (StartCoroutine(TypeText(demand, textField, textVoice)));
            textField.color = Color.white;
            StartCoroutine(FadeTo(backPanel, 0f, 0.5f));

        }
        else if (positives == 0 && negatives == 0)
        {
            teacher.sprite = angry;
            yield return StartCoroutine(TypeText("That is... entirely unremarkable. \nSuch a shame.", textField, textVoice));
            yield return new WaitForSeconds(0.5f);
            teacher.sprite = thinking;
            yield return (StartCoroutine(TypeText(demand, textField, textVoice)));
            textField.color = Color.white;
            StartCoroutine(FadeTo(backPanel, 0f, 0.5f));

        }
        cauldronShader.GetComponent<Renderer>().material.SetFloat("_BulgeScale", -.25f);
        FindObjectOfType<ItemMagnet>().modifier = -1;
        yield return new WaitForSeconds(2.0f);

    }


    public void testDemands()
    {
        StartCoroutine(GenerateDemand(2, 1));
    }

    public IEnumerator GenerateDemand(int maxDescriptors, int maxDetractors)
    {
        textField.color = Color.black;
        StartCoroutine(FadeTo(backPanel, 1f, 0.5f));

        possibleDescriptors.Clear();
        chosenDescriptors.Clear();
        chosenDetractors.Clear();
        foreach (Adjective a in shuffledRoster.Values)
            possibleDescriptors.Add(a);

        for (int i = 0; i < maxDescriptors; i++)
        {
            Adjective randomAdjective = possibleDescriptors[UnityEngine.Random.Range(0, possibleDescriptors.Count)];
            chosenDescriptors.Add(randomAdjective);
            possibleDescriptors.Remove(randomAdjective);
        }

        for (int i = 0; i < maxDetractors; i++)
        {
            Adjective randomAdjective = possibleDescriptors[UnityEngine.Random.Range(0, possibleDescriptors.Count)];
            chosenDetractors.Add(randomAdjective);
            possibleDescriptors.Remove(randomAdjective);
        }

        demand = "Summon me something...\n";
        if (chosenDescriptors.Count > 1)
        {
            demand += "@" + chosenDescriptors[0].ToString().ToUpper() + "@";

            for (int j = 1; j < chosenDescriptors.Count - 1; j++)
            {
                demand += "," + "@" + chosenDescriptors[j].ToString().ToUpper() + "@";
            }

            demand += " and " + "@" + chosenDescriptors[chosenDescriptors.Count - 1].ToString().ToUpper() + "@";
        }
        else if (chosenDescriptors.Count == 1)
        {
            demand += "@" + chosenDescriptors[0].ToString().ToUpper() + "@";
        }


        if (chosenDetractors.Count > 1)
        {
            demand += ", but not ";

            demand += "$" + chosenDetractors[0].ToString().ToUpper() + "$";

            for (int j = 1; j < chosenDetractors.Count - 1; j++)
            {
                demand += ", " + "$" + chosenDetractors[j].ToString().ToUpper() + "$";
            }
            demand += " or " + "$" + chosenDetractors[chosenDetractors.Count - 1].ToString().ToUpper() + "$";
        }
        else if (chosenDetractors.Count == 1)
        {
            demand += ", but not ";
            demand += "$" + chosenDetractors[0].ToString().ToUpper() + "$";
        }

        demand += "!";

        Debug.Log(demand);
        yield return(StartCoroutine(TypeText(demand, textField, textVoice)));
        yield return new WaitForSeconds(2.0f);
        textField.color = Color.white;
        StartCoroutine(FadeTo(backPanel, 0f, 0.5f));
    }


    // --- Text Display Functions ---

    public IEnumerator TypeText(string toParse, Text toWrite, AudioSource voice)
    {
        bool italics = false;
        bool bold = false;
        bool bad = false;
        bool good = false;

        bool voiced = (voice != null);
        float basePitch = 1.0f;

        string toFill = "";
        char[] letterArray = toParse.ToCharArray();

        bool ignore = false; //for ignoring special characters that toggle styles

        foreach (char nextletter in letterArray)
        {

            switch (nextletter)
            {

                case '@':
                    ignore = true; //make sure this character isn't printed by ignoring it
                    good = !good; //toggle red styling
                    break;
                case '$':
                    ignore = true; //make sure this character isn't printed by ignoring it
                    bad = !bad; //toggle red styling
                    break;
                case '*':
                    ignore = true; //make sure this character isn't printed by ignoring it
                    bold = !bold; //toggle bold styling
                    break;
                case '/':
                    ignore = true; //make sure this character isn't printed by ignoring it
                    italics = !italics; //toggle italic styling
                    break;
                case '+':
                    ignore = true;
                    textLaugh.Play();
                    teacher.sprite = laughing;
                    yield return new WaitForSeconds(textLaugh.clip.length);
                    yield return new WaitForSeconds(0.4f);
                    teacher.sprite = thinking;
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

                    letter = "<color=" + goodColor.ColorToHex() + ">" + letter + "</color>";

                }
                if (bad)
                {

                    letter = "<color=" + badColor.ColorToHex() + ">" + letter + "</color>";

                }
                toFill += letter;
                toWrite.text = toFill;

            }
            //make sure the next character isn't ignored
            ignore = false;
            if (letter.Equals(".") || letter.Equals(",") || letter.Equals(";") || letter.Equals("!"))
                yield return new WaitForSeconds(0.2f);
            else
            {
                if (voiced)
                {
                    textVoice.pitch = basePitch + UnityEngine.Random.Range(-0.2f, 0.2f);
                    textVoice.Play();
                }
                if(Input.GetButton("P1_AButton") || Input.GetButton("P0_AButton"))
                {
                    yield return new WaitForSeconds(0.00001f);
                }
                else
                {
                    yield return new WaitForSeconds(0.075f);
                }
            }
            if (voiced)
               voice.pitch = basePitch;
        }
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
