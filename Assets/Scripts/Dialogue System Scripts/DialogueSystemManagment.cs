using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueSystemManagment : MonoBehaviour
{
    public bool isTheyTalking = false; 

    public float typeSpeed = 1.0f;
    public List<Sprite> npcIdleSrpites;

    //UI
    private RectTransform conversationPanelBG;
    private RectTransform optionButtons;
    private RectTransform playerDialogueRT;
    private RectTransform npcDialogueRT;
    private RectTransform playerDialoguePanel; 
    private RectTransform npcDialoguePanel; 
    private RectTransform nextButtonRT; 
    private RectTransform doneButtonRT;
    private RectTransform optionART;
    private RectTransform optionBRT;
    private Image playerImage; 
    private Image npcImage;
    private Text optionAText; 
    private Text optionBText;
    private Text playerDialogues;
    private Text npcDialogues; 

    //Animation
    private Animator playerUIAnimator; 
    private Animator npcUIAnimator;

    //BACKEND
    public float paddingSpace; 
    public LevelManagment levelManager; 
    private PlayerDialogues playerDialoguesSystem; 
    private NPCDialogues npcDialoguesSystem;
    public GameStateSerialization gameStateManager;
    private GameObject Player;
    private GameObject NPC;
    private int npcPrefabIndex;
    private int characterIndex;
    private int playerDialogueIndex;
    private int npcDialogueIndex;
    private int totalDialogues; 
    private int levelNumber;
    private bool isWantEnd = false;
    public GameObject chatStartButton;

    // Start is called before the first frame update
    void Start()
    {
        //Find UI Componenets
        conversationPanelBG = GameObject.Find("Conversation Panel").transform.Find("BG").GetComponent<RectTransform>();
        
        playerImage = conversationPanelBG.transform.Find("Player").GetComponent<Image>();
        npcImage = conversationPanelBG.transform.Find("NPC").GetComponent<Image>();

        optionButtons = conversationPanelBG.transform.Find("Options/Option Buttons").GetComponent<RectTransform>();
        optionART = optionButtons.transform.Find("Option A").GetComponent<RectTransform>(); 
        optionBRT = optionButtons.transform.Find("Option B").GetComponent<RectTransform>(); 
        optionAText = optionART.transform.Find("Option A Btn/Text").GetComponent<Text>();
        optionBText = optionBRT.transform.Find("Option B Btn/Text").GetComponent<Text>();
       
        playerDialogueRT = conversationPanelBG.transform.Find("Player/Player Dialogue").GetComponent<RectTransform>(); 
        playerDialoguePanel = playerDialogueRT.transform.Find("Player Dialogues Panel").GetComponent<RectTransform>();
        playerDialogues = playerDialoguePanel.transform.Find("Inside/Text").GetComponent<Text>();
        nextButtonRT = playerDialoguePanel.transform.Find("Next").GetComponent<RectTransform>();
        
        npcDialogueRT = conversationPanelBG.transform.Find("NPC/NPC Dialogue").GetComponent<RectTransform>();    
        npcDialoguePanel = npcDialogueRT.transform.Find("NPC Dialogues Panel").GetComponent<RectTransform>();
        npcDialogues = npcDialoguePanel.transform.Find("Inside/Text").GetComponent<Text>();
        doneButtonRT = npcDialoguePanel.transform.Find("Done").GetComponent<RectTransform>();


        //Find other refereces
        playerUIAnimator = playerImage.transform.GetComponent<Animator>();
        npcUIAnimator = npcImage.transform.GetComponent<Animator>();

        levelManager = GameObject.Find("Level Manager").GetComponent<LevelManagment>();
        gameStateManager = GameObject.Find("Game State Manager").GetComponent<GameStateSerialization>();

        if(gameStateManager.userData.Gender == "Male")
        {
            characterIndex = gameStateManager.userData.CharacterIndex + 1;
        }
        else
        {
            characterIndex = gameStateManager.userData.CharacterIndex + 3;
        }
    }

    public void StartChatting(GameObject Player, GameObject NPC)
    {
        isTheyTalking = true; 
        chatStartButton.SetActive(false);

        this.Player = Player;
        this.NPC = NPC;

        // Stop player and npc to move 
        Player_Control playerMovemet = Player.GetComponent<Player_Control>();
        playerMovemet.CanPlayerMove(false);
        playerDialoguesSystem = Player.GetComponent<PlayerDialogues>();   

        NPCMovement npcMovement = NPC.GetComponent<NPCMovement>(); 
        npcMovement.CanNPCMove(false);
        npcDialoguesSystem = NPC.GetComponent<NPCDialogues>();    

        npcPrefabIndex = npcMovement.npcNumber; 
        levelNumber = levelManager.levelNumber;
        totalDialogues = playerDialoguesSystem.LEVELS[levelNumber - 1].NPCs[npcPrefabIndex].Industries[0].Solutions[0].Dailogues.Count - 1;

        //UI
        conversationPanelBG.localScale = new Vector3(1, 1, 1);
        npcImage.sprite = npcIdleSrpites[npcPrefabIndex];
        if (gameStateManager.userData.Gender == "Male")
        {
            characterIndex = gameStateManager.userData.CharacterIndex + 1;
        }
        else
        {
            characterIndex = gameStateManager.userData.CharacterIndex + 4;
        }
        PlayerAnimations(false, characterIndex);
        NPCAnimations(false, npcPrefabIndex);

        Options(true); 
    }

    private void Options(bool isShow)
    {
        if (isShow)
        {
            optionButtons.localScale = new(1, 1, 1);
            optionAText.text = playerDialoguesSystem.LEVELS[levelNumber - 1].NPCs[npcPrefabIndex].Industries[0].Solutions[0].Options[playerDialogueIndex];
            optionBText.text = "BYE!";
            float preferredSize = optionAText.preferredWidth;
            optionART.sizeDelta = new(preferredSize + paddingSpace, 90);
        }
        else
        {
            optionButtons.localScale = new(0, 0, 1);
            optionAText.text = "";
            optionBText.text = "";
        }
    }

    public void OptionChoosed(int btnIndex)
    {
        //0 = CONTINUE , 1 = STOP
        StartCoroutine(ShowPlayerDialogue(btnIndex));
        Options(false); 
    }

    private IEnumerator ShowPlayerDialogue(int dialogueProgress)
    {
        playerDialogueRT.localScale = new(1, 1, 1);
        PlayerAnimations(true, characterIndex);
        switch (dialogueProgress)
        {
            case 0:
                {
                    isWantEnd = false;
                    string fullDialogue = playerDialoguesSystem.LEVELS[levelNumber - 1].NPCs[npcPrefabIndex].Industries[0].Solutions[0].Dailogues[playerDialogueIndex];
                    string currentDialogue = null;
                    for (int i = 0; i < fullDialogue.Length; i++)
                    {
                        currentDialogue = fullDialogue.Substring(0, i);
                        playerDialogues.text = currentDialogue;
                        float preferredSize = playerDialogues.preferredWidth;
                        playerDialoguePanel.sizeDelta = new(preferredSize + paddingSpace, 100);

                        yield return new WaitForSeconds(typeSpeed);
                    }
                     
                    break;
                }
            default:
                {
                    isWantEnd = true;
                    string fullDialogue = "Bye!, we'll talk later";
                    string currentDialogue = null;
                    for (int i = 0; i < fullDialogue.Length; i++)
                    {
                        currentDialogue = fullDialogue.Substring(0, i);
                        playerDialogues.text = currentDialogue;
                        float preferredSize = playerDialogues.preferredWidth;
                        playerDialoguePanel.sizeDelta = new(preferredSize + paddingSpace, 100);

                        yield return new WaitForSeconds(typeSpeed);
                    }
                    break;
                }

        }

        nextButtonRT.localScale = new(1, 1, 1);
        PlayerAnimations(false, characterIndex);
    }


    private IEnumerator ShowNPCDialogues()
    {
        npcDialogueRT.localScale = new(-1, 1, 1);
        NPCAnimations(true,npcPrefabIndex);
        if (!isWantEnd)
        {
            string fullDialogue = npcDialoguesSystem.LEVELS[levelNumber - 1].Industries[0].Solution[0].Dailogues[npcDialogueIndex];
            Debug.Log(fullDialogue); 
            string currentDialogue = null; 
            for(int i =0; i < fullDialogue.Length; i++)
            {
                currentDialogue = fullDialogue.Substring(0, i); 
                npcDialogues.text = currentDialogue;
                float preferredSize = npcDialogues.preferredWidth;
                npcDialoguePanel.sizeDelta = new(preferredSize + paddingSpace, 100);

                yield return new WaitForSeconds(typeSpeed);
            }
            
        }
        else
        {
            string fullDialogue = "Bye! Bye! , we will definitely meet later";
            string currentDialogue = null;
            for (int i = 0; i < fullDialogue.Length; i++)
            {
                currentDialogue = fullDialogue.Substring(0, i);
                npcDialogues.text = currentDialogue;
                float preferredSize = npcDialogues.preferredWidth;
                npcDialoguePanel.sizeDelta = new(preferredSize + paddingSpace, 100);

                yield return new WaitForSeconds(typeSpeed);
            }
        }
        doneButtonRT.localScale = new(1, 1, 1);
        NPCAnimations(false, npcPrefabIndex);
    }

    public void Progress(string progressType)
    {
        switch(progressType)
        {
            case "NEXT":
                {
                    nextButtonRT.localScale = new(0, 0, 1);
                    playerDialogueRT.localScale = new(0, 0, 1);
                    playerDialogues.text = "";
                    StartCoroutine(ShowNPCDialogues());
                    break; 
                }
            case "DONE":
                {
                    if (playerDialogueIndex != totalDialogues)
                    {
                        if (!isWantEnd)
                        {
                            doneButtonRT.localScale = new(0, 0, 1);
                            npcDialogueRT.localScale = new(0, 0, 1);
                            npcDialogues.text = "";
                            npcDialogueIndex++;
                            playerDialogueIndex++;
                            Options(true);
                            break; 
                        }
                        else
                        {
                            ConversationEnd();
                            break; 
                        }
                    }
                    else
                    {
                        ConversationEnd();
                        break; 
                    }
                }
        }
        
    }

    private void PlayerAnimations(bool isTalking , int characterIndex)
    {
        playerUIAnimator.SetBool("isTalking", isTalking);
        playerUIAnimator.SetInteger("CharacterIndex", characterIndex); 
    }

    private void NPCAnimations(bool isTalking, int npcIndex)
    {
        npcUIAnimator.SetBool("isTalk", isTalking);
        npcUIAnimator.SetInteger("NpcIndex", npcIndex);
    }

    private void ConversationEnd()
    {
        PlayerAnimations(false, 0);
        NPCAnimations(false, -1); 

        npcDialogueIndex = 0; 
        playerDialogueIndex = 0;
        levelNumber = 0;
        isWantEnd = false;
        npcPrefabIndex = -1;
        characterIndex = 0; 
        totalDialogues = 0; 

        doneButtonRT.localScale = new(0, 0, 1);
        nextButtonRT.localScale = new(0, 0, 1);
        playerDialogues.text = ""; 
        npcDialogues.text = "";
        playerDialogueRT.localScale = new(0, 0, 1); 
        npcDialogueRT.localScale = new(0, 0, 1);
        optionAText.text = "";
        optionBText.text = "";
        optionButtons.localScale = new(0, 0, 1);
        conversationPanelBG.localScale = new(0, 0, 1);

        // let player and npc to move again
        Player_Control playerMovemet = Player.GetComponent<Player_Control>();
        playerMovemet.CanPlayerMove(true);
        Player.GetComponent<PlayerDetactNPC>().ChatEnd(); 
        playerDialoguesSystem = null;
        Player = null; 

        NPCMovement npcMovement = NPC.GetComponent<NPCMovement>();
        npcMovement.CanNPCMove(true);
        npcDialoguesSystem = null;
        NPC = null; 

        isTheyTalking = false;  
    }
}
