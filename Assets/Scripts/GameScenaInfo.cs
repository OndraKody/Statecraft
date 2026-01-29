using TMPro;
using UnityEngine;

public class GameScenaInfo : MonoBehaviour
{
    public TextMeshProUGUI partyNameText;
    private JsonLouder.Party party;

    private void Start()
    {
        if (GameSession.SelectedParty == null)
        {
            Debug.LogError("Session nemá uloženou stranu!");
            return;
        }

        partyNameText.text = GameSession.SelectedParty.name;
        Debug.Log("Naètena strana ze Session: " + GameSession.SelectedParty.name);
    }

    
    private bool DetermineGovernment(JsonLouder.Party party)
    {
        return party.seats >= 46; // -> hranici si mùžeš nastavit vlastní
    }
}
