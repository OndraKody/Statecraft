using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StatisticRowUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private RectTransform barFill;      // RectTransform - menime sizeDelta.x
    [SerializeField] private Image barFillImage;         // Image - menime barvu

    // Nastav stejne jako sirka BarBG v Unity inspectoru
    [SerializeField] private float maxBarWidth = 200f;

    [SerializeField] private bool higherIsBetter = true;

    private StatType statType;
    private bool isSetup = false;

    private Color colorGood = new Color(0.2f, 0.8f, 0.2f);
    private Color colorBad = new Color(0.9f, 0.2f, 0.2f);

    public void Setup(string name, StatType type, bool higherBetter)
    {
        if (nameText != null) nameText.text = name;
        statType = type;
        higherIsBetter = higherBetter;
        isSetup = true;

        Refresh();
    }

    public void Refresh()
    {
        if (!isSetup || GameManager.Instance == null) return;
        if (barFill == null || barFillImage == null) return;

        float value = GameManager.Instance.GetStatistic(statType);
        bool good = higherIsBetter ? value >= 50f : value < 50f;

        // Sirka baru - stejne jako SocialGroupRowUI
        Vector2 size = barFill.sizeDelta;
        size.x = (value / 100f) * maxBarWidth;
        barFill.sizeDelta = size;

        barFillImage.color = good ? colorGood : colorBad;
    }
}