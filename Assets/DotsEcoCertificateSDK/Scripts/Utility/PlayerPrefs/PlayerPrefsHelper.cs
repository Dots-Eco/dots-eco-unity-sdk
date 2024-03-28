using System.Collections.Generic;
using UnityEngine;

namespace DotsEcoCertificateSDK.Scripts.Utility
{
    internal static class PlayerPrefsHelper
    {
        public static void AddScratchedCertificate(string certificateID)
        {
            var key = Configurations.Instance.PlayerPrefsConfig.ScratchedCertificatesKey;
            var data = ReadJson(key, new ScratchedCertificatesData());
            if (data.ScratchedCertificateIDs.Contains(certificateID) == false)
                data.ScratchedCertificateIDs.Add(certificateID);
            WriteJson(key, data);
        }

        public static List<string> GetScratchedCertificates()
        {
            var key = Configurations.Instance.PlayerPrefsConfig.ScratchedCertificatesKey;
            var data = ReadJson(key, new ScratchedCertificatesData());
            return data?.ScratchedCertificateIDs;
        }

        public static bool IsScratched(string certificateID, bool defaultValue = false)
        {
            var key = Configurations.Instance.PlayerPrefsConfig.ScratchedCertificatesKey;
            var data = ReadJson(key, new ScratchedCertificatesData());
            return data.ScratchedCertificateIDs.Contains(certificateID);
        }

        private static TType ReadJson<TType>(string playerPrefsKey, TType defaultValue = null) where TType : class
        {
            var jsonString = PlayerPrefs.GetString(playerPrefsKey, string.Empty);
            var data = JsonUtility.FromJson<TType>(jsonString);
            return data ?? defaultValue;
        }

        private static void WriteJson<TType>(string playerPrefsKey, TType data)
        {
            var jsonString = JsonUtility.ToJson(data);
            if (string.IsNullOrEmpty(jsonString))
            {
                Debug.LogWarning($"Converting data of type {typeof(TType)} to JSON failed. Skipping write to '{playerPrefsKey}'");
                Debug.LogWarning($"Data: {data}");
                return;
            }

            PlayerPrefs.SetString(playerPrefsKey, jsonString);
            PlayerPrefs.Save();
        }
    }
}