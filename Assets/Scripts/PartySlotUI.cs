using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Localization; // Důležité!

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

        SetupLocalization();

        // Nastavení barvy loga
        if (ColorUtility.TryParseHtmlString(data.partyColor, out Color color))
            logoImage.color = color;
        else
            logoImage.color = Color.white;

        // Akce tlačítka
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

    private void SetupLocalization()
    {
        CleanupSubscriptions();

        if (partyData == null || string.IsNullOrEmpty(partyData.name))
            return;

        localizedName = new LocalizedString(tableName, partyData.name);
        localizedName.StringChanged += UpdateNameText;
        localizedName.RefreshString();
    }

    private void OnEnable()
    {
        SetupLocalization();
    }

    private void OnDisable()
    {
        CleanupSubscriptions();
    }
}
