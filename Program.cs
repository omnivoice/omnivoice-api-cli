using omnivoice.CLI;

using System.Text.Json;

// args = new string[] {
//     "-k", "xj/iNM5/7rz1sWk6bcHoMwx7ipZOqtcPAZvpBpl+N/U=",
//     "-o", "voice_login",
//     "-f", "workflow_cc9b7dff-d8e8-46c9-be25-fd02d489baa3.wav"
// };

if (args == null || args.Length == 0) {
    Parser.PrintHelp();
    return;
}

try {
    var client_runner = args.Parse();
    var run_result = await client_runner.Run();
    var json_opts = new JsonSerializerOptions() {
        WriteIndented = true,
    };
    if (run_result != null)
        Console.Write(JsonSerializer.Serialize(run_result, run_result.GetType(), json_opts));
    Environment.ExitCode = 0;
}
catch(Exception error) {
    Console.WriteLine("");
    Console.WriteLine("----------------------------");
    Console.WriteLine("Error:");
    Console.WriteLine(error.Message);
    Console.WriteLine("----------------------------");
    Console.WriteLine("");
    Parser.PrintHelp();
    Environment.ExitCode = 1;
}