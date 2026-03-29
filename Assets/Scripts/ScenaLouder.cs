using UnityEngine;
using UnityEngine.SceneManagement;

public class ScenaLouder : MonoBehaviour
{
    [SerializeField]
    private GameObject partySelectPanel;
    [SerializeField]
    private GameObject partyDetailPanel;
    [SerializeField]
    private GameObject settingsPanel;


    private void Awake()
    {
        Debug.Log("ScenaLouder aktivní objekt: " + gameObject.name);
    }
    private void Start()
    {
        // Na zaèátku chceme vidìt jen výbìr stran
        if (partySelectPanel != null) partySelectPanel.SetActive(false);
        if (partyDetailPanel != null) partyDetailPanel.SetActive(false);
        if (settingsPanel != null) settingsPanel.SetActive(false);
    }

    // Otevøe výbìr stran
    public void OpenPartySelect()
    {
        if (partySelectPanel != null) partySelectPanel.SetActive(true);
        if (partyDetailPanel != null) partyDetailPanel.SetActive(false);
    }

    // Zavøe výbìr stran
    public void ClosePartySelect()
    {
        if (partySelectPanel != null) partySelectPanel.SetActive(false);
        
    }

    // Otevøe detail strany
    public void OpenPartyDetail()
    {
        if (partyDetailPanel != null) partyDetailPanel.SetActive(true);
        if (partySelectPanel != null) partySelectPanel.SetActive(false);
    }

    // Zavøe detail a vrátí zpìt na výbìr
    public void ClosePartyDetail()
    {
        if (partyDetailPanel != null) partyDetailPanel.SetActive(false);
        if (partySelectPanel != null) partySelectPanel.SetActive(true);
    }
    public void LoadGame()
    {
        SceneManager.LoadScene("GameScene");
    }
    public void OpenSettings()
    {
        if (settingsPanel != null) settingsPanel.SetActive(true);
    }
    public void CloseSettings()
    {
        if (settingsPanel != null) settingsPanel.SetActive(false);
    }
    public void QuitGame()
    {
        Application.Quit();
        
    }
}
