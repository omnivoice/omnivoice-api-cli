namespace omnivoice.HeaderParams {
    internal enum HeaderParamType {
        /// <summary>
        /// Header name: "X-OMNIVOICE-SPEAKER-ID-TYPE-ONLY"
        /// </summary>
        SpeakerIdentifierTypeOnly,
        /// <summary>
        /// Header name: "X-OMNIVOICE-SPEAKER-ID"
        /// </summary>
        SpeakerIdentifier,
        /// <summary>
        /// Header name: "X-OMNIVOICE-META"
        /// </summary>
        Metadata,
        /// <summary>
        /// Header name: "X-OMNIVOICE-CHANNEL"
        /// </summary>
        Channel,
        /// <summary>
        /// Header name: "X-OMNIVOICE-WORKFLOW-ID"
        /// </summary>
        WorkflowId,
        /// <summary>
        /// Header name: "X-OMNIVOICE-LANG"
        /// </summary>
        Language,
    };
};