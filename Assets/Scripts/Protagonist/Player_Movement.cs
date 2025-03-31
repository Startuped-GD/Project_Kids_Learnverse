using System.Collections.Generic;
using UnityEngine;

public class Player_Control : MonoBehaviour
{
    public GameObject leftBtnObject; 
    public GameObject rightBtnObject;
    public GameObject runBtnObject;
    private bool isMoveLeft; 
    private bool isMoveRight;
    private bool isRun; 

    [Header("MOVEMENT")]   
    private bool canPlayerMove = false; 
    public float maxRunSpeed = 0f; 
    public float maxWalkSpeed = 0f;
    private float maxMoveSpeed = 0f;
    [Space]

    [Header("PHYSICS")]
    private Rigidbody2D playerRB;
    private SpriteRenderer playerRenderer;
    private GameObject playerCharacter;
    [Space]

    [Header("INPUT SYSTEM")]
    private float horizontalInput;
    private bool runInput;
    [Space]

    [Header("ANIMATION")]
    public string[] animationParameter = new string[2];
    private Animator playerAnim;

    private AudioManagement audioManager;

    // Start is called before the first frame update
    void Start()
    {
        canPlayerMove = false; 

        // Get Componenets
        playerRB = GetComponent<Rigidbody2D>();

        audioManager = GameObject.FindWithTag("Audio Manager").GetComponent<AudioManagement>();                
    }

    // Update is called once per frame
    void Update()
    {
        if (canPlayerMove)
        {
            // Player PlayerMovement
            PlayerMovement();
            PlayerMovementByButtons();
            PlayerRun(); 

            // Player Animation
            PlayerAnimator();

            // Player Audios
            PlayerAudio();

            // Player View 
            PlayerView();
        }
    }
    private void PlayerMovement()
    {
        horizontalInput = Input.GetAxis("Horizontal");
        runInput = Input.GetKey(KeyCode.LeftShift);
        if(Input.GetKey(KeyCode.Keypad6))
        {
            horizontalInput = 1;
        }
        else if(Input.GetKey(KeyCode.Keypad4))
        {
            horizontalInput = -1;
        }

        maxMoveSpeed = runInput ? maxRunSpeed : maxWalkSpeed;
    }

    private void PlayerMovementByButtons()
    {
        if (isMoveRight)
        {
            horizontalInput = 1;
        }
        else if (isMoveLeft)
        {
            horizontalInput = -1;
        }

        maxMoveSpeed = runInput ? maxRunSpeed : maxWalkSpeed;
    }

    private void PlayerRun()
    {
        runInput = isRun;
        maxMoveSpeed = runInput ? maxRunSpeed : maxWalkSpeed;
    }

    public void RunButton(bool isRun)
    {
        this.isRun = isRun; 
    }

    public void MoveRightButton(bool isMove)
    {
        if (canPlayerMove)
        {
            isMoveRight = isMove;
        }
        else
        {
            isMoveRight = false;
        }
    }

    public void MoveLeftButton(bool isMove)
    {
        if (canPlayerMove)
        {
            isMoveLeft = isMove;
        }
        else
        {
            isMoveLeft = false;
        }
    }

    private void FixedUpdate()
    {
        playerRB.velocity = new(horizontalInput * maxMoveSpeed * Time.fixedDeltaTime, playerRB.velocity.y); 
    }

    private void PlayerView()
    {
        if (horizontalInput != 0)
        {
            if (horizontalInput > 0.01)
            {
                playerRenderer.flipX = false;
            }
            else if (horizontalInput < -0.01)
            {
                playerRenderer.flipX = true;
            }
        }
    }

    private void PlayerAnimator()
    {
        if(horizontalInput != 0 && !runInput)
        {
            Debug.Log("Walk");
            Animations(animationParameter[0], true);
            Animations(animationParameter[1], false);
        }
        else if(horizontalInput != 0 && runInput)
        {
            Debug.Log("Run");
            Animations(animationParameter[1], true);
        }
        else
        {
            Animations(animationParameter[0], false); 
            Animations(animationParameter[1], false); 
        }
    }

    private void Animations(string parameterName, bool start_or_stop)
    {
        playerAnim.SetBool(parameterName, start_or_stop);
    }

    private void PlayerAudio()
    {
        if (horizontalInput != 0 && !runInput)
        {
            Debug.Log("Walk Audio");
            audioManager.PlayWalkingAudio();
            audioManager.StopRunningAudio();
        }
        else if (horizontalInput != 0 && runInput)
        {
            Debug.Log("Run Audio");
            audioManager.PlayRunningAudio(); 
            audioManager.StopWalkingAudio();
        }
        else
        {
            Debug.Log("No Walking No Running");
            audioManager.StopRunningAudio();
            audioManager.StopWalkingAudio();
        }
    }

    public void CanPlayerMove(bool move_or_not)
    {
        canPlayerMove = move_or_not;
        leftBtnObject.SetActive(canPlayerMove); 
        rightBtnObject.SetActive(canPlayerMove);
        isMoveRight = false;
        isMoveLeft = false; 

        // Stop Animation 
        Animations(animationParameter[0], false);

        //Stop Moving 
        maxMoveSpeed = 0f; 
    }

    public void GetOnScreenCharacter(GameObject character, SpriteRenderer charSR, Animator charAnimator)
    {
        playerRenderer = charSR;
        playerCharacter = character;
        playerAnim = charAnimator;
    }

    public void Pause_Resume(bool isPause)
    {
        if (isPause)
        {
            canPlayerMove = false;
        }
        else
        {
            canPlayerMove = true;   
        }
    }
}
