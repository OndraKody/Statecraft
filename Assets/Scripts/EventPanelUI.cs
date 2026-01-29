using TMPro;
using UnityEngine;
using UnityEngine.UI;
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
        currentEvent = gameEvent;
        gameObject.SetActive(true);

        titleText.text = gameEvent.title;
        descriptionText.text = gameEvent.description;

        optionAButton.GetComponentInChildren<TextMeshProUGUI>().text =
            gameEvent.optionA.text;

        optionBButton.GetComponentInChildren<TextMeshProUGUI>().text =
            gameEvent.optionB.text;

        optionAButton.onClick.RemoveAllListeners();
        optionAButton.onClick.AddListener(() => ApplyOption(gameEvent.optionA));

        optionBButton.onClick.RemoveAllListeners();
        optionBButton.onClick.AddListener(() => ApplyOption(gameEvent.optionB));
    }

    private void ApplyOption(EventManager.EventOption option)
    {
        GameManager.Instance.IncomeChanger(option.incomeChange);
        GameManager.Instance.Expenseschanger(option.expenseChange);

        gameObject.SetActive(false);
    }
}
