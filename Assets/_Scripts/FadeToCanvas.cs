using UnityEngine;
using System.Collections;
using VRStandardAssets.Utils;


public class FadeToCanvas : MonoBehaviour
{
    public UIFader startCanvas;
    public UIFader endCanvas;
    
    VRInteractiveItem interactiveItem;


    void Awake()
    {
        interactiveItem = transform.GetComponent<VRInteractiveItem>();
    }

    void OnEnable()
    {
        interactiveItem.OnDown += HandleDown;
    }


    void OnDisable()
    {
        interactiveItem.OnDown -= HandleDown;
    }

    public void HandleDown()
    {
        if (MySceneManager.mySceneManager.acceptInput)
            StartCoroutine(ToOtherCanvas());
    }

    IEnumerator ToOtherCanvas()
    {
        MySceneManager.mySceneManager.acceptInput = false;
        yield return StartCoroutine(startCanvas.InteruptAndFadeOut(true));

        endCanvas.gameObject.SetActive(true);
        yield return StartCoroutine(endCanvas.InteruptAndFadeIn(true));

        MySceneManager.mySceneManager.acceptInput = true;
        startCanvas.gameObject.SetActive(false);
    }
}
