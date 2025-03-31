using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct Solution_Dailogues
{
    public string Solution;
    [TextArea(2, 6)]
    public List<string> Dailogues;
}

[System.Serializable]
public struct Industry_Dailogues
{
    public string Industry;
    public List<Solution_Dailogues> Solution;
}

[System.Serializable]
public struct Level_Dailogues
{
    public string Level;
    public List<Industry_Dailogues> Industries; 
}

public class NPCDialogues : MonoBehaviour
{
    public List<Level_Dailogues> LEVELS = new(); 
}
