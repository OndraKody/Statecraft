using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    private JsonLouder.Party selectedParty;
    private double expenses;
    private double income;
    private double deficit;
    private double dept;
    private double actionPoints = 10000;
    private string gamePhase;
    private List<ActiveProject> activeProjects = new List<ActiveProject>();

    [SerializeField]
    private List<PolyciPanelUI.PolicyItem> allPolicies = new List<PolyciPanelUI.PolicyItem>();

    public float hdp = 50f;
    public float crime = 50f;
    public float health = 50f;
    public float education = 50f;
    public float poverty = 50f;

    [Header("Sociální skupiny ve státì")]
    public List<SocialGroupItem> socialGroups = new List<SocialGroupItem>();

    // ===== POLICY =====
    public void RegisterPolicy(PolyciPanelUI.PolicyItem policy)
    {
        if (!allPolicies.Contains(policy)) allPolicies.Add(policy);
    }
    public List<PolyciPanelUI.PolicyItem> GetAllPolicies() => allPolicies;

    // ===== PROJEKTY =====
    public class ActiveProject
    {
        public ProjectPanelUI.ProjectItem data;
        public int remainingTurns;
        public ActiveProject(ProjectPanelUI.ProjectItem item)
        {
            data = item;
            remainingTurns = item.durationInTurns;
        }
    }
    public void AddProject(ProjectPanelUI.ProjectItem item)
    {
        activeProjects.Add(new ActiveProject(item));
        Expenseschanger(item.expenseBonus);
    }
    public List<ActiveProject> GetActiveProjects() => activeProjects;

    // ===== EKONOMIKA =====
    public void IncomeChanger(float v) { income += v; }
    public void Expenseschanger(float v) { expenses += v; }
    public double GetIncome() => income;
    public double GetExpenses() => expenses;
    public void SetIncome(double v) { income = v; }
    public void SetExpensive(double v) { expenses = v; }
    public double GetBalance() => income - expenses;
    public double GetDept() => dept;
    public bool HasEnoughActionPoints(float cost) => actionPoints >= cost;
    public void UseActionPoints(float cost) { actionPoints -= cost; }
    public double GetActionPoints() => actionPoints;
    public void AddDebt(double v) { dept += v; }
    public void AddActionPoints(double v) { actionPoints += v; }
    public JsonLouder.Party GetSelectedParty() => selectedParty;

    public void SetParty(JsonLouder.Party party)
    {
        selectedParty = party;
        Debug.Log("GameManager: Ulozena strana -> " + party.name);
    }

    // ===== STATISTIKY =====
    public void ChangeStatistic(StatType stat, float amount)
    {
        switch (stat)
        {
            case StatType.HDP: hdp = Mathf.Clamp(hdp + amount, 0, 100); break;
            case StatType.Crime: crime = Mathf.Clamp(crime + amount, 0, 100); break;
            case StatType.Health: health = Mathf.Clamp(health + amount, 0, 100); break;
            case StatType.Education: education = Mathf.Clamp(education + amount, 0, 100); break;
            case StatType.Poverty: poverty = Mathf.Clamp(poverty + amount, 0, 100); break;
        }
    }
    public float GetStatistic(StatType stat)
    {
        switch (stat)
        {
            case StatType.HDP: return hdp;
            case StatType.Crime: return crime;
            case StatType.Health: return health;
            case StatType.Education: return education;
            case StatType.Poverty: return poverty;
            default: return 0f;
        }
    }

    // ===== SOCIÁLNÍ SKUPINY =====

    // Zmeni spokojenost skupiny podle GroupType - stejne jako ChangeStatistic
    public void ChangeSatisfaction(GroupType groupType, float amount)
    {
        var group = socialGroups.Find(g => g.type == groupType);
        if (group != null)
            group.satisfaction = Mathf.Clamp(group.satisfaction + amount, 0f, 100f);
        else
            Debug.LogWarning($"[GameManager] Skupina {groupType} nenalezena!");
    }

    public float GetSatisfaction(GroupType groupType)
    {
        var group = socialGroups.Find(g => g.type == groupType);
        return group != null ? group.satisfaction : 0f;
    }

    // Celkova podpora = vazeny prumer spokojenosti podle zastoupeni (power)
    // Pokud maji vsechny skupiny 50% spokojenost -> vysledek je 50%
    public float GetTotalSupport()
    {
        float totalPower = 0f;
        float weightedSum = 0f;

        foreach (var g in socialGroups)
        {
            totalPower += g.power;
            weightedSum += g.satisfaction * g.power;
        }

        return totalPower == 0f ? 0f : weightedSum / totalPower;
    }

    public bool IsWinning() => GetTotalSupport() > 50f;

    // ===== AWAKE / START =====
    private void Awake()
    {
        if (Instance == null) Instance = this;
        else { Destroy(gameObject); return; }

        allPolicies.Clear();
        PolicyButtonUI[] buttons = FindObjectsOfType<PolicyButtonUI>(true);

        float totalIncome = 0f;
        float totalExpenses = 0f;

        foreach (var btn in buttons)
        {
            if (btn.policyData == null) continue;
            if (!allPolicies.Contains(btn.policyData))
                allPolicies.Add(btn.policyData);
            totalIncome += btn.policyData.income;
            totalExpenses += btn.policyData.cost;
        }

        income = totalIncome;
        expenses = totalExpenses;

        Debug.Log($"[INIT] Income: {income}, Expenses: {expenses}, Pocet politik: {allPolicies.Count}");
    }

    private void Start()
    {
        if (GameSession.CurrentSaveSlot != -1 && SaveManager.Exists(GameSession.CurrentSaveSlot))
            LoadGameData();
        else if (GameSession.SelectedParty != null)
        {
            selectedParty = GameSession.SelectedParty;
            Debug.Log("Nova hra za -> " + selectedParty.name);
        }

        Invoke(nameof(RefreshGraphs), 0.1f);
    }

    private void RefreshGraphs()
    {
        var graphManager = FindObjectOfType<PolicyGraphManager>();
        if (graphManager != null) graphManager.UpdateGraphs();
    }

    // ===== SAVE / LOAD =====
    public void SaveAndExit()
    {
        SaveGameData();
        UnityEngine.SceneManagement.SceneManager.LoadScene("MainMenuScene");
    }

    private void SaveGameData()
    {
        if (GameSession.CurrentSaveSlot == -1) return;

        SaveData data = new SaveData();
        data.slotIndex = GameSession.CurrentSaveSlot;
        data.partyNameKey = selectedParty != null ? selectedParty.name : "";
        data.savedPartyData = selectedParty;
        data.dept = dept;
        data.actionPoints = actionPoints;
        data.hdp = hdp;
        data.crime = crime;
        data.health = health;
        data.education = education;
        data.poverty = poverty;

        // Uloz spokojenosti skupin
        foreach (var g in socialGroups)
            data.groupSatisfactions.Add(new GroupSatisfactionSaveData
            {
                groupType = g.type,
                satisfaction = g.satisfaction
            });

        // Uloz policy
        foreach (var policy in allPolicies)
        {
            if (policy.name == null || policy.name.IsEmpty) continue;
            string key = policy.name.TableEntryReference.ToString();
            if (string.IsNullOrEmpty(key)) continue;

            var pSave = new PolicySaveData
            {
                nameKey = key,
                currentIncome = policy.income,
                currentCost = policy.cost,
                savedHdpEffect = policy.savedHdpEffect,
                savedCrimeEffect = policy.savedCrimeEffect,
                savedHealthEffect = policy.savedHealthEffect,
                savedEducationEffect = policy.savedEducationEffect,
                savedPovertyEffect = policy.savedPovertyEffect,
                maxWidthHdp = policy.maxWidthHdp,
                maxWidthCrime = policy.maxWidthCrime,
                maxWidthHealth = policy.maxWidthHealth,
                maxWidthEducation = policy.maxWidthEducation,
                maxWidthPoverty = policy.maxWidthPoverty,
                initialized = policy.initialized
            };

            // Uloz efekty skupin
            foreach (var ge in policy.groupEffects)
                pSave.groupEffects.Add(new GroupEffectSaveData
                {
                    groupType = ge.groupType,
                    savedEffect = ge.savedEffect,
                    maxWidth = ge.maxWidth
                });

            data.policies.Add(pSave);
        }

        SaveManager.Save(GameSession.CurrentSaveSlot, data);
        Debug.Log($"[SAVE] Ulozeno {data.policies.Count} politik.");
    }

    private void LoadGameData()
    {
        SaveData data = SaveManager.Load(GameSession.CurrentSaveSlot);
        if (data == null) { Debug.LogWarning("[LOAD] Save soubor nenalezen!"); return; }

        selectedParty = data.savedPartyData;
        GameSession.SelectedParty = data.savedPartyData;
        dept = data.dept;
        actionPoints = data.actionPoints;
        hdp = data.hdp;
        crime = data.crime;
        health = data.health;
        education = data.education;
        poverty = data.poverty;

        // Nacti spokojenosti skupin
        foreach (var gs in data.groupSatisfactions)
        {
            var group = socialGroups.Find(g => g.type == gs.groupType);
            if (group != null) group.satisfaction = gs.satisfaction;
        }

        float totalIncome = 0f, totalExpenses = 0f;

        foreach (var savedP in data.policies)
        {
            var policy = allPolicies.Find(x =>
                x.name != null && !x.name.IsEmpty &&
                x.name.TableEntryReference.ToString() == savedP.nameKey);

            if (policy == null)
            {
                Debug.LogWarning($"[LOAD] Policy '{savedP.nameKey}' nenalezena!");
                continue;
            }

            policy.income = savedP.currentIncome;
            policy.cost = savedP.currentCost;
            policy.savedHdpEffect = savedP.savedHdpEffect;
            policy.savedCrimeEffect = savedP.savedCrimeEffect;
            policy.savedHealthEffect = savedP.savedHealthEffect;
            policy.savedEducationEffect = savedP.savedEducationEffect;
            policy.savedPovertyEffect = savedP.savedPovertyEffect;
            policy.maxWidthHdp = savedP.maxWidthHdp > 0 ? savedP.maxWidthHdp : 200f;
            policy.maxWidthCrime = savedP.maxWidthCrime > 0 ? savedP.maxWidthCrime : 200f;
            policy.maxWidthHealth = savedP.maxWidthHealth > 0 ? savedP.maxWidthHealth : 200f;
            policy.maxWidthEducation = savedP.maxWidthEducation > 0 ? savedP.maxWidthEducation : 200f;
            policy.maxWidthPoverty = savedP.maxWidthPoverty > 0 ? savedP.maxWidthPoverty : 200f;
            policy.initialized = savedP.initialized;

            // Nacti efekty skupin
            foreach (var geSave in savedP.groupEffects)
            {
                var ge = policy.groupEffects.Find(x => x.groupType == geSave.groupType);
                if (ge != null)
                {
                    ge.savedEffect = geSave.savedEffect;
                    ge.maxWidth = geSave.maxWidth > 0 ? geSave.maxWidth : 200f;
                }
            }

            totalIncome += policy.income;
            totalExpenses += policy.cost;
        }

        income = totalIncome;
        expenses = totalExpenses;
        Debug.Log($"[LOAD] Uspesne nacteno. Income: {income}, Expenses: {expenses}");
    }
}

public enum StatType { HDP, Crime, Health, Education, Poverty }