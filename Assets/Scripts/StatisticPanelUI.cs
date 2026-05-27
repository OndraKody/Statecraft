using UnityEngine;
using System.Collections.Generic;

public class StatisticsPanelUI : MonoBehaviour
{
    // Misto generovani prefabu si priradis rovky rucne v Inspectoru
    // protoze jsi si panel udelal sam v Unity
    [SerializeField] private StatisticRowUI hdpRow;
    [SerializeField] private StatisticRowUI educationRow;
    [SerializeField] private StatisticRowUI healthRow;
    [SerializeField] private StatisticRowUI crimeRow;
    [SerializeField] private StatisticRowUI povertyRow;

    private void OnEnable()
    {
        RefreshPanel();
    }

    public void RefreshPanel()
    {
        if (GameManager.Instance == null) return;

        // Inicializuj pokud jeste nebyly nastaveny
        hdpRow?.Setup("HDP", StatType.HDP, true);
        educationRow?.Setup("Vzdelani", StatType.Education, true);
        healthRow?.Setup("Zdravotnictvi", StatType.Health, true);
        crimeRow?.Setup("Kriminalita", StatType.Crime, false);
        povertyRow?.Setup("Chudoba", StatType.Poverty, false);
    }
}