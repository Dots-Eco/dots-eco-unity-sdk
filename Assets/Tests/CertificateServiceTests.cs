using NUnit.Framework;

public class CertificateServiceTests
{
    private CertificateService certificateService;
    private const string testAuthToken = "test-auth-token";

    [SetUp]
    public void Setup()
    {
        certificateService = new CertificateService(testAuthToken);
    }

    [Test]
    public void GetCertificateRequest_ValidInputs_CorrectlySetsAuthToken()
    {
        var request = certificateService.GetCertificateRequest("test-app-token", "test-certificate-id");

        Assert.IsNotNull(request);
        Assert.AreEqual(testAuthToken, request.GetRequestHeader("auth-token"));
    }

    [Test]
    public void CreateCertificateRequest_ValidInputs_CorrectlySetsAuthToken()
    {
        var request = certificateService.CreateCertificateRequest("test-app-token", 10, 5, "ezbz");

        Assert.IsNotNull(request);
        Assert.AreEqual(testAuthToken, request.GetRequestHeader("auth-token"));
    }
}
