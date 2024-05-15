using UnityEngine;
using UnityEngine.UI;

namespace DotsEcoCertificateSDK.Scripts.Utility
{
    internal class PlayerPrefsTester : MonoBehaviour
    {
        [SerializeField] private InputField _input;
        [SerializeField] private Button _read;
        [SerializeField] private Button _write;

        public void Write()
        {
            DotsEcoPlayerPrefsHelper.AddScratchedCertificate(_input.text);
        }

        public void Read()
        {
            Debug.LogError("Reading all:");
            var entries = DotsEcoPlayerPrefsHelper.GetScratchedCertificates();
            foreach (var entry in entries)
            {
                Debug.LogError($"Entry: {entry}");
            }
        }
    }
}