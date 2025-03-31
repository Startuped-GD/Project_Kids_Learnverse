using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseMenuManagment : MonoBehaviour
{
    public PlayerDetactNPC playerDetactNPC;
    public GameObject shopBtnObject; 
    // Animation
    [Header("ANIMATION")]
    private Animator pauseMenuAnim;
    public string pauseAnimParam;
    public string resumeAnimParam;
    public string settingPanelAnimParam; 

    private GameObject PMBackgroundObject; 

    private Player_Control playerControlSystem;
    private SceneManagment sceneManager;
    private AudioManagement audioManager;
    private SettingMenu settingsMenu;

    // Start is called before the first frame update
    private void Start()
    {
        // Get Componenets 
        pauseMenuAnim = GetComponent<Animator>();

        // Find Referenaces 
        PMBackgroundObject = transform.Find("Pause Menu").transform.Find("BG").gameObject;
        playerControlSystem = GameObject.FindWithTag("Player").GetComponent<Player_Control>();
        sceneManager = GameObject.Find("Scene Manager").GetComponent<SceneManagment>();
        audioManager = GameObject.FindWithTag("Audio Manager").GetComponent<AudioManagement>();
        settingsMenu = GameObject.FindWithTag("Setting Menu").GetComponent <SettingMenu>();    
        Debug.Log("Work");
    }

    public void GamePause()
    {
        shopBtnObject.SetActive(false); 
        playerDetactNPC.isStart = false;

        Debug.Log("Game Pause"); 
        // Pause Menu Show
        PMBackgroundObject.SetActive(true);
        pauseMenuAnim.SetBool(pauseAnimParam, true);

        //Audio
        audioManager.PlayPopUpSound(); 

        Time.timeScale = 0; 
    }

    public IEnumerator GameResume()
    {
        Debug.Log("Game Resume");
        Time.timeScale = 1; 

        // Pause Menu Hide
        pauseMenuAnim.SetBool(pauseAnimParam, false); 
        pauseMenuAnim.SetBool(resumeAnimParam, true); 
        pauseMenuAnim.SetBool(settingPanelAnimParam, false); 
        PMBackgroundObject.SetActive(false);
        yield return new WaitForSeconds(0.1f);

        pauseMenuAnim.SetBool(settingPanelAnimParam, false); 
        pauseMenuAnim.SetBool(pauseAnimParam, false);
        pauseMenuAnim.SetBool(resumeAnimParam, false);

        playerDetactNPC.isStart = true;
        shopBtnObject.SetActive(true); 

    }

    public void RestartLevel()
    {
        sceneManager.LoadAnyScene(sceneManager.currentSceneNumber); 
    }

    public void BackToLevelScene()
    {
        sceneManager.LoadAnyScene(4); 
    }

    public void OpenSettingPanel()
    {
        SettingsPanel(true); 
    }
    public void CloseSettingPanel()
    {
        SettingsPanel(false);
        settingsMenu.SaveNewSettingChanges(); 
    }

    private void SettingsPanel(bool open_or_Close)
    {
        pauseMenuAnim.SetBool(settingPanelAnimParam, open_or_Close);
        GameObject[] buttonObjects = GameObject.FindGameObjectsWithTag("Pause Menu Button");

        // Disable some buttons :- resume , back , settings , restart 
        for (int i = 0; i < 4; i++)
        {
            buttonObjects[i].GetComponent<Button>().interactable = !open_or_Close; 
        }

        //Audio
        audioManager.PlayPopUpSound(); 
    }
}
