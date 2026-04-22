using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Localization; // Tohle musíme pøidat

public class PartyPanelUI : MonoBehaviour
{
    [Header("Textové prvky")]
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI ideologyMainText;
    public TextMeshProUGUI ideologyS1Text;
    public TextMeshProUGUI ideologyS2Text;
    public TextMeshProUGUI goal1Text;
    public TextMeshProUGUI goal2Text;
    public TextMeshProUGUI goal3Text;

    [Header("Èíselné údaje")]
    public TextMeshProUGUI seatsText;
    public TextMeshProUGUI powerText;

    [Header("Nastavení")]
    [SerializeField] private string tableName = "StringTable";

    // Pomocné promìnné pro lokalizaci
    private LocalizedString locName, locMainIdeo, locS1, locS2, locG1, locG2, locG3;

    // Panel se aktualizuje pokaždé, když se zapne (SetActive(true))
    private void OnEnable()
    {
        RefreshPanel();
    }

    public void RefreshPanel()
    {
        if (GameManager.Instance == null) return;

        var party = GameManager.Instance.GetSelectedParty();
        if (party == null) return;

        // 1. Èištìní starých odbìrù
        Cleanup();

        // 2. Lokalizace textù (Klíèe -> Pøeklad)
        // Jméno a hlavní ideologie
        locName = CreateLoc(party.name, nameText);
        locMainIdeo = CreateLoc(party.ideology, ideologyMainText);

        // Vedlejší ideologie (s kontrolou pole)
        if (party.secundery_ideology.Length > 0)
            locS1 = CreateLoc(party.secundery_ideology[0], ideologyS1Text);
        else ideologyS1Text.text = "-";

        if (party.secundery_ideology.Length > 1)
            locS2 = CreateLoc(party.secundery_ideology[1], ideologyS2Text);
        else ideologyS2Text.text = "-";

        // Cíle
        if (party.goals.Length > 0) locG1 = CreateLoc(party.goals[0], goal1Text);
        if (party.goals.Length > 1) locG2 = CreateLoc(party.goals[1], goal2Text);
        if (party.goals.Length > 2) locG3 = CreateLoc(party.goals[2], goal3Text);

        // 3. Èísla (Ty se nepøekládají, tak je dáme rovnou)
        seatsText.text = party.seats.ToString();
        powerText.text = party.power.ToString() + " %";
    }

    // Pomocná funkce, aby kód nebyl moc dlouhý
    private LocalizedString CreateLoc(string key, TextMeshProUGUI targetText)
    {
        if (string.IsNullOrEmpty(key)) return null;
        var loc = new LocalizedString(tableName, key);
        loc.StringChanged += (val) => targetText.text = val;
        return loc;
    }

    private void Cleanup()
    {
        locName = null; locMainIdeo = null;
        locS1 = null; locS2 = null;
        locG1 = null; locG2 = null; locG3 = null;
    }

    private void OnDisable()
    {
        Cleanup();
    }

    public void Open()
    {
        gameObject.SetActive(true);
    }

    public void Close()
    {
        gameObject.SetActive(false);
    }
}