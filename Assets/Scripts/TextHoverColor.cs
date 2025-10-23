using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;

public class TextHoverColor : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public TextMeshProUGUI text;

    [Tooltip("Hex color format: #RRGGBB or #RRGGBBAA")]
    public string normalHex = "#FFFFFF";
    public string hoverHex = "#FFFF00";

    private Color normalColor;
    private Color hoverColor;

    private void Start()
    {
        if (text == null) text = GetComponent<TextMeshProUGUI>();

        if (!ColorUtility.TryParseHtmlString(normalHex, out normalColor))
            normalColor = Color.white;

        if (!ColorUtility.TryParseHtmlString(hoverHex, out hoverColor))
            hoverColor = Color.yellow;

        text.color = normalColor;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        text.color = hoverColor;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        text.color = normalColor;
    }
}
