using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;


namespace VRStandardAssets.Utils
{
    // This class is used to fade the entire screen to black (or
    // any chosen colour).  It should be used to smooth out the
    // transition between scenes or restarting of a scene.
    public class VRCameraFade : MonoBehaviour
    {
        public event Action OnFadeComplete;                             // This is called when the fade in or out has finished.
        
        [SerializeField] private Image m_FadeImage;                     // Reference to the image that covers the screen.
        [SerializeField] private AudioMixerSnapshot m_DefaultSnapshot;  // Settings for the audio mixer to use normally.
        [SerializeField] private AudioMixerSnapshot m_FadedSnapshot;    // Settings for the audio mixer to use when faded out.
        [SerializeField] private Color m_FadeColor = Color.black;       // The colour the image fades out to.
        [SerializeField] private float m_FadeDuration = 2.0f;           // How long it takes to fade in seconds.
        [SerializeField] private bool m_FadeInOnSceneLoad = false;      // Whether a fade in should happen as soon as the scene is loaded.
        
        private bool m_IsFading;                                        // Whether the screen is currently fading.
        private float m_FadeStartTime;                                  // The time when fading started.
        private Color m_FadeOutColor;                                   // This is a transparent version of the fade colour, it will ensure fading looks normal.


        public bool IsFading { get { return m_IsFading; } }


        private void Awake()
        {
            m_FadeOutColor = new Color(m_FadeColor.r, m_FadeColor.g, m_FadeColor.b, 0f);

            if (m_FadeInOnSceneLoad)
            {
                m_FadeImage.color = m_FadeColor;
                StartCoroutine(FadeIn(m_FadeDuration, false));
            }
            else
            {
                // Reverts back to normal snapshot
                if (m_DefaultSnapshot)
                    m_DefaultSnapshot.TransitionTo(0.001f);
            }
        }

        public IEnumerator FadeIn(float duration, bool fadeAudio)
        {
            // If not already fading start a coroutine to fade from the fade colour to the fade out colour.
            if (m_IsFading)
                yield break;
            yield return StartCoroutine(BeginFade(m_FadeColor, m_FadeOutColor, duration));
            m_FadeImage.transform.parent.gameObject.SetActive(false);

            // Fade in the audio over the same duration.
            if (m_DefaultSnapshot && fadeAudio)
            {
                m_FadedSnapshot.TransitionTo(0.001f);
                m_DefaultSnapshot.TransitionTo(duration);
            }
        }

        public IEnumerator BeginFadeOut (bool fadeAudio)
        {
            // Fade out the audio over the default duration.
            if(m_FadedSnapshot && fadeAudio)
                m_FadedSnapshot.TransitionTo (m_FadeDuration);

            yield return StartCoroutine(BeginFade(m_FadeOutColor, m_FadeColor, m_FadeDuration));
        }


        public IEnumerator BeginFadeOut(float duration, bool fadeAudio = true)
        {
            // Fade out the audio over the given duration.
            if(m_FadedSnapshot && fadeAudio)
                m_FadedSnapshot.TransitionTo (duration);

            yield return StartCoroutine(BeginFade(m_FadeOutColor, m_FadeColor, duration));
        }


        public IEnumerator BeginFadeIn (bool fadeAudio)
        {
            // Fade in the audio over the default duration.
            if(m_DefaultSnapshot && fadeAudio)
            {
                m_FadedSnapshot.TransitionTo(0.001f);
                m_DefaultSnapshot.TransitionTo(m_FadeDuration);
            }

            yield return StartCoroutine(BeginFade(m_FadeColor, m_FadeOutColor, m_FadeDuration));
            m_FadeImage.transform.parent.gameObject.SetActive(false);
        }


        public IEnumerator BeginFadeIn(float duration, bool fadeAudio)
        {
            // Fade in the audio over the given duration.
            if(m_DefaultSnapshot && fadeAudio)
            {
                m_FadedSnapshot.TransitionTo(0.001f);
                m_DefaultSnapshot.TransitionTo(m_FadeDuration);
            }

            yield return StartCoroutine(BeginFade(m_FadeColor, m_FadeOutColor, duration));
            m_FadeImage.transform.parent.gameObject.SetActive(false);
        }


        private IEnumerator BeginFade(Color startCol, Color endCol, float duration)
        {
            // Does not allow input while fading
            MySceneManager.mySceneManager.acceptInput = false;
            
            // Sets CameraCanvas that contains the fade to true
            m_FadeImage.transform.parent.gameObject.SetActive(true);

            // Fading is now happening.  This ensures it won't be interupted by non-coroutine calls.
            m_IsFading = true;

            // Execute this loop once per frame until the timer exceeds the duration.
            float timer = 0f;
            while (timer <= duration)
            {
                // Set the colour based on the normalised time.
                m_FadeImage.color = Color.Lerp(startCol, endCol, timer / duration);

                // Increment the timer by the time between frames and return next frame.
                timer += Time.deltaTime;
                yield return null;
            }

            // Fading is finished so allow other fading calls again.
            m_IsFading = false;

            // Reallows input after fading
            MySceneManager.mySceneManager.acceptInput = true;

            // If anything is subscribed to OnFadeComplete call it.
            if (OnFadeComplete != null)
                OnFadeComplete();
        }
    }
}