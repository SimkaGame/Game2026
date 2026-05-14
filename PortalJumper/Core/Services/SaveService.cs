namespace PortalJumper.Core.Services;

using System;
using System.IO;
using System.Text.Json;
using PortalJumper.Core.DTO;

public static class SaveService
{
    private static readonly string SavePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "savegame.json");

    public static void Save(SaveData data)
    {
        var options = new JsonSerializerOptions { WriteIndented = true };
        string json = JsonSerializer.Serialize(data, options);
        File.WriteAllText(SavePath, json);
    }

    public static SaveData? Load()
    {
        if (!File.Exists(SavePath)) return null;
        try
        {
            string json = File.ReadAllText(SavePath);
            return JsonSerializer.Deserialize<SaveData>(json);
        }
        catch
        {
            return null;
        }
    }
}