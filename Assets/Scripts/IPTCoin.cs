using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IPTCoin : MonoBehaviour
{
    private Text coinText;
    private RectTransform iptCoinPanel;
    public float paddingSize;

    private int totalCoin;

    // Start is called before the first frame update
    void Start()
    {
        iptCoinPanel = GameObject.Find("IPT Coin").GetComponent<RectTransform>();
        coinText = iptCoinPanel.transform.Find("IPT Coin Inside/Coin Text").GetComponent<Text>(); 
    }

    // Update is called once per frame
    void Update()
    {
        SetPanelSize(); 
        coinText.text = totalCoin.ToString();   
    }

    private void SetPanelSize()
    {
        float currentTextWidth = coinText.preferredWidth; 
        float newWidth = currentTextWidth + paddingSize; 
        iptCoinPanel.sizeDelta = new(newWidth, iptCoinPanel.sizeDelta.y); 
    }    

    public void IncreaseCoin(int coin)
    {
        totalCoin += coin; 
    }
}
