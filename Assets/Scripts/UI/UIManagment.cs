using UnityEngine;
using UnityEngine.UI;

public class UIManagment : MonoBehaviour
{
    public Store store;
    private SceneManagment sceneManager; 
    private LoginManagment loginManager;
    private LevelScreenManagment levelManager;
    private PauseMenuManagment pauseMenuManager;
    private Tutoriel gameTutoriel;
    private Lv1Managment level1;
    private PlayerCollisionDetaction playerCollisionDet;
    private LevelUIManagment levelUIManager;
    private AudioManagement audioManager;
    private BGM BGMusic;
    private DialogueSystemManagment dialogueManager; 

    private void Start()
    {
        // Find Referances 
        sceneManager = GameObject.Find("Scene Manager").GetComponent<SceneManagment>();
        //BGMusic = GameObject.Find("BGM").GetComponent<BGM>();

        // When scene is login scene
        if (sceneManager.currentSceneNumber == 2)
        {
            loginManager = GameObject.Find("Login Manager").GetComponent<LoginManagment>();
        }

        // When scene is level scene
        if(sceneManager.currentSceneNumber == 4)
        {
            levelManager = GameObject.Find("Level Manager").GetComponent<LevelScreenManagment>(); 
            audioManager = GameObject.FindWithTag("Audio Manager").GetComponent<AudioManagement>(); 
        }

        // when scene is game scene
        if(sceneManager.currentSceneNumber == 5)
        {
            Debug.Log("This is scene 3"); 
            pauseMenuManager = GameObject.Find("Pause").GetComponent<PauseMenuManagment>();
            gameTutoriel = GameObject.Find("Tutoriel").GetComponent<Tutoriel>();
            level1 = GameObject.Find("Lv1 Manager").GetComponent<Lv1Managment>();
            playerCollisionDet = GameObject.FindWithTag("Player").GetComponent<PlayerCollisionDetaction>();
            levelUIManager = GameObject.FindWithTag("Level Manager").GetComponent<LevelUIManagment>();
            audioManager = GameObject.FindWithTag("Audio Manager").GetComponent<AudioManagement>();
            dialogueManager = GameObject.FindWithTag("Dialogue System Manager").GetComponent<DialogueSystemManagment>(); 
        }
    }

    #region LOGIN SCREEN BUTTONS

    public void PressedLoginButton()
    {
        StartCoroutine(loginManager.EmailRegister()); 
    }

    public void PressedOTPEnterButton()
    {
        loginManager.OTPEntered(); 
    }

    public void PressedGoButton()
    {
        loginManager.UsernameEntered(); 
    }

    public void PressedNextButton()
    {
        loginManager.GenderAuth(); 
    }

    public void RightButtonPressed()
    {
        loginManager.ChangeAtRight(); 
    }

    public void LeftButtonPressed()
    {
        loginManager.ChangeAtLeft();
    }

    public void SelectButtonPressed()
    {
        loginManager.CharacterSelected(); 
    }

    public void GuestModeButton()
    {
        sceneManager.LoadAnyScene(3);
    }

    #endregion

    #region LEVEL SCREEN BUTTONS
    public void LevelButtonPressed(int levelNumber)
    {
        audioManager.LevelBTNClicked(); 
        levelManager.OpenLevel(levelNumber);
    }

    public void LSBackButtonPressed()
    {
        sceneManager.LoadAnyScene(3);
    }

    public void LvTwoBonus(bool isOpened)
    {
        Debug.Log("This Opened!");
        levelManager.OpenCloseSocialLikePanel(isOpened); 
    }

    public void FollowOnSocialMedia()
    {
        levelManager.FollowOnSocialMedia(); 
    }

    #endregion

    #region PAUSE MENU BUTTONS

    public void PauseButtonPressed()
    {
        pauseMenuManager.GamePause(); 
    }

    public void ResumeButtonPressed()
    {
       StartCoroutine(pauseMenuManager.GameResume()); 
    }

    public void RestartButtonPressed()
    {
        pauseMenuManager.RestartLevel(); 
    }

    public void BackButtonPressed()
    {
        pauseMenuManager.BackToLevelScene(); 
    }

    public void SettingButtonPressed()
    {
        pauseMenuManager.OpenSettingPanel(); 
    }

    public void SettingCloseButtonPressed()
    {
        pauseMenuManager.CloseSettingPanel(); 
    }
    #endregion

    #region TUTORIEL BUTTONS
    public void ReadyButtonPressed()
    {
        gameTutoriel.OffTutoriel(); 
    }
    #endregion

    #region SELECTION MENU BUTTONS
    public void Industry_SelectButtonPressed(int index)
    {
        StartCoroutine(level1.AfterSelection(2,index)); 
    }

    public void Problem_SelectButtonPressed(int index)
    {
        StartCoroutine(level1.AfterSelection(4,index)); 
    }

    public void Solution_DoneButtonPressed(int index)
    {
        StartCoroutine(level1.AfterSelection(6, index));
    }

    public void TargetAudience_DoneButton(int index)
    {
        StartCoroutine(level1.AfterSelection(8, index));
    }

    public void USP_DoneButton(int index)
    {
        StartCoroutine(level1.AfterSelection(10, index));
    }

    #endregion

    #region OTHERS
    public void LocationSwitchButtonPressed()
    {
        LocationSwitcherOutside locationSwitcher = playerCollisionDet.detactedLocationSwitcher.GetComponent<LocationSwitcherOutside>();
        StartCoroutine(locationSwitcher.LocationSwitchConfirm()); 
    }

    public void PlayButtonPressed()// Play btn of level start panel
    {
        // Audio 
        audioManager.PlayButtonPressAudio();

        levelUIManager.AfterClickOnPlay();
    }

    public void NextButtonPressed() // next of level finish panel
    {
        // Audio 
        audioManager.NextButtonPressAudio();

        levelUIManager.AfterClickOnNext(); 
    }

    public void BGMOnOff()
    {
        BGMusic.BGMOnOff(); 
    }

    public void PlayerDialogueOptions(int dialogueIndex)
    {
        dialogueManager.OptionChoosed(dialogueIndex);
    }

    public void DailogueSystem_ProgressButtonPressed(string ProgressType)
    {
        dialogueManager.Progress(ProgressType);
    }

    #endregion

    #region MOBILE

    public void OpenMobileButtonPressed()
    {
        //CODE!!
        Debug.Log("MOBILE OPENED!"); 
    }

    #endregion

    #region SHOP

    public void OpenShopButtonPressed()
    {
        store.OpenShop(); 
        Debug.Log("SHOP OPENED!");
    }

    public void CloseShopButtonPressed()
    {
        store.CloseShop(); 
        Debug.Log("SHOP Closed");
    }

    #endregion
}
