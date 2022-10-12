namespace omnivoice.HeaderParams {
    using System;
    using System.Collections.Generic;
    using System.Net.Http;

    internal static class HttpHeaderExtensions {
        static Dictionary<HeaderParamType, string> _header_name_map = new Dictionary<HeaderParamType, string>() {
            [HeaderParamType.Channel]                   = "X-OMNIVOICE-CHANNEL",
            [HeaderParamType.Language]                  = "X-OMNIVOICE-LANG",
            [HeaderParamType.Metadata]                  = "X-OMNIVOICE-META",
            [HeaderParamType.SpeakerIdentifier]         = "X-OMNIVOICE-SPEAKER-ID",
            [HeaderParamType.SpeakerIdentifierTypeOnly] = "X-OMNIVOICE-SPEAKER-ID-TYPE-ONLY",
            [HeaderParamType.WorkflowId]                = "X-OMNIVOICE-WORKFLOW-ID",
        };

        static Dictionary<HeaderParamType, Func<object?, bool>> _validator_map = new Dictionary<HeaderParamType, Func<object?, bool>>() {
            [HeaderParamType.Channel]                   = val => val as string == "telephony" || val as string == "web",
            [HeaderParamType.Language]                  = val => LanguageCode.Validate(val as string),
            [HeaderParamType.Metadata]                  = val => val is string && (val == null || (val as string ?? "").Length <= 1024),
            [HeaderParamType.SpeakerIdentifier]         = val => val is SpeakerIdParam && val != null && ((SpeakerIdParam)val).Value != null,
            [HeaderParamType.SpeakerIdentifierTypeOnly] = val => val as string == "phone" || val as string == "account",
            [HeaderParamType.WorkflowId]                = val => val is Guid,
        };

        public static void Set_OmniVoice_Parameter<TParam>(this HttpRequestMessage request, HeaderParamType header, TParam value) {
            if (!_header_name_map.TryGetValue(header, out var header_name))
                throw new ArgumentException($"Unknown header type: '{header}'");
            var validator = _validator_map[header];
            if (!validator(value)) throw new ArgumentException($"Value '{value}' is invalid for header type '{header}'");

            if (request.Headers.Contains(header_name)) {
                request.Headers.Remove(header_name);
            }
            request.Headers.Add(header_name, value?.ToString());
        }

        public static void Set_OmniVoice_ApiKey(this HttpClient client, string api_key) {
            client.DefaultRequestHeaders.TryAddWithoutValidation("Authorization", string.Format("ApiKey_{0}", api_key));
        }
    };
};