namespace omnivoice.HeaderParams {
    internal class SpeakerIdParam {
        public string Name { get; private set; }
        public string Value { get; private set; }
        
        public override string ToString() => $"{Name}|{Value}";

        public SpeakerIdParam(SpeakerIdentifierType type, string value) {
            Name = type.ToString().ToLower();
            Value = value;
        }
    };
};