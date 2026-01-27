using TMPro;
using UnityEngine;

public class TopBarUI : MonoBehaviour
{
    public TextMeshProUGUI incomeText;
    public TextMeshProUGUI expensesText;
    public TextMeshProUGUI balanceText;
    public TextMeshProUGUI deptText;

    private void Update()
    {
        if (GameManager.Instance == null) return;

        incomeText.text = GameManager.Instance.GetIncome().ToString("0");
        expensesText.text = GameManager.Instance.GetExpenses().ToString("0");
        balanceText.text = GameManager.Instance.GetBalance().ToString("0");
    }
}
