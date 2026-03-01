using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ProjectPanelUI : MonoBehaviour
{
    [System.Serializable]
    public class ProjectItem
    {
        public string name;
        public string description;

        
        public float expenseBonus;

        public float actionPointCost;
        public int durationInTurns;
    }
    [SerializeField]
    private TextMeshProUGUI nameText;
    [SerializeField]
    private TextMeshProUGUI descriptionText;
    [SerializeField]
    private TextMeshProUGUI costInput;
    [SerializeField]
    private TextMeshProUGUI actionPointInput;

    [SerializeField]
    private Button confirmButton;
    private ProjectItem currentProject;

    public void SetupProject(ProjectItem item)
    {
        currentProject = item;
        gameObject.SetActive(true);

        nameText.text = item.name;
        descriptionText.text = item.description;

        
        costInput.text = item.expenseBonus.ToString("0");
        actionPointInput.text = item.actionPointCost.ToString("0");

        confirmButton.onClick.RemoveAllListeners();
        confirmButton.onClick.AddListener(Confirm);
    }

    private void Confirm()
    {
        if (!GameManager.Instance.HasEnoughActionPoints(currentProject.actionPointCost))
        {
            Debug.Log("Nedostatek akèních bodù");
            return;
        }

        GameManager.Instance.UseActionPoints(currentProject.actionPointCost);
        GameManager.Instance.AddProject(currentProject);

        Debug.Log("Projekt spuštìn: " + currentProject.name);

        gameObject.SetActive(false);
    }
}
