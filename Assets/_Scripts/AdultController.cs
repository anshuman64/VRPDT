using UnityEngine;
using System.Collections;
using VRStandardAssets.Utils;


public class AdultController : MonoBehaviour
{
    [SerializeField] Animator adultAC;
    [SerializeField] AudioSource seaWavesAudio;
    [SerializeField] Canvas onOver;
    [SerializeField] Canvas endOnOver;


    AudioSource firstRecording;
    AudioSource secondRecording;
    AudioSource thirdRecording;

    VRInteractiveItem interactiveItem;

    bool isOver;
    int recording;

    void Awake()
    {
        interactiveItem = transform.GetComponent<VRInteractiveItem>();
    }

    void OnEnable()
    {
        interactiveItem.OnOver += HandleOver;
        interactiveItem.OnDown += HandleDown;
        interactiveItem.OnOut += HandleOut;
    }

    void OnDisable()
    {
        interactiveItem.OnOver -= HandleOver;
        interactiveItem.OnDown -= HandleDown;
        interactiveItem.OnOut -= HandleOut;
    }
    
    void HandleDown()
    {
        if (!MySceneManager.mySceneManager.acceptInput)
        {
            return;
        }

        if (!isOver)
        {
            onOver.enabled = false;
        }
        else
        {
            MySceneManager.mySceneManager.ToScene(MySceneManager.Scenes.MainMenu, true);
        }
    }

    void HandleOver()
    {
        if (!isOver)
        {
            onOver.enabled = true;
        }
        else
        {
            endOnOver.enabled = true;
        }
    }
    
    void HandleOut()
    {
        if (!isOver)
        {
            onOver.enabled = false;
        }
        else
        {
            endOnOver.enabled = false;
        }
    }

    // Use this for initialization
    void Start ()
    {
        firstRecording = GameObject.Find("FirstRecording").GetComponent<AudioSource>();
        secondRecording = GameObject.Find("SecondRecording").GetComponent<AudioSource>();
        thirdRecording = GameObject.Find("ThirdRecording").GetComponent<AudioSource>();

        recording = 0;
    }

    public void Crouch()
    {
        recording += 1;

        if (recording == 1)
        {
            StartCoroutine(PlayFirstCoroutine());
        }
        else if (recording == 2)
        {
            StartCoroutine(PlaySecondCoroutine());
        }
        else if (recording == 3)
        {
            StartCoroutine(PlayThirdCoroutine());
        }
    }

    IEnumerator PlayFirstCoroutine()
    {
        yield return new WaitForSeconds(1.5f);
        Debug.Log("Playing first recording...");

        seaWavesAudio.Pause();
        firstRecording.Play();

        yield return new WaitForSeconds(4f);

        seaWavesAudio.UnPause();

        Debug.Log("First recording ended");
    }

    IEnumerator PlaySecondCoroutine()
    {
        yield return new WaitForSeconds(1.5f);
        Debug.Log("Playing second recording...");

        seaWavesAudio.Pause();
        secondRecording.Play();

        yield return new WaitForSeconds(4f);

        seaWavesAudio.UnPause();

        Debug.Log("Second recording ended");
    }

    IEnumerator PlayThirdCoroutine()
    {
        yield return new WaitForSeconds(1.5f);
        Debug.Log("Playing third recording...");

        seaWavesAudio.Pause();
        thirdRecording.Play();

        adultAC.SetInteger("Change", 1);

        yield return new WaitForSeconds(4f);
        
        seaWavesAudio.UnPause();

        Debug.Log("Third recording ended");
    }

    public void EndIdle()
    {
        isOver = true;
        onOver.gameObject.SetActive(false);
        endOnOver.gameObject.SetActive(true);
    }
}
