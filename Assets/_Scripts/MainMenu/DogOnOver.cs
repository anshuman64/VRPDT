using UnityEngine;
using System.Collections;
using VRStandardAssets.Utils;

public class DogOnOver : MonoBehaviour
{

    [SerializeField]
    GameObject dog;
    [SerializeField]
    GameObject dogAnim;

    VRInteractiveItem interactiveItem;


    void Awake()
    {
        interactiveItem = transform.GetComponent<VRInteractiveItem>();
    }

    void OnEnable()
    {
        interactiveItem.OnOver += HandleOver;
        interactiveItem.OnOut += HandleOut;
    }

    void OnDisable()
    {
        interactiveItem.OnOver -= HandleOver;
        interactiveItem.OnOut -= HandleOut;
    }

    public void HandleOver()
    {
        dog.SetActive(false);
        dogAnim.SetActive(true);
    }

    public void HandleOut()
    {
        dog.SetActive(true);
        dogAnim.SetActive(false);
    }
}
