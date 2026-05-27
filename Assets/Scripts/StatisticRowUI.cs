using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StatisticRowUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private Image barFill;         // Image Type = Filled, Fill Method = Horizontal

    // True = vysoke % je dobre (HDP, Vzdelani, Zdravi) -> zelena
    // False = nizke % je dobre (Kriminalita, Chudoba) -> cervena kdyz vysoke
    [SerializeField] private bool higherIsBetter = true;

    private StatType statType;

    private Color colorGood = new Color(0.2f, 0.8f, 0.2f);  // zelena
    private Color colorBad = new Color(0.9f, 0.2f, 0.2f);  // cervena

    public void Setup(string name, StatType type, bool higherBetter)
    {
        nameText.text = name;
        statType = type;
        higherIsBetter = higherBetter;

        Refresh();
    }

    public void Refresh()
    {
        if (GameManager.Instance == null) return;

        float value = GameManager.Instance.GetStatistic(statType);
        bool good = higherIsBetter ? value >= 50f : value < 50f;

        barFill.fillAmount = value / 100f;
        barFill.color = good ? colorGood : colorBad;
    }
}