using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class ShareToSocial : MonoBehaviour
{
    public void ShareScore()
    {
        StartCoroutine(ScreenShotAndShare());
    }

    IEnumerator ScreenShotAndShare()
    {
        yield return new WaitForEndOfFrame();
        Texture2D tx = new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24, false);
        tx.ReadPixels(new Rect(0, 0, Screen.width, Screen.height), 0, 0);
        tx.Apply();

        string path = Path.Combine(Application.temporaryCachePath, "sharedImage.png");
        Destroy(tx);

        new NativeShare().AddFile(path).SetSubject("This is my score.")
            .SetText("share your score with your friend.")
            .Share();
    }

}
