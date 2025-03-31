using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using TMPro;
/*using Unity.VisualScripting;*/
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class LevelStart_Texts
{
    public string levelName;
    public string levelNumber;
    [TextArea(2,6)]
    public string levelShortDescription;
}

[System.Serializable]
public class LevelStart_UIComponenets
{
    public Text levelNameUI; 
    public Text levelNumberUI;
    public Text levelShortDescriptionUI;
    public Button playButton; 
}

public class LevelUIManagment : MonoBehaviour
{
    [Header("TEXTS MANAGE")]
    public List<LevelStart_Texts> levelStartTexts;

    [Header("UI")]
    public LevelStart_UIComponenets levelStartUI;

    [Space]

    public Animator LevelUIAnimator;
    public string LevelUIAnimParamName;
    public string LevelUIAnimParamName2;
    public Button NextButton;
    public Text levelNavigationMassage;
    public RectTransform levelNavigation;
    public float paddingWight;
    public Image lvStartBG; 
    public Image lvFinishBG;
    public Text someThingSelected; 

    public LevelManagment levelManager;
    public Player_Control playerController;
    public AudioManagement audioManager;
    public Tutoriel tutoriel;

    private int currentLv;

    private void Awake()
    {
        currentLv = PlayerPrefs.GetInt("Played Lv");
    }

    private void Start()
    {
        if (levelManager.isLevelStart)
        {
          StartCoroutine(LevelStart()); 
        }
    }

    private void Update()
    {
        if(levelManager.isLevelFinish)
        {
          StartCoroutine(LevelFinish());
        }

        // Get the preferred width and height of the text
        float textWidth = levelNavigationMassage.preferredWidth;

        levelNavigation.sizeDelta = new Vector2(textWidth+paddingWight, 80); 
    }
    public void UpdateLevelNavigationMassage(string _massage_)
    {
        levelNavigationMassage.text = _massage_;
    }

    public void UpdateSelection(string whatSelected)
    {
        someThingSelected.text = whatSelected;
    }

    private IEnumerator LevelStart()
    {
        // Stop player movement
        playerController.Pause_Resume(true); 
        lvStartBG.enabled = true;   

        // Update texts & buttton
        levelStartUI.levelNameUI.text = levelStartTexts[currentLv-1].levelName; 
        levelStartUI.levelNumberUI.text = levelStartTexts[currentLv-1].levelNumber;
        levelStartUI.levelShortDescriptionUI.text = levelStartTexts[currentLv-1].levelShortDescription;

        yield return new WaitForSeconds(2f);

        // Animation 
        LevelUIAnimator.SetBool(LevelUIAnimParamName, true);

        // Audio 
        audioManager.PlayPopUpSound(); 

        yield return new WaitForSeconds(0.5f);
        LevelUIAnimator.speed = 0; 
    }

    public void AfterClickOnPlay()
    {
        LevelUIAnimator.speed = 1;
        lvStartBG.enabled = false;   
        levelManager.Reset(); 
        LevelUIAnimator.SetBool(LevelUIAnimParamName, false);
        playerController.Pause_Resume(false);
        tutoriel.ShowTutoriel(); 
    }

    public IEnumerator LevelFinish()
    { 
        playerController.Pause_Resume(true); 
        NextButton.interactable = false;
        lvFinishBG.enabled = true;   

        // Animation 
        LevelUIAnimator.SetBool(LevelUIAnimParamName2, true);

        // Audio 
        audioManager.PlayPopUpSound();

        yield return new WaitForSeconds(0.2f);

        NextButton.interactable = true;
        levelManager.Reset(); 
    }

    public void AfterClickOnNext()
    {
        LevelUIAnimator.SetBool(LevelUIAnimParamName2, false);
        lvFinishBG.enabled = false;   
        levelManager.Reset();
        levelManager.GoToLevelScene();
    }
}
