using UnityEngine;
using System.IO;
using System;

public class Logger : MonoBehaviour
{
    public static Logger Instance;
    private string logFilePath;
    private StreamWriter logWriter;
    private bool isInitialized = false;
    private float lastLogTime = 0f;
    private const float LOG_COOLDOWN = 0.1f; // Minsta tid mellan loggar av samma typ
    private string lastLogMessage = "";
    private int lastLogCount = 0;

    public enum LogLevel
    {
        Debug,      // Detaljerad information för utvecklare
        Info,       // Normal operationell information
        Warning,    // Potentiella problem
        Error       // Kritiska fel
    }

    [SerializeField]
    private LogLevel minimumLogLevel = LogLevel.Debug; // Sätt till Debug för att få med all relevant information

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            InitializeLogger();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void InitializeLogger()
    {
        if (isInitialized) return;

        string logsDirectory = Path.Combine(Application.dataPath, "..", "Docs", "Logs");
        if (!Directory.Exists(logsDirectory))
        {
            Directory.CreateDirectory(logsDirectory);
        }

        string timestamp = DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss");
        logFilePath = Path.Combine(logsDirectory, $"game_log_{timestamp}.txt");
        
        try
        {
            logWriter = new StreamWriter(logFilePath);
            logWriter.WriteLine($"=== Spelloggar {timestamp} ===");
            logWriter.WriteLine($"Logger initialiserad med minimumLogLevel: {minimumLogLevel}");
            logWriter.Flush();
            isInitialized = true;
        }
        catch (Exception e)
        {
            Debug.LogError($"Kunde inte skapa loggfil: {e.Message}");
        }
    }

    public void Log(string message, LogLevel level = LogLevel.Info)
    {
        if (!isInitialized || level < minimumLogLevel) return;

        string timestamp = DateTime.Now.ToString("HH:mm:ss.fff");
        string logMessage = $"[{timestamp}] [{level}] {message}";

        // Kontrollera om detta är en upprepad logg
        if (message == lastLogMessage && Time.time - lastLogTime < LOG_COOLDOWN)
        {
            lastLogCount++;
            return;
        }

        // Om vi har upprepade loggar, skriv ut antalet
        if (lastLogCount > 0)
        {
            string repeatedMessage = $"[{timestamp}] [{level}] (Upprepat {lastLogCount} gånger) {lastLogMessage}";
            WriteToFile(repeatedMessage);
            WriteToUnityConsole(repeatedMessage, level);
            lastLogCount = 0;
        }

        // Skriv den nya loggen
        WriteToFile(logMessage);
        WriteToUnityConsole(logMessage, level);

        lastLogMessage = message;
        lastLogTime = Time.time;
    }

    private void WriteToFile(string message)
    {
        try
        {
            logWriter.WriteLine(message);
            logWriter.Flush();
        }
        catch (Exception e)
        {
            Debug.LogError($"Kunde inte skriva till loggfil: {e.Message}");
        }
    }

    private void WriteToUnityConsole(string message, LogLevel level)
    {
        switch (level)
        {
            case LogLevel.Debug:
                Debug.Log(message);
                break;
            case LogLevel.Info:
                Debug.Log(message);
                break;
            case LogLevel.Warning:
                Debug.LogWarning(message);
                break;
            case LogLevel.Error:
                Debug.LogError(message);
                break;
        }
    }

    void OnDestroy()
    {
        if (logWriter != null)
        {
            logWriter.Close();
        }
    }
} 