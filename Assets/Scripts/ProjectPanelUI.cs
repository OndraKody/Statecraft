using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;
using System.Collections.Generic;

public class ProjectPanelUI : MonoBehaviour
{
    [System.Serializable]
    public class StatEffect
    {
        public StatType statType;
        public float value;
    }

    [System.Serializable]
    public class GroupEffect
    {
        public GroupType groupType;
        public float value;
    }

    [System.Serializable]
    public class ProjectItem
    {
        // Lokalizovane texty - stejne jako PolicyItem
        public LocalizedString name;
        public LocalizedString description;

        public float expenseBonus;
        public float actionPointCost;
        public int durationInTurns;

        public List<StatEffect> statEffects = new List<StatEffect>();
        public List<GroupEffect> groupEffects = new List<GroupEffect>();
    }

    // ===== UI =====
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private TextMeshProUGUI descriptionText;
    [SerializeField] private TextMeshProUGUI costInput;
    [SerializeField] private TextMeshProUGUI actionPointInput;
    [SerializeField] private TextMeshProUGUI turnsText;
    [SerializeField] private Button confirmButton;

    [SerializeField] private GameObject costLabel;         
    [SerializeField] private GameObject actionPointLabel;
    [SerializeField] private GameObject roudLabel;
    
    // ===== EFEKTY =====
    [SerializeField] private GameObject effectRowPrefab;
    [SerializeField] private Transform effectsParent;

    private ProjectItem currentProject;

    private void OnEnable()
    {
        LocalizationSettings.SelectedLocaleChanged += OnLocaleChanged;
    }

    private void OnLocaleChanged(UnityEngine.Localization.Locale locale)
    {
        if (currentProject != null)
            GenerateEffectRows(currentProject);
    }

    // ===== SETUP =====
    public void SetupProject(ProjectItem item)
    {
        // Odhlasime predchozi lokalizaci
        if (currentProject != null)
        {
            if (currentProject.name != null)
                currentProject.name.StringChanged -= OnNameChanged;
            if (currentProject.description != null)
                currentProject.description.StringChanged -= OnDescriptionChanged;
        }

        currentProject = item;
        gameObject.SetActive(true);

        // Prihlasime se k novym lokalizovanym textum
        if (currentProject.name != null)
            currentProject.name.StringChanged += OnNameChanged;
        if (currentProject.description != null)
            currentProject.description.StringChanged += OnDescriptionChanged;

        bool isActive = GameManager.Instance != null && GameManager.Instance.IsProjectActive(item);
        UpdateProjectInfoMode(item, isActive);

        GenerateEffectRows(item);

        confirmButton.onClick.RemoveAllListeners();
        confirmButton.onClick.AddListener(Confirm);
    }

    private void OnNameChanged(string translatedText) => nameText.text = translatedText;

    public void RefreshCurrentProjectState()
    {
        if (currentProject == null) return;

        bool isActive = GameManager.Instance != null && GameManager.Instance.IsProjectActive(currentProject);
        UpdateProjectInfoMode(currentProject, isActive);
    }

    private void UpdateProjectInfoMode(ProjectItem item, bool isActive)
    {
        if (costLabel != null)
        {
            costLabel.SetActive(!isActive);
        }

        if (roudLabel != null)
        {
            roudLabel.SetActive(!isActive);
        }

        if (actionPointLabel != null)
        {
            actionPointLabel.SetActive(!isActive);
        }
        if (costInput != null)
        {
            costInput.gameObject.SetActive(!isActive);
            if (!isActive) costInput.text = item.expenseBonus.ToString("0");
        }

        if (actionPointInput != null)
        {
            actionPointInput.gameObject.SetActive(!isActive);
            if (!isActive) actionPointInput.text = item.actionPointCost.ToString("0");
        }

        if (turnsText != null)
        {
            turnsText.gameObject.SetActive(true);
            if (isActive)
            {
                int remainingTurns = GameManager.Instance.GetProjectRemainingTurns(item);
                turnsText.text = "Zbyva " + remainingTurns.ToString() + " kol";
            }
            else
            {
                turnsText.text = item.durationInTurns.ToString() + " kol";
            }
        }

        if (confirmButton != null)
            confirmButton.gameObject.SetActive(!isActive);

        if (effectsParent != null)
            effectsParent.gameObject.SetActive(!isActive);
    }
    private void OnDescriptionChanged(string translatedText) => descriptionText.text = translatedText;

    private void OnDisable()
    {
        LocalizationSettings.SelectedLocaleChanged -= OnLocaleChanged;
        if (currentProject != null)
        {
            if (currentProject.name != null)
                currentProject.name.StringChanged -= OnNameChanged;
            if (currentProject.description != null)
                currentProject.description.StringChanged -= OnDescriptionChanged;
        }
    }

    // ===== GENEROVANI RADKU EFEKTU =====
    private void GenerateEffectRows(ProjectItem item)
    {
        for (int i = effectsParent.childCount - 1; i >= 0; i--)
        {
            GameObject child = effectsParent.GetChild(i).gameObject;
            if (child.scene.IsValid()) Destroy(child);
        }

        foreach (var se in item.statEffects)
        {
            if (se.value == 0f) continue;
            bool positiveIsGood = se.statType != StatType.Crime && se.statType != StatType.Poverty;
            CreateEffectRow(LocalizedEnumNames.GetStatName(se.statType), se.value, positiveIsGood);
        }

        foreach (var ge in item.groupEffects)
        {
            if (ge.value == 0f) continue;
            CreateEffectRow(LocalizedEnumNames.GetGroupName(ge.groupType), ge.value, true);
        }
    }

    private void CreateEffectRow(string name, float value, bool positiveIsGood)
    {
        if (effectRowPrefab == null || effectsParent == null) return;

        GameObject rowGO = Instantiate(effectRowPrefab, effectsParent);
        ProjectEffectRowUI row = rowGO.GetComponent<ProjectEffectRowUI>();

        if (row != null)
            row.Setup(name, value, positiveIsGood);
        else
            Debug.LogWarning("ProjectEffectRowUI chybi na prefabu!");
    }

    // ===== POTVRZENI =====
    private void Confirm()
    {
        if (GameManager.Instance.IsProjectActive(currentProject))
        {
            RefreshCurrentProjectState();
            return;
        }

        if (!GameManager.Instance.HasEnoughActionPoints(currentProject.actionPointCost))
        {
            Debug.Log("Nedostatek akcnich bodu");
            return;
        }

        GameManager.Instance.UseActionPoints(currentProject.actionPointCost);
        GameManager.Instance.AddProject(currentProject);
        GameManager.Instance.SaveCurrentGame();

        Debug.Log("Projekt spusten");
        gameObject.SetActive(false);
    }

    private string GetStatName(StatType type)
    {
        switch (type)
        {
            case StatType.HDP: return "HDP";
            case StatType.Crime: return "Kriminalita";
            case StatType.Health: return "Zdravotnictvi";
            case StatType.Education: return "Vzdelani";
            case StatType.Poverty: return "Chudoba";
            default: return type.ToString();
        }
    }

    private string GetGroupName(GroupType type)
    {
        switch (type)
        {
            case GroupType.Poor: return "Chudi";
            case GroupType.MiddleClass: return "Stredni trida";
            case GroupType.Wealthy: return "Bohati";
            case GroupType.Nationalists: return "Nacionaliste";
            case GroupType.Liberals: return "Liberalove";
            case GroupType.Conservatives: return "Konzervativci";
            case GroupType.Capitalists: return "Kapitalist�";
            case GroupType.Socialists: return "Socialist�";
            case GroupType.Religious: return "Nabozenska skupina";
            case GroupType.Environmentalists: return "Environmentalist�";
            default: return type.ToString();
        }
    }
}