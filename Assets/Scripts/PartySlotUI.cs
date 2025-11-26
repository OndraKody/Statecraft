using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PartySlotUI : MonoBehaviour
{
    public TextMeshProUGUI nameText;
    public Image logoImage;
    public Button selectButton;

    public PartyDetailUI detailPanel;
    public ScenaLouder scenaLouder;
    private JsonLouder.Party partyData;
    public void Setup(JsonLouder.Party data)
    {
        partyData = data;
        nameText.text = data.name;

        // Nastaví barvu loga
        Color color;
        if (ColorUtility.TryParseHtmlString(data.partyColor, out color))
        {
            logoImage.color = color;
        }
        else
        {
            logoImage.color = Color.white; // fallback
        }

        // Nastaví akci tlaèítka
        selectButton.onClick.RemoveAllListeners();
        selectButton.onClick.AddListener(() => OnSelected(data));
    }
    private void OnSelected(JsonLouder.Party data)
    {
        Debug.Log($"[PartySlotUI] Clicked: {partyData?.name} | detailPanel={(detailPanel != null)} | scenaLouder={(scenaLouder != null)}");

        // Nevolat nic, co je null
        if (detailPanel != null)
        {
            detailPanel.Show(partyData);
        }
        else
        {
            Debug.LogWarning("[PartySlotUI] detailPanel je NULL!");
        }

        if (scenaLouder != null)
        {
            scenaLouder.OpenPartyDetail();
        }
        else
        {
            Debug.LogWarning("[PartySlotUI] scenaLouder je NULL! Nepøepínám panely.");
        }
    }


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
