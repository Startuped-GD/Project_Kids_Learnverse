using UnityEngine;
using UnityEngine.SceneManagement;

public class BGM : MonoBehaviour
{
    private static BGM instance;
    public AudioSource audioSource;
    public int musicOnOffIndex { get; private set; } = 1; // 1 = on , 0 = off
    public int CurrentScene;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // Make this GameObject persistent
        }
        else
        {
            Destroy(gameObject); // Destroy duplicate instances
        }

        musicOnOffIndex = 1; 
        MusicOnOffValume(); 
    }

    private void Start()
    {
        CurrentScene = SceneManager.GetActiveScene().buildIndex;
    }

    public void Update()
    {
        CurrentScene = SceneManager.GetActiveScene().buildIndex; 
    }

    public void BGMOnOff()
    {
        musicOnOffIndex++;
        if(musicOnOffIndex > 1)
        {
            musicOnOffIndex = 0; 
        }

        MusicOnOffValume(); 
    }

    public void MusicOnOffValume()
    {
        audioSource.volume = musicOnOffIndex; 
    }
}
