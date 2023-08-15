using System;
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.TestTools;

public class GetCertificateRequestBuilderIntegrationTests
{
    private string authToken;

    [SetUp]
    public void SetUp()
    {
        authToken = Environment.GetEnvironmentVariable("DOTS_AUTH_TOKEN");
        if (string.IsNullOrEmpty(authToken))
        {
            throw new ArgumentException("authToken is required!", nameof(authToken));
        }
    }

    [UnityTest]
    public IEnumerator TestIntegraton_GetCertificate_Success()
    {
        var builder = new GetCertificateRequestBuilder(authToken, "514588-6-5");

        var request = builder.BuildRequest();

        yield return request.SendWebRequest();
        CertificateResponse response = JsonUtility.FromJson<CertificateResponse>(request.downloadHandler.text);

        Assert.AreEqual("514588-6-5", response.certificate_id);
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

        Assert.AreEqual(UnityWebRequest.Result.Success, request.result);
    }
}
