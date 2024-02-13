using System.IO;
using UnityEngine;

namespace Utility
{
    public static class DeviceUtility
    {
        
        public static void SaveCertificateImageToDevice(Texture2D texture, string imageName)
        {
            string fileName = imageName + ".png";
            byte[] bytes = texture.EncodeToPNG();
            
            string downloadPath = GetDownloadPath();
            string destinationPath = Path.Combine(downloadPath, fileName);
            File.WriteAllBytes(destinationPath, bytes);
        }

        private static string GetDownloadPath()
        {
            string downloadPath;
#if UNITY_ANDROID && !UNITY_EDITOR
            using (AndroidJavaClass Environment = new AndroidJavaClass("android.os.Environment"))
            {
                using (AndroidJavaObject directoryDownloads =
                       Environment.CallStatic<AndroidJavaObject>("getExternalStoragePublicDirectory",
                           Environment.GetStatic<string>("DIRECTORY_DOWNLOADS")))
                {
                    downloadPath = directoryDownloads.Call<string>("getAbsolutePath");
                }
            }
#else
            downloadPath = Application.persistentDataPath;
#endif
            return downloadPath;
        }
        
    } // class
} //namespace