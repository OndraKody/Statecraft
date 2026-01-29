using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    private JsonLouder.Party selectedParty;
    private double expenses;
    private double income;
    private double deficit;
    private double dept;
    private double actionPoints = 100;
    private string gamePhase; 

    public void IncomeChanger(float IncomeChange)
    {
        income += IncomeChange;
    }
    public void Expenseschanger(float ExpensesChange)
    {
        expenses += ExpensesChange;
    }

    public double GetIncome()
    {
        return income;
    }

    public double GetExpenses()
    {
        return expenses;
    }
    public void SetIncome(double StartIncome)
    {
        income = StartIncome;
    }
    public void SetExpensive(double StartExpensive)
    {
        expenses = StartExpensive;
    }
    public double GetBalance()
    {
        return income - expenses;
    }
    public double GetDept()
    {
        return dept;
    }
    public bool HasEnoughActionPoints(float cost)
    {
        return actionPoints >= cost;
    }

    public void UseActionPoints(float cost)
    {
        actionPoints -= cost;
    }

    public double GetActionPoints()
    {
        return actionPoints;
    }
    public void AddDebt(double value)
    {
        dept += value;
    }

    public void AddActionPoints(double value)
    {
        actionPoints += value;
    }

    public JsonLouder.Party GetSelectedParty()
    {
        return selectedParty;
    }
    public void SetParty(JsonLouder.Party party)
    {
        selectedParty = party;
        Debug.Log("GameManager: Uložena strana -> " + party.name);
    }

    private void Awake()
    {

        
        if (Instance == null)
        {
            Instance = this;
            
        }
        else
        {
            Destroy(gameObject);
        }
        PolicyButtonUI[] buttons = FindObjectsOfType<PolicyButtonUI>();

        float totalIncome = 0f;
        float totalExpenses = 0f;

        foreach (var btn in buttons)
        {
            if (btn.policyData == null) continue;

            Debug.Log(
                $"POLICY: {btn.policyData.name} | income: {btn.policyData.income} | cost: {btn.policyData.cost}"
            );

            totalIncome += btn.policyData.income;
            totalExpenses += btn.policyData.cost;
        }

        income = totalIncome;
        expenses = totalExpenses;

        Debug.Log($"[INIT] Income: {income}, Expenses: {expenses}");
    }
    private void Start()
    {
        if (GameSession.SelectedParty != null)
        {
            selectedParty = GameSession.SelectedParty;
        }
    }
}
