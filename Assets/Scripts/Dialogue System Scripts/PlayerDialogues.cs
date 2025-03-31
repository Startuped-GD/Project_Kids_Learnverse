using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct PlayerDailogues
{
    public string Solution;
    [TextArea(2,6)]
    public List<string> Dailogues; 
    public List<string> Options; 
}

[System.Serializable]
public struct OnIndustry
{
    public string Industry;
    public List<PlayerDailogues> Solutions;
}

[System.Serializable]
public struct OnNPC
{
    public string NPC;
    public List<OnIndustry> Industries;
}

[System.Serializable]
public struct OnLevels
{
    public string Level; 
    public List<OnNPC> NPCs;
}

public class PlayerDialogues : MonoBehaviour
{
    public List<OnLevels> LEVELS = new(); 
}
