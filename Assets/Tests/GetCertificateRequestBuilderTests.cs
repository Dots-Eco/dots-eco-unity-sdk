using NUnit.Framework;

public class GetCertificateRequestBuilderTests
{
    [Test]
    public void Constructor_WithValidParameters_SetsRequiredFields()
    {
        var builder = new GetCertificateRequestBuilder("test-app-token", "test-cert-123");

        var request = builder.BuildRequest();

        Assert.IsTrue(request.url.EndsWith("test-cert-123"));
        Assert.AreEqual("test-app-token", request.GetRequestHeader("auth-token"));
    }

    [Test]
    public void Constructor_WithInvalidAppToken_ThrowsException()
    {
        Assert.Throws<System.ArgumentException>(() => new GetCertificateRequestBuilder("", "test-cert-123"));
    }

    [Test]
    public void Constructor_WithInvalidCertificateId_ThrowsException()
    {
        Assert.Throws<System.ArgumentException>(() => new GetCertificateRequestBuilder("test-auth-token", ""));
    }
}
