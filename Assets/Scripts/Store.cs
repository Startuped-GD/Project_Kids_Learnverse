using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Store : MonoBehaviour
{
    public Player_Control playerControl;
    public PlayerDetactNPC playerDetactNPC;

    public GameObject shopMenuObject;
    public Image shopBackground;
    public bool isShopOpened = false; 

    public void OpenShop()
    {
        isShopOpened = true;
        shopMenuObject.SetActive(true);
        playerControl.CanPlayerMove(false);
        playerDetactNPC.isStart = false;
        shopBackground.enabled = true; 
    }

    public void CloseShop()
    {
        isShopOpened = false;
        shopMenuObject.SetActive(false);
        playerControl.CanPlayerMove(true);
        playerDetactNPC.isStart = true;
        shopBackground.enabled = false; 
    }

}
