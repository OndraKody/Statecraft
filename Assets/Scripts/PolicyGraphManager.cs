using UnityEngine;
using System.Collections.Generic;

public class PolicyGraphManager : MonoBehaviour
{
    public PieChart incomeChart;
    public PieChart expenseChart;

    public void UpdateGraphs()
    {
        PolicyButtonUI[] buttons = FindObjectsOfType<PolicyButtonUI>(true);
        

        List<float> incomeValues = new List<float>();
        List<string> incomeNames = new List<string>();

        List<float> expenseValues = new List<float>();
        List<string> expenseNames = new List<string>();

        foreach (var button in buttons)
        {
            if (button.policyData == null)
                continue;

            if (button.policyData.income > 0)
            {
                incomeValues.Add(button.policyData.income);
                incomeNames.Add(button.policyData.name);
            }

            if (button.policyData.cost > 0)
            {
                expenseValues.Add(button.policyData.cost);
                expenseNames.Add(button.policyData.name);
            }
        }

        incomeChart.SetValues(incomeValues.ToArray(), incomeNames.ToArray());
        expenseChart.SetValues(expenseValues.ToArray(), expenseNames.ToArray());
    }
}