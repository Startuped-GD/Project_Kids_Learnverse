using System.Linq.Expressions;
using UnityEngine;

public class Tutoriel : MonoBehaviour
{
    public string animParameter;
    private int tutorielShow = 0; // 0 = show tutoriel , 1 = not
    private string userVisitStatus = null; 
    public GameObject panel; 
    private Animator tutorielAnim;
    public SceneManagment sceneManager;
    public PlayerDetactNPC playerDetaction;

    // Start is called before the first frame update
    void Start()
    {
        tutorielAnim = this.GetComponent<Animator>();
        tutorielShow = PlayerPrefs.GetInt("Visit Status", 0);
        userVisitStatus = PlayerPrefs.GetString("Visit Status"); 
    }

    public void OffTutoriel()
    {
        tutorielAnim.SetBool(animParameter, true);
        playerDetaction.isStart = true;
    }

    public void ShowTutoriel()
    {
        if(userVisitStatus == "First Time Visitor" && tutorielShow == 0)
        {
            panel.SetActive(true);
            tutorielShow = 1; 
            sceneManager.SaveTutoriel(tutorielShow);
        }
        else
        {
            playerDetaction.isStart = true; 
        }
    }
}
