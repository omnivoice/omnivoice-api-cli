namespace omnivoice
{
    /// <summary>
    /// Enumerates the authentication / login scenario types supported by omnivoice.
    /// </summary>
    public enum OmniVoiceWorkflowType
    {
        /// <summary>
        /// Authentication workflows are started with a call to StartVoiceVerification. The scenario has the following steps:
        /// 1. Start workflow with the user identifier (either phone number of account number)
        /// 2. Complete the workflow with a speech sample
        /// </summary>
        Authentication,

        /// <summary>
        /// This type of workflow is started with a call to StartVoiceLogin. The scenario is a single-step process where
        /// the speech sample contains the user's identifier (either phone number of account number). Omni Voice will run
        /// speech recognition on the sample first, to extract the identifier value (e.g. phone number). Then it will use
        /// the same speech sample to compare it against the user's voiceprint that they were enrolled with.
        /// </summary>
        Authorization_Weak,

        /// <summary>
        /// The workflow is started with a call to StartVoiceLogin_Strong.
        /// This type of workflow is the strongest of all as it's a multi-factor authentication process: it verifies the user's
        /// voice and sends a text message to their cell phone with a code. The code can then be further validated either by
        /// voice (with speech recognition followed by another voice authentication) or by direct submission of the verification
        /// code via a call to Authenticate.
        /// </summary>
        Authorization_Strong
    };
};
