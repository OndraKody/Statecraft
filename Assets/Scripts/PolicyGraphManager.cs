using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Localization; // Tohle možná budeš muset pøidat, pokud tu chybí

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

            // Vytáhneme si aktuální pøeklad pro tento moment
            string translatedName = button.policyData.name.GetLocalizedString();

            if (button.policyData.income > 0)
            {
                incomeValues.Add(button.policyData.income);
                // Použijeme získaný pøeklad
                incomeNames.Add(translatedName);
            }

            if (button.policyData.cost > 0)
            {
                expenseValues.Add(button.policyData.cost);
                // Použijeme získaný pøeklad
                expenseNames.Add(translatedName);
            }
        }

        incomeChart.SetValues(incomeValues.ToArray(), incomeNames.ToArray());
        expenseChart.SetValues(expenseValues.ToArray(), expenseNames.ToArray());
    }
}