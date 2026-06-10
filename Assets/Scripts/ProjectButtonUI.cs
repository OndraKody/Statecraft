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

    public ProjectItem ProjectData => projectData;

    private Button button;
    private Image backgroundImage;
    private Color defaultColor;
    [SerializeField] private Color activeColor = new Color(0.36f, 0.72f, 0.36f, 1f);

    private void Awake()
    {
        button = GetComponent<Button>();
        backgroundImage = GetComponent<Image>();

        if (backgroundImage != null)
            defaultColor = backgroundImage.color;

        if (button != null)
            button.onClick.AddListener(OnClick);

    }

    private void OnEnable()
    {
        RefreshVisualState();
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

    public void RefreshVisualState()
    {
        bool isActive = GameManager.Instance != null && GameManager.Instance.IsProjectActive(projectData);

        if (backgroundImage != null)
            backgroundImage.color = isActive ? activeColor : defaultColor;
    }
}
