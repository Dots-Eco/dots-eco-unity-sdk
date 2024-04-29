## Integration:

1. Open DotsEcoCertificateSDK/Data/DotsEcoTokenConfig
2. Set valid AppToken
3. Set valid AuthToken
4. Drag and drop CertificateCanvas.prefab *(DotsEcoCertificateSDK/Prefabs/CertificateCanvas)* to the first scene in your project

API provides Singleton access via CertificateManagerBehaviour.Instance.

## How to use API:

1. Start API before using - call CertificateManagerBehaviour.Instance.Start(string userID);
2. To show the wallet - CertificateManagerBehaviour.Instance.OpenWallet();