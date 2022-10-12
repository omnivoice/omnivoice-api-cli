namespace omnivoice
{
    /// <summary>
    /// Enumerates all states of the Omni Voice workflow. The state is used to determine the authentication outcome.
    /// </summary>
    public enum OmniVoiceWorkflowState
    {
        /// <summary>
        /// The user has been identified and passed the authentication challenge.
        /// </summary>
        Authenticated,

        /// <summary>
        /// The user has been identified by the authentication challegenge was either
        /// not attempted or timed out.
        /// </summary>
        Identified,

        /// <summary>
        /// The user has been identified but there is no voiceprint in the system to authenticate
        /// against.
        /// </summary>
        Unenrolled,

        /// <summary>
        /// The user has been identified but failed the authentication challenge (e.g. voiceprint mismatch).
        /// </summary>
        Unauthenticated,

        /// <summary>
        /// Only for "Authorization_Strong" workflow types. The state means that the user has been identified
        /// but Omni Voice has no phone number to send a verification to. The phone number is provided during
        /// enrollment.
        /// </summary>
        No_Modality_For_2nd_Factor,
    };
};
