using UnityEngine;

public class StatisticsPanelUI : MonoBehaviour
{
    [SerializeField] private StatisticRowUI hdpRow;
    [SerializeField] private StatisticRowUI educationRow;
    [SerializeField] private StatisticRowUI healthRow;
    [SerializeField] private StatisticRowUI crimeRow;
    [SerializeField] private StatisticRowUI povertyRow;

    private bool isInitialized = false;

    private void OnEnable()
    {
        RefreshPanel();
    }

    public void RefreshPanel()
    {
        if (GameManager.Instance == null) return;

        // Setup se vola jen jednou - nastavi typy a nazvy
        if (!isInitialized)
        {
            hdpRow?.Setup("HDP", StatType.HDP, true);
            educationRow?.Setup("Vzdelani", StatType.Education, true);
            healthRow?.Setup("Zdravotnictvi", StatType.Health, true);
            crimeRow?.Setup("Kriminalita", StatType.Crime, false);
            povertyRow?.Setup("Chudoba", StatType.Poverty, false);
            isInitialized = true;
        }
        else
        {
            // Jen aktualizuj bary - bez reinicializace
            hdpRow?.Refresh();
            educationRow?.Refresh();
            healthRow?.Refresh();
            crimeRow?.Refresh();
            povertyRow?.Refresh();
        }
    }
}