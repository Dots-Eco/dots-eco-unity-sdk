using System;
using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.TestTools;

public class CreateCertificateRequestBuilderIntegrationTests
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

    [Ignore(reason: "This is a mutating API call - avoid creating unbound test certificates")]
    [UnityTest]
    public IEnumerator Test_CreateCertificate_Success()
    {
        string authToken = Environment.GetEnvironmentVariable("DOTS_AUTH_TOKEN");
        var builder = new CreateCertificateRequestBuilder(authToken, 5, 10, "ezbz")
                        .WithNameOnCertificate("Eco Hero")
                        .WithRemoteUserEmail("ezbz@dots.eco")
                        .WithSendCertificateByEmail("yes");

        var request = builder.BuildRequest();

        yield return request.SendWebRequest();
        CertificateResponse response = JsonUtility.FromJson<CertificateResponse>(request.downloadHandler.text);

        Assert.IsNotEmpty(response.certificate_id);
        Assert.IsNotEmpty(response.certificate_url);
        Assert.IsNotEmpty(response.certificate_image_url);
        Assert.AreEqual(6, response.app_id);
        Assert.AreEqual(5, response.allocation_id);
        Assert.AreEqual("Dots.eco", response.app_name);
        Assert.AreEqual("ezbz", response.remote_user_id);
        Assert.AreEqual("Eco Hero", response.name_on_certificate);
        Assert.AreEqual(10, response.impact_qty);
        Assert.AreEqual(UnityWebRequest.Result.Success, request.result);
    }
}
