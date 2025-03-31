using UnityEngine.SceneManagement;
using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class SceneManagment : MonoBehaviour
{
    public GameObject sceneLoader;
    public Text loadingText;
    private float loadingCount;
    private bool isLoading = false;

    public int currentSceneNumber /*{ get; private set; }*/ = 0;

    private void Awake()
    {
        currentSceneNumber = SceneManager.GetActiveScene().buildIndex; 
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            if (currentSceneNumber == 1)
            {
                LoadAnyScene(2);
            }
        }

        if(isLoading)
        {
            loadingText.text = Mathf.FloorToInt(loadingCount).ToString() + "%"; 
        }
    }

    #region SCENE MANAGE
    public void LoadAnyScene(int sceneIndexNumber)
    {
        SceneManager.LoadScene(sceneIndexNumber);
    }

    public void OpenGameScene()
    {
        StartCoroutine(LoadYourScene());
        isLoading = true;
        sceneLoader.SetActive(true); 
    }

    private IEnumerator LoadYourScene()
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("Game");
        asyncLoad.allowSceneActivation = false;  // Load in the background

        while (!asyncLoad.isDone)
        {
            Debug.Log("Loading progress: " + (asyncLoad.progress * 100) + "%");
            loadingCount = asyncLoad.progress * 100; 

            if (asyncLoad.progress >= 0.9f)
            {
                asyncLoad.allowSceneActivation = true; // Activate scene when ready
            }
            yield return null;
        }
    }

    #endregion

    #region USER DATA MANAGE
    public void SaveUserEmail(string email)
    {
        PlayerPrefs.SetString("Email", email); 
        PlayerPrefs.Save();
    }

    public void SaveUsername(string Username)
    {
        PlayerPrefs.SetString("Username", Username);
        PlayerPrefs.Save(); 
    }

    public void SaveUserGender(string gender)
    {
        PlayerPrefs.SetString("Gender", gender); 
        PlayerPrefs.Save();
    }

    public void SaveUserSelectedCharacterIndex(int charIndex )
    {
        PlayerPrefs.SetInt("CharacterIndex", charIndex);
        PlayerPrefs.Save();
        Debug.Log("Saved Char! "+charIndex); 
    }

    public void SaveUserTotalIPT(int IPT)
    {
        PlayerPrefs.SetInt("IPT", IPT);
        PlayerPrefs.Save();
    }

    public void SaveUserUnlockedLevel(int Level)
    {
        PlayerPrefs.SetInt("Level", Level);
        PlayerPrefs.Save();
    }

    public void SaveTutoriel(int tutoriel)
    {
        PlayerPrefs.SetInt("Tutoriel",tutoriel);
        PlayerPrefs.Save();
    }

    public void SaveVisitStatus(string visitStatus)
    {
        PlayerPrefs.SetString("Visit Status", visitStatus);
        PlayerPrefs.Save();
        Debug.Log(visitStatus); 
    }

    public void StoreUserPlayedLevel(int level)
    {
        PlayerPrefs.SetInt("Played Lv", level); 
        PlayerPrefs.Save();
    }

    public void SaveChoosedLearningTopic(string Topic)
    {
        PlayerPrefs.SetString("Learning Topic" ,Topic); 
        PlayerPrefs.Save();
    }

    #endregion
}
