using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BubbleScript : MonoBehaviour
{
    public Transform placer; 

    // Update is called once per frame
    void Update()
    {
        if(placer != null)
        {
            this.transform.position = placer.position; 
        }
    }
}
