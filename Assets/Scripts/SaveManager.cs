using System.IO;
using System;
using UnityEngine;

public static class SaveManager
{
    // Cesta k souboru (napø. C:/Users/.../AppData/LocalLow/TvujProjekt/save_1.json)
    private static string GetPath(int slot) => Path.Combine(Application.persistentDataPath, $"save_{slot}.json");

    public static void Save(int slot, SaveData data)
    {
        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText(GetPath(slot), json);
        Debug.Log($"Hra byla ULOŽENA do: {GetPath(slot)}");
    }

    public static SaveData Load(int slot)
    {
        if (!File.Exists(GetPath(slot))) return null;
        try
        {
            string json = File.ReadAllText(GetPath(slot));
            SaveData data = JsonUtility.FromJson<SaveData>(json);
            if (data == null) throw new InvalidDataException("Save soubor neobsahuje platná data.");
            Debug.Log($"Hra byla NAÈTENA z: {GetPath(slot)}");
            return data;
        }
        catch (Exception exception)
        {
            Debug.LogError($"Save ve slotu {slot + 1} nelze naèíst: {exception.Message}");
            return null;
        }
    }

    public static void Delete(int slot)
    {
        if (File.Exists(GetPath(slot))) File.Delete(GetPath(slot));
    }

    public static bool Exists(int slot) => File.Exists(GetPath(slot));
}