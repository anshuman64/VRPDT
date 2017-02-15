using UnityEngine;
using System.Collections;
using VRStandardAssets.Utils;


public class FadeToScene : MonoBehaviour
{
    [SerializeField] MySceneManager.Scenes toScene;
    [SerializeField] bool fade;
    
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
            MySceneManager.mySceneManager.ToScene(toScene, fade);
    }
}
