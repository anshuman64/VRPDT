using UnityEngine;
using UnityEngine.UI;
using VRStandardAssets.Utils;
using UnityEngine.VR;

namespace VRStandardAssets.Menu
{
    public class MenuItemAnimation : MonoBehaviour
    {
        [SerializeField] private Transform m_Camera;
        [SerializeField] private AudioSource m_ClickAudio;
        [SerializeField] private AudioSource m_AcceptAudio;
        [SerializeField] private GameObject onOverBox;          
        [SerializeField] private float m_Speed = 4f;
        [SerializeField] Color color = Color.white;

        [SerializeField] private bool pop;
        [SerializeField] private bool changeTextColor;
        [SerializeField] private bool changeImageColor;
        [SerializeField] private bool rotate;
        [SerializeField] private bool click;
        [SerializeField] private bool accept;
        [SerializeField] private bool onOver;

        [SerializeField] private float m_PopDistance = 0.35f;
        [SerializeField] private float m_MaxRotation = 25f; 
        [SerializeField] private float m_MinRotation = -25f; 

        private VRInteractiveItem m_Item;
                           
        private Vector3 m_StartPosition;               
        private Vector3 m_PoppedPosition;                
        private Vector3 m_TargetPosition;                  

        private Text m_Text;
        private Image m_Image;
        private Color m_StartColor;
        private Color m_PoppedColor;
        private Color m_TargetColor;

        private Vector3 eulerRotation;
        private Quaternion m_StartRotation;
        private Quaternion m_PoppedRotation;
        private Quaternion m_TargetRotation;


        void Awake()
        {
            m_Item = transform.GetComponent<VRInteractiveItem>();
        }

        void OnEnable()
        {
            m_Item.OnOver += HandleOver;
            m_Item.OnDown += HandleDown;
            m_Item.OnOut += HandleOut;

            if (pop)
            {
                m_StartPosition = transform.position;
                m_PoppedPosition = transform.position - (transform.position - m_Camera.position) * m_PopDistance;
            }

            if (changeTextColor)
            {
                m_Text = transform.GetComponent<Text>();
                m_StartColor = m_Text.color;
                m_PoppedColor = color;
            }

            if (changeImageColor)
            {
                m_Image = transform.GetComponent<Image>();
                m_StartColor = m_Image.color;
                m_PoppedColor = color;
            }

            if (rotate)
            {
                m_StartRotation = transform.rotation;
            }

            if (onOver)
            {
                onOverBox.SetActive(false);
            }
        }

        void OnDisable()
        {
            m_Item.OnOver -= HandleOver;
            m_Item.OnDown -= HandleDown;
            m_Item.OnOut -= HandleOut;

            if (pop)
            {
                transform.position = m_StartPosition;
            }

            if (changeTextColor)
            {
                m_Text.color = m_StartColor;
            }

            if (changeImageColor)
            {
                m_Image.color = m_StartColor;
            }

            if (rotate)
            {
                transform.rotation = m_StartRotation;
            }

            if (onOver)
            {
                onOverBox.SetActive(false);
            }
        }


        private void Update()
        {
            if (pop)
                Pop();
            if (changeTextColor)
                ChangeTextColor();
            if (changeImageColor)
                ChangeImageColor();
            if (rotate)
                Rotate();
        }

        void Pop()
        {
            m_TargetPosition = m_Item.IsOver ? m_PoppedPosition : m_StartPosition;
            transform.position = Vector3.MoveTowards(transform.position, m_TargetPosition, m_Speed * Time.unscaledDeltaTime);
        }

        void ChangeTextColor()
        {
            m_TargetColor = m_Item.IsOver ? m_PoppedColor : m_StartColor;
            m_Text.color = Color.Lerp(m_Text.color, m_TargetColor, m_Speed * Time.unscaledDeltaTime);
        }

        void ChangeImageColor()
        {
            m_TargetColor = m_Item.IsOver ? m_PoppedColor : m_StartColor;
            m_Image.color = Color.Lerp(m_Image.color, m_TargetColor, m_Speed * Time.unscaledDeltaTime);
        }

        void Rotate()
        {
            eulerRotation = transform.rotation.eulerAngles;

            if (VRSettings.loadedDeviceName == "")
            {
                eulerRotation.x = m_Camera.parent.parent.localRotation.eulerAngles.x; // [Mouselook only]
                eulerRotation.y = m_Camera.parent.parent.localRotation.eulerAngles.y; // [Mouselook only]
            }
            else
            {
                eulerRotation.x = InputTracking.GetLocalRotation(VRNode.Head).eulerAngles.x; // [Headlook only]
                eulerRotation.y = InputTracking.GetLocalRotation(VRNode.Head).eulerAngles.y;  // [Headlook only]
            }
            eulerRotation.z = 0;

            if (eulerRotation.y < 270)
                eulerRotation.y += 360;
            if (eulerRotation.x < 270)
                eulerRotation.x += 360;

            eulerRotation.y = Mathf.Clamp(eulerRotation.y, 360 + m_MinRotation, 360 + m_MaxRotation);
            eulerRotation.x = Mathf.Clamp(eulerRotation.x, 360 + m_MinRotation, 360 + m_MaxRotation);

            m_PoppedRotation = Quaternion.Euler(eulerRotation);
            m_TargetRotation = m_Item.IsOver ? m_PoppedRotation : m_StartRotation;
            transform.rotation = Quaternion.Lerp(transform.rotation, m_TargetRotation, m_Speed * Time.unscaledDeltaTime);
        }

        void HandleOver()
        {
            if (click)
                m_ClickAudio.Play();

            if (onOver)
                onOverBox.SetActive(true);
        }

        void HandleOut()
        {
            if (onOver)
                onOverBox.SetActive(false);
        }

        void HandleDown()
        {
            if (accept)
                m_AcceptAudio.Play();
        }
    }
}