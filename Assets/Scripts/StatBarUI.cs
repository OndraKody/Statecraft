using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StatBarUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI statNameText;
    [SerializeField] private RectTransform negativeBar;
    [SerializeField] private Image negativeBarImage;
    [SerializeField] private RectTransform positiveBar;
    [SerializeField] private Image positiveBarImage;
    [SerializeField] private TextMeshProUGUI valueText;

    private Color positiveColor = new Color(0.2f, 0.8f, 0.2f);
    private Color negativeColor = new Color(0.9f, 0.2f, 0.2f);

    private float maxBarWidth = 200f;
    private float savedEffect = 0f;

    // Hodnota pri ktere je bar plny (100%)
    // Pro statistiky = 100, pro skupiny = 2 * baseEffect
    private float scaleMax = 100f;

    private bool isDestroyed = false;

    private void OnDestroy() { isDestroyed = true; }

    // scaleMax = hodnota pri ktere je bar plny
    // Pro stat bary: scaleMax = 100
    // Pro group bary: scaleMax = Mathf.Abs(baseEffect) * 2
    public void Init(string name, float savedValue, float maxWidth, float scaleMax = 100f)
    {
        if (isDestroyed) return;

        statNameText.text = name;
        maxBarWidth = maxWidth;
        savedEffect = savedValue;
        this.scaleMax = Mathf.Max(scaleMax, 1f); // min 1 aby nedoslo k deleni nulou

        if (positiveBarImage != null) positiveBarImage.color = positiveColor;
        if (negativeBarImage != null) negativeBarImage.color = negativeColor;

        UpdateBars(savedEffect);
        UpdateText(savedEffect);
    }

    public void UpdatePreview(float currentEffect)
    {
        if (isDestroyed || this == null) return;
        UpdateBars(currentEffect);
        UpdateText(currentEffect);
    }

    public void Confirm(float newEffect)
    {
        if (isDestroyed || this == null) return;
        savedEffect = newEffect;

        // TENTO ØÁDEK VYMAŽ NEBO ZAKOMENTUJ:
        // maxBarWidth = Mathf.Max(maxBarWidth * 0.5f, 5f); 

        UpdateBars(savedEffect);
        UpdateText(savedEffect);
    }

    public float GetMaxWidth() => maxBarWidth;
    public float GetSavedEffect() => savedEffect;

    private void UpdateBars(float value)
    {
        if (isDestroyed) return;

        if (value > 0f)
        {
            // Dela se scaleMax misto pevneho 100
            float width = Mathf.Clamp01(value / scaleMax) * maxBarWidth;
            SetBarWidth(positiveBar, width);
            SetBarWidth(negativeBar, 0f);
        }
        else if (value < 0f)
        {
            float width = Mathf.Clamp01(-value / scaleMax) * maxBarWidth;
            SetBarWidth(negativeBar, width);
            SetBarWidth(positiveBar, 0f);
        }
        else
        {
            SetBarWidth(positiveBar, 0f);
            SetBarWidth(negativeBar, 0f);
        }
    }

    private void UpdateText(float value)
    {
        if (isDestroyed || valueText == null) return;
        valueText.text = value > 0f
            ? "+" + value.ToString("0.0")
            : value.ToString("0.0");
    }

    private void SetBarWidth(RectTransform bar, float width)
    {
        if (bar == null || isDestroyed) return;
        Vector2 size = bar.sizeDelta;
        size.x = width;
        bar.sizeDelta = size;
    }
}