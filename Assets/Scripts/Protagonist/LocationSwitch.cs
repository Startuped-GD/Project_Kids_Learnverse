using System.Security.Cryptography;
using UnityEngine;
/*using UnityEngine.UI; */

public class PlayerLocationSwitch : MonoBehaviour
{
    public bool isOutside {  get; private set; }    // is player in outside location 

    private void Start()
    {
        isOutside = true; 
    }
    public void TransformPlayer(Transform newPosition , bool _isOutside_)
    {
        this.transform.position = newPosition.position;
        isOutside = _isOutside_;    
    }
}
