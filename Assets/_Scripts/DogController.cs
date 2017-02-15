using UnityEngine;
using System.Collections;
using VRStandardAssets.Utils;
using UnityEngine.UI;


public class DogController : MonoBehaviour
{
    [SerializeField] Animator dogAC;
    [SerializeField] Image m_Selection;
    [SerializeField] float m_SelectionDuration = 4f;

    [SerializeField] AudioSource seaWavesAudio;
    [SerializeField] AudioSource whineAudio;
    [SerializeField] AudioSource sadBarkAudio;
    [SerializeField] AudioSource pantingAudio;
    [SerializeField] AudioSource happyBarkAudio;

    [SerializeField] AudioSource firstRecording;
    [SerializeField] AudioSource secondRecording;
    [SerializeField] AudioSource thirdRecording;

    [SerializeField] Canvas firstOnOver;
    [SerializeField] Canvas secondOnOver;
    [SerializeField] Canvas thirdOnOver;
    [SerializeField] Canvas fourthOnOver;

    [HideInInspector] public bool isFirstPrompt;
    [HideInInspector] public bool isSecondPrompt;
    [HideInInspector] public bool isThirdPrompt;
    [HideInInspector] public bool isFourthPrompt;
    
    VRInteractiveItem interactiveItem;


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

    public void HandleDown()
    {
        if (!MySceneManager.mySceneManager.acceptInput)
        {
            return;
        }

        if (isFirstPrompt)
        {
            StartCoroutine(RecordFirst());
            StartCoroutine(FillSelectionRadial());
        }
        else if (isSecondPrompt)
        {
            StartCoroutine(RecordSecond());
            StartCoroutine(FillSelectionRadial());
        }
        else if (isThirdPrompt)
        {
            StartCoroutine(RecordThird());
            StartCoroutine(FillSelectionRadial());
        }
        else if (isFourthPrompt)
        {
            MySceneManager.mySceneManager.ToScene(MySceneManager.Scenes.BeachAdult, true);
        }
    }

    public void HandleOver()
    {
        if (isFirstPrompt)
        {
            firstOnOver.enabled = true;
        }
        else if(isSecondPrompt)
        {
            secondOnOver.enabled = true;
        }
        else if(isThirdPrompt)
        {
            thirdOnOver.enabled = true;
        }
        else if (isFourthPrompt)
        {
            fourthOnOver.enabled = true;
        }
    }

    void HandleOut()
    {
        if (isFirstPrompt)
        {
            firstOnOver.enabled = false;
        }
        else if(isSecondPrompt)
        {
            secondOnOver.enabled = false;
        }
        else if(isThirdPrompt)
        {
            thirdOnOver.enabled = false;
        }
        else if (isFourthPrompt)
        {
            fourthOnOver.enabled = false;
        }
    }

    public void FirstPrompt()
    {
        isFirstPrompt = true;
        firstOnOver.gameObject.SetActive(true);
        whineAudio.Play();
    }

    IEnumerator RecordFirst()
    {
        Debug.Log("Starting First Recording...");
        MySceneManager.mySceneManager.acceptInput = false;

        seaWavesAudio.Pause();
        whineAudio.Pause();

        firstRecording.clip = Microphone.Start(null, false, 4, 44100);

        yield return new WaitForSeconds(5f);

        whineAudio.UnPause();
        seaWavesAudio.UnPause();

        firstOnOver.gameObject.SetActive(false);
        isFirstPrompt = false;

        dogAC.SetInteger("Change", 1);

        MySceneManager.mySceneManager.acceptInput = true;
        Debug.Log("Stopping First Recording");
    }

    public void SadBark()
    {
        StartCoroutine(SadBarkCoroutine());
    }

    IEnumerator SadBarkCoroutine()
    {
        whineAudio.Stop();

        sadBarkAudio.Play();
        yield return new WaitForSeconds(1.4f);
        sadBarkAudio.Play();
    }

    public void SecondPrompt()
    {
        isSecondPrompt = true;
        secondOnOver.gameObject.SetActive(true);
        whineAudio.Play();
    }

    IEnumerator RecordSecond()
    {
        Debug.Log("Starting Second Recording...");
        MySceneManager.mySceneManager.acceptInput = false;

        seaWavesAudio.Pause();
        whineAudio.Pause();

        secondRecording.clip = Microphone.Start(null, false, 4, 44100);

        yield return new WaitForSeconds(5f);

        whineAudio.UnPause();
        seaWavesAudio.UnPause();

        secondOnOver.gameObject.SetActive(false);
        isSecondPrompt = false;
        dogAC.SetInteger("Change", 2);

        MySceneManager.mySceneManager.acceptInput = true;
        Debug.Log("Stopping Second Recording");
    }

    public void ThirdPrompt()
    {
        whineAudio.Stop();

        isThirdPrompt = true;
        thirdOnOver.gameObject.SetActive(true);
        pantingAudio.Play();
    }

    IEnumerator RecordThird()
    {
        Debug.Log("Starting Third Recording...");
        MySceneManager.mySceneManager.acceptInput = false;

        seaWavesAudio.Pause();
        pantingAudio.Pause();

        thirdRecording.clip = Microphone.Start(null, false, 4, 44100);

        yield return new WaitForSeconds(5f);

        pantingAudio.UnPause();
        seaWavesAudio.UnPause();

        thirdOnOver.gameObject.SetActive(false);
        isThirdPrompt = false;
        dogAC.SetInteger("Change", 3);

        MySceneManager.mySceneManager.acceptInput = true;
        Debug.Log("Stopping Third Recording");
    }

    public void HappyBark()
    {
        StartCoroutine(HappyBarkCoroutine());
    }

    IEnumerator HappyBarkCoroutine()
    {
        pantingAudio.Stop();
        happyBarkAudio.Play();

        yield return new WaitForSeconds(0.55f);
        happyBarkAudio.Play();
    }

    public void HappyIdle()
    {
        isFourthPrompt = true;
        fourthOnOver.gameObject.SetActive(true);
        pantingAudio.Play();
    }

    IEnumerator FillSelectionRadial()
    {
        // Create a timer and reset the fill amount.
        float timer = 0f;
        m_Selection.fillAmount = 0f;

        m_Selection.gameObject.SetActive(true);

        // This loop is executed once per frame until the timer exceeds the duration.
        while (timer < m_SelectionDuration)
        {
            // The image's fill amount requires a value from 0 to 1 so we normalise the time.
            m_Selection.fillAmount = timer / m_SelectionDuration;

            // Increase the timer by the time between frames and wait for the next frame.
            timer += Time.deltaTime;
            yield return null;
        }

        m_Selection.gameObject.SetActive(false);

        // When the loop is finished set the fill amount to be full.
        m_Selection.fillAmount = 1f;
    }
}
