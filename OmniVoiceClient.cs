namespace omnivoice {
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Threading.Tasks;
    using System.Text.Json;

    using HeaderParams;

    /// <summary>
    /// Provides a convenient dotnet wrapper around the Omni Voice REST API. The client exposes both, synchronous and asynchronous 
    /// methods for making the REST calls.
    /// </summary>
    public class OmniVoiceClient: IDisposable {
        static Func<string, string> trim = (str) => str?.Trim('/') ?? "";
        string _base_url = "https://omnivoice.tech/biometrics/api";
        string _api_name = "voiceBiometricsApi";
        HttpClient _http_client;
        string _channel;

        string _build_api_uri(params string[] path) {
            List<string> entries = new (path);
            entries.Insert(0, _api_name);
            entries.Insert(0, _base_url);
            return entries
                .Select(e => trim(e))
                .Where(e => !string.IsNullOrWhiteSpace(e))
                .Aggregate((e1, e2) => $"{e1}/{e2}");
        }

        async Task<T?> _process_response<T>(HttpResponseMessage response) where T: ApiCallResult, new() {
            if (!response.IsSuccessStatusCode) {
                return new T() {
                    Error = $"{(int)response.StatusCode} ({response.ReasonPhrase})",
                    Success = false,
                };
            }

            var response_body_str = await response.Content.ReadAsStringAsync();
            try {
                try {
                    var result = JsonSerializer.Deserialize<T>(response_body_str);
                    return result;
                }
                catch {
                    var response_json = JsonSerializer.Deserialize<Dictionary<string, object>>(response_body_str);
                    if (response_json != null && response_json.ContainsKey("error")) {
                        var error = response_json["error"] as string;
                        if (!string.IsNullOrEmpty(error))
                            throw new InvalidOperationException(string.Format("{0}: {1}", response.ReasonPhrase, response_json["error"]));
                        else return default(T);
                    }

                    throw new Exception();
                }
            }
            catch {
                response_body_str = string.IsNullOrWhiteSpace(response_body_str) ? "[EMPTY BODY]" : response_body_str;
                throw new InvalidOperationException($"{(int)response.StatusCode} {response.ReasonPhrase}: {response_body_str}");
            }
        }

        /// <summary>
        /// Requires a Publishable Key.
        /// Initiates an Authentication workflow - the most basic type of authentication scenario
        /// </summary>
        public async Task<OmniVoiceWorkflow?> StartVoiceVerification(SpeakerIdentifierType spk_id_type, string spk_id, string metadata = "") {
            string api_url = _build_api_uri("startVoiceVerification");
            using var request = HttpRequest.GET(api_url);
            request.Set_OmniVoice_Parameter(HeaderParamType.Channel, _channel);
            request.Set_OmniVoice_Parameter(HeaderParamType.SpeakerIdentifier, new SpeakerIdParam(spk_id_type, spk_id));
            request.Set_OmniVoice_Parameter(HeaderParamType.Metadata, metadata);
            using var response = await _http_client.SendAsync(request);
            return await _process_response<OmniVoiceWorkflow>(response);
        }

        /// <summary>
        /// Requires a Publishable Key.
        /// Initiates an Authorization_Weak workflow type. This workflow requires an audio sample with the user saying their phone or account number.
        /// This is not the strongest form but the most user-friendly. The expected file should be a wav sampled at 16KHZ.
        /// </summary>
        public async Task<OmniVoiceWorkflow?> VoiceLogin(SpeakerIdentifierType spk_id_type, string lang_code, string wav_filename, string metadata) {
            string api_url = _build_api_uri("voiceLogin");
            using var request = await HttpRequest.POST(api_url, wav_filename);
            request.Set_OmniVoice_Parameter(HeaderParamType.Language, lang_code);
            request.Set_OmniVoice_Parameter(HeaderParamType.Channel, _channel);
            request.Set_OmniVoice_Parameter(HeaderParamType.SpeakerIdentifierTypeOnly, spk_id_type.ToString().ToLower());
            request.Set_OmniVoice_Parameter(HeaderParamType.Metadata, metadata);
            using var response = await _http_client.SendAsync(request);
            return await _process_response<OmniVoiceWorkflow>(response);
        }

        /// <summary>
        /// Requires a Publishable Key.
        /// Initiates an Authorization_Weak workflow type. This workflow requires an audio sample with the user saying their phone or account number.
        /// This is not the strongest form but the most user-friendly. The expected file should be a wav sampled at 16KHZ.
        /// </summary>
        public async Task<OmniVoiceWorkflow?> VoiceLogin(SpeakerIdentifierType spk_id_type, string lang_code, Stream wav_file, string metadata) {
            string api_url = _build_api_uri("voiceLogin");
            using var request = await HttpRequest.POST(api_url, wav_file);
            request.Set_OmniVoice_Parameter(HeaderParamType.Language, lang_code);
            request.Set_OmniVoice_Parameter(HeaderParamType.Channel, _channel);
            request.Set_OmniVoice_Parameter(HeaderParamType.SpeakerIdentifierTypeOnly, spk_id_type.ToString().ToLower());
            request.Set_OmniVoice_Parameter(HeaderParamType.Metadata, metadata);
            using var response = await _http_client.SendAsync(request);
            return await _process_response<OmniVoiceWorkflow>(response);
        }

        /// <summary>
        /// Requires a Publishable Key.
        /// Initiates the strongest type of the authentication workflow - Authorization_Strong - which is a multi-factor authentication
        /// process. This workflow requires an audio sample with the user saying their phone number. If the audio sample has a valid
        /// phone number and it matches the user voiceprint, an SMS is then sent to their cell phone with a verification code. The code
        /// can then be either entered or submitted using voice to complete the authentication.
        /// </summary>
        public async Task<OmniVoiceWorkflow?> VoiceLogin_Strong(SpeakerIdentifierType spk_id_type, string lang_code, string wav_filename, string metadata) {
            string api_url = _build_api_uri("voiceLogin_Strong");
            using var request = await HttpRequest.POST(api_url, wav_filename);
            request.Set_OmniVoice_Parameter(HeaderParamType.Language, lang_code);
            request.Set_OmniVoice_Parameter(HeaderParamType.Channel, _channel);
            request.Set_OmniVoice_Parameter(HeaderParamType.SpeakerIdentifierTypeOnly, spk_id_type.ToString().ToLower());
            request.Set_OmniVoice_Parameter(HeaderParamType.Metadata, metadata);
            using var response = await _http_client.SendAsync(request);
            return await _process_response<OmniVoiceWorkflow>(response);
        }

        /// <summary>
        /// Requires a Publishable Key.
        /// Initiates the strongest type of the authentication workflow - Authorization_Strong - which is a multi-factor authentication
        /// process. This workflow requires an audio sample with the user saying their phone number. If the audio sample has a valid
        /// phone number and it matches the user voiceprint, an SMS is then sent to their cell phone with a verification code. The code
        /// can then be either entered or submitted using voice to complete the authentication.
        /// </summary>
        public async Task<OmniVoiceWorkflow?> VoiceLogin_Strong(SpeakerIdentifierType spk_id_type, string lang_code, Stream wav_file, string metadata) {
            string api_url = _build_api_uri("voiceLogin_Strong");
            using var request = await HttpRequest.POST(api_url, wav_file);
            request.Set_OmniVoice_Parameter(HeaderParamType.Language, lang_code);
            request.Set_OmniVoice_Parameter(HeaderParamType.Channel, _channel);
            request.Set_OmniVoice_Parameter(HeaderParamType.SpeakerIdentifierTypeOnly, spk_id_type.ToString().ToLower());
            request.Set_OmniVoice_Parameter(HeaderParamType.Metadata, metadata);
            using var response = await _http_client.SendAsync(request);
            return await _process_response<OmniVoiceWorkflow>(response);
        }

        /// <summary>
        /// Completes the authentication process for a previously started Authentication or Authorization_Strong workflow.
        /// </summary>
        public async Task<OmniVoiceWorkflow?> Authenticate(Guid workflow_id, string lang_code, string wav_filename) {
            string api_url = _build_api_uri("authenticate");
            using var request = await HttpRequest.POST(api_url, wav_filename);
            request.Set_OmniVoice_Parameter(HeaderParamType.Language, lang_code);
            request.Set_OmniVoice_Parameter(HeaderParamType.WorkflowId, workflow_id);
            using var response = await _http_client.SendAsync(request);
            return await _process_response<OmniVoiceWorkflow>(response);
        }

        /// <summary>
        /// Completes the authentication process for a previously started Authentication or Authorization_Strong workflow.
        /// </summary>
        public async Task<OmniVoiceWorkflow?> Authenticate(Guid workflow_id, string lang_code, Stream wav_file) {
            string api_url = _build_api_uri("authenticate");
            using var request = await HttpRequest.POST(api_url, wav_file);
            request.Set_OmniVoice_Parameter(HeaderParamType.Language, lang_code);
            request.Set_OmniVoice_Parameter(HeaderParamType.WorkflowId, workflow_id);
            using var response = await _http_client.SendAsync(request);
            return await _process_response<OmniVoiceWorkflow>(response);
        }

        /// <summary>
        /// Requires a Publishable Key.
        /// Completes the authentication process for a previously started Authorization_Strong workflow by submitting the verification code
        /// directly.
        /// </summary>
        public async Task<OmniVoiceWorkflow?> Authenticate(Guid workflow_id, string verification_code) {
            string api_url = _build_api_uri("authenticateWithCode", verification_code);
            using var request = HttpRequest.GET(api_url);
            request.Set_OmniVoice_Parameter(HeaderParamType.WorkflowId, workflow_id);
            using var response = await _http_client.SendAsync(request);
            return await _process_response<OmniVoiceWorkflow>(response);
        }

        /// <summary>
        /// Requires Private Key.
        /// Makes a call to the enrollVoiceSample API to enroll a user's voice sample for voice-printing.
        /// When enrolling phone numbers, make sure they are in the E.164 format (e.g. +[country_code]xxxxxxxxxx).
        /// For more information see https://www.itu.int/rec/T-REC-E.164/.
        /// </summary>
        public async Task<OmniVoiceEnrollVoiceSampleResult?> EnrollVoiceSample(SpeakerIdentifierType spk_id_type, string spk_id, string wav_filename, string metadata) {
            var api_url = _build_api_uri("enrollVoiceSample");
            using var request = await HttpRequest.POST(api_url.ToString(), wav_filename);
            request.Set_OmniVoice_Parameter(HeaderParamType.Channel, _channel);
            request.Set_OmniVoice_Parameter(HeaderParamType.SpeakerIdentifier, new SpeakerIdParam(spk_id_type, spk_id));
            request.Set_OmniVoice_Parameter(HeaderParamType.Metadata, metadata);
            using var response = await _http_client.SendAsync(request);
            return await _process_response<OmniVoiceEnrollVoiceSampleResult>(response);
        }

        /// <summary>
        /// Requires Private Key.
        /// Makes a call to the enrollVoiceSample API to enroll a user's voice sample for voice-printing.
        /// When enrolling phone numbers, make sure they are in the E.164 format (e.g. +[country_code]xxxxxxxxxx).
        /// For more information see https://www.itu.int/rec/T-REC-E.164/.
        /// </summary>
        public async Task<OmniVoiceEnrollVoiceSampleResult?> EnrollVoiceSample(SpeakerIdentifierType spk_id_type, string spk_id, Stream wav_file, string metadata) {
            var api_url = _build_api_uri("enrollVoiceSample");
            using var request = await HttpRequest.POST(api_url.ToString(), wav_file);
            request.Set_OmniVoice_Parameter(HeaderParamType.Channel, _channel);
            request.Set_OmniVoice_Parameter(HeaderParamType.SpeakerIdentifier, new SpeakerIdParam(spk_id_type, spk_id));
            request.Set_OmniVoice_Parameter(HeaderParamType.Metadata, metadata);
            using var response = await _http_client.SendAsync(request);
            return await _process_response<OmniVoiceEnrollVoiceSampleResult>(response);
        }

        public void Dispose() { _http_client.Dispose(); }

        public string BaseUrl => _base_url;

        /// <param name="api_key">
        /// Publishable key provides access to the AuthenticationApi while Private key provides access to both,
        /// EnrollmentApi and AuthenticationApi.
        /// </param>
        public OmniVoiceClient(string? api_key, string? api_url = "https://omnivoice.tech/biometrics/api", OmniVoiceChannelType channel = OmniVoiceChannelType.Web) {
            if (string.IsNullOrWhiteSpace(api_key)) throw new ArgumentException("Publishable key must not be empty or whitespace");
            _channel = channel.ToString().ToLower();
            _base_url = api_url ?? _base_url;
            _http_client = new HttpClient();
            _http_client.DefaultRequestHeaders.Accept.Clear();
            _http_client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            _http_client.DefaultRequestHeaders.Add("User-Agent", "OMNI Voice Client 1.2");
            _http_client.Set_OmniVoice_ApiKey(api_key);
        }
    };
};