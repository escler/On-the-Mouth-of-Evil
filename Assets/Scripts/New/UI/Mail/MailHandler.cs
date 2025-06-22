using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class MailHandler : MonoBehaviour
{
    public static MailHandler Instance { get; private set; }
    
    [Serializable]
    public class EmailEntry
    {
        public string timestamp;
        public string path; // "good" or "bad"
        public int message_index; // 1 to 5
    }
    
    [Serializable]
    public class EmailLog
    {
        public List<EmailEntry> email_log = new List<EmailEntry>();
    }
    
    private string savePath;
    private EmailLog emailLog;

    private void Awake()
    {
        if (Instance)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        savePath = Path.Combine(Application.persistentDataPath, "email_log.json");
        LoadEmails();
    }

    public void AddEmail(string path)
    {
        if (path != "good" && path != "bad")
        {
            Debug.LogError("Invalid path: " + path);
            return;
        }

        int nextIndex = GetNextMessageIndex(path);
        if (nextIndex > 5)
        {
            Debug.LogWarning("All messages for path '" + path + "' have already been sent.");
            return;
        }

        EmailEntry entry = new EmailEntry
        {
            timestamp = DateTime.UtcNow.ToString("o"),
            path = path,
            message_index = nextIndex
        };

        emailLog.email_log.Add(entry);
        SaveEmails();
        Debug.Log("Email added: " + path + " #" + nextIndex);
    }

    private int GetNextMessageIndex(string path)
    {
        int count = 0;
        foreach (var entry in emailLog.email_log)
        {
            if (entry.path == path)
                count++;
        }
        return count + 1;
    }

    private void LoadEmails()
    {
        if (File.Exists(savePath))
        {
            string json = File.ReadAllText(savePath);
            emailLog = JsonUtility.FromJson<EmailLog>(json);
        }
        else
        {
            emailLog = new EmailLog();
        }
    }

    private void SaveEmails()
    {
        string json = JsonUtility.ToJson(emailLog, true);
        File.WriteAllText(savePath, json);
    }

    public List<EmailEntry> GetEmailLog()
    {
        return emailLog.email_log;
    }

    // Para pruebas (por ejemplo desde un botón)
    [ContextMenu("Test Add Good Email")]
    private void TestAddGoodEmail()
    {
        AddEmail("good");
    }

    [ContextMenu("Test Add Bad Email")]
    private void TestAddBadEmail()
    {
        AddEmail("bad");
    }
    
    public void ResetEmails()
    {
        if (File.Exists(savePath))
        {
            File.Delete(savePath);
            Debug.Log("Email log deleted.");
        }

        emailLog = new EmailLog();
    }
}
