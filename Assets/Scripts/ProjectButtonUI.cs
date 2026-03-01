using UnityEngine;
using UnityEngine.UI;
using static ProjectPanelUI;

public class ProjectButtonUI : MonoBehaviour
{
    [SerializeField]
    private ProjectItem projectData;        // data této politiky
    [SerializeField]
    private ProjectPanelUI projectPanel;    // spoleèný panel se sliderem
    [SerializeField]
    private GameObject content;

    private Button button;

    private void Awake()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(OnClick);

    }

    private void OnClick()
    {
        if (projectPanel != null && projectData != null)
        {
            projectPanel.SetupProject(projectData);
        }
        else
        {
            Debug.LogWarning("PolicyButtonUI: Chybí data nebo panel");
        }
        content.SetActive(true);
    }
}
