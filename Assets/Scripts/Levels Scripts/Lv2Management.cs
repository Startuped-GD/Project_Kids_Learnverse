using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Lv2Management : MonoBehaviour
{
    public bool canLevelPlay;

    public Text levelNavigationText;
    public string levelNaviationMassage;

    public GameObject feedbackCounterUI;
    public Text feedbackCounterText;
    public int targetFBCount; 
    public int currentFBCount;

    public GameObject hintBtnObject;
    public GameObject hintPanel;
    public bool isHint = false; 

    // Start is called before the first frame update
    void Start()
    {
        isHint = false; 
    }

    public void LvStart()
    {
        levelNavigationText.text = levelNaviationMassage; 

        feedbackCounterUI.SetActive(true);
        feedbackCounterText.text = currentFBCount.ToString();

        hintBtnObject.SetActive(true);
    }

    public void HintButtonPressed()
    {
        isHint = !isHint; 
        hintPanel.SetActive(isHint); 
    }
}
