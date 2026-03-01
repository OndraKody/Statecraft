using UnityEngine;

public class TurnManeger : MonoBehaviour
{
    public static TurnManeger Instance;

    public int currentTurn = 1;

    [Header("Turn settings")]
    public float actionPointsPerTurn = 20f;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    public void EndTurn()
    {
        ApplyEconomy();
        StartNewTurn();
        UpdateProjects();
        

    }

    private void ApplyEconomy()
    {
        double deficit = GameManager.Instance.GetExpenses() - GameManager.Instance.GetIncome();

        if (deficit > 0)
        {
            GameManager.Instance.AddDebt(deficit);
        }
    }

    private void StartNewTurn()
    {
        currentTurn++;

        GameManager.Instance.AddActionPoints(actionPointsPerTurn);

        EventManager.Instance.TriggerRandomEvent();

        Debug.Log($"Zaèalo kolo {currentTurn}");
    }
    private void UpdateProjects()
    {
        var projects = GameManager.Instance.GetActiveProjects();

        for (int i = projects.Count - 1; i >= 0; i--)
        {
            projects[i].remainingTurns--;

            if (projects[i].remainingTurns <= 0)
            {
                EndProject(projects[i]);
                projects.RemoveAt(i);
            }
        }
    }

    private void EndProject(GameManager.ActiveProject project)
    {
      
        GameManager.Instance.Expenseschanger(-project.data.expenseBonus);

        Debug.Log("Projekt dokonèen: " + project.data.name);

        
    }

}
