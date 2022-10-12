namespace omnivoice.CLI;

using System.ComponentModel;
using System.Reflection;

public static class Parser {
    static string _help_txt;
    static Dictionary<string, PropertyInfo> _prop_map = new();

    static string _fmt_line(CliConfigAttribute? a) {
        string par = $"{a?.ShortArgName}, {a?.LongArgName}";
        string line = $"\t{par,-30}{a?.DocString}";
        return line;
    }

    static Parser() {
        _help_txt = typeof(ClientRunner)
            .GetProperties()
            .Select(prop => prop.GetCustomAttribute<CliConfigAttribute>(true))
            .Where(a => a != null)
            .Select(a => _fmt_line(a))
            .Aggregate((line1, line2) => $"{line1}\r\n{line2}");
        
        foreach(var prop_info in typeof(ClientRunner).GetProperties()) {
            var cli_config = prop_info.GetCustomAttribute<CliConfigAttribute>(true);
            if (cli_config != null) {
                _prop_map[cli_config.ShortArgName] = prop_info;
                _prop_map[cli_config.LongArgName] = prop_info;
            }
        }
    }

    public static ClientRunner Parse(this string[] args) {
        var result = new ClientRunner();
        result.Verbose = args[0].ToLower() == "--verbose";

        int i = result.Verbose ? 1 : 0;
        while(i < args.Length) {
            var sw = args[i];
            if (_prop_map.TryGetValue(sw, out var prop)) {
                i++;
                string val = i < args.Length ? args[i] : "";
                var prop_type = prop.PropertyType;
                var converter = TypeDescriptor.GetConverter(prop_type);
                var prop_val = converter.ConvertFromString(val);

                if (result.Verbose) Console.WriteLine($"prop_val[{sw}]={prop_val}");

                prop.SetValue(result, prop_val);
            }
            else {
                throw new Exception($"Switch '{sw}' is unknown");
            }

            i++;
        }

        return result;
    }

    public static void PrintHelp() {
        Console.WriteLine("omnivoice.tech by Omni Intelligence; https://omnivoice.tech/");
        Console.WriteLine("biometrics-api-cli.exe (Windows) or api-client-cli.dll (Non-Windows)");
        Console.WriteLine("****************************************************************");
        Console.WriteLine("Usage: dotnet biometrics-api-cli.dll [--verbose (optional)] [...switches]");
        Console.WriteLine("Switches:");
        Console.WriteLine(_help_txt);
        Console.WriteLine("The output of the command is a JSON structure described in detail here: https://omnivoice.tech/biometrics/docs/biometrics-api");
    }
};