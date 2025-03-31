using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Transition : MonoBehaviour
{
    public Animator transitionAnim;
    public AnimationClip animDownClip; 
    public string[] animParameters; 

    [Space]

    public SceneManagment sceneManager;
    public int nextSceneIndex;
    public int previousSceneIndex;
    private int goToSceneIndex;


    private void Start()
    {
        if (sceneManager.currentSceneNumber != 0)
        {
            StartCoroutine(StartTransition(0));
        }
    }

    public IEnumerator StartTransition(int paramIndex)
    {
        transitionAnim.SetBool(animParameters[paramIndex], true);

        float waitTime = animDownClip.length;
        yield return new WaitForSeconds(waitTime); 


        transitionAnim.SetBool(animParameters[paramIndex], false);
    }

    public void GoToSceneIndexChanging(int whichScene)
    {
        if(whichScene == 0)
        {
            goToSceneIndex = previousSceneIndex;
        }
        else
        {
            goToSceneIndex = nextSceneIndex;    
        }
    }

    public void LoadScene()
    {
        sceneManager.LoadAnyScene(goToSceneIndex); 
    }
}
