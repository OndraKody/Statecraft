using UnityEngine;
using UnityEngine.SceneManagement;

public class ScenaLouder : MonoBehaviour
{
    [SerializeField] private GameObject partySelectPanel;
    [SerializeField] private GameObject partyDetailPanel;
    [SerializeField] private GameObject settingsPanel;
    [SerializeField] private GameObject savePanel; // V Inspectoru sem pøiøaï tvùj SavePanel

    private void Awake()
    {
        Debug.Log("ScenaLouder aktivní objekt: " + gameObject.name);
    }

    private void Start()
    {
        // Na zaèátku neukazujeme nic kromì hlavního menu
        if (savePanel != null) savePanel.SetActive(false);
        if (partySelectPanel != null) partySelectPanel.SetActive(false);
        if (partyDetailPanel != null) partyDetailPanel.SetActive(false);
        if (settingsPanel != null) settingsPanel.SetActive(false);
    }

    // TUTO FUNKCI DEJ NA TLAÈÍTKO "START" v hlavním menu
    public void OpenSaveSlots()
    {
        if (savePanel != null) savePanel.SetActive(true);
        if (partySelectPanel != null) partySelectPanel.SetActive(false);
        if (partyDetailPanel != null) partyDetailPanel.SetActive(false);
    }

    public void CloseSaveSlots()
    {
        if (savePanel != null) savePanel.SetActive(false);
    }

    // Tuto funkci volá prázdný slot po kliknutí
    public void OpenPartySelect()
    {
        if (partySelectPanel != null) partySelectPanel.SetActive(true);
        if (partyDetailPanel != null) partyDetailPanel.SetActive(false);
        if (savePanel != null) savePanel.SetActive(false); // Skryje panel se sloty
    }

    public void ClosePartySelect()
    {
        if (partySelectPanel != null) partySelectPanel.SetActive(false);
        if (savePanel != null) savePanel.SetActive(true); // Vrátí zpìt panel se sloty
    }

    public void OpenPartyDetail()
    {
        if (partyDetailPanel != null) partyDetailPanel.SetActive(true);
        if (partySelectPanel != null) partySelectPanel.SetActive(false);
    }

    public void ClosePartyDetail()
    {
        if (partyDetailPanel != null) partyDetailPanel.SetActive(false);
        if (partySelectPanel != null) partySelectPanel.SetActive(true);
    }

    public void LoadGame()
    {
        SceneManager.LoadScene("GameScene");
    }

    public void ReturnToMainMenu()
    {
        SceneManager.LoadScene("MeinMenu");
    }

    public void OpenSettings() { if (settingsPanel != null) settingsPanel.SetActive(true); }
    public void CloseSettings() { if (settingsPanel != null) settingsPanel.SetActive(false); }
    public void QuitGame() { Application.Quit(); }
}