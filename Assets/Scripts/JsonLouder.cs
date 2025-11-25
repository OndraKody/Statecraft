using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using UnityEngine;

public class JsonLouder : MonoBehaviour
{
    [System.Serializable]
    public class Party
    {
        public string name;
        public string partyColor; // bude jse používat k reprezentacy strany
        public string ideology;
        public string[] secundery_ideology;
        public int power; // sila v porocen tech ve volbách
        public int seats; //poèet realných pozic v parlamentu
        public string[] goals;
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
        if (!File.Exists(filePath))
        {
            Debug.LogError("Soubor parties.json nenalezen!");
            return;
        }

        string json = File.ReadAllText(filePath);

        PartyWrapper wrapper = JsonUtility.FromJson<PartyWrapper>(json);
        LoadedParties = wrapper.parties;

        Debug.Log("Naèteno stran: " + LoadedParties.Length);

        // výpis do konzole
        for (int i = 0; i < LoadedParties.Length; i++)
        {
            Party p = LoadedParties[i];

            if (string.IsNullOrWhiteSpace(p.name))
            {
                Debug.Log($"Slot #{i}: prázdné místo pro hráèskou stranu");
                continue;
            }

            Debug.Log($"Strana #{i}: {p.name} | Ideologie: {p.ideology} | Mandáty: {p.seats}");
        }
    }

}
