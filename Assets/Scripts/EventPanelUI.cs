using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Localization; 
using static EventManager;

public class EventPanelUI : MonoBehaviour
{
    public TextMeshProUGUI titleText;
    public TextMeshProUGUI descriptionText;

    public Button optionAButton;
    public Button optionBButton;

    private EventManager.GameEvent currentEvent;

    public void Show(EventManager.GameEvent gameEvent)
    {
        // 1. Vyèistíme staré odbìry
        CleanupSubscriptions();

        currentEvent = gameEvent;
        gameObject.SetActive(true);

        // 2. Pøihlášení k odbìru zmìn textu
        if (currentEvent != null)
        {
            // Zde to døíve házelo chybu, protože skript neznal UnityEngine.Localization
            currentEvent.title.StringChanged += UpdateTitle;
            currentEvent.description.StringChanged += UpdateDescription;

            // DÙLEŽITÉ: Ve tvém EventManageru musí být u EventOption promìnná 'text' typu LocalizedString!
            if (currentEvent.optionA != null && currentEvent.optionA.text != null)
                currentEvent.optionA.text.StringChanged += UpdateOptionAText;

            if (currentEvent.optionB != null && currentEvent.optionB.text != null)
                currentEvent.optionB.text.StringChanged += UpdateOptionBText;
        }

        // 3. Nastavení tlaèítek
        optionAButton.onClick.RemoveAllListeners();
        optionAButton.onClick.AddListener(() => ApplyOption(gameEvent.optionA));

        optionBButton.onClick.RemoveAllListeners();
        optionBButton.onClick.AddListener(() => ApplyOption(gameEvent.optionB));
    }

    private void UpdateTitle(string translatedText)
    {
        titleText.text = translatedText;
    }

    private void UpdateDescription(string translatedText)
    {
        descriptionText.text = translatedText;
    }

    private void UpdateOptionAText(string translatedText)
    {
        var btnText = optionAButton.GetComponentInChildren<TextMeshProUGUI>();
        if (btnText != null) btnText.text = translatedText;
    }

    private void UpdateOptionBText(string translatedText)
    {
        var btnText = optionBButton.GetComponentInChildren<TextMeshProUGUI>();
        if (btnText != null) btnText.text = translatedText;
    }

    private void CleanupSubscriptions()
    {
        if (currentEvent != null)
        {
            currentEvent.title.StringChanged -= UpdateTitle;
            currentEvent.description.StringChanged -= UpdateDescription;

            if (currentEvent.optionA != null && currentEvent.optionA.text != null)
                currentEvent.optionA.text.StringChanged -= UpdateOptionAText;

            if (currentEvent.optionB != null && currentEvent.optionB.text != null)
                currentEvent.optionB.text.StringChanged -= UpdateOptionBText;
        }
    }

    private void OnDisable()
    {
        CleanupSubscriptions();
    }

    private void ApplyOption(EventManager.EventOption option)
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.IncomeChanger(option.incomeChange);
            GameManager.Instance.Expenseschanger(option.expenseChange);
        }

        gameObject.SetActive(false);
    }
}