using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Localization;

public class PolyciPanelUI : MonoBehaviour
{
    [System.Serializable]
    public class PolicyItem
    {
        public LocalizedString name;
        public LocalizedString description;
        public float income;
        public float cost;
        public float actionPointCost;
    }

    // ===== UI =====
    [SerializeField]
    private TextMeshProUGUI nameText;
    [SerializeField]
    private TextMeshProUGUI descriptionText;
    [SerializeField]
    private TextMeshProUGUI incomeInput;
    [SerializeField]
    private TextMeshProUGUI costInput;
    [SerializeField]
    private TextMeshProUGUI actionPointInput;
    [SerializeField]
    private Slider slider;
    [SerializeField]
    private Button confirmButton;

    // ===== DATA =====
    private PolicyItem currentItem;

    private float previewIncome;
    private float previewCost;
    private float apCost;

    // ===== SETUP =====
    public void Setup(PolicyItem item)
    {
        // 1. Než nastavíme novou policy, musíme odhlásit tu pøedchozí (pokud nìjaká byla)
        if (currentItem != null)
        {
            currentItem.name.StringChanged -= UpdateNameText;
            currentItem.description.StringChanged -= UpdateDescriptionText;
        }

        currentItem = item;
        gameObject.SetActive(true);

        // 2. Tady se pøihlásíme k novým textùm. 
        // Výhoda je, že jakmile pøidáš +=, Unity okamžitì zavolá tu funkci a text rovnou vyplní!
        if (currentItem != null)
        {
            currentItem.name.StringChanged += UpdateNameText;
            currentItem.description.StringChanged += UpdateDescriptionText;
        }

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

    // ===== FUNKCE PRO AKTUALIZACI TEXTÙ =====
    private void UpdateNameText(string translatedText)
    {
        nameText.text = translatedText;
    }

    private void UpdateDescriptionText(string translatedText)
    {
        descriptionText.text = translatedText;
    }

    // DÙLEŽITÉ: Když se panel vypne nebo znièí, musíme zrušit odbìr textù, jinak hrozí memory leak!
    private void OnDisable()
    {
        if (currentItem != null)
        {
            currentItem.name.StringChanged -= UpdateNameText;
            currentItem.description.StringChanged -= UpdateDescriptionText;
        }
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
        FindObjectOfType<PolicyGraphManager>().UpdateGraphs();
    }
}