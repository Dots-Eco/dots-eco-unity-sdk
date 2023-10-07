# dots-eco-unity-sdk

Dots.eco Certificate SDK for the Unity Platform

See [Assets README.md](Assets/DotsEcoCertificateSDK/README.md)

Asset Store Plugin URL: https://assetstore.unity.com/packages/tools/integration/dots-eco-forunity-sdk-263415

Asset Store Publisher URL: https://assetstore.unity.com/publishers/89931

# How to use:

1. In order to uses the SDK you must set the authentication token, it can be provided via an environment varialbe `DOTS_AUTH_TOKENn` (e.g., for CI or testing) or via unity's `EditorPrefs.GetString("DotsEco_AuthToken")` 
2. There are two app tokens in the SDK Configuration editor one for sandbox, used for testing the API and creating certificates via the editor window, and one for production used in the actual game. The production app token create real certificates and provides real (billed) impact so make sure you do not confuse the two. Sandbox certificates expire after some time to allow for a testing period and are not billed and do not make actual impact.


