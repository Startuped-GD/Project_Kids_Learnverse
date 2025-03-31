using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class musicOnOff : MonoBehaviour
{
    public Sprite[] sprites;
    private Image musicOnOffImage;
    private BGM BGMusic; 

    void Start()
    {
        //musicOnOffImage = this.transform.Find("Music On-Off Inside Icon/Music On-Off Btn").GetComponent<Image>();
        /*BGMusic = GameObject.Find("BGM").GetComponent<BGM>();*/
    }

    private void Update()
    {
        /*ChangeSprite(BGMusic.musicOnOffIndex); */
    }

    public void ChangeSprite(int spriteIndex)
    {
        musicOnOffImage.sprite = sprites[spriteIndex];
    }
}
