using System;
using System.Collections;
using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

namespace DotsEcoCertificateSDK.Impact
{
    public class ImpactView : MonoBehaviour
    {
        [SerializeField] private Image _icon;
        [SerializeField] private TextMeshProUGUI _label;
        [SerializeField] private bool showLogs = false;

        public void Setup(ImpactSummaryProjectEntity summaryProject)
        {
            StartCoroutine(LoadWebTexture(summaryProject.logo, AppLogoTextureLoaded));
            _label.text = summaryProject.unit_text + " " + summaryProject.total + " " + summaryProject.impact_unit;
        }

        public void Setup(ImpactSummaryTotalEntity summaryTotal)
        {
            StartCoroutine(LoadWebTexture(summaryTotal.icon, AppLogoTextureLoaded));
            _label.text = summaryTotal.unit_text + " " + summaryTotal.total + " " + summaryTotal.impact_unit;
        }
        
        private void AppLogoTextureLoaded(Texture2D texture)
        {
            _icon.sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.zero);
        }
        
        private IEnumerator LoadWebTexture(string url, Action<Texture2D> onTextureLoaded, Action onError = null)
        {
            url = Regex.Unescape(url);
    
            using UnityWebRequest textureRequest = UnityWebRequestTexture.GetTexture(url);
    
            yield return textureRequest.SendWebRequest();

            if (textureRequest.result == UnityWebRequest.Result.Success)
            {
                Texture2D loadedTexture = DownloadHandlerTexture.GetContent(textureRequest);
                
                onTextureLoaded?.Invoke(loadedTexture);
        
                if (showLogs) Debug.Log("Texture from web loaded");
            }
            else
            {
                onError?.Invoke();
                
                if (showLogs) Debug.LogError("Failed to load web texture: " + textureRequest.error);
            }
        }
    }
}