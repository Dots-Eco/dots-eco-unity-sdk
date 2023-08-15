
The SDK makes it easier to integrate Dots.eco certificates into your Unity project by providing out-of-the-box methods and objects for working with Dots.eco [API](https://docs.impact.dots.eco/)

## Capabilities:

* [Create a Certificate](https://docs.impact.dots.eco/endpoints/certificateRequestAllocation/) create a certificate providing impact
* [Get a Certificate](https://docs.impact.dots.eco/endpoints/singleCertificate/) get certificate information
* [Count impact for a user] (TODO)

To start with the SDK, you need to obtain your authentication and app tokens from your [Dots.eco Admin Page](https://impact.dots.eco/manage/).

### Authentication

* You can provide your authentication token by setting the environment variable:

```base
export DOTS_AUTH_TOKEN="YOUR_APP_TOKEN"
```

## Usage

To send requests to Dots.eco servers and receive responses, the SDK provides the `CertificateService` class so you don't have to write the boilerplate code for interacting with our APIs

# Known issues

## License

See the [LICENSE](https://raw.githubusercontent.com/Dots-Eco/dots-eco-unity-sdk/main/LICENSE) file.

## Contacts

* [Support team and feedback](https://www.dots.eco/contact-us/)
* [Engineering team](mailto:engineering@dots.eco)


## Additional resources

* [Dots.eco official website](https://dots.eco)
* [API documentation](https://docs.impact.dots.eco)