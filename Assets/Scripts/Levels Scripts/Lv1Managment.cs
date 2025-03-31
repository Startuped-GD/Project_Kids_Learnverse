using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Lv1Managment : MonoBehaviour
{
    // Level
    public bool canLevelPlay = false;
    private LevelUIManagment levelUIManager;
    private LevelManagment levelManager; 

    // Usp   
    [System.Serializable]
    public struct _USPs_
    {
        public string USP;
        public Transform Position; 
        public Sprite Sprite; 
    }

    // Target audience for each solution 
    [System.Serializable]
    public struct _TargetAudiences_
    {
        public string TargetAudience;
        public Transform Position;
        public Sprite Sprite; 
        public _USPs_ USP; 
    }

    // Solution for each problem 
    [System.Serializable]
    public struct _Solutions_
    {
        public string Solution;
        public Transform Position; 
        public Sprite Sprite; 
        public _TargetAudiences_ TargetAudiences;
    }

    // 3 problems for each industry 
    [System.Serializable]
    public struct _Problems_
    {
        public string Problem;
        public Transform Position;
        public Sprite Sprite; 
        public _Solutions_ Solution; 
    }

    // 5 Industry 
    [System.Serializable]
    public struct _Industries_
    {
        public string IndustryName;
        public Transform Position;
        public Sprite Sprite; 
        public _Problems_[] Problems; 
    }

    [Space]
    [Header("TEXTS,POSITIONS & SPRITES FOR BUBBLES")]
    public GameObject bubblePrefabObject;
    public _Industries_[] Industries;

    public string bubblesType { get; private set; } 
    private int bubbleCount = 0;
    private int bubbleCollection = 0; 
    private Transform CreatedBubbles;// All created bubble store here!

    // Selected Bubbles
    private int selectedIndustryIndex = 0;
    private int selectedProblemIndex = 0; 
    private int selectedSolutionIndex = 0;
    private int selectedTargetAudienceIndex = 0; 
    private int selectedUSPIndex = 0;
    public GameObject bubbleUI;
    private List<float> fillAmounts = new() { 0.2f, 0.333f, 1f };
    private int fillAmountIndex; 

    private List<string> threeProbelems = new();
    private string aSolution;
    private string aTargetAudienece;
    private string aUSP; 

    // Selection Menu Panels
    private GameObject selectionMenu;
    private Animator selectionMenuAnim;
    private int selectionMenuAnimCount;
    public string selectionMenuAnimParam;
    private List<GameObject> selectionMenuPanels = new();


    [Header("BUBBLES PLACING")]
    public List<GameObject> industryBubblePlace = new();
    public List<GameObject> problemBubblePlace = new();
    public List<GameObject> solutionBubblePlace = new();
    public List<GameObject> TABubblePlace = new();
    public List<GameObject> uspBubblePlace = new();
    public Vector2 defaultPosition;

    private void Start()
    {
    }
    public void LvStart()
    {
        if (canLevelPlay)
        {
            bubblesType = "Industry";
            bubbleCollection = 0;
            selectionMenuAnimCount = 0;

            // Find other references 
            bubbleUI.SetActive(true);
            CreatedBubbles = GameObject.Find("Created Bubbles").transform;
            levelUIManager = GameObject.FindWithTag("Level Manager").GetComponent<LevelUIManagment>();
            levelManager = GameObject.FindWithTag("Level Manager").GetComponent<LevelManagment>();

            // find selection panels :- 
            selectionMenu = GameObject.Find("Selection Menu").gameObject;
            for (int i = 0; i < selectionMenu.transform.childCount; i++)
            {
                GameObject currentPanel = selectionMenu.transform.GetChild(i).gameObject;
                selectionMenuPanels.Add(currentPanel);
            }
            selectionMenuAnim = selectionMenu.GetComponent<Animator>();

            if (levelManager.isLevelStart)
            {
                // Spawn Bubble 
                Bubbles();

                FillCollectedBubbleUI(1, true);

                string massage = "Find and collect 5 industries. Hint - Look for bubbles or orbs!";
                LevelMassages(massage);
            }

            bubbleUI.SetActive(canLevelPlay);
        }
    }

    private void Update()
    {
        bubbleUI.SetActive(canLevelPlay);
    }

    #region BUBBLES OF LEVEL 1
    private void Bubbles()
    {
        switch (bubblesType)
        {
            case "Industry":
                {
                    bubbleCount = 5;
                    bubbleCollection = 0;
                    selectionMenuAnimCount = 1;
                    fillAmountIndex = 0; 
                    SpawnBubbles(0);

                    break; 
                }
            case "Problems":
                {
                    bubbleCount = 3;
                    bubbleCollection = 0;
                    selectionMenuAnimCount = 3;
                    fillAmountIndex = 1; 
                    SpawnBubbles(1);

                    break; 
                }
            case "Solution":
                {
                    bubbleCount = 1;
                    bubbleCollection = 0;
                    selectionMenuAnimCount = 5;
                    fillAmountIndex = 2; 
                    SpawnBubbles(2);

                    break; 
                }
            case "Target Audience":
                {
                    bubbleCount = 1;
                    bubbleCollection = 0;
                    selectionMenuAnimCount = 7;
                    fillAmountIndex = 2; 
                    SpawnBubbles(3);

                    break; 
                }
            case "USP":
                {
                    bubbleCount = 1;
                    bubbleCollection = 0;
                    selectionMenuAnimCount = 9;
                    fillAmountIndex = 2; 
                    SpawnBubbles(4);

                    break; 
                }
        }
    }
    private void SpawnBubbles(int bubbleTypeIndex)
    {
        switch (bubbleTypeIndex)
        {
            case 0: // Industries Bubbles 
                {
                    for (int i = 0; i < bubbleCount; i++)
                    {
                        GameObject newBubble = bubblePrefabObject;
                        string containedText = Industries[i].IndustryName;
                        Transform containedPosition = Industries[i].Position;
                        Sprite containedSprite = Industries[i].Sprite;

                        // Create a bubble
                        CreateBubble(newBubble, containedPosition, containedSprite, containedText);
                    }

                    break; 
                }

            case 1: // Problems Bubbles 
                {
                    for (int i = 0; i < bubbleCount; i++)
                    {
                        GameObject newBubble = bubblePrefabObject;
                        string containedText = Industries[selectedIndustryIndex].Problems[i].Problem;
                        Transform containedPosition = Industries[selectedIndustryIndex].Problems[i].Position;
                        Sprite containedSprite = Industries[selectedIndustryIndex].Problems[i].Sprite;

                        // Store generate problems strings 
                        threeProbelems.Add(containedText); 

                        // Create a bubble 
                        CreateBubble(newBubble, containedPosition, containedSprite, containedText);
                    }

                    // Upadate problem panel
                    UpdateProblemPanel();

                    break; 
                }

            case 2: // Solutions Bubbles 
                {
                    GameObject newBubble = bubblePrefabObject;
                    string containedText = Industries[selectedIndustryIndex].Problems[selectedProblemIndex].Solution.Solution;
                    Transform containedPosition = Industries[selectedIndustryIndex].Problems[selectedProblemIndex].Solution.Position;
                    Sprite containedSprite = Industries[selectedIndustryIndex].Problems[selectedProblemIndex].Solution.Sprite;

                    // Store generated solution string 
                    aSolution = containedText;  

                    // Create a bubble 
                    CreateBubble(newBubble, containedPosition, containedSprite, containedText);

                    // Update solution panel 
                    UpdateSolutionPanel(); 

                    break; 
                }

            case 3: // Target Audience Bubbles 
                {
                    GameObject newBubble = bubblePrefabObject;
                    string containedText = Industries[selectedIndustryIndex].Problems[selectedProblemIndex].Solution.TargetAudiences.TargetAudience;
                    Transform containedPosition = Industries[selectedIndustryIndex].Problems[selectedProblemIndex].Solution.TargetAudiences.Position;
                    Sprite containedSprite = Industries[selectedIndustryIndex].Problems[selectedProblemIndex].Solution.TargetAudiences.Sprite;

                    // Store generated target audience string
                    aTargetAudienece = containedText; 

                    // Create an bubble 
                    CreateBubble(newBubble, containedPosition, containedSprite, containedText);

                    //Update target audience panel 
                    UpdateTargetAudiencePanel(); 

                    break; 
                }

            case 4: // USP Bubbles 
                {
                    GameObject newBubble = bubblePrefabObject;
                    string containedText = Industries[selectedIndustryIndex].Problems[selectedProblemIndex].Solution.TargetAudiences.USP.USP;
                    Transform containedPosition = Industries[selectedIndustryIndex].Problems[selectedProblemIndex].Solution.TargetAudiences.USP.Position;
                    Sprite containedSprite = Industries[selectedIndustryIndex].Problems[selectedProblemIndex].Solution.TargetAudiences.USP.Sprite;

                    // Store generated usp string
                    aUSP = containedText;

                    // Create an bubble 
                    CreateBubble(newBubble, containedPosition, containedSprite, containedText);

                    // Update USP panel 
                    UpdateUSPPanel();

                    break; 
                }
        }
    }

    private void CreateBubble(GameObject bubble , Transform spawnTransform,Sprite givenSprite ,string givenText)
    {
        // Create and store bubble 
        GameObject newBubble = Instantiate(bubble, spawnTransform.position, Quaternion.identity);
        newBubble.transform.parent = CreatedBubbles;

        // Change sprite 
        SpriteRenderer bubbleSprite = newBubble.transform.GetChild(0).transform.GetComponent<SpriteRenderer>();
        bubbleSprite.sprite = givenSprite;

        //Change contained text 
        TMP_Text bubbleText = newBubble.transform.GetChild(1).transform.GetChild(0).GetComponent<TMP_Text>();
        bubbleText.text = givenText;

        BubbleScript bubbleScript = newBubble.GetComponent<BubbleScript>();
        bubbleScript.placer = spawnTransform; 
    }

    public void IncreaseBubbleCollection()
    {
        bubbleCollection++;
        FillCollectedBubbleUI(0,true); 

        if(bubbleCollection == bubbleCount)
        {
            StartCoroutine(AllBubblesCollected());
        }
    }

    private void FillCollectedBubbleUI(int fill_or_Reset, bool isEnable)
    {
       /* Image FillAreaPanel = bubbleUI.transform.Find("Bubble Collection Filling").GetComponent<Image>();
        Image fill = FillAreaPanel.transform.Find("Fill Area/Fill").GetComponent<Image>(); 

        FillAreaPanel.enabled = isEnable;   
        fill.enabled = isEnable;  */ 
       Slider fillSlider = bubbleUI.transform.Find("Bubble Fill").GetComponent<Slider>();   

        if (fillSlider != null && isEnable)
        {
            if (fill_or_Reset == 0)
            {
                fillSlider.value += fillAmounts[fillAmountIndex];
            }
            else if(fill_or_Reset == 1) 
            {
                fillSlider.value = 0; 
            }
        }
    }

    private IEnumerator AllBubblesCollected()
    {
        yield return new WaitForSeconds(1f);
        switch (bubblesType)
        {
            case "Industry":
                {
                    string massage = "All Industry Bubbles Collected";
                    LevelMassages(massage);
                    levelUIManager.UpdateSelection("INDUSTRY"); 
                    break;
                }
            case "Problems":
                {
                    string massage = "All Problem Bubbles Collected";
                    levelUIManager.UpdateSelection("PROBLEM"); 
                    LevelMassages(massage);
                    break;
                }
            case "Solution":
                {
                    string massage = "Solution Bubble Collected";
                    levelUIManager.UpdateSelection("SOLUTION"); 
                    LevelMassages(massage);
                    break;
                }
            case "Target Audience":
                {
                    string massage = "Target Audince Bubble Collected";
                    LevelMassages(massage);
                    levelUIManager.UpdateSelection("TARGET AUDIENCE"); 
                    break;
                }
            case "USP":
                {
                    string massage = "USP Bubble Collected";
                    LevelMassages(massage);
                    levelUIManager.UpdateSelection("USP"); 
                    break;
                }
        }

        SelectionMenuAnim(selectionMenuAnimParam, selectionMenuAnimCount);
        FillCollectedBubbleUI(1, false);
    }
    private void SelectionMenuAnim(string animparameter, int animationIndex)
    {
        //Enable panel
        selectionMenuAnim.SetInteger(animparameter,animationIndex);
    }

    public IEnumerator AfterSelection(int animIndex , int selectionButtonIndex)
    {
        // Disable panel 
        SelectionMenuAnim(selectionMenuAnimParam, animIndex);

        switch (bubblesType)
        {
            case "Industry":
                {
                    selectedIndustryIndex = selectionButtonIndex;
                    break;
                }
            case "Problems":
                {
                    selectedProblemIndex = selectionButtonIndex;
                    break;
                }
            case "Solution":
                {
                    selectedSolutionIndex = selectionButtonIndex;
                    break;
                }
            case "Target Audience":
                {
                    selectedTargetAudienceIndex = selectionButtonIndex;
                    break;
                }
            case "USP":
                {
                    selectedUSPIndex = selectionButtonIndex;
                    break;
                }
        }

        yield return new WaitForSeconds(0.15f);

        // Next bubbles type's bubble spawn 
        switch (bubblesType)
        {
            case "Industry":
                {
                    bubblesType = "Problems";
                    string massage = "Find and collect 3 problems";
                    LevelMassages(massage);

                    Bubbles();
                    FillCollectedBubbleUI(1, true);

                    yield return new WaitForSeconds(1.25f);

                    // make selection panel animation default 
                    SelectionMenuAnim(selectionMenuAnimParam, 0);
                    break; 
                }
            case "Problems":
                {
                    bubblesType = "Solution";
                    string massage = "Find and collect a solution";
                    LevelMassages(massage);

                    Bubbles();
                    FillCollectedBubbleUI(1, true);

                    yield return new WaitForSeconds(1.25f);

                    // make selection panel animation default 
                    SelectionMenuAnim(selectionMenuAnimParam, 0);
                    break;
                }
            case "Solution":
                {
                    bubblesType = "Target Audience";
                    string massage = "Find and collect a target audience";
                    LevelMassages(massage);

                    Bubbles();
                    FillCollectedBubbleUI(1, true);

                    yield return new WaitForSeconds(1.25f);

                    // make selection panel animation default 
                    SelectionMenuAnim(selectionMenuAnimParam, 0);
                    break;
                }
            case "Target Audience":
                {
                    bubblesType = "USP";
                    string massage = "Find and collect a USP";
                    LevelMassages(massage);

                    Bubbles();
                    FillCollectedBubbleUI(1, true);

                    yield return new WaitForSeconds(1.25f);

                    // make selection panel animation default 
                    SelectionMenuAnim(selectionMenuAnimParam, 0);
                    break;
                }
            default:
                {
                    levelManager.LetLevelFinish(2); 
                    break; 
                }
        }
    }

    public void BubbleResetPlacing()
    {
        foreach(GameObject currentBubble in industryBubblePlace)
        {
            currentBubble.transform.position = defaultPosition; 
        }
        foreach (GameObject problemBubblePlacer in problemBubblePlace)
        {
            problemBubblePlacer.transform.position = defaultPosition; 
        }
    }

    public void BubblePlacing(int bubbleIndex , Vector2 position)
    {
        if (bubblesType == "Industry")
        {
            industryBubblePlace[bubbleIndex].transform.position = position;
        }
        else if (bubblesType == "Problems")
        {
            Debug.Log("This is"); 
            foreach (GameObject problemBubblePlacer in problemBubblePlace)
            {
                problemBubblePlacer.transform.position = position;
            }
        }
        else if (bubblesType == "Solution")
        {
            Debug.Log("This is");
            foreach (GameObject solutionBubblePlacer in solutionBubblePlace)
            {
                solutionBubblePlacer.transform.position = position;
            }
        }
        else if (bubblesType == "Target Audience")
        {
            Debug.Log("This is");
            foreach (GameObject taBubblePlacer in TABubblePlace)
            {
                taBubblePlacer.transform.position = position;
            }
        }
        else if (bubblesType == "USP")
        {
            Debug.Log("This is");
            foreach (GameObject uspBubblePlacer in uspBubblePlace)
            {
                uspBubblePlacer.transform.position = position;
            }
        }
    }

    // Update selection panels
    private void UpdateProblemPanel()
    {
        List<Text> problemTexts = new();

        for (int i = 0; i < 3; i++)
        {
            Transform Panel = selectionMenuPanels[1].transform.Find("Panel").transform; 
            Transform PanelChild = Panel.GetChild(i).transform;
            Transform TextParent = PanelChild.transform.Find("Problem").transform;
            Text problemText = TextParent.transform.Find("Problem Text").GetComponent<Text>();

            problemTexts.Add(problemText);

            if (i == 2)
            {
                for (int j = 0; j < problemTexts.Count; j++)
                {
                    problemTexts[j].text = threeProbelems[j];
                }
            }
        }
    }

    private void UpdateSolutionPanel()
    {
        Transform Panel = selectionMenuPanels[2].transform.Find("Panel");

        // Find problem box 
        Transform ProblemPanel = Panel.transform.Find("Problem").transform;
        Transform ProblemPanelBox = ProblemPanel.Find("Problem Box").transform;
        Transform ProblemText = ProblemPanelBox.Find("Problem Text").transform;
        Text ProblemContendText = ProblemText.Find("Problem Content Text").GetComponent<Text>();

        // Find solution box 
        Transform SolutionPanel = Panel.transform.Find("Solution").transform;
        Transform SolutionPanelBox = SolutionPanel.Find("Solution Box").transform;
        Transform SolutionText = SolutionPanelBox.Find("Solution Text").transform;
        Text SolutionContendText = SolutionText.Find("Solution Content Text").GetComponent<Text>();

        //Apply Stored solution's text 
        SolutionContendText.text = aSolution;
        ProblemContendText.text = threeProbelems[selectedProblemIndex]; 
    }

    private void UpdateTargetAudiencePanel()
    {
        // Find text
        Transform Panel = selectionMenuPanels[3].transform.Find("Panel").transform;
        Transform targetAudiencePanel = Panel.transform.Find("Target Audience").transform;
        Transform targetAudiencePanelBox = targetAudiencePanel.Find("Target Audience Box").transform; 
        Text targetAudienceText = targetAudiencePanelBox.Find("Target Audience Text").GetComponent <Text>();

        // Applye stored target audience text 
        targetAudienceText.text = aTargetAudienece; 
    }

    private void UpdateUSPPanel()
    {
        // Find text 
        Transform Panel = selectionMenuPanels[4].transform.Find("Panel").transform;
        Transform USPPanel = Panel.transform.Find("USP").transform;
        Transform USPPanelBox = USPPanel.transform.Find("USP Box").transform;
        Text USPText = USPPanelBox.transform.Find("USP Text").GetComponent<Text>();

        // Apple stored usp text 
        USPText.text = aUSP; 
    }

    #endregion

    #region LEVEL UI
    private void LevelMassages(string Massage)
    {
        levelUIManager.UpdateLevelNavigationMassage(Massage);
    }
    #endregion
}
