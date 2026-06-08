using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Localization;
using System.Collections.Generic;

public class PolyciPanelUI : MonoBehaviour
{
    // ===== GROUP EFFECT =====
    [System.Serializable]
    public class GroupEffect
    {
        public GroupType groupType;
        public float baseEffect;

        [HideInInspector] public float savedEffect;
        [HideInInspector] public float maxWidth = 200f;
        [HideInInspector] public bool initialized = false;

        public void Initialize()
        {
            if (!initialized)
            {
                savedEffect = baseEffect;
                initialized = true;
            }
        }
    }

    // ===== POLICY ITEM =====
    [System.Serializable]
    public class PolicyItem
    {
        public LocalizedString name;
        public LocalizedString description;
        public float income;
        public float cost;
        public float actionPointCost;

        public float hdpEffect;
        public float crimeEffect;
        public float healthEffect;
        public float educationEffect;
        public float povertyEffect;

        [HideInInspector] public float savedHdpEffect;
        [HideInInspector] public float savedCrimeEffect;
        [HideInInspector] public float savedHealthEffect;
        [HideInInspector] public float savedEducationEffect;
        [HideInInspector] public float savedPovertyEffect;

        [HideInInspector] public float maxWidthHdp = 200f;
        [HideInInspector] public float maxWidthCrime = 200f;
        [HideInInspector] public float maxWidthHealth = 200f;
        [HideInInspector] public float maxWidthEducation = 200f;
        [HideInInspector] public float maxWidthPoverty = 200f;

        public List<GroupEffect> groupEffects = new List<GroupEffect>();

        [HideInInspector] public bool initialized = false;

        public void Initialize()
        {
            if (!initialized)
            {
                savedHdpEffect = hdpEffect;
                savedCrimeEffect = crimeEffect;
                savedHealthEffect = healthEffect;
                savedEducationEffect = educationEffect;
                savedPovertyEffect = povertyEffect;

                foreach (var ge in groupEffects)
                    ge.Initialize();

                initialized = true;
            }
        }
    }

    // ===== UI =====
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private TextMeshProUGUI descriptionText;
    [SerializeField] private TextMeshProUGUI incomeInput;
    [SerializeField] private TextMeshProUGUI costInput;
    [SerializeField] private TextMeshProUGUI actionPointInput;
    [SerializeField] private Slider slider;
    [SerializeField] private Button confirmButton;

    [SerializeField] private GameObject statBarPrefab;
    [SerializeField] private Transform statBarsParent;
    [SerializeField] private float initialBarWidth = 200f;

    // ===== DATA =====
    private PolicyItem currentItem;
    private float previewIncome;
    private float previewCost;
    private float apCost;

    private StatBarUI hdpBar;
    private StatBarUI crimeBar;
    private StatBarUI healthBar;
    private StatBarUI educationBar;
    private StatBarUI povertyBar;

    private Dictionary<GroupType, StatBarUI> groupBars = new Dictionary<GroupType, StatBarUI>();

    // ===== SETUP =====
    public void Setup(PolicyItem item)
    {
        if (currentItem != null)
        {
            currentItem.name.StringChanged -= UpdateNameText;
            currentItem.description.StringChanged -= UpdateDescriptionText;
        }

        currentItem = item;
        currentItem.Initialize();

        gameObject.SetActive(true);

        currentItem.name.StringChanged += UpdateNameText;
        currentItem.description.StringChanged += UpdateDescriptionText;

        slider.minValue = -100;
        slider.maxValue = 100;
        slider.value = 0;

        previewIncome = item.income;
        previewCost = item.cost;
        apCost = 0f;

        incomeInput.text = item.income.ToString("0");
        costInput.text = item.cost.ToString("0");
        actionPointInput.text = "0";

        slider.onValueChanged.RemoveAllListeners();
        slider.onValueChanged.AddListener(UpdatePreview);

        confirmButton.onClick.RemoveAllListeners();
        confirmButton.onClick.AddListener(Confirm);

        GenerateStatBars(item);
        GenerateGroupBars(item);
    }

    private void UpdateNameText(string t) => nameText.text = t;
    private void UpdateDescriptionText(string t) => descriptionText.text = t;

    private void OnDisable()
    {
        if (currentItem != null)
        {
            currentItem.name.StringChanged -= UpdateNameText;
            currentItem.description.StringChanged -= UpdateDescriptionText;
        }
    }

    // ===== GENEROVANI STAT BARU =====
    private void GenerateStatBars(PolicyItem item)
    {
        if (statBarsParent == null) { Debug.LogWarning("statBarsParent neni prirazen!"); return; }

        for (int i = statBarsParent.childCount - 1; i >= 0; i--)
        {
            GameObject child = statBarsParent.GetChild(i).gameObject;
            if (child.scene.IsValid()) Destroy(child);
        }

        // Místo item.maxWidthHdp použijeme natvrdo initialBarWidth
        hdpBar = CreateBar(statBarsParent, "HDP", item.savedHdpEffect, item.hdpEffect, initialBarWidth);
        crimeBar = CreateBar(statBarsParent, "Zlocin", item.savedCrimeEffect, item.crimeEffect, initialBarWidth);
        healthBar = CreateBar(statBarsParent, "Zdravi", item.savedHealthEffect, item.healthEffect, initialBarWidth);
        educationBar = CreateBar(statBarsParent, "Vzdelanost", item.savedEducationEffect, item.educationEffect, initialBarWidth);
        povertyBar = CreateBar(statBarsParent, "Chudoba", item.savedPovertyEffect, item.povertyEffect, initialBarWidth);
    }

    // ===== GENEROVANI GROUP BARU =====
    private void GenerateGroupBars(PolicyItem item)
    {
        if (statBarsParent == null) return;

        groupBars.Clear();

        foreach (var ge in item.groupEffects)
        {
            if (ge.baseEffect == 0f) continue;

            string groupName = GetGroupName(ge.groupType);
            float scaleMax = Mathf.Abs(ge.baseEffect) * 2f;

            // Místo ge.maxWidth pøedáme initialBarWidth
            StatBarUI bar = CreateBar(statBarsParent, groupName, ge.savedEffect, ge.baseEffect, initialBarWidth, scaleMax);
            if (bar != null)
                groupBars[ge.groupType] = bar;
        }
    }

    // Vytvori bar - scaleMax defaultne 100 pro stat bary
    private StatBarUI CreateBar(Transform parent, string name, float savedValue, float baseEffect, float maxWidth, float scaleMax = 100f)
    {
        if (baseEffect == 0f) return null;
        if (parent == null || !parent.gameObject.scene.IsValid())
        {
            Debug.LogWarning("CreateBar: parent neni validni scene objekt!");
            return null;
        }

        GameObject barGO = Instantiate(statBarPrefab, parent);
        StatBarUI bar = barGO.GetComponent<StatBarUI>();

        if (bar != null)
            bar.Init(name, savedValue, maxWidth, scaleMax);
        else
            Debug.LogWarning("StatBarUI komponenta chybi na prefabu!");

        return bar;
    }

    // ===== NAHLED =====
    private void UpdatePreview(float value)
    {
        if (currentItem == null) return;

        float percent = value / 200f;

        previewIncome = currentItem.income * (1f + percent);
        previewCost = currentItem.cost * (1f + percent);
        apCost = Mathf.Abs(value) * currentItem.actionPointCost;

        incomeInput.text = previewIncome.ToString("0");
        costInput.text = previewCost.ToString("0");
        actionPointInput.text = apCost.ToString("0");

        if (hdpBar != null) hdpBar.UpdatePreview(currentItem.savedHdpEffect * (1f + percent));
        if (crimeBar != null) crimeBar.UpdatePreview(currentItem.savedCrimeEffect * (1f + percent));
        if (healthBar != null) healthBar.UpdatePreview(currentItem.savedHealthEffect * (1f + percent));
        if (educationBar != null) educationBar.UpdatePreview(currentItem.savedEducationEffect * (1f + percent));
        if (povertyBar != null) povertyBar.UpdatePreview(currentItem.savedPovertyEffect * (1f + percent));

        foreach (var ge in currentItem.groupEffects)
        {
            if (groupBars.TryGetValue(ge.groupType, out StatBarUI bar) && bar != null)
                bar.UpdatePreview(ge.savedEffect * (1f + percent));
        }
    }

    // ===== POTVRZENI =====
    private void Confirm()
    {
        if (GameManager.Instance == null) return;
        if (!GameManager.Instance.HasEnoughActionPoints(apCost))
        {
            Debug.Log("Nedostatek akcnich bodu");
            return;
        }

        float percent = slider.value / 200f;

        GameManager.Instance.UseActionPoints(apCost);
        GameManager.Instance.IncomeChanger(previewIncome - currentItem.income);
        GameManager.Instance.Expenseschanger(previewCost - currentItem.cost);

        float newHdp = currentItem.savedHdpEffect * (1f + percent);
        float newCrime = currentItem.savedCrimeEffect * (1f + percent);
        float newHealth = currentItem.savedHealthEffect * (1f + percent);
        float newEducation = currentItem.savedEducationEffect * (1f + percent);
        float newPoverty = currentItem.savedPovertyEffect * (1f + percent);

        GameManager.Instance.ChangeStatistic(StatType.HDP, newHdp - currentItem.savedHdpEffect);
        GameManager.Instance.ChangeStatistic(StatType.Crime, newCrime - currentItem.savedCrimeEffect);
        GameManager.Instance.ChangeStatistic(StatType.Health, newHealth - currentItem.savedHealthEffect);
        GameManager.Instance.ChangeStatistic(StatType.Education, newEducation - currentItem.savedEducationEffect);
        GameManager.Instance.ChangeStatistic(StatType.Poverty, newPoverty - currentItem.savedPovertyEffect);

        hdpBar?.Confirm(newHdp);
        crimeBar?.Confirm(newCrime);
        healthBar?.Confirm(newHealth);
        educationBar?.Confirm(newEducation);
        povertyBar?.Confirm(newPoverty);

        currentItem.income = previewIncome;
        currentItem.cost = previewCost;
        currentItem.savedHdpEffect = newHdp;
        currentItem.savedCrimeEffect = newCrime;
        currentItem.savedHealthEffect = newHealth;
        currentItem.savedEducationEffect = newEducation;
        currentItem.savedPovertyEffect = newPoverty;

        if (hdpBar != null) currentItem.maxWidthHdp = hdpBar.GetMaxWidth();
        if (crimeBar != null) currentItem.maxWidthCrime = crimeBar.GetMaxWidth();
        if (healthBar != null) currentItem.maxWidthHealth = healthBar.GetMaxWidth();
        if (educationBar != null) currentItem.maxWidthEducation = educationBar.GetMaxWidth();
        if (povertyBar != null) currentItem.maxWidthPoverty = povertyBar.GetMaxWidth();

        foreach (var ge in currentItem.groupEffects)
        {
            float newEffect = ge.savedEffect * (1f + percent);
            float delta = newEffect - ge.savedEffect;

            GameManager.Instance.ChangeSatisfaction(ge.groupType, delta);

            if (groupBars.TryGetValue(ge.groupType, out StatBarUI bar))
            {
                bar.Confirm(newEffect);
                ge.maxWidth = bar.GetMaxWidth();
            }

            ge.savedEffect = newEffect;
        }

        slider.value = 0;
        apCost = 0f;
        actionPointInput.text = "0";
        incomeInput.text = currentItem.income.ToString("0");
        costInput.text = currentItem.cost.ToString("0");

        FindObjectOfType<PolicyGraphManager>().UpdateGraphs();

        var socialPanel = FindObjectOfType<SocialGroupPanelUI>(true);
        if (socialPanel != null) socialPanel.RefreshPanel();

        var statsPanel = FindObjectOfType<StatisticsPanelUI>(true);
        if (statsPanel != null) statsPanel.RefreshPanel();
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
            case GroupType.Capitalists: return "Kapitalisté";
            case GroupType.Socialists: return "Socialisté";
            case GroupType.Religious: return "Nabozenska skupina";
            case GroupType.Environmentalists: return "Environmentalisté";
            default: return type.ToString();
        }
    }
}