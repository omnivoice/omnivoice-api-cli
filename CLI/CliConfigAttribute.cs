namespace omnivoice.CLI;

public class CliConfigAttribute: Attribute {
    public string LongArgName { get; init; }
    public string ShortArgName { get; init; }
    public string DocString { get; init; }

    public CliConfigAttribute(string long_name, string short_name, string doc) {
        LongArgName = long_name;
        ShortArgName = short_name;
        DocString = doc;
    }
};