using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameLoading : MonoBehaviour
{
    public TMP_Text loadingCountTxt;
    public float maxLoadSpeed = 17;
    public float minLoadSpeed = 0;
    private float currentLoadSpeed = 0; 
    private float loadPercentage = 0;
    private bool canLoad;

    [Space]

    public Transition transition; 

    // Start is called before the first frame update
    void Start()
    {
        loadPercentage = 0; 
        canLoad = true; 
    }

    // Update is called once per frame
    void Update()
    {
        if(canLoad)
        {
            currentLoadSpeed = Random.Range(minLoadSpeed, maxLoadSpeed); 
            loadPercentage += Time.deltaTime * currentLoadSpeed; 
            loadingCountTxt.text = Mathf.FloorToInt(loadPercentage).ToString() + "%";

            if(loadPercentage >= 100)
            {
                loadPercentage = 100;
                loadingCountTxt.text = Mathf.FloorToInt(loadPercentage).ToString() + "%";
                canLoad = false; 
            }
        }
        else
        {
            transition.GoToSceneIndexChanging(1); 
            StartCoroutine(transition.StartTransition(1)); 
        }
    }
}
