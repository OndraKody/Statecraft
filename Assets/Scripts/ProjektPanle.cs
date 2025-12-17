using UnityEngine;

public class ProjektPanle : MonoBehaviour
{
    [SerializeField]
    private GameObject FinancePanelPT;
    [SerializeField]
    private GameObject JusticePanelPT;
    [SerializeField]
    private GameObject ForeighPanelPT;
    [SerializeField]
    private GameObject SocialHealtPanelPT;
    [SerializeField]
    private GameObject TransportPanelPT;
    [SerializeField]
    private GameObject DeffensPanelPT;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (FinancePanelPT != null) FinancePanelPT.SetActive(true);
        if (JusticePanelPT != null) JusticePanelPT.SetActive(false);
        if (ForeighPanelPT != null) ForeighPanelPT.SetActive(false);
        if (SocialHealtPanelPT != null) SocialHealtPanelPT.SetActive(false);
        if (TransportPanelPT != null) TransportPanelPT.SetActive(false);
        if (DeffensPanelPT != null) DeffensPanelPT.SetActive(false);
    }
    public void FinancePT()
    {
        if (FinancePanelPT != null) FinancePanelPT.SetActive(true);
        if (JusticePanelPT != null) JusticePanelPT.SetActive(false);
        if (ForeighPanelPT != null) ForeighPanelPT.SetActive(false);
        if (SocialHealtPanelPT != null) SocialHealtPanelPT.SetActive(false);
        if (TransportPanelPT != null) TransportPanelPT.SetActive(false);
        if (DeffensPanelPT != null) DeffensPanelPT.SetActive(false);
    }
    public void JusticePT()
    {
        if (FinancePanelPT != null) FinancePanelPT.SetActive(false);
        if (JusticePanelPT != null) JusticePanelPT.SetActive(true);
        if (ForeighPanelPT != null) ForeighPanelPT.SetActive(false);
        if (SocialHealtPanelPT != null) SocialHealtPanelPT.SetActive(false);
        if (TransportPanelPT != null) TransportPanelPT.SetActive(false);
        if (DeffensPanelPT != null) DeffensPanelPT.SetActive(false);
    }
    public void ForeighPT()
    {
        if (FinancePanelPT != null) FinancePanelPT.SetActive(false);
        if (JusticePanelPT != null) JusticePanelPT.SetActive(false);
        if (ForeighPanelPT != null) ForeighPanelPT.SetActive(true);
        if (SocialHealtPanelPT != null) SocialHealtPanelPT.SetActive(false);
        if (TransportPanelPT != null) TransportPanelPT.SetActive(false);
        if (DeffensPanelPT != null) DeffensPanelPT.SetActive(false);
    }
    public void SocialHealtPT()
    {
        if (FinancePanelPT != null) FinancePanelPT.SetActive(false);
        if (JusticePanelPT != null) JusticePanelPT.SetActive(false);
        if (ForeighPanelPT != null) ForeighPanelPT.SetActive(false);
        if (SocialHealtPanelPT != null) SocialHealtPanelPT.SetActive(true);
        if (TransportPanelPT != null) TransportPanelPT.SetActive(false);
        if (DeffensPanelPT != null) DeffensPanelPT.SetActive(false);
    }
    public void TransportPT()
    {
        if (FinancePanelPT != null) FinancePanelPT.SetActive(false);
        if (JusticePanelPT != null) JusticePanelPT.SetActive(false);
        if (ForeighPanelPT != null) ForeighPanelPT.SetActive(false);
        if (SocialHealtPanelPT != null) SocialHealtPanelPT.SetActive(false);
        if (TransportPanelPT != null) TransportPanelPT.SetActive(true);
        if (DeffensPanelPT != null) DeffensPanelPT.SetActive(false);
    }
    public void DeffensPT()
    {
        if (FinancePanelPT != null) FinancePanelPT.SetActive(false);
        if (JusticePanelPT != null) JusticePanelPT.SetActive(false);
        if (ForeighPanelPT != null) ForeighPanelPT.SetActive(false);
        if (SocialHealtPanelPT != null) SocialHealtPanelPT.SetActive(false);
        if (TransportPanelPT != null) TransportPanelPT.SetActive(false);
        if (DeffensPanelPT != null) DeffensPanelPT.SetActive(true);
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
