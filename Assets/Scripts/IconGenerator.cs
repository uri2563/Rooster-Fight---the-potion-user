using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEditor;
//https://answers.unity.com/questions/1601619/access-or-remove-background-color-of-assetpreview.html
public class IconGenerator : MonoBehaviour
{
    private Camera camera;
    /*
    public void OnEnable()
    {
        StartCoroutine(Captures());
    }

    public int TargetSize = 256;
    public GameObject targetObj;
    public Color TransparentColor;
    IEnumerator Captures()
    {
        camera = Camera.main;
        yield return new WaitForSeconds(0.5f);
        camera.backgroundColor = TransparentColor;
        Texture2D sTex = ScreenCapture.CaptureScreenshotAsTexture(1);
        if (sTex.height < TargetSize || sTex.width < TargetSize)
        {
            Debug.LogError("Screen Resolution is too small to capture target image");
        }
        int i = 0;
        Color[] pixels = sTex.GetPixels((sTex.width - TargetSize) / 2, (sTex.height - TargetSize) / 2, TargetSize, TargetSize);
        TransparentColor = pixels[i];
        for (; i < pixels.Length; i++)
        {
            if (pixels[i] == TransparentColor)
                pixels[i] = new Color();
        }
        Texture2D tex = new Texture2D(TargetSize, TargetSize);
        tex.SetPixels(pixels);
        string cardPath = "Assets/Resources/icons/" + targetObj.name + "_Icon.png";
        byte[] bytes = tex.EncodeToPNG();
        System.IO.File.WriteAllBytes(cardPath, bytes);
        AssetDatabase.ImportAsset(cardPath);
        TextureImporter ti = (TextureImporter)TextureImporter.GetAtPath(cardPath);
        ti.textureType = TextureImporterType.Sprite;
        ti.SaveAndReimport();
    }*/
}