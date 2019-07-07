# Gosho Security

## Run Application
In order to fully use the functions of this service, you need to :

### Set Up Azure Cognitive Service - Face API

1. Go to [Azure Cognitive Services/Face](https://azure.microsoft.com/en-us/services/cognitive-services/face/).
2. In the *GoshoSecurityAPI/appsettings.json* configuration file insert the Azure Face API Key and given endpoint.

Example:
```
"CognitiveServicesFace": {
    "ApiKey": "e9b4ac498e804b9c9f2131aebfcf65d4",
    "ApiEndpoint": "https://westcentralus.api.cognitive.microsoft.com"
  }
```
### Set Up Cloudinary

1. Create a [Cloudinary Account](https://cloudinary.com/).
2. In the *JobFinder.Web/appsettings.json* configuration file insert your CloudName, ApiKey, ApiSecret.

Example:
```
"CloudinarySettings": {
    "CloudName": "jobfindersite",
    "ApiKey": "9*************7",
    "ApiSecret": "rht*********************Bro"
```

### Configure JSON We Token / JTW

Example:
```
"Jwt": {
    "SecretKey": "Your Secret Key",
    "Issuer": "GoshoSecurity",
    "Audience": "GoshoSecurityUsers"
  }
```

### Enjoy Gosho Security
