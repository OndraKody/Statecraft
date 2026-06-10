using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ProjectEffectRowUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI effectNameText;
    [SerializeField] private TextMeshProUGUI valueText;
    [SerializeField] private GameObject arrowUp;    // zelena sipka nahoru
    [SerializeField] private GameObject arrowDown;  // cervena sipka dolu

    private Color colorPositive = new Color(0.2f, 0.8f, 0.2f);
    private Color colorNegative = new Color(0.9f, 0.2f, 0.2f);

    // effectName = nazev statistiky nebo skupiny
    // value = hodnota efektu (kladna = dobra, zaporna = spatna)
    // positiveIsGood = true pro HDP/Zdravi/Vzdelani, false pro Kriminalita/Chudoba
    public void Setup(string effectName, float value, bool positiveIsGood = true)
    {
        effectNameText.text = effectName;

        bool isGood = positiveIsGood ? value > 0f : value < 0f;

        // Sipky
        if (arrowUp != null) arrowUp.SetActive(isGood);
        if (arrowDown != null) arrowDown.SetActive(!isGood);

        // Hodnota
        valueText.text = value > 0f ? "+" + value.ToString("0") : value.ToString("0");
        valueText.color = isGood ? colorPositive : colorNegative;
    }
}