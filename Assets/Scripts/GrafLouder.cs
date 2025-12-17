using UnityEngine;

public class GrafLouder : MonoBehaviour
{
    public GameObject Income;
    public GameObject Vydaje;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (Income != null) Income.SetActive(true);
        if (Vydaje != null) Vydaje.SetActive(false);
    }
    public void OpenIncomePie()
    {
        if (Income != null) Income.SetActive(true);
        if (Vydaje != null) Vydaje.SetActive(false);
    }
    public void OpenVydajePie()
    {
        if (Income != null) Income.SetActive(false);
        if (Vydaje != null) Vydaje.SetActive(true);
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
