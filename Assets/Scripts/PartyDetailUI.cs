using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PartyDetailUI : MonoBehaviour
{
    public TextMeshProUGUI nameText;


    public TextMeshProUGUI ideologyMainText;       // Hhalvní ideologie
    

    public TextMeshProUGUI ideologyS1Text;         // Idel_S1
    public TextMeshProUGUI ideologyS2Text;         // Idel_S2

    public TextMeshProUGUI goal1Text;              // Cil1
    public TextMeshProUGUI goal2Text;              // Cil2
    public TextMeshProUGUI goal3Text;              // Cil3

    public Button selectButton;

    public void Show(JsonLouder.Party data)
    {
        gameObject.SetActive(true);
        nameText.text = data.name;
        ideologyMainText.text = data.ideology;
        ideologyS1Text.text = data.secundery_ideology[0];
        ideologyS2Text.text = data.secundery_ideology[1];
        goal1Text.text = data.goals[0];
        goal2Text.text = data.goals[1];
        goal3Text.text = data.goals[2];

        selectButton.onClick.RemoveAllListeners();
        selectButton.onClick.AddListener(() => OnSelectParty(data));
        // zde bude vyplnìní UI
    }
    private void OnSelectParty(JsonLouder.Party data)
    {
        Debug.Log("Vybral jsi FINÁLNÌ stranu: " + data.name);
        GameManager.Instance.selectedParty = data;
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
