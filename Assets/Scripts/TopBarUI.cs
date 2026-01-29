using TMPro;
using UnityEngine;

public class TopBarUI : MonoBehaviour
{
    public TextMeshProUGUI incomeText;
    public TextMeshProUGUI expensesText;
    public TextMeshProUGUI balanceText;
    public TextMeshProUGUI deptText;
    public TextMeshProUGUI actionPointsText;

    private void Update()
    {
        if (GameManager.Instance == null) return;

        incomeText.text = GameManager.Instance.GetIncome().ToString("0");
        expensesText.text = GameManager.Instance.GetExpenses().ToString("0");
        balanceText.text = GameManager.Instance.GetBalance().ToString("0");
        actionPointsText.text = GameManager.Instance.GetActionPoints().ToString("0");
        deptText.text = GameManager.Instance.GetDept().ToString("0");
    }
}
