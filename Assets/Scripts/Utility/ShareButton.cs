using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Utility
{
    [RequireComponent(typeof(Button))]
    public class ShareButton : MonoBehaviour
    {
        [SerializeField] private string sharePopUpText = "Share your certificate";
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

        private void OnApplicationFocus(bool focus)
        {
            isFocus = focus;
        }

        private void ShareText()
        {

#if UNITY_ANDROID && !UNITY_EDITOR

            if (!isProcessing)
            {
                StartCoroutine(ShareTextInAndroid());
            }

#else
            GUIUtility.systemCopyBuffer = Link;
#endif

        }
        
        private IEnumerator ShareRoutine(string text)
        {
            yield return new WaitForEndOfFrame();
            Social.localUser.Authenticate((bool success) =>
            {
                if (success)
                {
                    Social.ReportProgress("Achievement01", 100.0, (bool success2) =>
                    {
                        // Share the text
                        Social.ReportProgress("Achievement02", 100.0, (bool success3) =>
                        {
                            // Perform additional actions after sharing
                        });
                    });
                }
            });
        }

#if UNITY_ANDROID && !UNITY_EDITOR

        private IEnumerator ShareTextInAndroid()
        {
            isProcessing = true;

            if (!Application.isEditor)
            {
                // Create intent for action send
                AndroidJavaClass intentClass = new AndroidJavaClass("android.content.Intent");
                AndroidJavaObject intentObject = new AndroidJavaObject("android.content.Intent");
                intentObject.Call<AndroidJavaObject>("setAction", intentClass.GetStatic<string>("ACTION_SEND"));

                // Put text and subject extra
                intentObject.Call<AndroidJavaObject>("setType", "text/plain");

                intentObject.Call<AndroidJavaObject>("putExtra", intentClass.GetStatic<string>("EXTRA_SUBJECT"),
                    shareSubject);
                intentObject.Call<AndroidJavaObject>("putExtra", intentClass.GetStatic<string>("EXTRA_TEXT"),
                    shareMessage + "\n" + Link);

                // Call createChooser method of activity class
                AndroidJavaClass unity = new AndroidJavaClass("com.unity3d.player.UnityPlayer");

                AndroidJavaObject currentActivity = unity.GetStatic<AndroidJavaObject>("currentActivity");
                AndroidJavaObject chooser = intentClass.CallStatic<AndroidJavaObject>
                    ("createChooser", intentObject, "Share your certificate");

                currentActivity.Call("startActivity", chooser);
            }

            yield return new WaitUntil(() => isFocus);
            isProcessing = false;
        }
        
#endif
        
    }
}