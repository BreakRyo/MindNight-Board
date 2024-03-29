using System;
using System.Diagnostics;
using System.IO;

public class Tail
{
    private string _filepath;
    private Action<string> _callback;
    private bool _running;
    private Process _tailProcess;

    public Tail(string filepath, Action<string> callback)
    {
        _filepath = filepath;
        _callback = callback;
        _running = false;
    }

    public void Start()
    {
        if (_running)
            throw new InvalidOperationException("Tail is already running.");

        _running = true;
        

        ProcessStartInfo startInfo = new ProcessStartInfo
        {
            FileName = "powershell",
            Arguments = $"-Command Get-Content '{_filepath}' -Wait -Tail 1 -Encoding UTF8",
            RedirectStandardOutput = true,
            UseShellExecute = false,
            CreateNoWindow = true
        };

        _tailProcess = new Process
        {
            StartInfo = startInfo,
            EnableRaisingEvents = true
        };

        _tailProcess.OutputDataReceived += (sender, e) =>
        {
            if (!string.IsNullOrEmpty(e.Data))
            {
                string[] lines = e.Data.Split('\n');
                foreach (string line in lines)
                {
                    string trimmedLine = line.Trim();
                    if (!string.IsNullOrEmpty(trimmedLine))
                    {
                        Console.WriteLine("[DEBUG] " + trimmedLine);
                        _callback(trimmedLine);
                    }
                }
            }
        };

        _tailProcess.Start();
        _tailProcess.BeginOutputReadLine();
    }

    public void Stop()
    {
        if (_running)
        {
            _running = false;
            _tailProcess.Kill();
            _tailProcess.Dispose();
        }
    }
}