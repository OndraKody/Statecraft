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
        public float income;
        public float cost;
        public float actionPointCost;
    }

    // ===== UI =====
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI descriptionText;
    public TextMeshProUGUI incomeInput;
    public TextMeshProUGUI costInput;
    public TextMeshProUGUI actionPointInput;

    public Slider slider;
    public Button confirmButton;

    // ===== DATA =====
    private PolicyItem currentItem;

    private float previewIncome;
    private float previewCost;
    private float apCost;

    // ===== SETUP =====
    public void Setup(PolicyItem item)
    {
        currentItem = item;
        gameObject.SetActive(true);

        nameText.text = item.name;
        descriptionText.text = item.description;

        slider.minValue = -100;
        slider.maxValue = 100;
        slider.value = 0;

        incomeInput.text = item.income.ToString("0");
        costInput.text = item.cost.ToString("0");
        actionPointInput.text = "0";

        slider.onValueChanged.RemoveAllListeners();
        slider.onValueChanged.AddListener(UpdatePreview);

        confirmButton.onClick.RemoveAllListeners();
        confirmButton.onClick.AddListener(Confirm);
    }

    // ===== NÁHLED =====
    private void UpdatePreview(float value)
    {
        if (currentItem == null) return;

        float percent = value / 200f; // -50 % až +50 %

        previewIncome = currentItem.income * (1 + percent);
        previewCost = currentItem.cost * (1 + percent);

        apCost = Mathf.Abs(value) * currentItem.actionPointCost;

        incomeInput.text = previewIncome.ToString("0");
        costInput.text = previewCost.ToString("0");
        actionPointInput.text = apCost.ToString("0");
    }

    // ===== POTVRZENÍ =====
    private void Confirm()
    {
        if (GameManager.Instance == null) return;

        if (!GameManager.Instance.HasEnoughActionPoints(apCost))
        {
            Debug.Log("Nedostatek akèních bodù");
            return;
        }

        
        float incomeDelta = previewIncome - currentItem.income;
        float costDelta = previewCost - currentItem.cost;

       
        GameManager.Instance.UseActionPoints(apCost);

       
        GameManager.Instance.IncomeChanger(incomeDelta);
        GameManager.Instance.Expenseschanger(costDelta);

        
        currentItem.income = previewIncome;
        currentItem.cost = previewCost;

        // reset UI
        slider.value = 0;
        actionPointInput.text = "0";

        incomeInput.text = currentItem.income.ToString("0");
        costInput.text = currentItem.cost.ToString("0");
    }

    // ===== TEST DATA =====

}
