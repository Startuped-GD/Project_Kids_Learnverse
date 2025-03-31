using System.Collections;
using UnityEngine;

public class PlayerDetactNPC : MonoBehaviour
{
    public bool isStart = false;
    public bool isNpcDetacted = false;
    public GameObject detactedNPC;
    public GameObject chatStartButton;

    private bool isChatting = false;
    private DialogueSystemManagment dialogueSystemManager; 

    // Start is called before the first frame update
    void Start()
    {
        dialogueSystemManager = GameObject.Find("Dialogue System Manager").GetComponent<DialogueSystemManagment>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isStart)
        {
            if (isNpcDetacted)
            {
                if (Input.GetKeyDown(KeyCode.Space) && !isChatting)
                {
                    chatStartButton.SetActive(false);
                    isChatting = true;
                    PlayerWantsToChat();
                }
            }

            if (isNpcDetacted && !isChatting)
            {
                chatStartButton.SetActive(true);
            }
            else
            {
                chatStartButton.SetActive(false);
            }
        }
    }

    public void PlayerWantsToChat()
    {
        isChatting = true; 
        dialogueSystemManager.StartChatting(this.gameObject, detactedNPC); 
        chatStartButton.SetActive(false); 
    }

    public void ChatEnd()
    {
        isChatting = false;
    }
}
