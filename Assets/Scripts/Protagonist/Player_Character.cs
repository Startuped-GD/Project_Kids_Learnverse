using System.Collections.Generic;
using UnityEngine;

public class Player_Character : MonoBehaviour
{
    // Character 
    public List<GameObject> maleCharacter = new(); 
    public List<GameObject> femaleCharacter = new();
    private GameObject onScreenCharacter; 
    private int characterIndex = 0;

    private Player_Control playerController;

    private void Awake()
    {
        playerController = this.GetComponent<Player_Control>();
    }
    // Start is called before the first frame update
    void Start()
    {
        // Load 
        LoadUserSelectedCharacter(); 
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FindAllCharacters()
    {
      /*  for (int i = 0; i < this.transform.childCount; i++)
        {
            GameObject charactersParent = this.transform.GetChild(i).gameObject; 
            for(int j =0; j < charactersParent.transform.childCount; j++)
            {
                GameObject currentCharacter = charactersParent.transform.GetChild(j).gameObject; 
                if(i == 0)
                {
                    maleCharacter.Add(currentCharacter);    
                }
                else
                {
                    femaleCharacter.Add(currentCharacter);    
                }
            }
        }*/
    }

    private void LoadUserSelectedCharacter()
    {
        string gender = PlayerPrefs.GetString("Gender");
        characterIndex = PlayerPrefs.GetInt("CharacterIndex"); 
        SetOnScreenCharacter(characterIndex, gender);
    }

    public void ChangeCharacter(int characterIndex , string characterGender)
    {
        SetOnScreenCharacter(characterIndex, characterGender); 
    }

    private void SetOnScreenCharacter(int charIndex, string charGender)
    {

        if(charGender == "Male")
        {
            Destroy(onScreenCharacter); 
            onScreenCharacter = Instantiate(maleCharacter[charIndex], this.transform.position, Quaternion.identity); 
            onScreenCharacter.transform.parent = this.transform; 
        }
        else
        {
            Destroy(onScreenCharacter);
            onScreenCharacter = Instantiate(femaleCharacter[charIndex], this.transform.position, Quaternion.identity);
            onScreenCharacter.transform.parent = this.transform;
        }

        SpriteRenderer characterRenderer = onScreenCharacter.GetComponent<SpriteRenderer>();
        Animator characterAnimator = onScreenCharacter.GetComponent<Animator>();
        if (onScreenCharacter != null)
        {
            Debug.Log("gameobject not null");
        }
        else
        {
            Debug.Log("gameobect null");
        }
        if (characterAnimator != null )
        {
            Debug.Log("Animator not null");
        }
        else
        {
            Debug.Log("Animator null");
        }

        if (characterRenderer != null)
        {
            Debug.Log("Render not null");
        }
        else
        {
            Debug.Log("Render null");
        }

        playerController.GetOnScreenCharacter(onScreenCharacter, characterRenderer, characterAnimator);
    }
}
