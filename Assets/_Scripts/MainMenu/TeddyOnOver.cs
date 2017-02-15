using UnityEngine;
using System.Collections;
using VRStandardAssets.Utils;

public class TeddyOnOver : MonoBehaviour {

    [SerializeField]
    GameObject teddyStand;
    [SerializeField]
    GameObject teddySit;

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
        teddySit.SetActive(false);
        teddyStand.SetActive(true);
    }

    public void HandleOut()
    {
        teddySit.SetActive(true);
        teddyStand.SetActive(false);
    }
}
