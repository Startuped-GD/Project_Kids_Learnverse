using UnityEngine;

public class LearningSubject : MonoBehaviour
{
    public SceneManagment sceneManager;

    public string ChoosedTopic;

    public void TopicChoosedButton(string topic)
    {
        ChoosedTopic = topic;
        sceneManager.SaveChoosedLearningTopic(topic);   
        sceneManager.LoadAnyScene(4);
    }
}
