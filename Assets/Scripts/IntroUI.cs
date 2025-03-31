using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntroUI : MonoBehaviour
{
    public SceneManagment sceneManager; 

    public void SkipIntroVideoScene()
    {
        sceneManager.LoadAnyScene(2); 
    }
}
