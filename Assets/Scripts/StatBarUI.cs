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

    // Priznak ze objekt byl zrusen - zamezi chybam pri volani po Destroy
    private bool isDestroyed = false;

    private void OnDestroy()
    {
        isDestroyed = true;
    }

    public void Init(string name, float savedValue, float maxWidth)
    {
        if (isDestroyed) return;

        statNameText.text = name;
        maxBarWidth = maxWidth;
        savedEffect = savedValue;

        if (positiveBarImage != null) positiveBarImage.color = positiveColor;
        if (negativeBarImage != null) negativeBarImage.color = negativeColor;

        UpdateBars(savedEffect);
        UpdateText(savedEffect);
    }

    // Slider se pohybuje - aktualizuj graf i text
    public void UpdatePreview(float currentEffect)
    {
        if (isDestroyed || this == null) return;
        UpdateBars(currentEffect);
        UpdateText(currentEffect);
    }

    // Potvrzeno - uloz novy efekt a zkrat max sirku
    public void Confirm(float newEffect)
    {
        if (isDestroyed || this == null) return;
        savedEffect = newEffect;
        maxBarWidth = Mathf.Max(maxBarWidth * 0.5f, 5f);
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
            float width = Mathf.Clamp01(value / 100f) * maxBarWidth;
            SetBarWidth(positiveBar, width);
            SetBarWidth(negativeBar, 0f);
        }
        else if (value < 0f)
        {
            float width = Mathf.Clamp01(-value / 100f) * maxBarWidth;
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
        // Null check - zamezi MissingReferenceException po Destroy
        if (bar == null || isDestroyed) return;
        Vector2 size = bar.sizeDelta;
        size.x = width;
        bar.sizeDelta = size;
    }
}