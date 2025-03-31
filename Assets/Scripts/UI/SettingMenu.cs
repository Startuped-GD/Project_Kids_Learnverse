using System.Collections.Generic;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;
using UnityEngine.Jobs;
using UnityEngine.UI;

public class SettingMenu : MonoBehaviour
{

    // Character
    public List<Sprite> maleCharacterSprites = new(); 
    public List<Sprite> femaleCharacterSprites = new();
    private List<Image> characterImages = new();
    private List<Toggle> characterToggleBTN = new();
    public int userSelectedCharIndex { get; private set; }

    // Gender 
    private Toggle maleToggle;
    private Toggle femaleToggle;
    private string userGender; 

    // Username
    private InputField usernameInputField;

    //Close 
    private Button closeButton;

    //Music & Sound 
    private Slider musicSlider;
    private Slider soundSlider;
    private AudioManagement audioManager;
    private BGM bgm; 

    // Other 
    private Player_Character playerCharacterManag;
    private GameStateSerialization gameStateManager;
    private SceneManagment sceneManager;

    private void Awake()
    {
        
    }

    // Start is called before the first frame update
    void Start()
    {
        // gender option references 
        Transform genderOption = transform.Find("Gender").transform;
        maleToggle = genderOption.GetChild(1).transform.GetChild(0).GetComponent<Toggle>();
        femaleToggle = genderOption.GetChild(2).transform.GetChild(0).GetComponent<Toggle>();

        // Character option references 
        Transform characterOption = transform.Find("Character").transform;
        for (int i = 1; i < characterOption.childCount; i++)
        {
            Image character = characterOption.GetChild(i).transform.GetComponent<Image>();
            characterImages.Add(character);
        }
        for (int i = 0; i < characterImages.Count; i++)
        {
            Debug.Log(i);
            characterToggleBTN.Add(characterImages[i].transform.GetChild(0).transform.GetChild(0).GetComponent<Toggle>());
        }

        // Username option reference
        usernameInputField = transform.Find("Username Field").transform.GetChild(1).GetComponent<InputField>();

        // Music and sound slider
        soundSlider = GameObject.Find("Sound Slider Bar").transform.GetChild(0).GetComponent<Slider>(); 
        musicSlider = GameObject.Find("Music Slider Bar").transform.GetChild(0).GetComponent<Slider>(); 

        playerCharacterManag = GameObject.FindWithTag("Player").GetComponent<Player_Character>();
        gameStateManager= GameObject.Find("Game State Manager").GetComponent<GameStateSerialization>();
        sceneManager = GameObject.FindWithTag("Scene Manager").GetComponent<SceneManagment>();
        audioManager = GameObject.FindWithTag("Audio Manager").GetComponent<AudioManagement>();
       /* bgm = GameObject.Find("BGM").GetComponent<BGM>();*/

        soundSlider.value = 1;
        if (bgm != null)
        {
            musicSlider.value = bgm.audioSource.volume;
        }

        closeButton = GameObject.Find("Close Button").GetComponent<Button>(); 

        // Load Usersaved Data
        GetUserSavedData();
    }

    // Update is called once per frame
    void Update()
    {
        CloseButtonEnableDisable(); 
    }

    public void FemaleToggleOff()
    {
        femaleToggle.isOn = false;
        userGender = "Male"; 
        ChangeCharacters(maleCharacterSprites);
        playerCharacterManag.ChangeCharacter(userSelectedCharIndex, userGender);
    }

    public void MaleToggleOff()
    {
        maleToggle.isOn = false;
        userGender = "Female"; 
        ChangeCharacters(femaleCharacterSprites);
        playerCharacterManag.ChangeCharacter(userSelectedCharIndex, userGender);
    }

    private void ChangeCharacters(List<Sprite> newSprites)
    {
        for(int i =0; i < characterImages.Count; i++)
        {
            Sprite newCharacterSprite = newSprites[i];
            characterImages[i].sprite = newCharacterSprite; 
        }
    }

    public void Character1Toggle()
    {
        characterToggleBTN[1].isOn = false;
        characterToggleBTN[2].isOn = false;
        userSelectedCharIndex = 0;

        // Change character of player
        ChangeMainPlayer(); 
    }

    public void Character2Toggle()
    {
        characterToggleBTN[0].isOn = false;
        characterToggleBTN[2].isOn = false;
        userSelectedCharIndex = 1;

        // Change character of player
        ChangeMainPlayer();
    }

    public void Character3Toggle()
    {
        characterToggleBTN[0].isOn = false;
        characterToggleBTN[1].isOn = false;
        userSelectedCharIndex = 2;

        // Change character of player
        ChangeMainPlayer(); 
    }

    private void ChangeMainPlayer()
    {
        playerCharacterManag.ChangeCharacter(userSelectedCharIndex, userGender); 
    }

    private void GetUserSavedData()
    {
        usernameInputField.text = PlayerPrefs.GetString("Username");

        userGender = PlayerPrefs.GetString("Gender");
        if (userGender == "Female")
        {
            femaleToggle.isOn = true;
        }
        else if (userGender == "Male")
        {
            maleToggle.isOn = true; 
        }

        userSelectedCharIndex = PlayerPrefs.GetInt("CharacterIndex");
        Debug.Log("Load Char! " + userSelectedCharIndex); 
        characterToggleBTN[userSelectedCharIndex].isOn = true;
    }

    public void SoundSliding()
    {
        audioManager.ChangeInValume(soundSlider.value); 
    }
    
    public void MusicSliding()
    {
        bgm.audioSource.volume = musicSlider.value; 
    }

    private void CloseButtonEnableDisable()
    {
        if(usernameInputField.text.Length < 3)
        {
            closeButton.interactable = false; 
        }
        else if (!maleToggle.isOn && !femaleToggle.isOn)
        {
            closeButton.interactable = false; 
        }
        else if (!characterToggleBTN[0].isOn && !characterToggleBTN[1].isOn && !characterToggleBTN[2].isOn)
        {
            closeButton.interactable = false; 
        }
        else
        {
            closeButton.interactable = true; 
        }
    }

    public void SaveNewSettingChanges()
    {
        gameStateManager.StoreUsername(usernameInputField.text); 
        gameStateManager.StoreUserGender(userGender); 
        gameStateManager.StoreUserCharacterIndex(userSelectedCharIndex);
        gameStateManager.SaveUserStoredData(); 

        sceneManager.SaveUsername(usernameInputField.text);
        sceneManager.SaveUserSelectedCharacterIndex(userSelectedCharIndex);
        sceneManager.SaveUserGender(userGender);    
    }
}
