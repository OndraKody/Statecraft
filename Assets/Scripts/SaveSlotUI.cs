using TMPro;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;
using UnityEngine.UI;

public class SaveSlotUI : MonoBehaviour
{
    public int slotIndex; // V Inspectoru nastav 1, 2, nebo 3

    [Header("UI Objekty")]
    public GameObject emptyState;    // Objekt s "+"
    public GameObject occupiedState; // Objekt s názvem strany

    [Header("Komponenty")]
    public TextMeshProUGUI partyNameText; // Text v occupiedState
    public Button mainButton;    // Hlavní tlaèítko pøes celý slot
    public Button deleteButton;  // Malé tlaèítko X pro smazání
    public ScenaLouder scenaLouder;

    private void OnEnable()
    {
        RefreshSlot();
    }



    public void RefreshSlot()
    {
        bool hasSave = SaveManager.Exists(slotIndex);
        emptyState.SetActive(!hasSave);
        occupiedState.SetActive(hasSave);

        mainButton.onClick.RemoveAllListeners();

        if (hasSave)
        {
            SaveData data = SaveManager.Load(slotIndex);

            // Kontrola, jestli klíè není prázdný
            if (!string.IsNullOrEmpty(data.partyNameKey))
            {
                // Bezpeèné získání lokalizovaného textu
                partyNameText.text = LocalizationSettings.StringDatabase.GetLocalizedString("jsonParty", data.partyNameKey);
            }
            else
            {
                partyNameText.text = "Neznámá strana";
            }

            mainButton.onClick.AddListener(() => {
                GameSession.CurrentSaveSlot = slotIndex;
                scenaLouder.LoadGame();
            });

            if (deleteButton != null)
            {
                deleteButton.onClick.RemoveAllListeners();
                deleteButton.onClick.AddListener(() => {
                    SaveManager.Delete(slotIndex);
                    RefreshSlot();
                });
            }
        }
        else
        {
            mainButton.onClick.AddListener(() => {
                GameSession.CurrentSaveSlot = slotIndex;
                scenaLouder.OpenPartySelect();
            });
        }
    }
}