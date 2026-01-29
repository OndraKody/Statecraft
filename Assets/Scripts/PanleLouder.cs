using UnityEngine;

public class PanleLouder : MonoBehaviour
{
    [SerializeField]
    private GameObject FinancePanel;
    [SerializeField]
    private GameObject JusticePanel;
    [SerializeField]
    private GameObject ForeighPanel;
    [SerializeField]
    private GameObject SocialHealtPanel;
    [SerializeField]
    private GameObject TransportPanel;
    [SerializeField]
    private GameObject DeffensPanel;
    [SerializeField]
    private GameObject ProjectPanel;
    [SerializeField]
    private GameObject GrafPanel;
    [SerializeField]
    private GameObject PousaPanel;
    [SerializeField]
    private GameObject DataPanel;
    [SerializeField]
    private GameObject GroupPanel;
    [SerializeField]
    private GameObject PartyPanel;
    [SerializeField]
    private GameObject PolicyPanel;
    [SerializeField]
    private GameObject EventPanel;
    [SerializeField]
    private PartyPanelUI partyPanel;
    private void Awake()
    {
        if (FinancePanel != null) FinancePanel.SetActive(true);
        if (JusticePanel != null) JusticePanel.SetActive(true);
        if (ForeighPanel != null) ForeighPanel.SetActive(true);
        if (SocialHealtPanel != null) SocialHealtPanel.SetActive(true);
        if (TransportPanel != null) TransportPanel.SetActive(true);
        if (DeffensPanel != null) DeffensPanel.SetActive(true);
    }
    void Start()
    {
        if (FinancePanel != null) FinancePanel.SetActive(true);
        if (JusticePanel != null) JusticePanel.SetActive(false);
        if (ForeighPanel != null) ForeighPanel.SetActive(false);
        if (SocialHealtPanel != null) SocialHealtPanel.SetActive(false);
        if (TransportPanel != null) TransportPanel.SetActive(false);
        if (DeffensPanel != null) DeffensPanel.SetActive(false);
        if (ProjectPanel != null) ProjectPanel.SetActive(false);
        if (GrafPanel != null) GrafPanel.SetActive(false);
        if (PousaPanel != null) PousaPanel.SetActive(false);
        if (DataPanel != null) DataPanel.SetActive(false);
        if (GroupPanel != null) GroupPanel.SetActive(false);
        if (PartyPanel != null) PartyPanel.SetActive(false);
        if (PolicyPanel != null) PolicyPanel.SetActive(false);
        if (EventPanel != null) EventPanel.SetActive(false);
    }
    public void OpenProjekt()
    {
        if (ProjectPanel != null) ProjectPanel.SetActive(true);
    }
    public void ClouseProjekt()
    {
        if (ProjectPanel != null) ProjectPanel.SetActive(false);
    }
    public void OpenGraf()
    {
        if (GrafPanel != null) GrafPanel.SetActive(true);
    }
    public void ClouseGraf()
    {
        if (GrafPanel != null) GrafPanel.SetActive(false);
    }
    public void OpenPousa()
    {
        if (PousaPanel != null) PousaPanel.SetActive(true);
    }
    public void ClousePouse()
    {
        if (PousaPanel != null)  PousaPanel.SetActive(false);
    }
    public void OpenData()
    {
        if (DataPanel != null) DataPanel.SetActive(true);
    }
    public void CloseData()
    {
        if (DataPanel != null) DataPanel.SetActive(false);
    }
    public void OpenParty()
    {
        
        if (PartyPanel != null) PartyPanel.SetActive(true);
        partyPanel.Open();
    }
    public void CloseParty()
    {
        if (PartyPanel != null) PartyPanel.SetActive(false);
    }
    public void OpenGroups()
    {
        if (GroupPanel != null) GroupPanel.SetActive(true);
    }
    public void CloseGroups()
    {
        if (GroupPanel != null) GroupPanel.SetActive(false);
    }
    public void ClosePolicy()
    {
        if (PolicyPanel != null) PolicyPanel.SetActive(false);
    }
    public void Finance()
    {
        if (FinancePanel != null) FinancePanel.SetActive(true);
        if (JusticePanel != null) JusticePanel.SetActive(false);
        if (ForeighPanel != null) ForeighPanel.SetActive(false);
        if (SocialHealtPanel != null) SocialHealtPanel.SetActive(false);
        if (TransportPanel != null) TransportPanel.SetActive(false);
        if (DeffensPanel != null) DeffensPanel.SetActive(false);
    }
    public void Justice()
    {
        if (FinancePanel != null) FinancePanel.SetActive(false);
        if (JusticePanel != null) JusticePanel.SetActive(true);
        if (ForeighPanel != null) ForeighPanel.SetActive(false);
        if (SocialHealtPanel != null) SocialHealtPanel.SetActive(false);
        if (TransportPanel != null) TransportPanel.SetActive(false);
        if (DeffensPanel != null) DeffensPanel.SetActive(false);
    }
    public void ForeighPolicy()
    {
        if (FinancePanel != null) FinancePanel.SetActive(false);
        if (JusticePanel != null) JusticePanel.SetActive(false);
        if (ForeighPanel != null) ForeighPanel.SetActive(true);
        if (SocialHealtPanel != null) SocialHealtPanel.SetActive(false);
        if (TransportPanel != null) TransportPanel.SetActive(false);
        if (DeffensPanel != null) DeffensPanel.SetActive(false);
    }
    public void SocialHealt()
    {
        if (FinancePanel != null) FinancePanel.SetActive(false);
        if (JusticePanel != null) JusticePanel.SetActive(false);
        if (ForeighPanel != null) ForeighPanel.SetActive(false);
        if (SocialHealtPanel != null) SocialHealtPanel.SetActive(true);
        if (TransportPanel != null) TransportPanel.SetActive(false);
        if (DeffensPanel != null) DeffensPanel.SetActive(false);
    }
    public void Transport()
    {
        if (FinancePanel != null) FinancePanel.SetActive(false);
        if (JusticePanel != null) JusticePanel.SetActive(false);
        if (ForeighPanel != null) ForeighPanel.SetActive(false);
        if (SocialHealtPanel != null) SocialHealtPanel.SetActive(false);
        if (TransportPanel != null) TransportPanel.SetActive(true);
        if (DeffensPanel != null) DeffensPanel.SetActive(false);
    }
    public void Deffens()
    {
        if (FinancePanel != null) FinancePanel.SetActive(false);
        if (JusticePanel != null) JusticePanel.SetActive(false);
        if (ForeighPanel != null) ForeighPanel.SetActive(false);
        if (SocialHealtPanel != null) SocialHealtPanel.SetActive(false);
        if (TransportPanel != null) TransportPanel.SetActive(false);
        if (DeffensPanel != null) DeffensPanel.SetActive(true);
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
