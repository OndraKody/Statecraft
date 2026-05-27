using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SocialGroupRowUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private TextMeshProUGUI shareText;
    [SerializeField] private RectTransform satBarFill;
    [SerializeField] private Image satBarFillImage;
    [SerializeField] private TextMeshProUGUI satValueText;
    [SerializeField] private TextMeshProUGUI badgeText;

    [SerializeField] private float maxBarWidth = 120f;

    private Color colorHappy = new Color(0.39f, 0.55f, 0.13f);
    private Color colorNeutral = new Color(0.73f, 0.46f, 0.09f);
    private Color colorUnhappy = new Color(0.64f, 0.18f, 0.18f);

    // Reference na skupinu pro cleanup
    private SocialGroupItem currentGroup;

    public void Setup(SocialGroupItem group)
    {
        // Odhlasime predchozi lokalizaci
        if (currentGroup != null && currentGroup.localizedName != null)
            currentGroup.localizedName.StringChanged -= OnNameChanged;

        currentGroup = group;

        // Prihlasime se k lokalizovanemu nazvu - stejne jako v PolicyItem
        if (group.localizedName != null)
            group.localizedName.StringChanged += OnNameChanged;

        shareText.text = group.power.ToString("0") + " %";
        UpdateSatisfaction(group);
    }

    private void OnNameChanged(string translatedName)
    {
        nameText.text = translatedName;
    }

    private void OnDisable()
    {
        if (currentGroup != null && currentGroup.localizedName != null)
            currentGroup.localizedName.StringChanged -= OnNameChanged;
    }

    private void OnDestroy()
    {
        if (currentGroup != null && currentGroup.localizedName != null)
            currentGroup.localizedName.StringChanged -= OnNameChanged;
    }

    public void UpdateSatisfaction(SocialGroupItem group)
    {
        float sat = group.satisfaction;

        // Sirka baru
        Vector2 size = satBarFill.sizeDelta;
        size.x = (sat / 100f) * maxBarWidth;
        satBarFill.sizeDelta = size;

        satValueText.text = sat.ToString("0") + " %";

        Color c;
        string badge;

        if (sat >= 60f) { c = colorHappy; badge = "Spokojeni"; }
        else if (sat >= 45f) { c = colorNeutral; badge = "Neutralni"; }
        else { c = colorUnhappy; badge = "Nespokojeni"; }

        satBarFillImage.color = c;
        badgeText.text = badge;
        badgeText.color = c;
    }
}