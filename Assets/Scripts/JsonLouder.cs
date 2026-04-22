using System.IO;
using UnityEngine;

public class JsonLouder : MonoBehaviour
{
    [System.Serializable]
    public class Party
    {
        public string name; // Teï obsahuje klíè (napø. party_national_union_name)
        public string partyColor;
        public string ideology; // Klíè pro ideologii
        public string[] secundery_ideology; // Pole klíèù
        public int power;
        public int seats;
        public string[] goals; // Pole klíèù
    }

    [System.Serializable]
    public class PartyWrapper
    {
        public Party[] parties;
    }

    public Party[] LoadedParties { get; private set; }
    private string filePath;

    private void Awake()
    {
        filePath = Path.Combine(Application.streamingAssetsPath, "parties.json");
        LoadParties();
    }

    public void LoadParties()
    {
        if (!File.Exists(filePath)) return;
        string json = File.ReadAllText(filePath);
        PartyWrapper wrapper = JsonUtility.FromJson<PartyWrapper>(json);
        LoadedParties = wrapper.parties;
    }
}