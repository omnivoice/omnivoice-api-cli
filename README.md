# OMNI Voice CLI

The easiest way to get started with OMNI Voice is to use the Command Line Interface (CLI) that can be run on Windows, Linux and macOS. Its source code as well as pre-compiled binaries are available on our github repository at: https://github.com/omnivoice/omnivoice-api-cli.

The CLI is built with .net 6 so you'll need to install the runtime (https://dotnet.microsoft.com/en-us/download/dotnet/6.0) to be able to run it.

A complete API reference is available [here](https://omnivoice.tech/biometrics/docs/biometrics-api). See also [Best Practices](https://omnivoice.tech/biometrics/docs/best-practices) that explain the speech sample requirements in detail.

## Prerequisites

Be sure to familiarize yourself with [OMNI Voice Flows](https://omnivoice.tech/biometrics/docs/omnivoice-flows) and [Security Model](https://omnivoice.tech/biometrics/docs/security-model) (at least the "Accessing OMNI Voice API" part) first.

To run the API, you will need to:

1. Obtain API Keys (Publishable and Private)
2. Find out your public IP Address and add it to the ACL https://omnivoice.tech/biometrics/manage-API-keys

## Running the CLI

The CLI accepts command-line parameters the *usual* way and outputs the returned results in JSON format. A complete list of parameters is as follows:
```
> biometrics-api-cli.exe

omnivoice.tech by Omni Intelligence; https://omnivoice.tech/
biometrics-api-cli.exe (Windows) or api-client-cli.dll (Non-Windows)
****************************************************************
Usage: dotnet biometrics-api-cli.dll [--verbose (optional)] [...switches]
Switches:
        -u, --api-url                 URL of the omnivoice biometrics backend; defaults to https://omnivoice.tech/biometrics/api
        -k, --api-key                 A required publishable or private API key; see https://omnivoice.tech/biometrics/docs/api-keys for details
        -h, --channel                 Channel of the operation; defaults to 'web'
        -o, --api-operation           A required API operation to be executed; possible values are: 'voice_verification', 'voice_login', 'voice_login_strong', 'authenticate_wav', 'authenticate_code' and 'enroll'
        -n, --id-name                 The name of speaker identifier (either 'account' or 'phone'); defaults to 'phone', if not provided; required for 'voice_verification', 'voice_login', 'voice_login_strong' and 'enroll' operations
        -v, --id-value                The value of speaker identifier; required for 'voice_verification' and 'enroll' operations
        -f, --wav-filename            The name of the wav file; required for 'voice_login', 'voice_login_strong', 'authenticate_wav' and 'enroll' operations
        -m, --metadata                A string up to 1024 bytes long; may be optionally provided for any of the API operations
        -w, --workflow-id             A workflow uuid identifier that is required for 'authenticate_wav' and 'authenticate_code'
        -c, --verification-code       The verification code is required only for 'authenticate_code'
        -l, --language-code           A bcp47 language code required for 'voice_login', 'voice_login_strong' and 'authenticate_wav'; see https://omnivoice.tech/biometrics/docs/supported-languages for all supported languages
The output of the command is a JSON structure described in detail here: https://omnivoice.tech/biometrics/docs/biometrics-api

```

## CLI Examples

### Enrolment

You will need to record a wav file containing your voice (see also [Best Practices](https://omnivoice.tech/biometrics/docs/best-practices) that explain the speech sample requirements in detail). The wav file format is documented [here](https://omnivoice.tech/biometrics/docs/biometrics-api).

> Note: The mobile phone number must be provided in E.164 format (e.g. +1234567890)

> Note: Enrolment requires a valid phone number; it will not complete until the speaker clicks on the link sent to the mobile phone

```
    > biometrics-api-cli.exe -k [PASTE PRIVATE API KEY HERE] -v [YOUR MOBILE PHONE NUMBER] -o enroll -f [PATH TO WAV AUDIO]
```

### Voice Verification

You will need to record a wav file containing your voice (see also [Best Practices](https://omnivoice.tech/biometrics/docs/best-practices) that explain the speech sample requirements in detail). The wav file format is documented [here](https://omnivoice.tech/biometrics/docs/biometrics-api).

> Note workflow ID - it is returned by the first command and must be passed to the second command with the `-w` key

```
> biometrics-api-cli.exe -k [PASTE PUB API KEY HERE] -v +1234567890 -o voice_verification
{
  "id": "853121cd-63f8-4809-9463-190a70ad5e7f",
  "WorkflowType": "authentication",
  "state": "identified",
  "metadata": "",
  "expired": false,
  "identifier": "\u002B1234567890",
  "authentications": [],
  "success": true,
  "error": null
}
> biometrics-api-cli.exe -k [PASTE PUB API KEY HERE] -v +1234567890 -o authenticate_wav -f [PATH TO WAV AUDIO] -w 853121cd-63f8-4809-9463-190a70ad5e7f
{
  "id": "853121cd-63f8-4809-9463-190a70ad5e7f",
  "WorkflowType": "authentication",
  "state": "authenticated",
  "metadata": "",
  "expired": false,
  "identifier": "\u002B1234567890",
  "authentications": [
    {
      "timestamp": "2022-09-15T03:13:21.4680457+00:00",
      "sampleLength_sec": 6.69,
      "sampleQuality": 1,
      "score": 0.792033600131906
    }
  ],
  "success": true,
  "error": null
}
```

### Voice Log-in

You will need to record a wav file containing your voice (see also [Best Practices](https://omnivoice.tech/biometrics/docs/best-practices) that explain the speech sample requirements in detail). The wav file format is documented [here](https://omnivoice.tech/biometrics/docs/biometrics-api).

```
> biometrics-api-cli.exe -k [PASTE PUB API KEY HERE] -v +1234567890 -o voice_login -f [PATH TO WAV AUDIO]
{
  "id": "49500a56-f2d5-4abd-bc61-fafe6b6bb8d5",
  "WorkflowType": "authentication",
  "state": "authenticated",
  "metadata": "",
  "expired": false,
  "identifier": "\u002B1234567890",
  "authentications": [
    {
      "timestamp": "2022-09-15T03:28:20.2895252+00:00",
      "sampleLength_sec": 6.72,
      "sampleQuality": 1,
      "score": 0.804761457555627
    }
  ],
  "success": true,
  "error": null
}
```

### Voice Log-in Strong

You will need to record a wav file containing your voice (see also [Best Practices](https://omnivoice.tech/biometrics/docs/best-practices) that explain the speech sample requirements in detail). The wav file format is documented [here](https://omnivoice.tech/biometrics/docs/biometrics-api).

> Note workflow ID - it is returned by the first command and must be passed to the second command with the `-w` key

> Note: see state of the workflow returned by the first command - it is "identified" and not "authenticated" meaning that the workflow expects a verification code is expected

The second command passes authentication code sent to the mobile phone.

```
> biometrics-api-cli.exe -k [PASTE PUB API KEY HERE] -v +1234567890 -o voice_login_strong -f [PATH TO WAV AUDIO]
{
  "id": "1824c51f-0eb7-4ba3-8eea-7de9f0712f3e",
  "WorkflowType": "authentication",
  "state": "identified",
  "metadata": "",
  "expired": false,
  "identifier": "\u002B1234567890",
  "authentications": [
    {
      "timestamp": "2022-09-15T03:29:31.0029064+00:00",
      "sampleLength_sec": 6.72,
      "sampleQuality": 1,
      "score": 0.804761457555627
    }
  ],
  "success": true,
  "error": null
}

> biometrics-api-cli.exe -k [PASTE PUB API KEY HERE] -v +1234567890 -o authenticate_code -c [VERIF CODE] -w 1824c51f-0eb7-4ba3-8eea-7de9f0712f3e
```
