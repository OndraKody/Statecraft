using System.Collections.Generic;
using System.IO;
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

    // Specialni promenne pro zmeny z udalosti - ukladaji se oddelene do JSON
    private double eventIncome = 0;
    private double eventExpenses = 0;
    private List<EventStatEffectSaveData> eventStatEffects = new List<EventStatEffectSaveData>();
    private List<EventGroupEffectSaveData> eventGroupEffects = new List<EventGroupEffectSaveData>();
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

        public ActiveProject(ProjectPanelUI.ProjectItem item, int turnsLeft)
        {
            data = item;
            remainingTurns = turnsLeft;
        }
    }
    public void AddProject(ProjectPanelUI.ProjectItem item)
    {
        if (IsProjectActive(item)) return;

        activeProjects.Add(new ActiveProject(item));
        Expenseschanger(item.expenseBonus);
        RefreshProjectButtons();
    }

    public bool IsProjectActive(ProjectPanelUI.ProjectItem item)
    {
        if (item == null) return false;
        return activeProjects.Exists(project => project.data == item);
    }

    public int GetProjectRemainingTurns(ProjectPanelUI.ProjectItem item)
    {
        if (item == null) return 0;

        ActiveProject activeProject = activeProjects.Find(project => project.data == item);
        return activeProject != null ? activeProject.remainingTurns : 0;
    }

    public void RefreshProjectButtons()
    {
        ProjectButtonUI[] projectButtons = FindObjectsOfType<ProjectButtonUI>(true);
        foreach (var projectButton in projectButtons)
            if (projectButton != null)
                projectButton.RefreshVisualState();
    }
    public List<ActiveProject> GetActiveProjects() => activeProjects;

    private void AddLoadedProject(ProjectPanelUI.ProjectItem item, int remainingTurns)
    {
        if (item == null) return;
        if (IsProjectActive(item)) return;
        activeProjects.Add(new ActiveProject(item, Mathf.Max(1, remainingTurns)));
    }
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

    // ===== EVENT INCOME/EXPENSE - specialni promenne pro zmeny z udalosti =====
    public void AddEventIncome(float v)
    {
        eventIncome += v;
    }
    public void AddEventExpense(float v)
    {
        eventExpenses += v;
    }
    public double GetEventIncome() => eventIncome;
    public double GetEventExpenses() => eventExpenses;

    public void AddEventStatistic(StatType statType, float value)
    {
        var existing = eventStatEffects.Find(x => x.statType == statType);
        if (existing != null) existing.value += value;
        else eventStatEffects.Add(new EventStatEffectSaveData { statType = statType, value = value });
    }

    public void AddEventGroupEffect(GroupType groupType, float value)
    {
        var existing = eventGroupEffects.Find(x => x.groupType == groupType);
        if (existing != null) existing.value += value;
        else eventGroupEffects.Add(new EventGroupEffectSaveData { groupType = groupType, value = value });
    }
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

    public float GetTotalSupport()
    {
        float totalPower = 0f, weightedSum = 0f;
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
        int loadedPolicies = PolicyJsonLoader.ApplyPoliciesToButtons(buttons);
        if (loadedPolicies > 0)
            Debug.Log($"[INIT] Policies nacteny z JSONu: {loadedPolicies}");

        float totalIncome = 0f, totalExpenses = 0f;
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
        UnityEngine.SceneManagement.SceneManager.LoadScene("MeinMenu");
    }

    public void SaveCurrentGame()
    {
        SaveGameData();
    }

    private void SaveGameData()
    {
        if (GameSession.CurrentSaveSlot == -1) return;

        SaveData data = new SaveData();
        data.slotIndex = GameSession.CurrentSaveSlot;
        data.currentTurn = TurnManeger.Instance != null ? TurnManeger.Instance.currentTurn : 1;
        data.lastResolvedElectionTurn = ElectionManager.Instance != null ? ElectionManager.Instance.LastResolvedElectionTurn : 0;
        data.electionLost = ElectionManager.Instance != null && ElectionManager.Instance.IsGameLocked;
        data.partyNameKey = selectedParty != null ? selectedParty.name : "";
        data.savedPartyData = selectedParty;
        data.dept = dept;
        data.actionPoints = actionPoints;
        data.hdp = hdp;
        data.crime = crime;
        data.health = health;
        data.education = education;
        data.poverty = poverty;

        // Uloz event zmeny
        data.eventIncome = eventIncome;
        data.eventExpenses = eventExpenses;
        data.eventStatEffects.AddRange(eventStatEffects);
        data.eventGroupEffects.AddRange(eventGroupEffects);
        foreach (var g in socialGroups)
            data.groupSatisfactions.Add(new GroupSatisfactionSaveData
            { groupType = g.type, satisfaction = g.satisfaction });

        SaveProjectStates(data);


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

            foreach (var ge in policy.groupEffects)
                pSave.groupEffects.Add(new GroupEffectSaveData
                { groupType = ge.groupType, savedEffect = ge.savedEffect, maxWidth = ge.maxWidth });

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
        if (TurnManeger.Instance != null && data.currentTurn > 0)
            TurnManeger.Instance.currentTurn = data.currentTurn;
        GameSession.SelectedParty = data.savedPartyData;
        dept = data.dept;
        actionPoints = data.actionPoints;
        hdp = data.hdp;
        crime = data.crime;
        health = data.health;
        education = data.education;
        poverty = data.poverty;

        // Nacti event zmeny
        eventIncome = data.eventIncome;
        eventExpenses = data.eventExpenses;
        eventStatEffects = data.eventStatEffects ?? new List<EventStatEffectSaveData>();
        eventGroupEffects = data.eventGroupEffects ?? new List<EventGroupEffectSaveData>();
        if (data.groupSatisfactions == null) data.groupSatisfactions = new List<GroupSatisfactionSaveData>();
        if (data.policies == null) data.policies = new List<PolicySaveData>();
        if (data.projects == null) data.projects = new List<ProjectSaveData>();
        foreach (var gs in data.groupSatisfactions)
        {
            var group = socialGroups.Find(g => g.type == gs.groupType);
            if (group != null) group.satisfaction = gs.satisfaction;
        }

        float totalIncome = 0f, totalExpenses = 0f;
        activeProjects.Clear();

        foreach (var savedP in data.policies)
        {
            var policy = allPolicies.Find(x =>
                x.name != null && !x.name.IsEmpty &&
                x.name.TableEntryReference.ToString() == savedP.nameKey);

            if (policy == null) { Debug.LogWarning($"[LOAD] Policy '{savedP.nameKey}' nenalezena!"); continue; }

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

        RestoreActiveProjects(data.projects);
        RefreshProjectButtons();

        float activeProjectExpenses = 0f;
        foreach (var project in activeProjects)
            if (project.data != null)
                activeProjectExpenses += project.data.expenseBonus;

        // Pricti event zmeny a aktivni projekty k nactenym hodnotam
        income = totalIncome + eventIncome;
        expenses = totalExpenses + eventExpenses + activeProjectExpenses;
        Debug.Log($"[LOAD] Income: {income} (event: {eventIncome}), Expenses: {expenses} (event: {eventExpenses})" );

        if (ElectionManager.Instance != null)
            ElectionManager.Instance.RestoreState(data.lastResolvedElectionTurn, data.electionLost);
    }

    private void RestoreActiveProjects(List<ProjectSaveData> savedProjects)
    {
        if (savedProjects == null || savedProjects.Count == 0) return;

        ProjectButtonUI[] projectButtons = FindObjectsOfType<ProjectButtonUI>(true);

        foreach (var savedProject in savedProjects)
        {
            if (savedProject == null || !savedProject.isActive) continue;

            ProjectPanelUI.ProjectItem project = FindProjectByKey(projectButtons, savedProject.nameKey);
            if (project == null)
            {
                Debug.LogWarning($"[LOAD] Projekt '{savedProject.nameKey}' nenalezen!");
                continue;
            }

            AddLoadedProject(project, savedProject.remainingTurns);
        }

        Debug.Log($"[LOAD] Aktivni projekty: {activeProjects.Count}");
    }

    private void SaveProjectStates(SaveData data)
    {
        Dictionary<string, ActiveProject> activeByKey = new Dictionary<string, ActiveProject>();
        foreach (var activeProject in activeProjects)
        {
            string activeKey = GetProjectKey(activeProject.data);
            if (string.IsNullOrEmpty(activeKey)) continue;
            if (!activeByKey.ContainsKey(activeKey))
                activeByKey.Add(activeKey, activeProject);
        }

        HashSet<string> savedKeys = new HashSet<string>();
        ProjectButtonUI[] projectButtons = FindObjectsOfType<ProjectButtonUI>(true);
        foreach (var button in projectButtons)
        {
            ProjectPanelUI.ProjectItem project = button != null ? button.ProjectData : null;
            string key = GetProjectKey(project);
            if (string.IsNullOrEmpty(key) || savedKeys.Contains(key)) continue;

            bool isActive = activeByKey.TryGetValue(key, out ActiveProject activeProject);
            data.projects.Add(new ProjectSaveData
            {
                nameKey = key,
                isActive = isActive,
                remainingTurns = isActive ? activeProject.remainingTurns : 0
            });
            savedKeys.Add(key);
        }

        foreach (var activeProject in activeProjects)
        {
            string key = GetProjectKey(activeProject.data);
            if (string.IsNullOrEmpty(key) || savedKeys.Contains(key)) continue;

            data.projects.Add(new ProjectSaveData
            {
                nameKey = key,
                isActive = true,
                remainingTurns = activeProject.remainingTurns
            });
            savedKeys.Add(key);
        }
    }
    private ProjectPanelUI.ProjectItem FindProjectByKey(ProjectButtonUI[] buttons, string nameKey)
    {
        if (buttons == null || string.IsNullOrEmpty(nameKey)) return null;

        foreach (var button in buttons)
        {
            ProjectPanelUI.ProjectItem project = button != null ? button.ProjectData : null;
            if (project == null || project.name == null || project.name.IsEmpty) continue;

            if (GetProjectKey(project) == nameKey)
                return project;
        }

        return null;
    }

    private string GetProjectKey(ProjectPanelUI.ProjectItem project)
    {
        if (project == null || project.name == null || project.name.IsEmpty) return "";
        return project.name.TableEntryReference.ToString();
    }
}

public enum StatType { HDP, Crime, Health, Education, Poverty }
