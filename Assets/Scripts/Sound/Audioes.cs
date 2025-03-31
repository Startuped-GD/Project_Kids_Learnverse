using System.Collections;
using System.Collections.Generic;
/*using Unity.VisualScripting;*/
using UnityEngine;

[System.Serializable]   
public class Audioes 
{
    public string AudioName;
    public AudioSource AudioSource;  
    public bool Mute;
    public bool PlayOnAwake;
    public bool Loop;
    [Range(0f, 1f)] 
    public float Valume;
    [Range(-3f, 3f)] 
    public float Pitch; 
}
