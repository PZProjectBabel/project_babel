using DocChecker;

Console.OutputEncoding = System.Text.Encoding.UTF8;
var exitCode = await DocChecker.DocChecker.RunAsync(args);
Environment.Exit(exitCode);
