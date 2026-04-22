using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Localization; // Nutné pro lokalizaci

public class PartyDetailUI : MonoBehaviour
{
    [Header("UI Reference")]
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI ideologyMainText;
    public TextMeshProUGUI ideologyS1Text;
    public TextMeshProUGUI ideologyS2Text;
    public TextMeshProUGUI goal1Text;
    public TextMeshProUGUI goal2Text;
    public TextMeshProUGUI goal3Text;
    public Button selectButton;

    [Header("Nastavení")]
    [SerializeField] private string tableName = "StringTable"; // Název tvé tabulky v Unity

    // Lokalizaèní objekty pro všechna pole
    private LocalizedString locName;
    private LocalizedString locMainIdeo;
    private LocalizedString locS1Ideo;
    private LocalizedString locS2Ideo;
    private LocalizedString locGoal1;
    private LocalizedString locGoal2;
    private LocalizedString locGoal3;

    public void Show(JsonLouder.Party data)
    {
        gameObject.SetActive(true);

        // 1. Nejdøíve odhlásíme staré odbìry, aby se texty nemíchaly
        CleanupSubscriptions();

        // 2. Vytvoøíme nové lokalizaèní objekty pomocí klíèù z JSONu a pøihlásíme se ke zmìnám

        // Jméno
        locName = new LocalizedString(tableName, data.name);
        locName.StringChanged += (val) => nameText.text = val;

        // Hlavní ideologie
        locMainIdeo = new LocalizedString(tableName, data.ideology);
        locMainIdeo.StringChanged += (val) => ideologyMainText.text = val;

        // Vedlejší ideologie (pøedpokládáme, že v JSONu jsou aspoò 2)
        if (data.secundery_ideology.Length >= 2)
        {
            locS1Ideo = new LocalizedString(tableName, data.secundery_ideology[0]);
            locS1Ideo.StringChanged += (val) => ideologyS1Text.text = val;

            locS2Ideo = new LocalizedString(tableName, data.secundery_ideology[1]);
            locS2Ideo.StringChanged += (val) => ideologyS2Text.text = val;
        }

        // Cíle (pøedpokládáme, že v JSONu jsou aspoò 3)
        if (data.goals.Length >= 3)
        {
            locGoal1 = new LocalizedString(tableName, data.goals[0]);
            locGoal1.StringChanged += (val) => goal1Text.text = val;

            locGoal2 = new LocalizedString(tableName, data.goals[1]);
            locGoal2.StringChanged += (val) => goal2Text.text = val;

            locGoal3 = new LocalizedString(tableName, data.goals[2]);
            locGoal3.StringChanged += (val) => goal3Text.text = val;
        }

        // Nastavení tlaèítka
        selectButton.onClick.RemoveAllListeners();
        selectButton.onClick.AddListener(() => OnSelectParty(data));
    }

    private void OnSelectParty(JsonLouder.Party data)
    {
        Debug.Log("Vybral jsi FINÁLNÌ stranu: " + data.name);
        // Poznámka: GameSession.SelectedParty teï bude obsahovat data s klíèi, 
        // což je v poøádku, protože i zbytek hry bude používat klíèe pro zobrazení.
        GameSession.SelectedParty = data;
    }

    private void CleanupSubscriptions()
    {
        // Tady ruènì odhlásíme anonymní funkce (lambda), které jsme vytvoøili v Show()
        // Aby to bylo v C# èisté pøi používání anonymních funkcí, 
        // staèí u LocalizedString vymazat delegáty nebo nechat objekt zaniknout.
        // U LocalizedString vytvoøených pøes 'new' staèí reference zahodit.
        locName = null;
        locMainIdeo = null;
        locS1Ideo = null;
        locS2Ideo = null;
        locGoal1 = null;
        locGoal2 = null;
        locGoal3 = null;
    }

    private void OnDisable()
    {
        CleanupSubscriptions();
    }
}