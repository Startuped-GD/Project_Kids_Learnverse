using System.Collections;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class SocialSharing : MonoBehaviour
{
    [TextArea(3,6)]
    public string shareMassage;
    public string shareSubject;

    public void ShareTheApp()
    {
        StartCoroutine(SocialSharingFunction());
    }

    private IEnumerator SocialSharingFunction()
    {
        yield return new WaitForEndOfFrame();

        Texture2D ss = new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24, false);
        ss.ReadPixels(new Rect(0, 0, Screen.width, Screen.height), 0, 0);
        ss.Apply();

        string filePath = Path.Combine(Application.temporaryCachePath, "shared img.png");
        File.WriteAllBytes(filePath, ss.EncodeToPNG());

        // To avoid memory leaks
        Destroy(ss);

        new NativeShare().AddFile(filePath)
            .SetSubject(shareSubject).SetText(shareMassage).SetUrl("https://github.com/yasirkula/UnityNativeShare")
            .SetCallback((result, shareTarget) => Debug.Log("Share result: " + result + ", selected app: " + shareTarget))
            .Share();
    }

    public void InviteAFriend()
    {
        //CODE!
        Debug.Log("Invite A Friend"); 
    }


}
