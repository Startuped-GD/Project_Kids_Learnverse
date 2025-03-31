using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManagment : MonoBehaviour
{
    public int levelNumber {  get; private set; }   
    public bool isLevelStart { get; private set; } 
    public bool isLevelFinish { get; private set; }

    public List<GameObject> allLvManager; 
    private SceneManagment sceneManager;
    private GameStateSerialization gameStateManager;

    private void Awake()
    {
        levelNumber = PlayerPrefs.GetInt("Played Lv"); 
        isLevelStart = true;
        isLevelFinish = false;

        sceneManager = GameObject.FindWithTag("Scene Manager").GetComponent<SceneManagment>();
        gameStateManager = GameObject.Find("Game State Manager").GetComponent<GameStateSerialization>();

        if(levelNumber == 1)
        {
            allLvManager[0].GetComponent<Lv1Managment>().canLevelPlay = true;
            allLvManager[0].GetComponent<Lv1Managment>().LvStart(); 
        }
        else if(levelNumber == 2)
        {
            allLvManager[1].GetComponent<Lv2Management>().canLevelPlay = true;
            allLvManager[1].GetComponent<Lv2Management>().LvStart(); 
        }
    }

    public void LetLevelFinish(int nextLevelNumber)
    {
        isLevelStart = false; 
        isLevelFinish = true;

        LevelData(nextLevelNumber);
    }
    private void LevelData(int newLevelNumber)
    {
        gameStateManager.StoreUserUnlockedLevel(newLevelNumber);
        gameStateManager.StoreUserTotalIPT(300);
        gameStateManager.SaveUserStoredData();
    }

    public void GoToLevelScene()
    {
        sceneManager.LoadAnyScene(4);
    }

    public void Reset()
    {
        isLevelFinish = false;
        isLevelStart = false; 
    }
}
