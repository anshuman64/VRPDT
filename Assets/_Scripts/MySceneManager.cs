using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using VRStandardAssets.Utils;

public class MySceneManager : MonoBehaviour
{
    public static MySceneManager mySceneManager;

    [SerializeField] VRCameraFade cameraFade;

    public enum Scenes { MainMenu, BeachDog, BeachAdult };

    // Bool to accept clicks on UI
    [HideInInspector] public bool acceptInput = true;

    AsyncOperation async;

    void Awake()
    {
        mySceneManager = this;
    }

    IEnumerator FadeToScene(Scenes sceneName)
    {
        // While the camera is already fading, wait.
        while (cameraFade.IsFading)
        {
            yield return null;
        }

        async = SceneManager.LoadSceneAsync(sceneName.ToString());
        async.allowSceneActivation = false;

        // Wait for the camera to fade out.
        yield return StartCoroutine(cameraFade.BeginFadeOut(true));
        
        while (async.progress < 0.9f)
        {
            yield return null;
        }

        // Load the level.
        async.allowSceneActivation = true;
    }

    // Public function that can either fade to scene or go directly to scene
    public void ToScene(Scenes sceneName, bool fade)
    {
        if (fade)
        {
            StartCoroutine(FadeToScene(sceneName));
        }
        else
        {
            SceneManager.LoadScene(sceneName.ToString());
        }
    }
}
