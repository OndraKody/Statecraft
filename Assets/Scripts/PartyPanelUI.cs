using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PartyPanelUI : MonoBehaviour
{
    public TextMeshProUGUI nameText;

    public TextMeshProUGUI ideologyMainText;
    public TextMeshProUGUI ideologyS1Text;
    public TextMeshProUGUI ideologyS2Text;

    public TextMeshProUGUI goal1Text;
    public TextMeshProUGUI goal2Text;
    public TextMeshProUGUI goal3Text;

    public TextMeshProUGUI seatsText;
    public TextMeshProUGUI powerText;

    private void Start()
    {
        
        
        
    }
    private void Update()
    {
        if (GameManager.Instance == null)
        {
            Debug.LogError("GameManager neexistuje");
            return;
        }

        var party = GameManager.Instance.GetSelectedParty();
        if (party == null)
        {
            Debug.LogError("SelectedParty je NULL");
            return;
        }

        nameText.text = party.name;
        ideologyMainText.text = party.ideology;

        ideologyS1Text.text = party.secundery_ideology.Length > 0 ? party.secundery_ideology[0] : "-";
        ideologyS2Text.text = party.secundery_ideology.Length > 1 ? party.secundery_ideology[1] : "-";

        goal1Text.text = party.goals.Length > 0 ? party.goals[0] : "-";
        goal2Text.text = party.goals.Length > 1 ? party.goals[1] : "-";
        goal3Text.text = party.goals.Length > 2 ? party.goals[2] : "-";

        seatsText.text = party.seats.ToString();
        powerText.text = party.power.ToString() + " %";
    }
    public void Open()
    {
        

        

    }

    public void Close()
    {
        gameObject.SetActive(false);
    }
}
