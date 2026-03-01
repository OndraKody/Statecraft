using UnityEngine;

public class PolicyGraphManager : MonoBehaviour
{
    public PieChart incomeChart;
    public PieChart expenseChart;

    public void UpdateGraphs()
    {
        PolicyButtonUI[] buttons = GameManager.Instance.GetPolicy();

        if (buttons.Length == 0)
        {
            Debug.LogWarning("Žádné PolicyButtonUI nenalezeny.");
            return;
        }

        float[] incomeValues = new float[buttons.Length/2];
        float[] expenseValues = new float[buttons.Length/2];
        string[] namesIncome = new string[buttons.Length/2];
        string[] namesExpense = new string[buttons.Length/2];

        for (int i = 0; i < buttons.Length; i++)
        {
            if (buttons[i].policyData.income != 0)
            {
                for (int j = 0; j < incomeValues.Length; j++)
                {
                    incomeValues[j] = buttons[i].policyData.income;
                    namesIncome[j] = buttons[i].policyData.name;
                    Debug.Log("dìlaj icome");
                }
            }
            else
            {
                for (int j = 0; i < expenseValues.Length; j++)
                {
                    expenseValues[j] = buttons[i].policyData.cost;
                    namesExpense[j] = buttons[i].policyData.name;
                    Debug.Log("dìlaj cost");
                }
            }
            Debug.Log("dìlaj");
                
            
        }

        incomeChart.SetValues(incomeValues, namesIncome);
        expenseChart.SetValues(expenseValues, namesExpense);
    }
}