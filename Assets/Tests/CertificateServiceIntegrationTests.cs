using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;
using UnityEngine;

namespace DotsEcoCertificateSDK
{
    public class CertificateServiceIntegrationTests
    {
        private CertificateService certificateService;
        private static string TestCertificateId = "514588-6-5";
        private static string TestAppToken = "6-c2b4554f";

        [SetUp]
        public void SetUp()
        {
            certificateService = new CertificateService("TestObject");

        }

        [UnityTest]
        public IEnumerator GetCertificate_ValidData_SuccessCallbackIsInvoked()
        {
            var request = certificateService.GetCertificateRequest(TestAppToken, TestCertificateId);

            yield return request.SendWebRequest();

            CertificateResponse response = JsonUtility.FromJson<CertificateResponse>(request.downloadHandler.text);
            Assert.IsNotNull(response);
            Assert.IsEmpty(request.downloadHandler.error);
            Assert.IsNotNull(request.downloadHandler.text);
            Assert.AreEqual(TestCertificateId, response.certificate_id);
            Assert.AreEqual("https://impact.dots.eco/certificate/7ed883f8-84e5-4c70-a343-8382b92b47e5", response.certificate_url);
            Assert.AreEqual("https://impact.dots.eco/certificate/img/7ed883f8-84e5-4c70-a343-8382b92b47e5.jpg", response.certificate_image_url);
            Assert.AreEqual(6, response.app_id);
            Assert.AreEqual("Dots.eco", response.app_name);
            Assert.AreEqual("ezbz", response.remote_user_id);
            Assert.AreEqual("Eco Hero", response.name_on_certificate);
            Assert.AreEqual("gold", response.certificate_design);
            Assert.AreEqual("12345", response.certificate_info);
            Assert.AreEqual(10, response.impact_qty);
            Assert.AreEqual(40, response.impact_type_id);
            Assert.AreEqual("Plant Trees", response.impact_type_name);
            Assert.AreEqual("In Progress", response.impact_status);
            Assert.AreEqual("1692119842", response.created_timestamp);
            Assert.AreEqual(5, response.allocation_id);
            Assert.AreEqual("Madagascar", response.country);
            Assert.AreEqual(-16.271471999999999, response.geolocation[0].lat);
            Assert.AreEqual(44.446511000000001, response.geolocation[0].lng);
        }

        [UnityTest]
        public IEnumerator GetCertificate_InvalidData_ErrorCallbackIsInvoked()
        {
            var request = certificateService.GetCertificateRequest("invalid_appToken", "invalid_certificateId");

            yield return request.SendWebRequest();

            Assert.IsNotNull(request.downloadHandler.text);
            Assert.IsNotNull(request.downloadHandler.error);

            ErrorResponse response = JsonUtility.FromJson<ErrorResponse>(request.downloadHandler.text);
            Assert.AreEqual("Certificate with ID \u0027invalid_certificateId\u0027 was not found", response.message);
        }

    }
}