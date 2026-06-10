using TMPro;
using UnityEngine;
using UnityEngine.Localization.Settings;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TurnManeger : MonoBehaviour
{
    public static TurnManeger Instance;

    public int currentTurn = 1;

    [Header("Turn settings")]
    public float actionPointsPerTurn = 20f;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        if (GetComponent<ElectionManager>() == null)
            gameObject.AddComponent<ElectionManager>();
    }

    public void EndTurn()
    {
        if (ElectionManager.Instance != null && ElectionManager.Instance.IsGameLocked) return;

        ApplyEconomy();
        UpdateProjects();
        StartNewTurn();
        GameManager.Instance.SaveCurrentGame();
    }

    private void ApplyEconomy()
    {
        double deficit = GameManager.Instance.GetExpenses() - GameManager.Instance.GetIncome();
        if (deficit > 0)
            GameManager.Instance.AddDebt(deficit);
    }

    private void StartNewTurn()
    {
        currentTurn++;
        GameManager.Instance.AddActionPoints(actionPointsPerTurn);

        if (ElectionManager.IsElectionTurn(currentTurn))
            ElectionManager.Instance.ShowElection();
        else if (EventManager.Instance != null)
            EventManager.Instance.TriggerRandomEvent();

        var partyPanel = FindObjectOfType<PartyPanelUI>(true);
        if (partyPanel != null) partyPanel.RefreshElectionCountdown();

        Debug.Log($"Zacalo kolo {currentTurn}");
    }

    private void UpdateProjects()
    {
        var projects = GameManager.Instance.GetActiveProjects();
        bool projectStateChanged = false;

        for (int i = projects.Count - 1; i >= 0; i--)
        {
            projects[i].remainingTurns--;
            projectStateChanged = true;

            if (projects[i].remainingTurns <= 0)
            {
                CompleteProject(projects[i]);
                projects.RemoveAt(i);
            }
        }

        if (projectStateChanged)
        {
            GameManager.Instance.RefreshProjectButtons();

            var projectPanel = FindObjectOfType<ProjectPanelUI>(true);
            if (projectPanel != null) projectPanel.RefreshCurrentProjectState();
        }
    }

    private void CompleteProject(GameManager.ActiveProject project)
    {
        // Odeber bonus na vydaje
        GameManager.Instance.Expenseschanger(-project.data.expenseBonus);

        // Aplikuj efekty na statistiky
        foreach (var se in project.data.statEffects)
        {
            GameManager.Instance.ChangeStatistic(se.statType, se.value);
            Debug.Log($"Projekt dokoncen - statistika {se.statType}: {se.value:+0;-0}");
        }

        // Aplikuj efekty na socialni skupiny
        foreach (var ge in project.data.groupEffects)
        {
            GameManager.Instance.ChangeSatisfaction(ge.groupType, ge.value);
            Debug.Log($"Projekt dokoncen - skupina {ge.groupType}: {ge.value:+0;-0}");
        }

        // Obnov panely
        var statsPanel = FindObjectOfType<StatisticsPanelUI>(true);
        if (statsPanel != null) statsPanel.RefreshPanel();

        var socialPanel = FindObjectOfType<SocialGroupPanelUI>(true);
        if (socialPanel != null) socialPanel.RefreshPanel();

        Debug.Log("Projekt dokoncen: " + project.data.name);
    }
}

public class ElectionManager : MonoBehaviour
{
    public const int ElectionInterval = 16;

    public static ElectionManager Instance { get; private set; }
    public bool IsGameLocked { get; private set; }
    public int LastResolvedElectionTurn { get; private set; }

    private GameObject electionPanel;
    private TextMeshProUGUI titleText;
    private TextMeshProUGUI resultText;
    private Button actionButton;
    private TextMeshProUGUI actionButtonText;
    private bool lastElectionWon;

    private void Awake()
    {
        Instance = this;
        CreateElectionPanel();
        LocalizationSettings.SelectedLocaleChanged += OnLocaleChanged;
    }

    private void OnDestroy()
    {
        LocalizationSettings.SelectedLocaleChanged -= OnLocaleChanged;
        if (Instance == this) Instance = null;
    }

    public static int GetTurnsUntilNextElection(int currentTurn)
    {
        int completedTurns = Mathf.Max(0, currentTurn - 1);
        int remainder = completedTurns % ElectionInterval;
        return remainder == 0 && completedTurns > 0 ? ElectionInterval : ElectionInterval - remainder;
    }

    public static bool IsElectionTurn(int currentTurn)
    {
        int completedTurns = currentTurn - 1;
        return completedTurns > 0 && completedTurns % ElectionInterval == 0;
    }

    public void ShowElection()
    {
        if (GameManager.Instance == null || electionPanel == null) return;

        lastElectionWon = GameManager.Instance.IsWinning();
        IsGameLocked = !lastElectionWon;
        LastResolvedElectionTurn = TurnManeger.Instance != null ? TurnManeger.Instance.currentTurn : 0;
        UpdatePanelText();

        electionPanel.SetActive(true);
        electionPanel.transform.SetAsLastSibling();
    }

    public void RestoreState(int lastResolvedElectionTurn, bool electionLost)
    {
        LastResolvedElectionTurn = lastResolvedElectionTurn;
        IsGameLocked = electionLost;

        if (electionLost)
        {
            lastElectionWon = false;
            UpdatePanelText();
            electionPanel.SetActive(true);
            electionPanel.transform.SetAsLastSibling();
        }
        else if (lastResolvedElectionTurn == 0 && TurnManeger.Instance != null &&
                 IsElectionTurn(TurnManeger.Instance.currentTurn))
        {
            ShowElection();
        }
    }

    private void ContinueGame()
    {
        if (!lastElectionWon) return;
        electionPanel.SetActive(false);
    }

    private void ReturnToMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MeinMenu");
    }

    private void OnLocaleChanged(UnityEngine.Localization.Locale locale)
    {
        if (electionPanel != null && electionPanel.activeSelf)
            UpdatePanelText();
    }

    private void UpdatePanelText()
    {
        bool english = LocalizationSettings.SelectedLocale != null &&
            LocalizationSettings.SelectedLocale.Identifier.Code.StartsWith("en");
        float support = GameManager.Instance != null ? GameManager.Instance.GetTotalSupport() : 0f;

        titleText.text = english ? "Election results" : "Vysledky voleb";
        if (lastElectionWon)
        {
            resultText.text = english
                ? $"Victory\nYour support is {support:0.0}%."
                : $"Vitezstvi\nVase podpora je {support:0.0} %.";
            actionButtonText.text = english ? "Continue" : "Pokracovat";
        }
        else
        {
            resultText.text = english
                ? $"Defeat\nYour support is only {support:0.0}%."
                : $"Prohra\nVase podpora je pouze {support:0.0} %.";
            actionButtonText.text = english ? "Return to menu" : "Vratit se do menu";
        }

        actionButton.onClick.RemoveAllListeners();
        actionButton.onClick.AddListener(lastElectionWon ? ContinueGame : ReturnToMenu);
    }

    private void CreateElectionPanel()
    {
        Canvas canvas = FindObjectOfType<Canvas>();
        if (canvas == null)
        {
            Debug.LogError("ElectionManager: Canvas nebyl nalezen.");
            return;
        }

        electionPanel = CreateUiObject("ElectionPanel", canvas.transform);
        RectTransform panelRect = electionPanel.GetComponent<RectTransform>();
        Stretch(panelRect);
        Image overlay = electionPanel.AddComponent<Image>();
        overlay.color = new Color(0f, 0f, 0f, 0.75f);

        GameObject window = CreateUiObject("ElectionWindow", electionPanel.transform);
        RectTransform windowRect = window.GetComponent<RectTransform>();
        windowRect.anchorMin = windowRect.anchorMax = new Vector2(0.5f, 0.5f);
        windowRect.sizeDelta = new Vector2(620f, 390f);
        Image windowImage = window.AddComponent<Image>();
        windowImage.color = new Color(0.94f, 0.92f, 0.84f, 1f);

        titleText = CreateText("Title", window.transform, 38f, FontStyles.Bold);
        SetRect(titleText.rectTransform, new Vector2(0f, 125f), new Vector2(540f, 70f));

        resultText = CreateText("Result", window.transform, 30f, FontStyles.Normal);
        SetRect(resultText.rectTransform, new Vector2(0f, 25f), new Vector2(540f, 130f));

        GameObject buttonObject = CreateUiObject("ActionButton", window.transform);
        RectTransform buttonRect = buttonObject.GetComponent<RectTransform>();
        SetRect(buttonRect, new Vector2(0f, -125f), new Vector2(300f, 70f));
        Image buttonImage = buttonObject.AddComponent<Image>();
        buttonImage.color = new Color(0.78f, 0.7f, 0.48f, 1f);
        actionButton = buttonObject.AddComponent<Button>();
        actionButton.targetGraphic = buttonImage;

        actionButtonText = CreateText("Text", buttonObject.transform, 25f, FontStyles.Bold);
        Stretch(actionButtonText.rectTransform);

        electionPanel.SetActive(false);
    }

    private static GameObject CreateUiObject(string objectName, Transform parent)
    {
        GameObject obj = new GameObject(objectName, typeof(RectTransform));
        obj.layer = 5;
        obj.transform.SetParent(parent, false);
        return obj;
    }

    private static TextMeshProUGUI CreateText(string objectName, Transform parent, float fontSize, FontStyles style)
    {
        GameObject obj = CreateUiObject(objectName, parent);
        TextMeshProUGUI text = obj.AddComponent<TextMeshProUGUI>();
        text.fontSize = fontSize;
        text.fontStyle = style;
        text.color = new Color(0.12f, 0.12f, 0.12f, 1f);
        text.alignment = TextAlignmentOptions.Center;
        return text;
    }

    private static void SetRect(RectTransform rect, Vector2 position, Vector2 size)
    {
        rect.anchorMin = rect.anchorMax = new Vector2(0.5f, 0.5f);
        rect.anchoredPosition = position;
        rect.sizeDelta = size;
    }

    private static void Stretch(RectTransform rect)
    {
        rect.anchorMin = Vector2.zero;
        rect.anchorMax = Vector2.one;
        rect.offsetMin = Vector2.zero;
        rect.offsetMax = Vector2.zero;
    }
}

