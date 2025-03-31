using System.Collections;
using UnityEngine;
using UnityEngine.Video;

public class IntroVideo : MonoBehaviour
{
    public float waitTime;
    public VideoPlayer introVideoPlayer;

    private SceneManagment sceneManager;
    public Transition transition;

    private void Start()
    {
        sceneManager = GameObject.Find("Scene Manager").GetComponent<SceneManagment>();

        StartCoroutine(IntroVideoPlay());
    }

    // Update is called once per frame
    void Update()
    {
        if (introVideoPlayer != null)
        {
            if (introVideoPlayer.isPlaying)
            {
                if (introVideoPlayer.time >= introVideoPlayer.length - 0.5f)
                {
                    LoadLoginScene();
                }
            }
        }
    }

    private IEnumerator IntroVideoPlay()
    {
        yield return new WaitForSeconds(waitTime);
        introVideoPlayer.Play();
    }

    private void LoadLoginScene()
    {
        sceneManager.LoadAnyScene(2); 
    }
}
