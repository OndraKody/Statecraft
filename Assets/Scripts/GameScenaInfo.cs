using TMPro;
using UnityEngine;

public class GameScenaInfo : MonoBehaviour
{
    public TextMeshProUGUI partyNameText;
    public TextMeshProUGUI governmentStatusText;

    private void Start()
    {
        
    }

    
    private bool DetermineGovernment(JsonLouder.Party party)
    {
        return party.seats >= 46; // -> hranici si mùžeš nastavit vlastní
    }
}
