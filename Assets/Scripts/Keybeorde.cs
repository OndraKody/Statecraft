using UnityEngine;

public class Keybeorde : MonoBehaviour
{
    public GameObject pausePanel;
    public GameObject ideasPanel;
    public GameObject graphsPanel;
    public GameObject partiesPanel;
    public GameObject groupsPanel;
    public GameObject detailePanel;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }


    void ShowPanel(GameObject panel)
    {
        ideasPanel.SetActive(false);
        graphsPanel.SetActive(false);
        partiesPanel.SetActive(false);
        groupsPanel.SetActive(false);
        detailePanel.SetActive(false);

        panel.SetActive(true);
    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1)) ShowPanel(ideasPanel);
        if (Input.GetKeyDown(KeyCode.Alpha3)) ShowPanel(graphsPanel);
        if (Input.GetKeyDown(KeyCode.Alpha2)) ShowPanel(partiesPanel);
        if (Input.GetKeyDown(KeyCode.Alpha4)) ShowPanel(groupsPanel);
        if (Input.GetKeyDown(KeyCode.Alpha5)) ShowPanel(detailePanel);

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            pausePanel.SetActive(!pausePanel.activeSelf);
        }
    }
}
