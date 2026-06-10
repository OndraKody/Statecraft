using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Collections;

public class EventPanelUI : MonoBehaviour
{
    [Header("Texty")]
    public TextMeshProUGUI titleText;
    public TextMeshProUGUI descriptionText;

    [Header("Faze 1 - vyber moznosti")]
    public GameObject choicePhase;   // panel se dvema tlacitky
    public Button optionAButton;
    public Button optionBButton;

    [Header("Faze 2 - prehled efektu")]
    public GameObject effectPhase;      // panel se ScrollView a Confirm
    public Transform effectsParent;     // Content v ScrollView
    public Button confirmButton;
    public GameObject effectRowPrefab;  // stejny prefab jako u projektu

    private EventManager.GameEvent currentEvent;
    private EventManager.EventOption selectedOption;

    // ===== ZOBRAZENI EVENTU =====
    public void Show(EventManager.GameEvent gameEvent)
    {
        CleanupSubscriptions();

        currentEvent = gameEvent;
        gameObject.SetActive(true);

        // Prihlaseni k lokalizaci
        currentEvent.title.StringChanged += UpdateTitle;
        currentEvent.description.StringChanged += UpdateDescription;

        if (currentEvent.optionA?.text != null)
            currentEvent.optionA.text.StringChanged += UpdateOptionAText;
        if (currentEvent.optionB?.text != null)
            currentEvent.optionB.text.StringChanged += UpdateOptionBText;

        // Zobraz fazi 1 - dva buttony
        choicePhase.SetActive(true);
        effectPhase.SetActive(false);

        // Nastav tlacitka
        optionAButton.onClick.RemoveAllListeners();
        optionAButton.onClick.AddListener(() => SelectOption(gameEvent.optionA));

        optionBButton.onClick.RemoveAllListeners();
        optionBButton.onClick.AddListener(() => SelectOption(gameEvent.optionB));

        confirmButton.onClick.RemoveAllListeners();
        confirmButton.onClick.AddListener(Confirm);
    }

    // ===== VYBER MOZNOSTI =====
    private void SelectOption(EventManager.EventOption option)
    {
        selectedOption = option;

        // Prepni na fazi 2
        choicePhase.SetActive(false);
        effectPhase.SetActive(true);

        // Vygeneruj radky efektu
        GenerateEffectRows(option);
        StartCoroutine(RefreshEffectsScroll());
    }

    // ===== GENEROVANI RADKU EFEKTU =====
    private void GenerateEffectRows(EventManager.EventOption option)
    {
        // Vycisti stare radky
        for (int i = effectsParent.childCount - 1; i >= 0; i--)
        {
            GameObject child = effectsParent.GetChild(i).gameObject;
            if (child.scene.IsValid()) Destroy(child);
        }

        // Income zmena
        if (option.incomeChange != 0f)
            CreateEffectRow("Příjmy", option.incomeChange, true);

        // Expense zmena
        if (option.expenseChange != 0f)
            CreateEffectRow("Výdaje", option.expenseChange, false);

        // Statistiky
        foreach (var se in option.statEffects)
        {
            if (se.value == 0f) continue;
            bool positiveIsGood = se.statType != StatType.Crime && se.statType != StatType.Poverty;
            CreateEffectRow(GetStatName(se.statType), se.value, positiveIsGood);
        }

        // Socialni skupiny
        foreach (var ge in option.groupEffects)
        {
            if (ge.value == 0f) continue;
            CreateEffectRow(GetGroupName(ge.groupType), ge.value, true);
        }
    }

    private void CreateEffectRow(string name, float value, bool positiveIsGood)
    {
        if (effectRowPrefab == null || effectsParent == null) return;

        GameObject rowGO = Instantiate(effectRowPrefab, effectsParent);
        ProjectEffectRowUI row = rowGO.GetComponent<ProjectEffectRowUI>();

        if (row != null)
            row.Setup(name, value, positiveIsGood);
    }

    private IEnumerator RefreshEffectsScroll()
    {
        // Destroy() a layout komponenty se projevi az na konci framu.
        yield return null;

        Canvas.ForceUpdateCanvases();
        RectTransform content = effectsParent as RectTransform;
        if (content != null)
            LayoutRebuilder.ForceRebuildLayoutImmediate(content);

        ScrollRect scrollRect = effectsParent.GetComponentInParent<ScrollRect>();
        if (scrollRect != null)
        {
            scrollRect.vertical = true;
            scrollRect.verticalNormalizedPosition = 1f;
            scrollRect.StopMovement();
        }
    }

    // ===== POTVRZENI - aplikuje efekty =====
    private void Confirm()
    {
        if (selectedOption == null || GameManager.Instance == null)
        {
            gameObject.SetActive(false);
            return;
        }

        // Aplikuj income/expense zmeny
        // Ukladame je do specialnich event promennych v GameManageru
        if (selectedOption.incomeChange != 0f)
        {
            GameManager.Instance.IncomeChanger(selectedOption.incomeChange);
            GameManager.Instance.AddEventIncome(selectedOption.incomeChange);
        }
        if (selectedOption.expenseChange != 0f)
        {
            GameManager.Instance.Expenseschanger(selectedOption.expenseChange);
            GameManager.Instance.AddEventExpense(selectedOption.expenseChange);
        }

        // Aplikuj statistiky
        foreach (var se in selectedOption.statEffects)
        {
            GameManager.Instance.ChangeStatistic(se.statType, se.value);
            GameManager.Instance.AddEventStatistic(se.statType, se.value);
        }

        // Aplikuj skupiny
        foreach (var ge in selectedOption.groupEffects)
        {
            GameManager.Instance.ChangeSatisfaction(ge.groupType, ge.value);
            GameManager.Instance.AddEventGroupEffect(ge.groupType, ge.value);
        }

        // Obnov panely
        var statsPanel = FindObjectOfType<StatisticsPanelUI>(true);
        if (statsPanel != null) statsPanel.RefreshPanel();

        var socialPanel = FindObjectOfType<SocialGroupPanelUI>(true);
        if (socialPanel != null) socialPanel.RefreshPanel();

        GameManager.Instance.SaveCurrentGame();

        gameObject.SetActive(false);
    }

    // ===== LOKALIZACE =====
    private void UpdateTitle(string t) => titleText.text = t;
    private void UpdateDescription(string t) => descriptionText.text = t;

    private void UpdateOptionAText(string t)
    {
        var txt = optionAButton.GetComponentInChildren<TextMeshProUGUI>();
        if (txt != null) txt.text = t;
    }

    private void UpdateOptionBText(string t)
    {
        var txt = optionBButton.GetComponentInChildren<TextMeshProUGUI>();
        if (txt != null) txt.text = t;
    }

    private void CleanupSubscriptions()
    {
        if (currentEvent == null) return;
        currentEvent.title.StringChanged -= UpdateTitle;
        currentEvent.description.StringChanged -= UpdateDescription;
        if (currentEvent.optionA?.text != null)
            currentEvent.optionA.text.StringChanged -= UpdateOptionAText;
        if (currentEvent.optionB?.text != null)
            currentEvent.optionB.text.StringChanged -= UpdateOptionBText;
    }

    private void OnDisable() => CleanupSubscriptions();

    // ===== POMOCNE METODY =====
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
            case GroupType.Capitalists: return "Kapitalisté";
            case GroupType.Socialists: return "Socialisté";
            case GroupType.Religious: return "Nabozenska skupina";
            case GroupType.Environmentalists: return "Environmentalisté";
            default: return type.ToString();
        }
    }
}
