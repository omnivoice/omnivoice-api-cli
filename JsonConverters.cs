namespace omnivoice {
    using System;
    using System.Text.Json;
    using System.Text.Json.Serialization;

    public class WorkflowTypeConverter: JsonConverter<OmniVoiceWorkflowType> {
        public override OmniVoiceWorkflowType Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options) {
            var value = reader.GetString()?.ToLower();
            OmniVoiceWorkflowType result = OmniVoiceWorkflowType.Authentication;
            switch (value) {
                case "authentication":
                    result = OmniVoiceWorkflowType.Authentication;
                    break;
                case "authorization-weak":
                    result = OmniVoiceWorkflowType.Authorization_Weak;
                    break;
                case "authorization-strong":
                    result = OmniVoiceWorkflowType.Authorization_Strong;
                    break;
                default:
                    throw new InvalidOperationException(string.Format("Unexpected value for workflow type: '{0}'", value));
            }
            return result;
        }

        public override void Write(Utf8JsonWriter writer, OmniVoiceWorkflowType value, JsonSerializerOptions options) {
            switch(value) {
                case OmniVoiceWorkflowType.Authentication:
                    writer.WriteStringValue("authentication");
                    break;
                case OmniVoiceWorkflowType.Authorization_Weak:
                    writer.WriteStringValue("authorization-weak");
                    break;
                case OmniVoiceWorkflowType.Authorization_Strong:
                    writer.WriteStringValue("authorization-strong");
                    break;
            }
        }
    };

    public class WorkflowStateConverter: JsonConverter<OmniVoiceWorkflowState> {
        public override OmniVoiceWorkflowState Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options) {
            var value = reader.GetString()?.ToLower();
            OmniVoiceWorkflowState result = OmniVoiceWorkflowState.Authenticated;
            switch (value) {
                case "authenticated":
                    result = OmniVoiceWorkflowState.Authenticated;
                    break;
                case "identified":
                    result = OmniVoiceWorkflowState.Identified;
                    break;
                case "unenrolled":
                    result = OmniVoiceWorkflowState.Unenrolled;
                    break;
                case "unauthenticated":
                    result = OmniVoiceWorkflowState.Unauthenticated;
                    break;
                case "no-modality-for-2nd-factor":
                    result = OmniVoiceWorkflowState.No_Modality_For_2nd_Factor;
                    break;
                default:
                    throw new InvalidOperationException(string.Format("Unexpected value for workflow state: '{0}'", value));
            }
            return result;
        }

        public override void Write(Utf8JsonWriter writer, OmniVoiceWorkflowState value, JsonSerializerOptions options) {
            switch (value) {
                case OmniVoiceWorkflowState.Authenticated:
                    writer.WriteStringValue("authenticated");
                    break;
                case OmniVoiceWorkflowState.Identified:
                    writer.WriteStringValue("identified");
                    break;
                case OmniVoiceWorkflowState.Unenrolled:
                    writer.WriteStringValue("unenrolled");
                    break;
                case OmniVoiceWorkflowState.Unauthenticated:
                    writer.WriteStringValue("unauthenticated");
                    break;
                case OmniVoiceWorkflowState.No_Modality_For_2nd_Factor:
                    writer.WriteStringValue("no-modality-for-2nd-factor");
                    break;
            }
        }
    };
};