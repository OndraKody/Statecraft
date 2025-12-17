using TMPro;
using UnityEngine;

public class GameScenaInfo : MonoBehaviour
{
    public TextMeshProUGUI partyNameText;
    public TextMeshProUGUI governmentStatusText;

    private void Start()
    {
        var selected = GameManager.Instance.selectedParty;

        if (selected == null)
        {
            partyNameText.text = "Žádná strana nevybrána!";
            return;
        }

        // Ukáže název strany
        partyNameText.text = "Hraješ za: " + selected.name;

        // Zavoláme funkci, která urèí vládu / opozici
        bool jeVlada = DetermineGovernment(selected);

        if (jeVlada)
            governmentStatusText.text = "Tvoje strana je VE VLÁDÌ";
        else
            governmentStatusText.text = "Jsi v OPOZICI";
    }

    
    private bool DetermineGovernment(JsonLouder.Party party)
    {
        return party.seats >= 46; // -> hranici si mùžeš nastavit vlastní
    }
}
