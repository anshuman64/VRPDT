using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.VR;

namespace VRStandardAssets.Utils
{
    // This class simply insures the head tracking behaves correctly when the application is paused.
    public class VRTrackingReset : MonoBehaviour
    {
        private void Awake()
        {
            SceneManager.sceneLoaded += SceneLoaded;
        }

        void SceneLoaded(Scene scene, LoadSceneMode mode)
        {
            InputTracking.Recenter();
        }
        
        private void OnApplicationPause(bool pauseStatus)
        {
            InputTracking.Recenter();
        }
    }
}