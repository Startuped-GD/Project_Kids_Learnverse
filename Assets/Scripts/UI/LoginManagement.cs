using System.Collections;
using UnityEngine.Networking;
using UnityEngine.UI;
using UnityEngine;
using System.Collections.Generic;

public class LoginManagment : MonoBehaviour
{
    [Header("LOGIN ANIMATION")]
    public string panelAnimParamName;
    private Animator loginAnim;
    public string displayMassageAnimParamName; 
    private Animator massagesAnim;

    [Header("MASSAGES")]
    public List<Text> Massages = new(); 

    [Header("AUTHENTICATION")]
    public InputField emailInputField;
    private string userEnteredEmail;

    [Space]
    [Header("OTP")]
    public InputField otpInputField;
    private string userEnteredOTP;

    [Space]
    [Header("RE-SEND OTP")]
    public float totalSecond;
    private float remainingTime;
    public Text timeText;
    public GameObject resentOtpBtnObject;
    private bool canStartTime = false;
    private bool isResend = false;

    [Space]
    [Header("USERNAME")]
    public InputField usernameInputField;
    private string userEnteredName;

    [Space]
    [Header("GENDER")]
    public Toggle maleToggle;
    public Toggle femaleToggle;
    private string genderInString; 

    [Space]
    [Header("CHARACTER")]
    public Image characterImage;
    public List<Sprite> maleSprites = new(); 
    public List<Sprite> femaleSprites = new();
    private int spriteNumber = 0; 

    private SceneManagment sceneManager;
    private GameStateSerialization gameStateManager; 
    private GameObject LoadingBar;

    // Start is called before the first frame update
    void Start()
    {
        remainingTime = totalSecond;

        loginAnim = GetComponent<Animator>();

        // Find 
        sceneManager = GameObject.Find("Scene Manager").GetComponent<SceneManagment>();
        LoadingBar = GameObject.Find("Loading Bar");
        gameStateManager = GameObject.Find("Game State Manager").GetComponent<GameStateSerialization>(); 

        // Show first massage 
        string massageToDisplay = "Enter Email Address!"; 
        Display_Remove_Massages(0,massageToDisplay);
    }

    public void Update()
    {
        if (canStartTime)
        {
            if (remainingTime > 0)
            {
                remainingTime -= Time.deltaTime;
                Timer(remainingTime);
            }
            else if(remainingTime <= 0)
            {
                if (!isResend)
                {
                    isResend = true;
                    canStartTime = false;
                    resentOtpBtnObject.SetActive(true);
                }
            }
        }
    }


    #region ANIMATION
    public IEnumerator PanelChangingAnimation(int animNumber)
    {
        Enable_Disable_Inputs(false);
        loginAnim.SetInteger(panelAnimParamName, animNumber);

        yield return new WaitForSeconds(0.3f); 
        Enable_Disable_Inputs(true);
    }

    #endregion


    #region EMAIL 

    public IEnumerator EmailRegister()
    {
        // Show Loading 
        Start_Or_Stop_Loading(true);

        //Disable All Inputs 
        Enable_Disable_Inputs(false); 

        // Register 
        userEnteredEmail = emailInputField.text;
        StartCoroutine(VerifyEnteredEmailAddress()); 

        yield return null; 
    }

    private IEnumerator VerifyEnteredEmailAddress()
    {
        yield return new WaitForSeconds(0.6f);
        Debug.Log("Verifaction");
        
        // Email entered 
        if (userEnteredEmail.Length != 0)
        {
            // Email not valid 
            if(!userEnteredEmail.Contains("@") || !userEnteredEmail.Contains("."))
            {
                // Show massage 
                string massageToDisplay = "Invalid Email Addresse Entered!";
                Display_Remove_Massages(0, massageToDisplay);

                // Stop Loading
                Start_Or_Stop_Loading(false);

                Enable_Disable_Inputs(true);
            }
            // Email vaild 
            else
            {
                StartCoroutine(SentOTPToUserEmail(userEnteredEmail));
            }
        }
        // No email enetered
        else
        {
            // Show massage 
            string massageToDisplay = "No Email Addresse Entered!";
            Display_Remove_Massages(0, massageToDisplay);

            // Stop Loading
            Start_Or_Stop_Loading(false);

            Enable_Disable_Inputs(true);
        }
    }
    #endregion


    #region OTP

    private IEnumerator SentOTPToUserEmail(string emailID)
    {
        canStartTime = true; 

        string sendOtpURL = $"https://app.startuped.ai/api/User/GetOTP?Email={emailID}"; 

        using UnityWebRequest sendOTPWebRequest = UnityWebRequest.Get(sendOtpURL) ;
        yield return sendOTPWebRequest.SendWebRequest(); 

        if(sendOTPWebRequest != null )
        {
            if(sendOTPWebRequest.result == UnityWebRequest.Result.Success)
            {
                string generatedOTP = sendOTPWebRequest.downloadHandler.text;
                // Show massage 
                string massageToDisplay = "OTP Send! Check You Mail Box";
                Display_Remove_Massages(1, massageToDisplay);

                // Stop Loading
                Start_Or_Stop_Loading(false);

                // Email to otp panel 
                StartCoroutine(PanelChangingAnimation(1));

                gameStateManager.UserStatus(emailID); // Check this user is first
                                                      // time user or returning user

            }
            else
            {
                string massageToDisplay = "OOPS! Something Wrong, Try Again";
                Display_Remove_Massages(0, massageToDisplay);

                // Stop Loading
                Start_Or_Stop_Loading(false);

                Enable_Disable_Inputs(true); 
            }
        }
    }

    private void Timer(float time)
    {
        time += 1;
        float minuts = Mathf.FloorToInt(time / 60); 
        float seconds = Mathf.FloorToInt(time % 60);

        timeText.text = minuts.ToString() + ":" + seconds.ToString();  
    }

    public void ResendOTP()
    {
        isResend = false;
        remainingTime = totalSecond;
        StartCoroutine(SentOTPToUserEmail(emailInputField.text));
        resentOtpBtnObject.SetActive(false);
        Debug.Log("DOne"); 
    }

    public void OTPEntered()
    {
        // Disable inputs 
        Enable_Disable_Inputs(false); 

        // OTP
        userEnteredOTP = otpInputField.text; 

        if(userEnteredOTP.Length != 0)
        {
            // Show Loading
            Start_Or_Stop_Loading(true);

            StartCoroutine(VerifyEnteredOTP(userEnteredEmail, userEnteredOTP)); 
        }
        else
        {
            string massageToDisplay = "No OTP Entered";
            Display_Remove_Massages(1, massageToDisplay);

            Enable_Disable_Inputs(true);
        }
    }

    private IEnumerator VerifyEnteredOTP(string emailID , string OTP)
    {
        string verifyOtpUrl = $"https://app.startuped.ai/api/User/VerifyOTP?EmailOrContactNumber={emailID}&OTP={OTP}"; 

        using UnityWebRequest verifyOTPRequest = UnityWebRequest.Get(verifyOtpUrl);
        yield return verifyOTPRequest.SendWebRequest();

        if(verifyOTPRequest != null)
        {
            if(verifyOTPRequest.result == UnityWebRequest.Result.Success)
            {
                string massageToDisplay = "Enter Username";
                Display_Remove_Massages(2, massageToDisplay);

                if (gameStateManager.isFirstTimeVisit)
                {
                    //OTP Panel to username panel 
                    StartCoroutine(PanelChangingAnimation(2));

                    Start_Or_Stop_Loading(false);  // Stop Loading

                    gameStateManager.StoreUserEmail(emailID); 
                }
                else
                {
                    gameStateManager.LoadData(emailID);

                    // Start loadig
                    StartCoroutine(LoadingWithWaitTime());
                }
            }
            else
            {
                string massageToDisplay = "OOPS! Incorrect OTP";
                Display_Remove_Massages(1, massageToDisplay);
                Enable_Disable_Inputs(true);

                // Stop Loading
                Start_Or_Stop_Loading(false);
            }
        }
    }

    #endregion

    #region GENDER
    public void GenderAuth()
    {
        Debug.Log("AUth FInish"); 
        Enable_Disable_Inputs(false);

        if (maleToggle.isOn && femaleToggle.isOn)
        {
            string massageToDisplay = "Choose One";
            Display_Remove_Massages(3, massageToDisplay);
            Enable_Disable_Inputs(true);
        }
        else if(!maleToggle.isOn && !femaleToggle.isOn)
        {
            string massageToDisplay = "Choose Your Preference";
            Display_Remove_Massages(3, massageToDisplay);
            Enable_Disable_Inputs(true);
        }
        else if(maleToggle.isOn)
        {
            Debug.Log("Playing");
            sceneManager.SaveUserGender("Male");
            genderInString = "Male";

            // Set 1st male character in character selection screen
            characterImage.sprite = maleSprites[0];

            string massageToDisplay = "Choose Your Character";
            Display_Remove_Massages(4, massageToDisplay);

            gameStateManager.StoreUserGender(genderInString);

            // Gender auth to character selection
            StartCoroutine(PanelChangingAnimation(4));

        }
        else if(femaleToggle.isOn)
        {
            sceneManager.SaveUserGender("Female");
            genderInString = "Female";

            // Set 1st female character in character selection screen
            characterImage.sprite = femaleSprites[0];

            string massageToDisplay = "Choose Your Character";
            Display_Remove_Massages(4, massageToDisplay);

            gameStateManager.StoreUserGender(genderInString);

            // Gender auth to character selection
            StartCoroutine(PanelChangingAnimation(4));
        }
    }

    #endregion

    #region USERNAME

    public void UsernameEntered()
    {
        // Disable all inputs 
        Enable_Disable_Inputs(false); 
        
        // Username
        userEnteredName = usernameInputField.text;  

        if(userEnteredName.Length == 0)
        {
            string massageToDisplay = "No Username Entered";
            Display_Remove_Massages(2, massageToDisplay);

            Enable_Disable_Inputs(true);
        }
        else if(userEnteredName.Length != 0 && userEnteredName.Length < 4)
        {
            string massageToDisplay = "Username Is Too Short";
            Display_Remove_Massages(2, massageToDisplay);

            Enable_Disable_Inputs(true);
        }
        else if(userEnteredName.Length >= 10)
        {
            string massageToDisplay = "Username Is Too Long";
            Display_Remove_Massages(2, massageToDisplay);

            Enable_Disable_Inputs(true);
        }
        else
        {
            string massageToDisplay = "Choose Your Preference";
            Display_Remove_Massages(3, massageToDisplay);

            sceneManager.SaveUsername(userEnteredName);

            gameStateManager.StoreUsername(userEnteredName);    

            // Username panel to gender auth
            StartCoroutine(PanelChangingAnimation(3));
        }
    }

    #endregion

    #region CHARACTER SELECTION 

    public void ChangeAtRight()
    {
        spriteNumber++;

        if (spriteNumber > 2)
        {
            spriteNumber = 0; 
        }

        ChangeCharacterSprite(spriteNumber);    
    }
    public void ChangeAtLeft()
    {
        spriteNumber--;
        if (spriteNumber < 0)
        {
            spriteNumber = 2; 
        }

        ChangeCharacterSprite(spriteNumber);    
    }

    private void ChangeCharacterSprite(int spriteIndex)
    {
        if (genderInString == "Male")
        {
            characterImage.sprite = maleSprites[spriteIndex];
        }
        else
        {
            characterImage.sprite = femaleSprites[spriteIndex];
        }
    }

    public void CharacterSelected()
    {
        //Save user selected character index 
        sceneManager.SaveUserSelectedCharacterIndex(spriteNumber);

        string massageToDisplay = " ";
        Display_Remove_Massages(4, massageToDisplay);

        gameStateManager.StoreUserCharacterIndex(spriteNumber);
        gameStateManager.SaveUserStoredData(); 

        // Start loadig s
        StartCoroutine(LoadingWithWaitTime());
    }

    #endregion
    private void Display_Remove_Massages(int massageNumber, string massage)
    {
        Massages[massageNumber].text = massage; 
    }

    private void Enable_Disable_Inputs(bool enableorDisable)
    {
        emailInputField.interactable = enableorDisable;
        usernameInputField.interactable = enableorDisable;
        otpInputField.interactable = enableorDisable;  
        maleToggle.interactable = enableorDisable;
        femaleToggle.interactable = enableorDisable;    
    }

    private IEnumerator LoadingWithWaitTime()
    {
        Start_Or_Stop_Loading(true);

        yield return new WaitForSeconds(2f);
        
        Start_Or_Stop_Loading(false);
        sceneManager.LoadAnyScene(3); 
    }

    private void Start_Or_Stop_Loading(bool start_or_Stop)
    {
        LoadingBar.transform.GetChild(0).gameObject.SetActive(start_or_Stop);
    }
}