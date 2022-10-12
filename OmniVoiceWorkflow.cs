namespace omnivoice {
    using System;
    using System.Text.Json.Serialization;

    /// <summary>
    /// The workflow holds information on the Omni Voice authentication process. The authentication attempt
    /// outcome is determined by checking the workflow state.
    /// </summary>
    public class OmniVoiceWorkflow: ApiCallResult {
        [JsonPropertyName("id")]
        public Guid Id { get; set; }

        /// <summary>
        /// Determines the authentication scenario.
        /// </summary>
        //[JsonPropertyName("workflowType")]
        [JsonConverter(typeof(WorkflowTypeConverter))]
        public OmniVoiceWorkflowType WorkflowType { get; set; }

        /// <summary>
        /// The state is used to determine the authentication outcome.
        /// </summary>
        [JsonPropertyName("state")]
        [JsonConverter(typeof(WorkflowStateConverter))]
        public OmniVoiceWorkflowState State { get; set; }

        /// <summary>
        /// The metadata is stored as part of the Omni Voice enrollments and can contain any kind of
        /// data associated with a user. For example, this could be the internal user ID.
        /// The metadata is limited to 1024 bytes of internal storage.
        /// </summary>
        [JsonPropertyName("metadata")]
        public string? Metadata { get; set; }

        /// <summary>
        /// Indicates whether the workflow is expired prior to completion. Expired workflows must fail
        /// log-in attempts.
        /// </summary>
        [JsonPropertyName("expired")]
        public bool Expired { get; set; }

        /// <summary>
        /// The value of the speaker identifier that the voice-biometrics system has found during its identification process.
        /// </summary>
        [JsonPropertyName("identifier")]
        public string? Identifier { get; set; }

        [JsonPropertyName("authentications")]
        public List<VoiceAuthentication> Authentications { get; set; } = new ();

        public override string ToString() {
            var result = string.Format(
                "{0}: {1} [Id={2}, Expired={3}, Meta={4}]",
                WorkflowType, State, Id, Expired, Metadata
            );
            return result;
        }

        public OmniVoiceWorkflow() {}
    };
};