using UnityEngine;
using UnityEngine.UI;
using Utility;

namespace DotsEcoCertificateSDK
{
    public class CertificateViewSelector : MonoBehaviour
    {
        [SerializeField] private Image scratchImage;

        [SerializeField] private Image verticalImage;
        [SerializeField] private Image horizontalImage;

        [SerializeField] private CanvasHelper canvasHelper;
        
        [SerializeField] private GameObject verticalView;
        [SerializeField] private GameObject horizontalView;

        private void Start()
        {
            verticalView.SetActive(false);
            horizontalView.SetActive(false);

            SelectView(canvasHelper.LastOrientation);
        }

        private void OnEnable()
        {
            canvasHelper.OnOrientationChanged += SelectView;
        }
        
        private void OnDisable()
        {
            canvasHelper.OnOrientationChanged -= SelectView;
        }

        private void SelectView(DeviceScreenOrientation newOrientation)
        {
            if (newOrientation == DeviceScreenOrientation.Vertical)
            {
                verticalView.SetActive(true);
                horizontalView.SetActive(false);

                MoveScratchImage(verticalImage);
            }
            else
            {
                verticalView.SetActive(false);
                horizontalView.SetActive(true);

                MoveScratchImage(horizontalImage);
            }
        }

        private void MoveScratchImage(Image newImage)
        {
            scratchImage.transform.parent = newImage.rectTransform;

            scratchImage.sprite = newImage.sprite;

            scratchImage.rectTransform.anchorMin = new Vector2(0, 0);
            scratchImage.rectTransform.anchorMax = new Vector2(1, 1);
            
            scratchImage.rectTransform.offsetMin = new Vector2(0, 0);
            scratchImage.rectTransform.offsetMax = new Vector2(0, 0);
        }
    }   
}
