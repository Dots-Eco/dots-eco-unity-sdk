using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DotsEcoCertificateSDK
{
    public static class CertificateDebugger
    {
        public static void DebugCertificateResponseAsJSONString(CertificateResponse certificateResponse)
        {
            Debug.Log(JsonUtility.ToJson(certificateResponse));
        }
        
        public static void DebugCertificate(CertificateResponse certificateResponse)
        {
            Debug.Log("certificate_id: " + certificateResponse.certificate_id);
            Debug.Log("certificate_url: " + certificateResponse.certificate_url);
            Debug.Log("certificate_image_url: " + certificateResponse.certificate_image_url);
            Debug.Log("app_id: " + certificateResponse.app_id);
            Debug.Log("app_name: " + certificateResponse.app_name);
            Debug.Log("remote_user_id: " + certificateResponse.remote_user_id);
            Debug.Log("name_on_certificate: " + certificateResponse.name_on_certificate);
            Debug.Log("certificate_design: " + certificateResponse.certificate_design);
            Debug.Log("certificate_info: " + certificateResponse.certificate_info);
            Debug.Log("impact_qty: " + certificateResponse.impact_qty);
            Debug.Log("impact_type_id: " + certificateResponse.impact_type_id);
            Debug.Log("impact_type_name: " + certificateResponse.impact_type_name);
            Debug.Log("impact_status: " + certificateResponse.impact_status);
            Debug.Log("created_timestamp: " + certificateResponse.created_timestamp);
            Debug.Log("allocation_id: " + certificateResponse.allocation_id);
            Debug.Log("country: " + certificateResponse.country);

            foreach (Geolocation geolocation in certificateResponse.geolocation)
            {
                Debug.Log($"Geolocation point: lat: {geolocation.lat}, lng: {geolocation.lng}");
            }

            Debug.Log("App logo url: " + certificateResponse.rendering.app.logo_url);

            Debug.Log("Certificate header: " + certificateResponse.rendering.certificate_header);
            Debug.Log("Greeting: " + certificateResponse.rendering.greeting);

            Debug.Log("impact type category: " + certificateResponse.rendering.theme.impact_type_category.icon_url);

            Debug.Log("theme name: " + certificateResponse.rendering.theme.category_theme.name);
            Debug.Log("primary color: " + certificateResponse.rendering.theme.category_theme.primary);
            Debug.Log("secondary color: " + certificateResponse.rendering.theme.category_theme.secondary);
            Debug.Log("tertiary color: " + certificateResponse.rendering.theme.category_theme.tertiary);
            Debug.Log("background color: " + certificateResponse.rendering.theme.category_theme.background);
            Debug.Log("scratch image url: " + certificateResponse.rendering.theme.category_theme.scratch_image_url);

            for (int i = 0; i < certificateResponse.geolocation.Count; i++)
            {
                Debug.Log($"Geolocation point {i}: lat: {certificateResponse.geolocation[i].lat}, " +
                    $"lng: {certificateResponse.geolocation[i].lng}");
            }
        }
    }
}
