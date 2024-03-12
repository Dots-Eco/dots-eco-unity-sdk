using System;
using UnityEngine;
using UnityEngine.UI;

namespace Utility
{
    [RequireComponent(typeof(Button))]
    public class ShareButton : MonoBehaviour
    {
        [SerializeField] private string shareSubject = "My impact";
        [SerializeField] private string shareMessage = "Check my impact: ";

        public string Link;

        private Button button;

        private bool isFocus = false;
        private bool isProcessing = false;

        private void Awake()
        {
            if (!button)
            {
                button = GetComponent<Button>();
            }
        }

        private void Start()
        {
            button.onClick.AddListener(ShareText);
        }

        private void ShareText()
        {
            
#if (UNITY_ANDROID || UNITY_IOS)
            
            new NativeShare().SetSubject(shareSubject)
                .SetText(shareMessage)
                .SetUrl(Link)
                .Share();      
            
#else

            GUIUtility.systemCopyBuffer = Link;
            
#endif
            
        }
    }
}