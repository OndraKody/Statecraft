using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Localization; // Dùležité!

public class PartySlotUI : MonoBehaviour
{
    public TextMeshProUGUI nameText;
    public Image logoImage;
    public Button selectButton;

    [HideInInspector] public PartyDetailUI detailPanel;
    [HideInInspector] public ScenaLouder scenaLouder;

    private JsonLouder.Party partyData;

    // --- LOKALIZACE ---
    [SerializeField] private string tableName = "StringTable"; // Název tvé tabulky v Unity
    private LocalizedString localizedName;

    public void Setup(JsonLouder.Party data)
    {
        partyData = data;

        // Nastavení lokalizace jména
        CleanupSubscriptions();
        if (!string.IsNullOrEmpty(data.name))
        {
            localizedName = new LocalizedString(tableName, data.name);
            localizedName.StringChanged += UpdateNameText;
        }

        // Nastavení barvy loga
        if (ColorUtility.TryParseHtmlString(data.partyColor, out Color color))
            logoImage.color = color;
        else
            logoImage.color = Color.white;

        // Akce tlaèítka
        selectButton.onClick.RemoveAllListeners();
        selectButton.onClick.AddListener(() => OnSelected(data));
    }

    private void UpdateNameText(string translatedText)
    {
        nameText.text = translatedText;
    }

    private void OnSelected(JsonLouder.Party data)
    {
        if (detailPanel != null) detailPanel.Show(partyData);
        if (scenaLouder != null) scenaLouder.OpenPartyDetail();
    }

    private void CleanupSubscriptions()
    {
        if (localizedName != null) localizedName.StringChanged -= UpdateNameText;
    }

    private void OnDisable()
    {
        CleanupSubscriptions();
    }
}