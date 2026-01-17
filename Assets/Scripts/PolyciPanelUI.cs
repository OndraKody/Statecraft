using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;


public class PolyciPanelUI : MonoBehaviour
{
    [System.Serializable]
    public class PolicyItem
    {
        public string name;
        public string description;
        public float income;          // základní pøíjem
        public float cost;            // základní výdaj
        public float baseValue;       // síla posunu slideru
        public float actionPointCost; // cena za 1 bod slideru
    }

    // ===== UI PRVKY =====
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI descriptionText;

    public TextMeshProUGUI costInput;
    public TextMeshProUGUI incomeInput;

    public TextMeshProUGUI actionPointInput;

    public Slider slider;

    // ===== AKTUÁLNÍ DATA =====
    private PolicyItem currentItem;

    // ===== NASTAVENÍ DAT =====
    public void Setup(PolicyItem item)
    {
        currentItem = item;

        nameText.text = item.name;
        descriptionText.text = item.description;

        slider.minValue = -100;
        slider.maxValue = 100;
        slider.value = 0;

        UpdateValues(0);

        slider.onValueChanged.RemoveAllListeners();
        slider.onValueChanged.AddListener(UpdateValues);
    }

    // ===== AKTUALIZACE HODNOT =====
    private void UpdateValues(float sliderValue)
    {
        if (currentItem == null) return;

        float changePercent = sliderValue / 200f; // -50% až +50%

        float finalIncome = currentItem.income > 0
            ? currentItem.income + (currentItem.income * changePercent)
            : 0;

        float finalCost = currentItem.cost > 0
            ? currentItem.cost + (currentItem.cost * changePercent)
            : 0;

        incomeInput.text = finalIncome.ToString("0");
        costInput.text = finalCost.ToString("0");

        float apCost = Mathf.Abs(sliderValue) * currentItem.actionPointCost;
        actionPointInput.text = apCost.ToString("0");

        if (GameManager.Instance != null)
        {
            GameManager.Instance.income += finalIncome;
            GameManager.Instance.expenses += finalCost;
        }
    }

    // ===== TEST DATA =====
    public PolicyItem testItem1 = new PolicyItem
    {
        name = "Daò z pøíjmu",
        description = "Daò z pøíjmù ovlivòuje státní rozpoèet a ekonomiku.",
        income = 500,
        cost = 0,
        baseValue = 50,
        actionPointCost = 2
    };

    private void Start()
    {
        Setup(testItem1);
    }
}
