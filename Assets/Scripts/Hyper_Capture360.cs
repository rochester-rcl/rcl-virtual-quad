//==========================================================
// Purpose: To capture 360 images and videos on runtime.
//
// Usage: Attach to a gameobject with a camera and configure in editor.
//==========================================================

using System.IO;
using UnityEngine;
using UnityEngine.Rendering;

public class Hyper_Capture360 : MonoBehaviour
{
    private RenderTexture cubemapLeftEye;
    private RenderTexture cubemapRightEye;
    private RenderTexture cubemapMono;
    private RenderTexture crossCubemap;
    private RenderTexture equirect_mono;
    //private RenderTexture equirect_stereo;

    public string saveFolder = "360_Captures";
    public string fileName = "Capture_01";
    private string fullFileName = "";
    [Tooltip("720, 1080, 1440 (QHD), 2160 (4K)")] public int verticalResolution = 1080;
    public enum SaveFormat { PNG, JPG };
    public SaveFormat saveFormat = SaveFormat.JPG;
    public enum RenderType { Stereo, Mono, Both };
    public RenderType renderType = RenderType.Mono;
    public bool followCam = true;
    public bool saveEquirectangular = true;
    public bool continuousCapture = false;

    public float stereoSeparation = 0.064f;
    [SerializeField] KeyCode captureKey = KeyCode.C;

    Camera cam;

    void Start()
    {
        string m_Path = Application.dataPath;

        //Output the Game data path to the console
        //Debug.Log("Path : " + m_Path);

        cubemapLeftEye = new RenderTexture(verticalResolution, verticalResolution, 24, RenderTextureFormat.ARGB32);
        cubemapLeftEye.dimension = TextureDimension.Cube;
        cubemapRightEye = new RenderTexture(verticalResolution, verticalResolution, 24, RenderTextureFormat.ARGB32);
        cubemapRightEye.dimension = TextureDimension.Cube;
        cubemapMono = new RenderTexture(verticalResolution, verticalResolution, 24, RenderTextureFormat.ARGB32);
        cubemapMono.dimension = TextureDimension.Cube;
        crossCubemap = new RenderTexture(verticalResolution, verticalResolution / 4 * 3, 24, RenderTextureFormat.ARGB32) { dimension = TextureDimension.Tex2D };

        switch (renderType)
        {
            case RenderType.Mono:
                equirect_mono = new RenderTexture(verticalResolution * 2, verticalResolution, 24, RenderTextureFormat.ARGB32); //equirect width should be twice the height of cubemap
                break;

            case RenderType.Stereo:
                equirect_mono = new RenderTexture(verticalResolution * 2, verticalResolution * 2, 24, RenderTextureFormat.ARGB32); //equirect height is twice because over-under
                break;
        }
    }

    private void Update()
    {
        // for testing only
        if (Input.GetKeyUp(captureKey))
        {
            saveEquirectangular = true;
            Capture360();
            SaveAsImage();
        }
    }

    private void LateUpdate()
    {
        if (continuousCapture)
        {
            Capture360();
        }
    }

    public void Capture360()
    {
        CheckForCamera();
        CaptureCubemaps();
        //if (equirect == null) // why??
        //    return;
        if (saveEquirectangular)
        {
            ConvertCubeToEquirect();
        }
    }

    private void CheckForCamera()
    {
        cam = GetComponent<Camera>();
        if (cam == null)
        {
            cam = GetComponentInParent<Camera>();
        }
        if (cam == null)
        {
            Debug.LogWarning("Stereo 360 capture node has no camera or parent camera");
        }
    }

    private void CaptureCubemaps()
    {
        switch (renderType)
        {
            case RenderType.Mono:
                if (followCam)
                {
                    cam.stereoSeparation = 0f;
                    cam.RenderToCubemap(cubemapMono, 63, Camera.MonoOrStereoscopicEye.Left); //because stereo rendering does follow camera
                }
                else
                {
                    cam.RenderToCubemap(cubemapMono, 63, Camera.MonoOrStereoscopicEye.Mono);
                }
                break;

            case RenderType.Stereo:
                cam.stereoSeparation = stereoSeparation;
                cam.RenderToCubemap(cubemapLeftEye, 63, Camera.MonoOrStereoscopicEye.Left);
                cam.RenderToCubemap(cubemapRightEye, 63, Camera.MonoOrStereoscopicEye.Right);
                if (!followCam)
                {
                    Debug.LogWarning("Follow Cam is always enabled for Stereo Rendering");
                }

                break;
        }
    }

    private void ConvertCubeToEquirect()
    {
        switch (renderType)
        {
            case RenderType.Mono:
                equirect_mono = new RenderTexture(verticalResolution * 2, verticalResolution, 24, RenderTextureFormat.ARGB32); //equirect width should be twice the height of cubemap
                if (followCam)
                {
                    cubemapMono.ConvertToEquirect(equirect_mono, Camera.MonoOrStereoscopicEye.Mono);
                }
                else
                {
                    cubemapMono.ConvertToEquirect(equirect_mono, Camera.MonoOrStereoscopicEye.Mono);
                }
                break;

            case RenderType.Stereo:
                equirect_mono = new RenderTexture(verticalResolution * 2, verticalResolution * 2, 24, RenderTextureFormat.ARGB32); //equirect height is twice because over-under
                cubemapLeftEye.ConvertToEquirect(equirect_mono, Camera.MonoOrStereoscopicEye.Left);
                cubemapRightEye.ConvertToEquirect(equirect_mono, Camera.MonoOrStereoscopicEye.Right);
                break;
        }
    }

    private void SaveAsImage()
    {
        if (renderType == RenderType.Mono)
        {
            fullFileName = (Application.dataPath + Path.DirectorySeparatorChar + saveFolder + Path.DirectorySeparatorChar + fileName);
        }
        else
        {
            fullFileName = (Application.dataPath + Path.DirectorySeparatorChar + saveFolder + Path.DirectorySeparatorChar + fileName + "_stereo");
        }

        if (saveFormat == SaveFormat.PNG)
        {
            SavePNG(equirect_mono, fullFileName);
        }
        else
        {
            SaveJPG(equirect_mono, fullFileName);
        }
    }

    public static void SavePNG(RenderTexture rt, string filePath)
    {
        byte[] bytes = ToTexture2D(rt).EncodeToPNG();
        File.WriteAllBytes(filePath + ".png", bytes);
        Debug.Log("Saved to " + filePath);
    }

    public static void SaveJPG(RenderTexture rt, string filePath, int quality = 75)
    {
        byte[] bytes = ToTexture2D(rt).EncodeToJPG(quality);
        File.WriteAllBytes(filePath + ".jpg", bytes);
        //Debug.Log("Saved");
    }

    private static Texture2D ToTexture2D(RenderTexture rTex)
    {
        Texture2D tex = new Texture2D(rTex.width, rTex.height, TextureFormat.RGB24, false);
        RenderTexture.active = rTex;
        tex.ReadPixels(new Rect(0, 0, rTex.width, rTex.height), 0, 0);
        tex.Apply();
        return tex;
    }
}
