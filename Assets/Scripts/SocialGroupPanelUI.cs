using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class SocialGroupPanelUI : MonoBehaviour
{
    [Header("Support karta")]
    [SerializeField] private TextMeshProUGUI supportPercentText;
    [SerializeField] private TextMeshProUGUI supportLabelText;
    [SerializeField] private RectTransform supportBarFill;
    [SerializeField] private Image supportBarFillImage;
    [SerializeField] private TextMeshProUGUI statusBadgeText;
    [SerializeField] private float supportBarMaxWidth = 200f;

    [Header("Seznam skupin")]
    [SerializeField] private GameObject rowPrefab;
    [SerializeField] private Transform rowParent;

    private Color colorWin = new Color(0.39f, 0.55f, 0.13f);
    private Color colorLose = new Color(0.64f, 0.18f, 0.18f);

    // Reference na radky - abychom je jen aktualizovali misto mazani a znovuvytvareni
    private List<SocialGroupRowUI> rows = new List<SocialGroupRowUI>();

    private void OnEnable()
    {
        RefreshPanel();
    }

    // Vola se z PolyciPanelUI po kazdem Confirm
    public void RefreshPanel()
    {
        if (GameManager.Instance == null) return;

        var groups = GameManager.Instance.socialGroups;

        // Aktualizuj support kartu
        float support = GameManager.Instance.GetTotalSupport();
        bool winning = GameManager.Instance.IsWinning();

        supportPercentText.text = support.ToString("0") + " %";

        if (supportLabelText != null)
            supportLabelText.text = "celkova podpora";

        // Sirka support baru
        if (supportBarFill != null)
        {
            Vector2 size = supportBarFill.sizeDelta;
            size.x = (support / 100f) * supportBarMaxWidth;
            supportBarFill.sizeDelta = size;
        }

        if (supportBarFillImage != null)
            supportBarFillImage.color = winning ? colorWin : colorLose;

        if (statusBadgeText != null)
        {
            statusBadgeText.text = winning ? "Vitez" : "Prohrava";
            statusBadgeText.color = winning ? colorWin : colorLose;
        }

        // Pokud radky jeste neexistuji, vygeneruj je
        if (rows.Count == 0)
        {
            GenerateRows(groups);
        }
        else
        {
            // Jinak jen aktualizuj spokojenost - bez mazani a znovu vytvareni
            for (int i = 0; i < rows.Count && i < groups.Count; i++)
                rows[i].UpdateSatisfaction(groups[i]);
        }
    }

    private void GenerateRows(List<SocialGroupItem> groups)
    {
        // Bezpecne smazani starych radku
        for (int i = rowParent.childCount - 1; i >= 0; i--)
        {
            GameObject child = rowParent.GetChild(i).gameObject;
            if (child.scene.IsValid())
                Destroy(child);
        }
        rows.Clear();

        foreach (var group in groups)
        {
            GameObject rowGO = Instantiate(rowPrefab, rowParent);
            SocialGroupRowUI row = rowGO.GetComponent<SocialGroupRowUI>();

            if (row != null)
            {
                row.Setup(group);
                rows.Add(row);
            }
            else
            {
                Debug.LogWarning("SocialGroupRowUI chybi na prefabu!");
            }
        }
    }
}