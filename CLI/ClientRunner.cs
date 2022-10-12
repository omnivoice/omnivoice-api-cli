namespace omnivoice.CLI;

public class ClientRunner {
    public bool Verbose { get; set; }

    [CliConfig(long_name: "--api-url", short_name: "-u", doc: "URL of the omnivoice biometrics backend; defaults to https://omnivoice.tech/biometrics/api")]
    public string? ApiUrl { get; set; } = "https://omnivoice.tech/biometrics/api";

    [CliConfig(long_name: "--api-key", short_name: "-k", doc: "A required publishable or private API key; see https://omnivoice.tech/biometrics/docs/api-keys for details")]
    public string? ApiKey { get; set; }

    [CliConfig(long_name: "--channel", short_name: "-h", doc: "Channel of the operation; defaults to 'web'")]
    public OmniVoiceChannelType Channel { get; set; } = OmniVoiceChannelType.Web;

    [CliConfig(long_name: "--api-operation", short_name: "-o", doc: "A required API operation to be executed; possible values are: 'voice_verification', 'voice_login', 'voice_login_strong', 'authenticate_wav', 'authenticate_code' and 'enroll'")]
    public ClientOp Op { get; set; } = ClientOp.Unconfigured;

    [CliConfig(long_name: "--id-name", short_name: "-n", doc: "The name of speaker identifier (either 'account' or 'phone'); defaults to 'phone', if not provided; required for 'voice_verification', 'voice_login', 'voice_login_strong' and 'enroll' operations")]
    public SpeakerIdentifierType SpkId_Name { get; set; } = SpeakerIdentifierType.Phone;

    [CliConfig(long_name: "--id-value", short_name: "-v", doc: "The value of speaker identifier; required for 'voice_verification' and 'enroll' operations")]
    public string? SpkId_Value { get; set; }

    [CliConfig(long_name: "--wav-filename", short_name: "-f", doc: "The name of the wav file; required for 'voice_login', 'voice_login_strong', 'authenticate_wav' and 'enroll' operations")]
    public string? WavFilename { get; set; }

    [CliConfig(long_name: "--metadata", short_name: "-m", doc: "A string up to 1024 bytes long; may be optionally provided for any of the API operations")]
    public string? Metadata { get; set; }

    [CliConfig(long_name: "--workflow-id", short_name: "-w", doc: "A workflow uuid identifier that is required for 'authentiate_wav' and 'authenticate_code'")]
    public Guid? WorkflowId { get; set; }

    [CliConfig(long_name: "--verification-code", short_name: "-c", doc: "The verification code is required only for 'authenticate_code'")]
    public string? VerificationCode { get; set; }

    [CliConfig(long_name: "--language-code", short_name: "-l", doc: "A bcp47 language code required for 'voice_login', 'voice_login_strong' and 'authenticate_wav'; see https://omnivoice.tech/biometrics/docs/supported-languages for all supported languages")]
    public string? LanguageCode { get; set; }
    public async Task<ApiCallResult?> Run() {
        var client = new OmniVoiceClient(ApiKey, ApiUrl, Channel);
        switch(Op) {
            default:
                throw new Exception("API operation is not configured; you must specify API operation with '-o'");
            case ClientOp.Authenticate_Code:
                if (!WorkflowId.HasValue) throw new Exception("Workflow Id is required for this operation");
                if (VerificationCode == null) throw new Exception("Verification Code is required for this operation");
                return await client.Authenticate(WorkflowId.Value, VerificationCode);
            case ClientOp.Authenticate_Wav:
                if (!WorkflowId.HasValue) throw new Exception("Workflow Id is required for this operation");
                if (WavFilename == null || !File.Exists(WavFilename)) throw new Exception($"Wav file not found (-f='{WavFilename ?? "null"}')");
                if (LanguageCode == null && Verbose) Console.WriteLine("Language code not specified, defaulting to 'en-US'");
                return await client.Authenticate(WorkflowId.Value, LanguageCode ?? "en-us", WavFilename);
            case ClientOp.Enroll:
                if (WavFilename == null || !File.Exists(WavFilename)) throw new Exception($"Wav file not found (-f='{WavFilename ?? "null"}')");
                if (SpkId_Value == null) throw new Exception("Speaker Identifier value must be provided ('-v' switch)");
                return await client.EnrollVoiceSample(SpkId_Name, SpkId_Value, WavFilename, Metadata ?? "");
            case ClientOp.Voice_Login:
                if (LanguageCode == null && Verbose) Console.WriteLine("Language code not specified, defaulting to 'en-US'");
                if (WavFilename == null || !File.Exists(WavFilename)) throw new Exception($"Wav file not found (-f='{WavFilename ?? "null"}')");
                return await client.VoiceLogin(SpkId_Name, LanguageCode ?? "en-us", WavFilename, Metadata ?? "");
            case ClientOp.Voice_Login_Strong:
                if (LanguageCode == null && Verbose) Console.WriteLine("Language code not specified, defaulting to 'en-US'");
                if (WavFilename == null || !File.Exists(WavFilename)) throw new Exception($"Wav file not found (-f='{WavFilename ?? "null"}')");
                return await client.VoiceLogin_Strong(SpkId_Name, LanguageCode ?? "en-us", WavFilename, Metadata ?? "");
            case ClientOp.Voice_Verification:
                if (SpkId_Value == null) throw new Exception("Speaker Identifier value must be provided ('-v' switch)");
                return await client.StartVoiceVerification(SpkId_Name, SpkId_Value, Metadata ?? "");
        }
    }
};