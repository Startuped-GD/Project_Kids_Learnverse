using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

[System.Serializable]
public class UserData
{
    public string EmailID = null;
    public string Username = null;
    public string Gender = null;
    public int CharacterIndex = 0;
    public int TotalIPT = 0;
    public int UnlockedLevelNumber = 0; 
}

public class GameStateSerialization : MonoBehaviour
{
    public UserData userData = new(); 

    public bool isFirstTimeVisit { get; private set; } = false; 
    private string dataLoadingAPI = "https://app.startuped.ai/api/User/GetUser";
    private string dataSavingAPI = "https://app.startuped.ai/api/User/UpdateGameSettings";

    private SceneManagment sceneManager;

    private void Awake()
    {
        sceneManager = GameObject.Find("Scene Manager").GetComponent<SceneManagment>(); 

        int currentScene = UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex;

        if(currentScene > 1)
        {
            Debug.Log("Load From Player Prefs");
            userData.EmailID = PlayerPrefs.GetString("Email");
            userData.Username = PlayerPrefs.GetString("Username");
            userData.Gender = PlayerPrefs.GetString("Gender");
            userData.CharacterIndex = PlayerPrefs.GetInt("CharacterIndex");
            userData.TotalIPT = PlayerPrefs.GetInt("IPT");
            userData.UnlockedLevelNumber = PlayerPrefs.GetInt("Level"); 
        }
        else
        {
            Debug.Log("Scene is 1"); 
        }
    }

    #region USER STATUS CHECKING

    public void UserStatus(string emailID)
    {
        StartCoroutine(FatchUserStatus(emailID, dataLoadingAPI)); 
    }

    private IEnumerator FatchUserStatus(string emailAddress, string APIBackpoint)
    {
        string URL = $"{APIBackpoint}?email={emailAddress}"; //url created 

        UnityWebRequest newRequest = UnityWebRequest.Get(URL);
        yield return newRequest.SendWebRequest(); 

        if(newRequest.result == UnityWebRequest.Result.Success)
        {
            Debug.Log(newRequest.result);

            string downloadedJsonData = newRequest.downloadHandler.text;
            if(downloadedJsonData.Contains("gameSettings"))
            {
                int startIndex = downloadedJsonData.IndexOf("gameSettings")+"gameSettings".Length+3;
                int endIndex = downloadedJsonData.IndexOf("}", startIndex);
                int lenght = endIndex - startIndex; 
                string gameSettingJson = downloadedJsonData.Substring(startIndex,lenght);

                if(gameSettingJson.Contains("\"username\":\"\""))
                {
                    isFirstTimeVisit = true; 

                }
                else
                {
                    isFirstTimeVisit = false; 
                }

            }
            else
            {
                isFirstTimeVisit = true;
            }
        }
        else
        {
            isFirstTimeVisit = true; 
        }
    }
    #endregion

    #region LOAD USER'S DATA

    public void LoadData(string emailID)
    {
        StartCoroutine(UserDataLoad(emailID, dataLoadingAPI));
    }

    private IEnumerator UserDataLoad(string emailAddress , string APIBackpoint)
    {
        string URL = $"{APIBackpoint}?email={emailAddress}";

        UnityWebRequest newRequest = UnityWebRequest.Get(URL);
        yield return newRequest.SendWebRequest(); 

        if(newRequest.result == UnityWebRequest.Result.Success)
        {
            string downloadedJsonData = newRequest.downloadHandler.text;

            APIResponseJson apiReponseJson = JsonUtility.FromJson<APIResponseJson>(downloadedJsonData);
            GameSettingsJson gameSettings = apiReponseJson.gameSettings;

            // Store user's loaded data 
            StoreLoadedData(gameSettings,emailAddress); 
        }
        else
        {
            Debug.Log("Failed"); 
        }
    }

    private void StoreLoadedData(GameSettingsJson gameSettingsData, string EmailAddress)
    {
        userData.EmailID = EmailAddress; 
        userData.Username = gameSettingsData.username; 
        userData.Gender = gameSettingsData.gender;
        userData.CharacterIndex = gameSettingsData.character; 
        userData.TotalIPT = gameSettingsData.totalPoints;
        userData.UnlockedLevelNumber = gameSettingsData.currentLevel;

        // Save all data in playerPrefs 
        UserDataPlayerPrefs(); 
    }

    [System.Serializable]   
    public class APIResponseJson
    {
        public GameSettingsJson gameSettings; 
    }

    [System.Serializable]
    public class GameSettingsJson
    {
        public string username;
        public string gender;
        public int character;
        public int totalPoints;
        public int currentLevel;
    }
    #endregion

    #region SAVE USER'S DATA

    public void StoreUserEmail(string EmailID)
    {
        userData.EmailID = EmailID; 
    }

    public void StoreUsername(string Username)
    {
        userData.Username = Username;   
    }

    public void StoreUserGender(string Gender)
    {
        userData.Gender = Gender;
    }

    public void StoreUserCharacterIndex(int CharacterIndex)
    {
        userData.CharacterIndex = CharacterIndex;
    }

    public void StoreUserTotalIPT(int  TotalIPT)
    {
        userData.TotalIPT = TotalIPT;
    }

    public void StoreUserUnlockedLevel(int UnlockedLevelNumber)
    {
        userData.UnlockedLevelNumber= UnlockedLevelNumber;  
    }

    public void SaveUserStoredData()
    {
        GameSettingJson gameSettingsJson = new()
        {
            username = userData.Username,
            gender = userData.Gender,
            character = userData.CharacterIndex,
            totalPoints = userData.TotalIPT,
            currentLevel = userData.UnlockedLevelNumber
        };

        string gameDataJson = JsonUtility.ToJson(gameSettingsJson);
        string EmailAddress = userData.EmailID;

        StartCoroutine(SaveUserData(EmailAddress, dataSavingAPI, gameDataJson)); 
    }

    private IEnumerator SaveUserData(string emailAddress , string APIBackpoint, string gameDataJson)
    {
        string URl = $"{APIBackpoint}?email={emailAddress}";

        UnityWebRequest newRequest = new UnityWebRequest(URl, "POST"); 

        newRequest.SetRequestHeader("Content-Type", "application/json");
        Byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(gameDataJson);
        newRequest.uploadHandler = new UploadHandlerRaw(bodyRaw);

        yield return newRequest.SendWebRequest(); 

        if(newRequest.result == UnityWebRequest.Result.Success )
        {
            Debug.Log(newRequest.result);
            UserDataPlayerPrefs(); 
        }
        else
        {
            Debug.Log("Failed"); 
        }

    }

    public class GameSettingJson 
    {
        public string username;
        public string gender; 
        public int character;
        public int totalPoints;
        public int currentLevel;
    }

    #endregion

    public void UserDataPlayerPrefs()
    {
        sceneManager.SaveUserEmail(userData.EmailID); 
        sceneManager.SaveUsername(userData.Username); 
        sceneManager.SaveUserGender(userData.Gender); 
        sceneManager.SaveUserSelectedCharacterIndex(userData.CharacterIndex);
        sceneManager.SaveUserTotalIPT(userData.TotalIPT);
        sceneManager.SaveUserUnlockedLevel(userData.UnlockedLevelNumber);
        if (isFirstTimeVisit)
        {
            sceneManager.SaveVisitStatus("First Time Visitor");
        }
        else
        {
            sceneManager.SaveVisitStatus("Returning");
        }
    }
}
