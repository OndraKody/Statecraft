using System.IO;
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
        string json = File.ReadAllText(GetPath(slot));
        Debug.Log($"Hra byla NAÈTENA z: {GetPath(slot)}");
        return JsonUtility.FromJson<SaveData>(json);
    }

    public static void Delete(int slot)
    {
        if (File.Exists(GetPath(slot))) File.Delete(GetPath(slot));
    }

    public static bool Exists(int slot) => File.Exists(GetPath(slot));
}