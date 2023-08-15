using NUnit.Framework;

public class CreateCertificateRequestBuilderTests
{
    [Test]
    public void Constructor_WithValidParameters_SetsRequiredFields()
    {
        var builder = new CreateCertificateRequestBuilder("test-app-token", 10, 1, "ezbz");

        var request = builder.BuildRequest();

        string requestBody = System.Text.Encoding.UTF8.GetString(request.uploadHandler.data);
        Assert.IsTrue(requestBody.Contains("test-app-token"));
        Assert.IsTrue(requestBody.Contains("10"));
        Assert.IsTrue(requestBody.Contains("1"));
        Assert.IsTrue(requestBody.Contains("ezbz"));
    }

    [Test]
    public void WithNameOnCertificate_SetsNameOnCertificate()
    {
        var builder = new CreateCertificateRequestBuilder("test-app-token", 10, 1, "ezbz")
                        .WithNameOnCertificate("Eco Hero");

        var request = builder.BuildRequest();
        string requestBody = System.Text.Encoding.UTF8.GetString(request.uploadHandler.data);
        Assert.IsTrue(requestBody.Contains("Eco Hero"));
    }
    [Test]
    public void Constructor_WithInvalidAppToken_ThrowsException()
    {
        Assert.Throws<System.ArgumentException>(() => new CreateCertificateRequestBuilder("", 10, 1, "ezbz"));
    }
}
